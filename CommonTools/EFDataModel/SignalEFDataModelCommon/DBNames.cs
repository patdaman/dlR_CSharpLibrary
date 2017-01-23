using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SignalEFDataModel.SGNL_WAREHOUSE;
using SignalEFDataModel.SGNL_ANALYTICS;
using SignalEFDataModel.SGNL_FINANCE;
using SignalEFDataModel.SGNL_INTERNAL;
using SignalEFDataModel.SGNL_LIS;

namespace SignalEFDataModel
{
    public class DBNames
    {
        public static string SGNL_Warehouse = "SGNL_Warehouse";
        public static string SGNL_LIS = "SGNL_LIS";
        public static string SGNL_Internal = "SGNL_Internal";
        public static string SGNL_Finance = "SGNL_Finance";
        public static string SGNL_Analytics = "SGNL_ANALYTICS";
        public static string SGNL_AnalyticsProcessLog = "SGNL_AnalyticsProcessLog";
        public static string SGNL_PxPortal = "SGNL_PxPortal";
    }


    public class DBNamespace
    {
        public static string SGNL_Warehouse = "SignalEFDataModel.SGNL_WAREHOUSE";
        public static string SGNL_LIS = "SignalEFDataModel.SGNL_LIS";
        public static string SGNL_Internal = "SignalEFDataModel.SGNL_INTERNAL";
        public static string SGNL_Finance = "SignalEFDataModel.SGNL_FINANCE";
        public static string SGNL_Analytics = "SignalEFDataModel.SGNL_ANALYTICS";
        public static string SGNL_PxPortal = "SignalEFDataModel.SGNL_PXPORTAL";

    };

    public class DBAssemblyName
    {
        public static string SGNL_Warehouse = "SignalEFDataModel.SGNL_WAREHOUSE";
        public static string SGNL_LIS = "SignalEFDataModel.SGNL_LIS";
        public static string SGNL_Internal = "SignalEFDataModel.SGNL_INTERNAL";
        public static string SGNL_Finance = "SignalEFDataModel.SGNL_FINANCE";
        public static string SGNL_Analytics = "SignalEFDataModel.SGNL_ANALYTICS";
        public static string SGNL_PxPortal = "SignalEFDataModel.SGNL_PXPORTAL";
    };

    public static class DatabaseTypeInfo
    {
        public static string GetNamespaceName(string dbname)
        {
            Debug.Assert(dbname != null);
            string name = null;
            switch (dbname.ToLower())
            {
                case "sgnl_warehouse":
                    name = DBNamespace.SGNL_Warehouse;
                    break;
                case "sgnl_lis":
                    name = DBNamespace.SGNL_LIS;
                    break;
                case "sgnl_internal":
                    name = DBNamespace.SGNL_Internal;
                    break;
                case "sgnl_finance":
                    name = DBNamespace.SGNL_Finance;
                    break;
                case "sgnl_analytics":
                    name = DBNamespace.SGNL_Analytics;
                    break;
                case "sgnl_pxportal":
                    name = DBNamespace.SGNL_PxPortal;
                    break;
                default:
                    throw new Exception("Unknown database name");
            }
            return name;
        }


        public static string GetAssemblyName(string dbname)
        {
            Debug.Assert(dbname != null);
            string name = null;
            switch (dbname.ToLower())
            {
                case "sgnl_warehouse":
                    name = DBAssemblyName.SGNL_Warehouse;
                    break;
                case "sgnl_lis":
                    name = DBAssemblyName.SGNL_LIS;
                    break;
                case "sgnl_internal":
                    name = DBAssemblyName.SGNL_Internal;
                    break;
                case "sgnl_finance":
                    name = DBAssemblyName.SGNL_Finance;
                    break;
                case "sgnl_analytics":
                    name = DBAssemblyName.SGNL_Analytics;
                    break;
                case "sgnl_pxportal":
                    name = DBAssemblyName.SGNL_PxPortal;
                    break;
                default:
                    throw new Exception("Unknown database name");
            }
            return name;
        }        
    }
    

}
