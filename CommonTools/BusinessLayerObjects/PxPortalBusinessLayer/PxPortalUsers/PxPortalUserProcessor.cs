///-------------------------------------------------------------------------------------------------
// <copyright file="UserManager.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Dtorres</author>
// <date>20160516</date>
// <summary>Implements the user manager class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF = SignalEFDataModel.SGNL_PXPORTAL;
using WhEF = SignalEFDataModel.SGNL_WAREHOUSE;
using System.Diagnostics;
using CommonUtils.Crypto;
using System.Text.RegularExpressions;
using CommonUtils.Email;
using System.Net.Mail;
using BusinessLayer;
using System.IO;
using System.Collections.Specialized;
using ViewModel;
using AppDataLib.Enums;

namespace BusinessLayer.PxPortal
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for users. </summary>
    ///
    /// <remarks>   Dtorres, 20160516. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class PxPortalUserProcessor : IPxPortalUserProcessor
    {
        /// <summary>   The password expiration in months. </summary>
        private const int PasswordExpirationMonths = 6;
        private int salesRepId = 0;


        /// <summary>   Context for the databases. </summary>
        private EF.SGNL_PXPORTALEntities DbContext;
        private WhEF.SGNL_WAREHOUSEEntities WarehouseDbContext;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the PxPortalWebApp.Domain.UserManager class.
        /// </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <param name="db">   The database. </param>
        ///-------------------------------------------------------------------------------------------------

        public PxPortalUserProcessor(EF.SGNL_PXPORTALEntities db)
        {
            this.DbContext = db;
            this.WarehouseDbContext = new EFDataModel.SGNL_WAREHOUSE.SGNL_WAREHOUSEEntities();
        }


        public PxPortalUserProcessor(string pxPortalConn)
        {
            this.DbContext = new EFDataModel.SGNL_PXPORTAL.SGNL_PXPORTALEntities(pxPortalConn);
            this.WarehouseDbContext = new EFDataModel.SGNL_WAREHOUSE.SGNL_WAREHOUSEEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalBusinessLayer.PxPortal.PxPortalUserProcessor
        /// class.
        /// </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160927. </remarks>
        ///
        /// <param name="db">   The database. </param>
        /// <param name="whDb"> The wh database. </param>
        ///-------------------------------------------------------------------------------------------------

        public PxPortalUserProcessor(EF.SGNL_PXPORTALEntities db, WhEF.SGNL_WAREHOUSEEntities whDb)
        {
            this.DbContext = db;
            this.WarehouseDbContext = whDb;
        }

        public PxPortalUserProcessor(string pxPortalConn, string warehouseConn)
        {
            this.DbContext = new EFDataModel.SGNL_PXPORTAL.SGNL_PXPORTALEntities(pxPortalConn);
            this.WarehouseDbContext = new EFDataModel.SGNL_WAREHOUSE.SGNL_WAREHOUSEEntities(warehouseConn);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Authenticate user. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="emailaddress"> Name of the user. </param>
        /// <param name="password">     The password. </param>
        /// <param name="PxPortalUserProfile">  Output parameter: If user authenticates then the user's profile
        ///                             is populated. </param>
        ///
        /// <returns>   An AuthenticationResult. </returns>
        ///-------------------------------------------------------------------------------------------------

        public AuthenticationResult AuthenticateUser(string emailaddress, string password, out PxPortalUserProfile PxPortalUserProfile)
        {
            Debug.Assert(emailaddress != null && password != null);
            if (emailaddress == null || password == null)
                throw new ArgumentNullException("email address or password were null");

            AuthenticationResult result = AuthenticationResult.InvalidPassword;
            EF.UserPassword userPassword = GetUserPasswordByUserName(emailaddress);

            if (userPassword == null)
            {
                result = AuthenticationResult.InvalidUserName;
            }
            //does user need to reset password 
            else if (
                ((DateTimeOffset.Now > userPassword.ExpirationDate) || userPassword.ChangeRequired) && userPassword.EnforceChangeRequired == true)
            {
                result = AuthenticationResult.PasswordResetRequired;
            }
            //does password match 
            else if (CheckUserPassword(password, userPassword))
            {
                result = AuthenticationResult.Success;
            }
            else
            {
                result = AuthenticationResult.InvalidPassword;
            }

            //Populate output parameter 
            PxPortalUserProfile = null;
            if (result == AuthenticationResult.Success)
            {
                PxPortalUserProfile = GetUserProfile(userPassword.User.Email);
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets user profile. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="userName"> Name of the user. </param>
        ///
        /// <returns>   The user profile. </returns>
        ///-------------------------------------------------------------------------------------------------

        public PxPortalUserProfile GetUserProfile(string userName)
        {
            Debug.Assert(userName != null);
            EF.User user = GetEFUserByName(userName);
            if (user == null)
                throw new UserNotFoundException("User not found");
            List<string> allroles = (from role in user.Roles
                                     select role.RoleName).ToList();
            var PxPortalUserProfile = new PxPortalUserProfile()
            {
                email = user.Email,
                id = user.id.ToString(),
                firstName = user.FirstName,
                lastName = user.LastName,
                isEmailAuthorizedForResults = user.EmailAuthorizedForResults,
                roles = allroles,
                highestRole = GetHighestRole(allroles).ToString(),
            };
            return PxPortalUserProfile;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets user access filters. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161013. </remarks>
        ///
        /// <param name="username"> The username. </param>
        ///
        /// <returns>   The user access filters. </returns>
        ///-------------------------------------------------------------------------------------------------

        public IEnumerable<UserAccessFilter> GetUserAccessFilters(string username)
        {
            string wildcard = "-888";
            string separator = "_";
            string globalAdmin = wildcard + separator + wildcard + separator + wildcard;
            string noDoc = wildcard;
            string noClient = separator + wildcard;
            string noSalesRep = wildcard + separator;
            var user = GetEFUserQueryByName(username);
            var accessFilters = user
                             .SelectMany(many => many.AccessMaps)
                             .Where(f => f.AccessFilter.IsActive == true)
                             .Select(f => new
                             {
                                 ClientId = f.AccessFilter.ClientId,
                                 PhysicianId = f.AccessFilter.PhysicianId,
                                 SalesRepId = f.AccessFilter.SalesRepId,
                             }).Distinct().ToList();
            List<string> filterObjects = (from acp in accessFilters
                                     select (acp.SalesRepId.ToString() + separator + acp.PhysicianId.ToString() + separator + acp.ClientId.ToString())
            ).Distinct().ToList();
            var filterData = WarehouseDbContext.vi_PxPortalUserCases
                         .Where(a =>
                            // Match all 3 columns in Access Filter:
                            filterObjects.Contains(a.SalesRepId.ToString() + separator + a.DoctorId.ToString() + separator + a.ClientId.ToString())
                            // Match SalesRep, wildcards for DoctorId and ClientId:
                            || filterObjects.Contains(a.SalesRepId.ToString() + separator + noDoc + noClient)
                            // Match SalesRep and ClientId, wildcard DoctorId:
                            || filterObjects.Contains(a.SalesRepId.ToString() + separator + noDoc + separator + a.ClientId.ToString())
                            // Match SalesRep and DoctorId, wildcard ClientId:
                            || filterObjects.Contains(a.SalesRepId.ToString() + separator + a.DoctorId + noClient)
                            // No SalesRep, match DoctorId and ClientId:
                            || filterObjects.Contains(noSalesRep + a.DoctorId.ToString() + separator + a.ClientId.ToString())
                            // No SalesRepId, wildcard doctor, match ClientId:
                            || filterObjects.Contains(noSalesRep + noDoc + separator + a.ClientId.ToString())
                            // All three columns are wildcards:
                            || filterObjects.Contains(globalAdmin))
                             .Select(detail => new
                             {
                                 ClientId = detail.ClientId,
                                 ClientCode = detail.ClientCode ?? string.Empty,
                                 ClientName = detail.ClientName ?? string.Empty,
                                 PhysicianId = detail.DoctorId ?? 0,
                                 PhysicianFullName = detail.OrderingPhysicianFullName ?? string.Empty,
                                 SalesRepId = detail.SalesRepId ?? 0,
                                 SalesRepName = (detail.SalesRepFirstName + " " + detail.SalesRepLastName) ?? string.Empty,
                                 TerritoryId = detail.TerritoryId ?? 0,
                                 TerritoryName = detail.TerritoryWhenAccessioned ?? string.Empty
                             }).Distinct().ToList();

            List<UserAccessFilter> viewModelFilters = new List<UserAccessFilter>();
            foreach (var filter in filterData)
            {
                viewModelFilters.Add(new UserAccessFilter()
                {
                    ClientId = filter.ClientId,
                    ClientCode = filter.ClientCode,
                    ClientName = filter.ClientName,
                    PhysicianId = filter.PhysicianId,
                    PhysicianFullName = filter.PhysicianFullName,
                    SalesRepId = filter.SalesRepId,
                    SalesRepName = filter.SalesRepName,
                    TerritoryId = filter.TerritoryId,
                    TerritoryName = filter.TerritoryName
                });
            }
            return viewModelFilters;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates new user. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="emailaddress">         The emailaddress. </param>
        /// <param name="password">             The password. </param>
        /// <param name="keyStretchIterations"> The key stretch iterations. </param>
        /// <param name="saltSize">             Size of the salt. </param>
        /// <param name="passwordKeySize">      Size of the password key. </param>
        ///
        /// <returns>   The new new user. </returns>
        ///-------------------------------------------------------------------------------------------------

        public RegisterNewUserResult CreateNewUser(string emailaddress,
            string password,
            int keyStretchIterations,
            int saltSize,
            int passwordKeySize,
            List<string> roles,
            string accountName,
            string userFirstName = null,
            string userLastName = null
            )
        {
            RegisterNewUserResult result = RegisterNewUserResult.UnknownError;

            if (emailaddress == null || password == null)
                throw new ArgumentNullException("email address or password were null");

            if (DbContext.Users.Any(u => u.Email == emailaddress))
            {
                result = RegisterNewUserResult.UserAlreadyExists;
            }
            else if (!PasswordComplexityOk(password))
            {
                result = RegisterNewUserResult.NewPasswordComplexityFailed;
            }
            else
            {
                EF.User user = GetNewUser(emailaddress, password, keyStretchIterations, saltSize, passwordKeySize, roles, userFirstName, userLastName);
                if (salesRepId != 0)
                {
                    user.UserType = (from type in DbContext.UserTypes
                                     where type.Name.ToLower().Trim() == "salesrep"
                                     select type).FirstOrDefault();
                    user.ExternalId = salesRepId;
                }
                AddUser(user);
                DbContext.SaveChanges();
                var userId = user.id;
                result = RegisterNewUserResult.Success;
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the user described by emailaddress. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160928. </remarks>
        ///
        /// <param name="emailaddress"> Name of the user. </param>
        ///
        /// <returns>   A RemoveUserResult. </returns>
        ///-------------------------------------------------------------------------------------------------

        public RemoveUserResult RemoveUser(string userName)
        {
            return RemoveUser(userName, false);
        }

        public RemoveUserResult RemoveUser(string userName, bool force)
        {
            var user = (from portal in DbContext.Users
                        where portal.UserName.ToLower().Trim() == userName.ToLower().Trim()
                        select portal).FirstOrDefault();
            if (user != null)
                return DeleteUser(user, force);
            else
                return RemoveUserResult.UserDoesNotExist;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Upgrade user security if neccessary. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="username">         The username. </param>
        /// <param name="password">         The password. </param>
        /// <param name="targetIterations"> Target iterations. </param>
        /// <param name="targetSaltSize">   Size of the target salt. </param>
        /// <param name="targetKeySize">    Size of the target key. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool UpgradeUserSecurityIfNeeded(string username, string password, int targetIterations, int targetSaltSize, int targetKeySize)
        {
            bool argumentsOk = (username != null && password != null && targetIterations >= 1000 && targetSaltSize >= 32 && targetKeySize >= 20);
            Debug.Assert(argumentsOk);
            if (!argumentsOk)
                throw new Exception("Invalid arguments.");

            bool isUpgraded = false;
            EF.UserPassword userPassword = GetUserPasswordByUserName(username);
            Debug.Assert(userPassword != null);
            EF.User user = userPassword.User;
            //upgrade security parameters if there is new iteraions, salt or key size desired. 
            if (user != null
                && (userPassword.KeyStretchIterations != targetIterations || userPassword.SaltSize != targetSaltSize || userPassword.KeySize != targetKeySize))
            {
                PxPortalUserProfile notUsed;
                var authResult = AuthenticateUser(username, password, out notUsed);
                Debug.Assert(authResult == AuthenticationResult.Success);
                if (authResult == AuthenticationResult.Success)
                {
                    PBKDFHasher hasher = new PBKDFHasher(targetIterations, targetSaltSize, targetKeySize);
                    HashResult hashresult = hasher.ComputeHash(password);
                    userPassword.Password = hashresult.Hash;
                    userPassword.Salt = hashresult.Salt;
                    userPassword.KeyStretchIterations = targetIterations;
                    userPassword.SaltSize = targetSaltSize;
                    userPassword.KeySize = targetKeySize;

                    DbContext.SaveChanges();
                    isUpgraded = true;
                }
            }
            return isUpgraded;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets new user. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <param name="emailaddress">         The emailaddress. </param>
        /// <param name="password">             The password. </param>
        /// <param name="keyStretchIterations"> The key stretch iterations. </param>
        /// <param name="saltSize">             Size of the salt. </param>
        /// <param name="passwordKeySize">      Size of the password key. </param>
        ///
        /// <returns>   The new user. </returns>
        ///-------------------------------------------------------------------------------------------------

        private EF.User GetNewUser(string emailaddress, string password, int keyStretchIterations, int saltSize, int passwordKeySize, List<string> roles = null, string userFirstName = null, string userLastName = null, int ? userTypeId = null, int? accessMapId = null, int? ExternalId = null)
        {
            PBKDFHasher hasher = new PBKDFHasher(keyStretchIterations, saltSize, passwordKeySize);
            HashResult hash = hasher.ComputeHash(password);

            var newPassword = new EF.UserPassword()
            {
                ChangeRequired = false,
                CreatedDate = DateTimeOffset.Now,
                EnforceChangeRequired = true,
                ExpirationDate = DateTimeOffset.Now.AddMonths(PasswordExpirationMonths),
                IsActive = true,
                KeySize = passwordKeySize,
                KeyStretchIterations = keyStretchIterations,
                Password = hash.Hash,
                Salt = hash.Salt,
                SaltSize = saltSize
            };


            var user = new EF.User()
            {
                CreatedDate = DateTimeOffset.Now,
                Email = emailaddress,
                FirstName = userFirstName,
                IsActive = true,
                LastName = userLastName,
                UserName = emailaddress                
            };

            user.UserPasswords.Add(newPassword);

            if (roles == null || roles.Count() == 0)
            {
                user.Roles.Add(DbContext.Roles.Where(r => r.RoleName == RoleValues.User).First());
            }
            else
            {
                foreach (string roleName in roles)
                {
                    user.Roles.Add(DbContext.Roles.Where(r => r.RoleName == roleName).First());
                    if (roleName == "SalesRep")
                    {
                        salesRepId = WarehouseDbContext.vi_PxPortalUserCases
                                         .Where(ufn => user.FirstName == ufn.SalesRepFirstName)
                                         .Where(uln => user.LastName == uln.SalesRepLastName)
                                         .Select(rep => rep.SalesRepId)
                                         .FirstOrDefault() ?? 0;
                    }
                }
            }
            return user;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Resets the user password described by username. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="username"> The username. </param>
        ///
        /// <returns>   true if user password was successfully reset. False otherwise. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool ResetUserPassword(string username, EmailObject eo)
        {
            Debug.Assert(username != null);
            if (username == null)
                throw new ArgumentNullException();

            bool wasReset = false;
            EF.UserPassword userPassword = GetUserPasswordByUserName(username);
            if (userPassword != null)
            {
                EF.User user = userPassword.User;
                const int RandomPasswordLength = 15;
                PBKDFHasher hasher = new PBKDFHasher(userPassword.KeyStretchIterations, userPassword.SaltSize, userPassword.KeySize);
                string randomPassword = hasher.GenerateRandomPassword(RandomPasswordLength);
                HashResult hash = hasher.ComputeHash(randomPassword);
                // userPassword.Password = hash.Hash;
                // userPassword.Salt = hash.Salt;
                //userPassword.ChangeRequired = true; //User must change password at next login 

                userPassword.IsActive = false;
                var newPassword = new EF.UserPassword()
                {
                    ChangeRequired = true, //User must change password at next login
                    CreatedDate = DateTimeOffset.Now,
                    EnforceChangeRequired = true,
                    ExpirationDate = DateTimeOffset.Now.AddMonths(PasswordExpirationMonths),
                    IsActive = true,
                    KeySize = userPassword.KeySize,
                    KeyStretchIterations = userPassword.KeyStretchIterations,
                    Password = hash.Hash,
                    Salt = hash.Salt,
                    SaltSize = userPassword.SaltSize
                };

                user.UserPasswords.Add(newPassword);
                DbContext.SaveChanges();

                SendPasswordResetEmail(user, randomPassword, eo);
                wasReset = true;
            }
            return wasReset;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change user password. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="username">     The username. </param>
        /// <param name="oldpassword">  The oldpassword. </param>
        /// <param name="newpassword">  The newpassword. </param>
        ///
        /// <returns>   A ChangePasswordResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ChangePasswordResult ChangeUserPassword(string username, string oldpassword, string newpassword, EmailObject eo)
        {
            bool argumentsOk = (username != null && oldpassword != null && newpassword != null);
            Debug.Assert(argumentsOk, "Invalid arguments");
            if (!argumentsOk)
                throw new ArgumentNullException("Invalid arguments");

            var result = ChangePasswordResult.UnknownError;
            EF.UserPassword userPassword = GetUserPasswordByUserName(username);
            if (userPassword == null)
            {
                result = ChangePasswordResult.InvalidUserName;
            }
            else if (!CheckUserPassword(oldpassword, userPassword))
            {
                result = ChangePasswordResult.InvalidPassword;
            }
            else if (!PasswordComplexityOk(newpassword))
            {
                result = ChangePasswordResult.NewPasswordComplexityFailed;
            }
            else if (!PasswordHistoryOk(username, newpassword))
            {
                result = ChangePasswordResult.NewPasswordHistoryFailed;
            }
            else
            {
                PBKDFHasher hasher = new PBKDFHasher(userPassword.KeyStretchIterations, userPassword.SaltSize, userPassword.KeySize);
                HashResult hash = hasher.ComputeHash(newpassword);

                userPassword.IsActive = false;
                var newPassword = new EF.UserPassword()
                {
                    ChangeRequired = false,
                    CreatedDate = DateTimeOffset.Now,
                    EnforceChangeRequired = true,
                    ExpirationDate = DateTimeOffset.Now.AddMonths(PasswordExpirationMonths),
                    IsActive = true,
                    KeySize = userPassword.KeySize,
                    KeyStretchIterations = userPassword.KeyStretchIterations,
                    Password = hash.Hash,
                    Salt = hash.Salt,
                    SaltSize = userPassword.SaltSize
                };
                EF.User user = userPassword.User;
                user.UserPasswords.Add(newPassword);

                DbContext.SaveChanges();
                result = ChangePasswordResult.Success;

                SendConfirmationEmail(user, eo);
            }
            return result;
        }

        // Regex to search for replacement tokens in template.  
        public static readonly Regex re = new Regex(@"\<#=(\w+)\#>", RegexOptions.Compiled);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a confirmation email. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161005. </remarks>
        ///
        /// <param name="user"> The user. </param>
        /// <param name="eo">   The eo. </param>
        ///-------------------------------------------------------------------------------------------------
        public void SendConfirmationEmail(EF.User user, EmailObject eo)
        {

            string msgBody = File.ReadAllText(@".\EmailTemplates\changePasswordTemplate.html");
            ListDictionary replacements = new ListDictionary();
            replacements.Add("FullName", user.FirstName + " " + user.LastName);
            replacements.Add("Email", user.Email);
            msgBody = re.Replace(msgBody.Replace("<# =", "<#="), match => { return replacements.Contains(match.Groups[1].Value) ? replacements[match.Groups[1].Value].ToString() : match.Value; });

            // Instantiate new email message object
            var email = new EmailMessage()
            {
                EmailTo = user.Email,
                FromAddress = eo.FromAddress,//"isemail@signalgenetics.com",
                BccAddresses = eo.BccAddresses,//null,
                CcAddresses = eo.CcAddresses,//null,
                EmailSubject = "Confirmation of password change on Physicians Portal",
                EmailUsername = eo.EmailUsername,//"isemail@signalgenetics.com",
                EmailPassword = eo.EmailPassword,//"V6x0OEFj6c2H6f4k",
                EmailBody = msgBody,
                ReplyToList = eo.ReplyToList,//null,
                HostAddress = eo.HostAddress,//"smtp.office365.com",
                HostPort = eo.HostPort,//587
            };
            var message = email.CreateEmailObject();
            message.Send();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a password reset email. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161005. </remarks>
        ///
        /// <param name="user">             The user. </param>
        /// <param name="randomPassword">   The random password. </param>
        /// <param name="eo">               The eo. </param>
        ///-------------------------------------------------------------------------------------------------
        public void SendPasswordResetEmail(EF.User user, string randomPassword, EmailObject eo)
        {

            string msgBody = File.ReadAllText(@".\EmailTemplates\resetPasswordTemplate.html");
            ListDictionary replacements = new ListDictionary();
            replacements.Add("FullName", user.FirstName + " " + user.LastName);
            replacements.Add("Email", user.Email);
            replacements.Add("RandomPassword", randomPassword);
            msgBody = re.Replace(msgBody.Replace("<# =", "<#="), match => { return replacements.Contains(match.Groups[1].Value) ? replacements[match.Groups[1].Value].ToString() : match.Value; });

            // Instantiate new email message object
            var email = new EmailMessage()
            {
                EmailTo = user.Email,
                FromAddress = eo.FromAddress,//"isemail@signalgenetics.com",
                BccAddresses = eo.BccAddresses,//null,
                CcAddresses = eo.CcAddresses,//null,
                EmailSubject = "Confirmation of password reset on Physicians Portal",
            };
            var message = email.CreateEmailObject();
            message.Send();

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the user profile. </summary>
        ///
        /// <remarks>   Dtorres, 20161012. </remarks>
        ///
        /// <param name="username">             The username. </param>
        /// <param name="firstName">            The person's first name. </param>
        /// <param name="lastName">             The person's last name. </param>
        /// <param name="isEmailAuthorized">    true if this object is email authorized. </param>
        ///
        /// <returns>   A PxPortalUserProfile. </returns>
        ///-------------------------------------------------------------------------------------------------

        public PxPortalUserProfile UpdateUserProfile(string username, string firstName, string lastName, bool isEmailAuthorized)
        {            
            EF.User user = GetEFUserByName(username);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.EmailAuthorizedForResults = isEmailAuthorized;
            user.EmailAuthorizedDate = isEmailAuthorized == true ? (DateTimeOffset?) DateTimeOffset.UtcNow : null;
            DbContext.SaveChanges();
            PxPortalUserProfile up = GetUserProfile(username);
            return up;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Password complexity ok. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <param name="password"> The password. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        private bool PasswordComplexityOk(string password)
        {
            int pswLength = 8;
            var regexItem = new Regex("^(?=.*[!@#$%^&*?]).+$");
            bool IsComplex = (password.Length >= pswLength && (regexItem.IsMatch(password)));
            return IsComplex;
        }

        private bool PasswordHistoryOk(string username, string password)
        {
            bool IsNewPassword = true;
            List<EF.UserPassword> lastUserPass = GetPasswordHistoryByUserName(username, 5);
            foreach (EF.UserPassword up in lastUserPass)
            {
                if (CheckUserPassword(password, up))
                    IsNewPassword = false;
            }
            return IsNewPassword;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a user. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <param name="user"> The user. </param>
        ///-------------------------------------------------------------------------------------------------
        private void AddUser(EF.User user)
        {
            DbContext.Users.Add(user);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the user. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161005. </remarks>
        ///
        /// <param name="user">     The user. </param>
        /// <param name="force">    true to force. </param>
        ///
        /// <returns>   A RemoveUserResult. </returns>
        ///-------------------------------------------------------------------------------------------------

        private RemoveUserResult DeleteUser(EF.User user, bool force)
        {
            if (force)
            {
                DbContext.Users.Remove(user);
            }
            else
            {
                user.IsActive = false;
            }
            DbContext.SaveChanges();
            return RemoveUserResult.Success;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets user by user name. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <param name="username"> The username. </param>
        ///
        /// <returns>   The user by user name. </returns>
        ///-------------------------------------------------------------------------------------------------
        private EF.User GetEFUserByName(string username)
        {
            Debug.Assert(username != null);
            EF.User user = DbContext.Users.Where(u => u.Email == username).FirstOrDefault();
            return user;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets ef user query by name. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161014. </remarks>
        ///
        /// <param name="username"> The username. </param>
        ///
        /// <returns>   The ef user query by name. </returns>
        ///-------------------------------------------------------------------------------------------------

        private IQueryable<EF.User> GetEFUserQueryByName(string username)
        {
            Debug.Assert(username != null);
            IQueryable<EF.User> user = DbContext.Users.Where(u => u.UserName.ToLower().Trim() == username.ToLower().Trim());
            return user;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets user password by user name. </summary>
        ///
        /// <remarks>   Ssur, 20161003. </remarks>
        ///
        /// <param name="username"> The username. </param>
        ///
        /// <returns>   The user password by user name. </returns>
        ///-------------------------------------------------------------------------------------------------
        private EF.UserPassword GetUserPasswordByUserName(string username)
        {
            Debug.Assert(username != null);
            EF.UserPassword userPassword = DbContext.UserPasswords
                .Where(up => up.User.UserName.ToLower().Trim() == username.ToLower().Trim() && up.IsActive)
                .FirstOrDefault();
            return userPassword;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Check user password. </summary>
        ///
        /// <remarks>   Dtorres, 20160516. </remarks>
        ///
        /// <param name="password"> The password. </param>
        /// <param name="user">     The user. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        private bool CheckUserPassword(string password, EF.UserPassword user)
        {
            PBKDFHasher hasher = new PBKDFHasher(iterationCount: user.KeyStretchIterations, saltSizeBytes: user.SaltSize, keyLength: user.KeySize);
            bool matches = hasher.CheckPassword(password, user.Password, user.Salt);
            return matches;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets password history by user name. </summary>
        ///
        /// <remarks>   Ssur, 20161003. </remarks>
        ///
        /// <param name="username"> The username. </param>
        /// <param name="top">      The top. </param>
        ///
        /// <returns>   The password history by user name. </returns>
        ///-------------------------------------------------------------------------------------------------
        private List<EF.UserPassword> GetPasswordHistoryByUserName(string username, int top)
        {
            Debug.Assert(username != null);
            List<EF.UserPassword> lastUserPassword = DbContext.UserPasswords
                .Where(up => up.User.UserName == username).Take(top).OrderByDescending(up => up.CreatedDate)
                .ToList();

            return lastUserPassword;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List roles. </summary>
        ///
        /// <remarks>   Ssur, 20161003. </remarks>
        ///
        /// <returns>   A List&lt;string&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<string> ListRoles()
        {
            List<string> list = new List<string>();
            DbContext.Roles.ToList().ForEach(r => list.Add(r.RoleName));
            return list;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets user role names. </summary>
        ///
        /// <remarks>   Ssur, 20161003. </remarks>
        ///
        /// <param name="username"> The username. </param>
        ///
        /// <returns>   The user role names. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<string> GetUserRoleNames(string username)
        {
            var roles = (from user in DbContext.Users
                         where user.UserName.ToLower().Trim() == username.ToLower().Trim()
                         select user.Roles).ToList();
            return (from role in roles
                    select role.FirstOrDefault().RoleName).ToList();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets highest role. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161004. </remarks>
        ///
        /// <param name="roles">    The roles. </param>
        ///
        /// <returns>   The highest role. </returns>
        ///-------------------------------------------------------------------------------------------------
        public PxPortalUserRoles GetHighestRole(List<string> roles)
        {
            var orderMap = new Dictionary<PxPortalUserRoles, int>() {
                                { PxPortalUserRoles.GlobalAdmin, 0 },
                                { PxPortalUserRoles.GlobalReader, 1 },
                                { PxPortalUserRoles.AccountAdmin, 2 },
                                { PxPortalUserRoles.SalesRep, 3 },
                                { PxPortalUserRoles.User, 4 }
                            };
            List<PxPortalUserRoles> roleList = new List<PxPortalUserRoles>();
            foreach (var role in roles)
            {
                var enumResult = new PxPortalUserRoles();
                if (Enum.TryParse(role, out enumResult))
                    roleList.Add(enumResult);
            }
            var orderedRoles = roleList.OrderBy(r => orderMap[r]);

            return orderedRoles.First();
        }

        
    }
}
