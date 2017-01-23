// <copyright file="PxPortalUserCaseProcessor.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160909</date>
// <summary>Implements the px portal user case processor class for linking cases to users and 
// checking user access to cases</summary>

using CommonUtils.Logging;
///-------------------------------------------------------------------------------------------------
///-------------------------------------------------------------------------------------------------
using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using BusinessLayer.PxPortal;
using SignalEFDataModel.SGNL_PXPORTAL;
using System.Collections;
using AppDataLib.Enums;
using ViewModel;

namespace BusinessLayer
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   UserCase Processor tell you what permissions users have to what cases. </summary>
    ///
    /// <remarks>   Dtorres, 20160916. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class PxPortalUserCaseProcessor : BehaviorBase
    {
        /// <summary>   The logger. </summary>
        public static LoggingPatternUser logger = new LoggingPatternUser() { Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType) };

        private PxPortalUserProcessor UserProcessor { get; set; }

        public PxPortalUserCaseProcessor() : base()
        {
            Init();
        }

        public PxPortalUserCaseProcessor(string analyticsConnString, string sgnlLisConnString, string pxPortalConnString, string warehouseConnString) : base()
        {
            AnalyticsDbContext = new SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities(analyticsConnString);
            LISDbContext = new SignalEFDataModel.SGNL_LIS.SGNL_LISEntities(sgnlLisConnString);
            PxPortalDbContext = new SignalEFDataModel.SGNL_PXPORTAL.SGNL_PXPORTALEntities(pxPortalConnString);
            WarehouseDbContext = new SignalEFDataModel.SGNL_WAREHOUSE.SGNL_WAREHOUSEEntities(warehouseConnString);
            Init();
        }

        public PxPortalUserCaseProcessor(SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities anaDbContext,
                                    SignalEFDataModel.SGNL_LIS.SGNL_LISEntities lisDbContext,
                                    SignalEFDataModel.SGNL_PXPORTAL.SGNL_PXPORTALEntities pxPortalContext,
                                    SignalEFDataModel.SGNL_WAREHOUSE.SGNL_WAREHOUSEEntities warehouseDbContext) : base()
        {
            AnalyticsDbContext = anaDbContext;
            LISDbContext = lisDbContext;
            PxPortalDbContext = pxPortalContext;
            WarehouseDbContext = warehouseDbContext;
            Init();

        }


        private void Init()
        {
            UserProcessor = new PxPortalUserProcessor(PxPortalDbContext);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets access filters. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161006. </remarks>
        ///
        /// <param name="username"> The username. </param>
        ///
        /// <returns>   The access filters. </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<AccessFilter> GetAccessFilters(string username)
        {
            var user = GetQueryableUser(username);
            var accFilters = user
                             .SelectMany(many => many.AccessMaps)
                             .Where(f => f.AccessFilter.IsActive == true)
                             .Select(f => new
                             {
                                 ClientId = f.AccessFilter.ClientId,
                                 PhysicianId = f.AccessFilter.PhysicianId,
                                 SalesRepId = f.AccessFilter.SalesRepId,
                             }).Distinct().ToList();
            List<AccessFilter> efAccessFilters = new List<AccessFilter>();
            foreach (var filter in accFilters)
            {
                efAccessFilters.Add(new AccessFilter()
                {
                    ClientId = filter.ClientId,
                    PhysicianId = filter.PhysicianId,
                    SalesRepId = filter.SalesRepId
                });
            }
            return efAccessFilters;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets queryable user. </summary>
        ///
        /// <remarks>   Ssur, 20161003. </remarks>
        ///
        /// <param name="username"> The username. </param>
        ///
        /// <returns>   The queryable user. </returns>
        ///-------------------------------------------------------------------------------------------------

        private IQueryable<User> GetQueryableUser(string username)
        {
            return (from user in PxPortalDbContext.Users
                         where user.UserName.ToLower().Trim() == username.ToLower().Trim()
                         where user.IsActive == true
                         select user);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets user cases. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160915. </remarks>
        ///
        /// <param name="username">     The username. </param>
        /// <param name="startDate">    The start date. </param>
        /// <param name="endDate">      The end date. </param>
        ///
        /// <returns>   The user cases. </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<string> GetUserCases(string username, DateTime startDate, DateTime endDate)
        {
            var userTypeName = (from user in PxPortalDbContext.Users
                            where user.UserName == username
                            select user.UserType.Name
                            ).FirstOrDefault();

            var accessFilters = GetAccessFilters(username);
            PxPortalUserTypes userTypeEnum = new PxPortalUserTypes();
            Enum.TryParse(userTypeName, out userTypeEnum);
            switch (userTypeEnum)
            {
                case PxPortalUserTypes.GlobalAdmin:
                    return GetHipaaDataCases(accessFilters, startDate, endDate);
                case PxPortalUserTypes.GlobalReader:
                    return GetHipaaDataCases(accessFilters, startDate, endDate);
                case PxPortalUserTypes.HipaaDataReader:
                    return GetHipaaDataCases(accessFilters, startDate, endDate);
                case PxPortalUserTypes.NoHipaaData:
                    return GetNoHipaaDataCases(accessFilters, startDate, endDate);
                default:
                    throw new UnauthorizedAccessException(string.Format("Type not found for user {0}, access denied.", username));
            };
            throw new UnauthorizedAccessException(string.Format("Type not found for user {0}, access denied.", username));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets no hipaa data cases. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161014. </remarks>
        ///
        /// <param name="accFilters">   The accumulate filters. </param>
        /// <param name="startDate">    The start date. </param>
        /// <param name="endDate">      The end date. </param>
        ///
        /// <returns>   The no hipaa data cases. </returns>
        ///-------------------------------------------------------------------------------------------------

        private List<string> GetNoHipaaDataCases(List<AccessFilter> accFilters, DateTime startDate, DateTime endDate)
        {
            // todo
            //throw new NotImplementedException();
            return GetHipaaDataCases(accFilters, startDate, endDate);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets hipaa data cases. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20161014. </remarks>
        ///
        /// <param name="accessFilters">    The access filters. </param>
        /// <param name="startDate">        The start date. </param>
        /// <param name="endDate">          The end date. </param>
        ///
        /// <returns>   The hipaa data cases. </returns>
        ///-------------------------------------------------------------------------------------------------

        private List<string> GetHipaaDataCases(List<AccessFilter> accessFilters, DateTime startDate, DateTime endDate)
        {
            string wildcard = "-888";
            string separator = "_";
            string globalAdmin = wildcard + separator + wildcard + separator + wildcard;
            string noDoc = wildcard;
            string noClient = separator + wildcard;
            string noSalesRep = wildcard + separator;

            List<string> accounts = (from acp in accessFilters
                                     select (acp.SalesRepId.ToString() + separator + acp.PhysicianId.ToString() + separator + acp.ClientId.ToString())
                        ).Distinct().ToList();



            List<string> cases = WarehouseDbContext.vi_PxPortalUserCases
                         .Where(a =>
                            // Match all 3 columns in Access Filter:
                            accounts.Contains(a.SalesRepId.ToString() + separator + a.DoctorId.ToString() + separator + a.ClientId.ToString())
                            // Match SalesRep, wildcards for DoctorId and ClientId:
                            || accounts.Contains(a.SalesRepId.ToString() + separator + noDoc + noClient)
                            // Match SalesRep and ClientId, wildcard DoctorId:
                            || accounts.Contains(a.SalesRepId.ToString() + separator + noDoc + separator + a.ClientId.ToString())
                            // Match SalesRep and DoctorId, wildcard ClientId:
                            || accounts.Contains(a.SalesRepId.ToString() + separator + a.DoctorId + noClient) 
                            // No SalesRep, match DoctorId and ClientId:
                            || accounts.Contains(noSalesRep + a.DoctorId.ToString() + separator + a.ClientId.ToString())
                            // No SalesRepId, wildcard doctor, match ClientId:
                            || accounts.Contains(noSalesRep + noDoc + separator + a.ClientId.ToString())
                            // All three columns are wildcards:
                            || accounts.Contains(globalAdmin))
                         .Where(uc => uc.OrderDate >= startDate && uc.OrderDate < endDate)
                         .OrderBy(uc => uc.CaseNumber)
                         .Select(uc => uc.CaseNumber).Distinct().ToList();
            return cases;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   User can access case. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160916. </remarks>
        ///
        /// <param name="username">     The username. </param>
        /// <param name="caseNumber">   The case number. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool UserCanAccessCase(string username, string caseNumber)
        {
            var roles = (from user in PxPortalDbContext.Users
                         where user.UserName == username
                         select user.Roles).FirstOrDefault().ToList();
            var roleNames = (from role in roles
                             select role.RoleName).ToList();
            return UserCanAccessCase(username, caseNumber, roleNames);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   User can access case. </summary>
        ///
        /// <remarks>   Ssur, 20161003. </remarks>
        ///
        /// <exception cref="UnauthorizedAccessException">  Thrown when an Unauthorized Access error
        ///                                                 condition occurs. </exception>
        ///
        /// <param name="username">     The username. </param>
        /// <param name="caseNumber">   The case number. </param>
        /// <param name="roleNames">    List of names of the roles. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool UserCanAccessCase(string username, string caseNumber, List<string> roleNames)
        {
            if (roleNames == null || roleNames.Count() == 0)
            {
                throw new UnauthorizedAccessException("Role was null");
            }
            else
            {
                foreach (var role in roleNames)
                {
                    switch (role)
                    {
                        case "GlobalAdmin":
                            return true;
                        case "GlobalReader":
                            return true;
                        case "SalesRep":
                            return true;
                        case "AccountAdmin":
                            return true;
                        case "User":
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
