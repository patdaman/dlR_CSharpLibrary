///-------------------------------------------------------------------------------------------------
// <date>20160216</date>
// <summary>Declares the IYearNumberedPeriodItem interface</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{
    public interface IYearNumberedPeriodItem : IEquatable<IYearNumberedPeriodItem>, IComparable<IYearNumberedPeriodItem>
    {
        PeriodicityType Periodicity { get; set; }
        int Year { get; set; }
        int PeriodNumber { get; set; }
        DateTime BeginDate { get; set; }
        DateTime EndDate { get; set; }

        string ToShortString();
        string ToLongString();

        string ToFormattedString(string fmt);

        int PeriodsBetween(IYearNumberedPeriodItem other);

        IYearNumberedPeriodItem GetNextPeriodItem();
        IYearNumberedPeriodItem IncrementByPeriods(int nperiods);
        IYearNumberedPeriodItem IncrementByYears(int nyears);

    }
}
