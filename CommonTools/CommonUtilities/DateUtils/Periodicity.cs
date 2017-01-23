using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent periodicity types. </summary>
    ///-------------------------------------------------------------------------------------------------

    public enum PeriodicityType
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent periodicity reference types. </summary>
    ///-------------------------------------------------------------------------------------------------

    public enum PeriodicityRefType
    {
        ToDate, // last period to date
        Calendar, // by calendar 
        RefDate // by reference last date
    }

  

    
}
