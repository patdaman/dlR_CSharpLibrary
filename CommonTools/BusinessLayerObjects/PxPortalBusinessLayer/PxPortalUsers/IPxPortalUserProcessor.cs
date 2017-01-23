using CommonUtils.Email;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PxPortal
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent authentication results. </summary>
    ///
    /// <remarks>   Dtorres, 20160515. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum AuthenticationResult
    {
        /// <summary>   User authenticated successfully. </summary>
        Success,
        /// <summary>   User does not exist. </summary>
        InvalidUserName,
        /// <summary>   User did not authenticate. Invalid password. </summary>
        InvalidPassword,
        /// <summary>   User authenticated but password reset is required. </summary>
        PasswordResetRequired,
        /// <summary>   An enum constant representing the unknown error option. </summary>
        UnknownError
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent change password results. </summary>
    ///
    /// <remarks>   Dtorres, 20160516. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum ChangePasswordResult
    {
        /// <summary>   An enum constant representing the success option. </summary>
        Success,

        /// <summary>   An enum constant representing the invalid user name option. </summary>
        InvalidUserName,

        /// <summary>   An enum constant representing the invalid password option. </summary>
        InvalidPassword,

        /// <summary>   An enum constant representing the password complexity failed option. </summary>
        NewPasswordComplexityFailed,

        /// <summary>   An enum constant representing the password history failed option. </summary>
        NewPasswordHistoryFailed,

        /// <summary>   An enum constant representing the unknown error option. </summary>
        UnknownError
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent register new user results. </summary>
    ///
    /// <remarks>   Dtorres, 20160516. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum RegisterNewUserResult
    {
        /// <summary>   An enum constant representing the success option. </summary>
        Success,

        /// <summary>   An enum constant representing the user already exists option. </summary>
        UserAlreadyExists,

        /// <summary>   An enum constant representing the new password complexity failed option. </summary>
        NewPasswordComplexityFailed,
        /// <summary>   An enum constant representing the unknown error option. </summary>
        UnknownError
    }

    public enum RemoveUserResult
    {
        /// <summary>   An enum constant representing the success option. </summary>
        Success,

        /// <summary>   An enum constant representing the user does not exist option. </summary>
        UserDoesNotExist,
        
        /// <summary>   An enum constant representing the unknown error option. </summary>
        UnknownError
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for user manager. </summary>
    ///
    /// <remarks>   Dtorres, 20160516. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IPxPortalUserProcessor
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Authenticate user.  </summary>
        ///
        /// <param name="userName">     Name of the user. </param>
        /// <param name="password">     The password. </param>
        /// <param name="userProfile">  Output parameter: If user authenticates then the user's profile is populated.</param>
        ///
        /// <returns>   An AuthenticationResult. </returns>
        ///-------------------------------------------------------------------------------------------------

        AuthenticationResult AuthenticateUser(string username, string password, out PxPortalUserProfile userProfile);

        PxPortalUserProfile GetUserProfile(string username);

        RegisterNewUserResult CreateNewUser(string username, string password, int keyStretchIterations, int saltSize, int passwordKeySize, List<string> roles=null, string accountName=null, string userFirstName = null, string userLastName = null);

        ChangePasswordResult ChangeUserPassword(string username, string oldpassword, string newpassword, EmailObject eo);

        RemoveUserResult RemoveUser(string username);

        RemoveUserResult RemoveUser(string username, bool force);

        IEnumerable<UserAccessFilter> GetUserAccessFilters(string username);

        bool ResetUserPassword(string username, EmailObject eo);


        PxPortalUserProfile UpdateUserProfile(string username, string firstName, string lastName, bool isEmailAuthorized);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Upgrade user security. </summary>
        ///
        /// <param name="username">         The username. </param>
        /// <param name="targetIterations"> Target iterations. </param>
        /// <param name="targetSaltSize">   Size of the target salt. </param>
        /// <param name="targetKeySize">    Size of the target key. </param>
        ///
        /// <returns>   true if security needs to be upgraded, false otherwise. </returns>
        ///-------------------------------------------------------------------------------------------------

        bool UpgradeUserSecurityIfNeeded(string username, string password, int targetIterations, int targetSaltSize, int targetKeySize);
        List<string> ListRoles();
    }
}
