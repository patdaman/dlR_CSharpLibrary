using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PxPortal
{
    public class UserAccessFilter
    {
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public int PhysicianId { get; set; }
        public string PhysicianFullName { get; set; }
        public int TerritoryId { get; set; }
        public string TerritoryName { get; set; }
        public int SalesRepId { get; set; }
        public string SalesRepName { get; set; }
    }
}
