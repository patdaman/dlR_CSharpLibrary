using System;
using System.Runtime.InteropServices;
using SysS = System.Security;



namespace CommonUtils.SecureString
{
    [SysS.SuppressUnmanagedCodeSecurity]
    public static class SecureStringHelper
    {
        // Methods
        public static unsafe SysS.SecureString CreateSecureString(string plainString)
        {
            SysS.SecureString str;
            if (string.IsNullOrEmpty(plainString))
            {
                return new SysS.SecureString();
            }
            fixed (char* str2 = plainString)
            {
                char* chPtr = str2;
                str = new SysS.SecureString(chPtr, plainString.Length);
                str.MakeReadOnly();
            }
            return str;
        }

        public static string CreateString(SysS.SecureString secureString)
        {
            string str;
            IntPtr zero = IntPtr.Zero;
            if ((secureString == null) || (secureString.Length == 0))
            {
                return string.Empty;
            }
            try
            {
                zero = Marshal.SecureStringToBSTR(secureString);
                str = Marshal.PtrToStringBSTR(zero);
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(zero);
                }
            }
            return str;
        }
    }
}
