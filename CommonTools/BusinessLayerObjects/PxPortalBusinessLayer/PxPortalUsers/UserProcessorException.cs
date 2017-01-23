using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PxPortal
{
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found.") { }
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected UserNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class PasswordDoesNotMatchException : Exception
    {
        public PasswordDoesNotMatchException() : base("User password does not match.") { }
        public PasswordDoesNotMatchException(string message) : base(message) { }
        public PasswordDoesNotMatchException(string message, Exception inner) : base(message, inner) { }
        protected PasswordDoesNotMatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
