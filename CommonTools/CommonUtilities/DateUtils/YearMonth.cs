///-------------------------------------------------------------------------------------------------
// <date>20160224</date>
// <summary>Implements the year month class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A year month. </summary>
    ///-------------------------------------------------------------------------------------------------

    public class YearMonth : YearNumberedPeriodItem<YearMonth>
    {

        public YearMonth()
        {
            Periodicity = PeriodicityType.Monthly;
        }

        public YearMonth(DateTime? dt)
        {
            Periodicity = PeriodicityType.Monthly;
            if (dt.HasValue)
            {
                try
                {
                    Year = dt.Value.Year;
                    PeriodNumber = dt.Value.Month;
                    makeBegEndDate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        public YearMonth(int yearp, int month)
        {
            Periodicity = PeriodicityType.Monthly;
            if (yearp < 0) return;
            int year = yearp;
            if (yearp.ToString().Length < 4)
                year = 2000 + yearp;
            if (0 < month && month < 13)
            {
                Year = year;
                PeriodNumber = month;
                makeBegEndDate();
            }
        }

        private void makeBegEndDate()
        {
            BeginDate = new DateTime(Year, PeriodNumber, 1);
            EndDate = BeginDate.AddMonths(1).AddDays(-1);
        }
        public override string ToLongString()
        {
            return Year.ToString() + "-Mon" + PeriodNumber;
        }

        public override string ToShortString()
        {
            return Year.ToString().Substring(Math.Max(0, Year.ToString().Length - 2)) + "M" + PeriodNumber;
        }

        public static List<YearMonth> GetPeriodsCovering(DateTime fromdate, DateTime todate)
        {
            List<YearMonth> ywl = new List<YearMonth>();
            DateTime lo = fromdate.Date;
            DateTime hi = todate.Date;
            int comp = DateTime.Compare(fromdate, todate);

            if (comp == 0)
            {
                ywl.Add(new YearMonth(lo));
                //ywl.Add(new YearMonth(lo.AddMonths(1)));
                return ywl;
            }

            if (comp > 0)
            {
                lo = todate.Date;
                hi = fromdate.Date;
            }

            DateTime curwf = lo;
            bool done = false;
            while (!done)
            {
                ywl.Add(new YearMonth(curwf));
                if (DateTime.Compare(hi, curwf) <= 0)
                    done = true;
                else
                    curwf = curwf.AddMonths(1);
            }

            return ywl;
        }

        public override YearMonth NextPeriodItem()
        {
            return AddPeriod(1);
        }

        public override YearMonth AddPeriod(int nperiods)
        {
            return new YearMonth(BeginDate.AddMonths(nperiods));
        }

        public override YearMonth AddYear(int nyears)
        {
            return new YearMonth(BeginDate.AddYears(nyears));
        }

        public override int PeriodsBetween(IYearNumberedPeriodItem other)
        {
            if (other.Periodicity != PeriodicityType.Monthly)
                return int.MaxValue;
            int nyrs = other.Year - Year;
            int nmon = other.PeriodNumber - PeriodNumber;

            return nyrs * 12 + nmon;

        }
    }
}
