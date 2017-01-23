using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Configuration
    {
        public string ApiBuildTag { set; get; }
        public string AppBuildTag { set; get; }
        public string ApiAssemblyVersion { set; get; }
        public string AppAssemblyVersion { set; get; }
        public string ApiServer { get; set; }
        public string AppServer { get; set; }
        public string DB_LIS { get; set; }
        public string DB_Warehouse { get; set; }
        public string DB_Internal { get; set; }
        public string DB_Finance { get; set; }
        public string ServerTime { get; set; }
       
    }
}
