using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Crypto
{
    public class HashResult
    {
        public String Hash;
        public String Salt;
    }


    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Uses PBKDF key stretching algorithm to generate a hash + salt for passwords. Makes use 
    ///             of microsoft's Rfc2898 algorithm that implements PBKDF2. 
    ///             See https://www.owasp.org/index.php/Using_Rfc2898DeriveBytes_for_PBKDF2https://www.owasp.org/index.php/Using_Rfc2898DeriveBytes_for_PBKDF2
    ///             
    ///               </summary>
    /// 
    /// <usage> Initialize </usage>
    ///
    /// <remarks>   20160516. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class PBKDFHasher
    {

        /// <summary>   The default key length. 20 Seems to be what everyone says to use. Greater than this 
        ///             gives you diminishing returns given hashing algorithm.  </summary>
        private const int DefaultKeyLength = 20;
        /// <summary>   The default salt size. Salt size should be ~= key length  </summary>
        private const int DefaultSaltSize = 32;
        /// <summary>   Iteration count should be high enough to cause a computational "lag". This should be reviewed each year.  </summary>
        private const int DefaultIterationCount = 10000;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the salt size in bytes. </summary>
        ///
        /// <value> The salt size bytes. </value>
        ///-------------------------------------------------------------------------------------------------

        public int SaltSizeBytes { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the number of iterations for PBKDF2. </summary>
        ///
        /// <value> The number of iterations. </value>
        ///-------------------------------------------------------------------------------------------------

        public int IterationCount { get; private set; }

        public int KeyLength { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the CommonUtils.Crypto.PBKDFHasher class.
        /// </summary>
        ///
        /// <remarks>   20160516. </remarks>
        ///
        /// <param name="saltSizeBytes">    The salt size bytes. </param>
        /// <param name="iterationCount">   The number of iterations. </param>
        ///-------------------------------------------------------------------------------------------------

        public PBKDFHasher(int iterationCount = DefaultIterationCount, int saltSizeBytes = DefaultSaltSize, int keyLength = DefaultKeyLength)
        {
            this.SaltSizeBytes = saltSizeBytes;
            this.IterationCount = iterationCount;
            this.KeyLength = keyLength;
        }


        public HashResult ComputeHash(string password)
        {
            Debug.Assert(password != null);
            if (password == null)
                throw new ArgumentNullException();

            
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSizeBytes, IterationCount);            
            byte[] hash = rfc2898DeriveBytes.GetBytes(this.KeyLength);
            byte[] salt = rfc2898DeriveBytes.Salt;

            var result = new HashResult()
            {
                Hash = Convert.ToBase64String(hash),
                Salt = Convert.ToBase64String(salt)
            };
            return result;
        }

        public bool CheckPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltbytes = Convert.FromBase64String(storedSalt);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltbytes, this.IterationCount);            
            string passwordHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(DefaultKeyLength));
            return passwordHash == storedHash;
        }

        public string GenerateRandomPassword( int passwordLength )
        {
            return System.Web.Security.Membership.GeneratePassword(passwordLength, 0);
        }
    }
}
