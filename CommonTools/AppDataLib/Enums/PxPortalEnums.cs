using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDataLib.Enums
{
    public enum PxPortalCaseStatus
    {
        Unknown,            //This is for development, Unknown should never really occur.
        WaitingForSpecimen,
        SpecimenReceived,
        Processing,
        InReview,
        ResultsReady,
        Cancelled
    }

    public enum PxPortalUserRoles
    {
        GlobalAdmin = 0,
        GlobalReader = 1,
        SalesRep = 2,
        AccountAdmin = 3,
        User = 4,
        Inactive = 5
    }

    public enum PxPortalUserTypes
    {
        GlobalAdmin,
        GlobalReader,
        HipaaDataReader,
        NoHipaaData
    }
}
