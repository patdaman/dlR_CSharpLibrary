using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PxPortal
{
    public class InMemoryUserDefinition
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string PasswordHash { get; set; }
        public List<string> Roles { get; set; }

        public InMemoryUserDefinition(string id, string email, string name, string pass, List<string> roles)
        {
            if (ValidationUtils.IsValidEmail(email))
            {
                Id = id;
                UserName = name;
                Email = email;
                Password = pass;
                Roles = roles;
            }
        }
    }
}
