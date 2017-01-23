using CommonUtils.Email;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PxPortal
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for in memory users. Used for development </summary>
    ///
    /// <remarks>   Dtorres, 20160515. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class InMemoryUserManager : IPxPortalUserProcessor
    {

        public List<InMemoryUserDefinition> InMemoryUsersList = new List<InMemoryUserDefinition>()
        {
            new InMemoryUserDefinition("user-1", "ss@sg.com", "Sudipto Sur", "sss", new List<string> { "User" }),
            new InMemoryUserDefinition("user-2", "ad@sg.com", "Big Admin", "pass", new List<string> { "Admin" }),
            new InMemoryUserDefinition("user-3", "rp@sg.com", "Rita Philavanh", "123", new List<string> { "User" })
        };

        public AuthenticationResult AuthenticateUser(string userName, string password, out PxPortalUserProfile userProfile)
        {
            Debug.Assert(userName != null || password != null);
            AuthenticationResult result = AuthenticationResult.InvalidPassword;

            //Calculate authentation result
            InMemoryUserDefinition user = InMemoryUsersList.Where(u => u.Email == userName).FirstOrDefault();
            if (user == null)
                result = AuthenticationResult.InvalidUserName;
            else
            {
                if (user.Password == password)
                {
                    result = AuthenticationResult.Success;
                }
                else
                    result = AuthenticationResult.InvalidPassword;
            }

            //Populate output parameter 
            userProfile = null;
            if (result == AuthenticationResult.Success)
            {
                userProfile = new PxPortalUserProfile()
                {
                    email = user.Email,                    
                    firstName = user.UserName,
                    id = user.Id,
                    roles = user.Roles
                    //(from role in user.Roles
                    //         select role.RoleName).ToList()
                };
            }

            return result;
        }

        public ChangePasswordResult ChangeUserPassword(string username, string oldpassword, string newpassword, EmailObject eo)
        {
            throw new NotImplementedException();
        }

        public RegisterNewUserResult CreateNewUser(string username, string password, int keyStretchIterations, int saltSize, int passwordKeySize, List<string> role, string accountName, string userFirstName = null, string userLastName = null)
        {
            throw new NotImplementedException();
        }

        public PxPortalUserProfile GetUserProfile(string userName)
        {
            Debug.Assert(userName != null);
            InMemoryUserDefinition user = InMemoryUsersList.Where(u => u.Email == userName).FirstOrDefault();
            if (user == null)
                throw new Exception("User not found");
            var userProfile = new PxPortalUserProfile()
            {
                email = user.Email,
                firstName = user.UserName,
                id = user.Id,
                roles = user.Roles
            };
            return userProfile;
        }

        public List<string> ListRoles()
        {
            throw new NotImplementedException();
        }

        public void PasswordResetByAdmin(string emailaddress)
        {
            throw new NotImplementedException();
        }

        public void PasswordResetByUser(string emailaddress)
        {
            throw new NotImplementedException();
        }

        public void RegisterNewUser(string emailaddress, string password)
        {
            throw new NotImplementedException();
        }

        public RemoveUserResult RemoveUser(string username)
        {
            throw new NotImplementedException();
        }

        public RemoveUserResult RemoveUser(string username, bool force)
        {
            throw new NotImplementedException();
        }

        public bool ResetUserPassword(string username, EmailObject eo)
        {
            throw new NotImplementedException();
        }

        public PxPortalUserProfile UpdateUserProfile(string username, string firstName, string lastName, bool isEmailAuthorized)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserAccessFilter> GetUserAccessFilters(string username)
        {
            throw new NotImplementedException();
        }

        public bool UpgradeUserSecurityIfNeeded(string username, string password, int targetIterations, int targetSaltSize, int targetKeySize)
        {
            return false;
        }
    }
}
