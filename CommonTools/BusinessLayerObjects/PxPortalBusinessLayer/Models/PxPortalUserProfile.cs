using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PxPortal
{
    public class PxPortalUserProfile
    {
        public string id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public bool isEmailAuthorizedForResults { get; set; }

        public string email { get; set; }
        public List<string> roles { get; set; }

        public string highestRole { get; set; }
    }
}
