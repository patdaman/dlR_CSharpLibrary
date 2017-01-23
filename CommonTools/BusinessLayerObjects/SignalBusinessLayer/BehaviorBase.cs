using SignalEFDataModel;
using SignalEFDataModel.SGNL_ANALYTICS;
using SignalEFDataModel.SGNL_FINANCE;
using SignalEFDataModel.SGNL_INTERNAL;
using SignalEFDataModel.SGNL_LIS;
using SignalEFDataModel.SGNL_WAREHOUSE;
using SignalEFDataModel.SGNL_PXPORTAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BusinessLayer
{
    public class BehaviorBase
    {
        public SignalEFDataModel.SGNL_WAREHOUSE.SGNL_WAREHOUSEEntities WarehouseDbContext { get; set; }
        public SignalEFDataModel.SGNL_LIS.SGNL_LISEntities LISDbContext { get; set; }
        public SignalEFDataModel.SGNL_INTERNAL.SGNL_INTERNALEntities InternalDbContext { get; set; }
        public SignalEFDataModel.SGNL_FINANCE.SGNL_FINANCEEntities FinanceDbContext { get; set; }
        public SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities AnalyticsDbContext { get; set; }

        public SignalEFDataModel.SGNL_PXPORTAL.SGNL_PXPORTALEntities PxPortalDbContext { get; set; }
                
        public Dictionary<String, DbContext> DBNameToEntities;

        #region Constructors
        public BehaviorBase()
        {
            WarehouseDbContext = new SGNL_WAREHOUSEEntities();
            LISDbContext = new SGNL_LISEntities();
            InternalDbContext = new SGNL_INTERNALEntities();
            FinanceDbContext = new SGNL_FINANCEEntities();
            AnalyticsDbContext = new SGNL_ANALYTICSEntities();
            PxPortalDbContext = new SGNL_PXPORTALEntities();

            DBNameToEntities = new Dictionary<string, DbContext>();            
            DBNameToEntities.Add(DBNames.SGNL_Warehouse, WarehouseDbContext);
            DBNameToEntities.Add(DBNames.SGNL_LIS, LISDbContext);
            DBNameToEntities.Add(DBNames.SGNL_Internal, InternalDbContext);
            DBNameToEntities.Add(DBNames.SGNL_Finance, FinanceDbContext);
            DBNameToEntities.Add(DBNames.SGNL_Analytics, AnalyticsDbContext);
            DBNameToEntities.Add(DBNames.SGNL_PxPortal, PxPortalDbContext);

#if Release
#else
            WarehouseDbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            LISDbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            InternalDbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }


        public DbContext GetDbContext(string dbname)
        {
            switch (dbname.ToLower())
            {
                case "sgnl_warehouse":
                    return DBNameToEntities[DBNames.SGNL_Warehouse];
                case "sgnl_lis":
                    return DBNameToEntities[DBNames.SGNL_LIS];
                case "sgnl_internal":
                    return DBNameToEntities[DBNames.SGNL_Internal];
                case "sgnl_finance":
                    return DBNameToEntities[DBNames.SGNL_Finance];
                case "sgnl_analytics":
                    return DBNameToEntities[DBNames.SGNL_Analytics];
                case "sgnl_pxportal":
                    return DBNameToEntities[DBNames.SGNL_PxPortal];
                default:
                    throw new Exception("Unknown database name");
            }

        }
        #endregion

    }
}
