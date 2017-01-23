using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{
    public class YearQuarter : YearNumberedPeriodItem<YearQuarter>
    {
        public YearQuarter()
        {
            Periodicity = PeriodicityType.Quarterly;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the CommonUtils.DateUtils.YearQuarter class from the year and quarter
        /// </summary>
        /// <param name="year">     The year. </param>
        /// <param name="quarter">  The quarter. Should be between 1 and 4 for valid result</param>
        ///-------------------------------------------------------------------------------------------------

        public YearQuarter(int yearp, int quarter)
        {
            Periodicity = PeriodicityType.Quarterly;
            if (yearp < 0) return;
            int year = yearp;
            if (yearp.ToString().Length < 4)
                year = 2000 + yearp;
            if (0 < quarter && quarter < 5)
            {
                Year = year;
                PeriodNumber = quarter;
                makeBegEndDate();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the CommonUtils.DateUtils.YearQuarter class from a datetime
        /// object. Returns the quarter containing the provided date.
        /// </summary>
        /// <param name="dt">   The dt Date/Time. </param>
        ///-------------------------------------------------------------------------------------------------

        public YearQuarter(DateTime? dt)
        {
            Periodicity = PeriodicityType.Quarterly;
            if (dt.HasValue)
            {
                try
                {
                    Year = dt.Value.Year;
                    PeriodNumber = (int)(Math.Floor((float)(dt.Value.Month - 1) / 3.0) + 1.0);
                    makeBegEndDate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private void makeBegEndDate()
        {
            BeginDate = GetQuarterFirstDate(Year, PeriodNumber);
            EndDate = BeginDate.AddMonths(3).AddDays(-1);
        }

        public static DateTime GetQuarterFirstDate(int year, int quarter)
        {
            return new DateTime(year, ((quarter - 1) * 3 + 1), 1);
        }

        public override string ToShortString()
        {
            return Year.ToString().Substring(Math.Max(0, Year.ToString().Length - 2)) + "Q" + PeriodNumber;
        }

        public override string ToLongString()
        {
            return Year.ToString() + "-Qtr" + PeriodNumber;
        }

        public static List<YearQuarter> GetPeriodsCovering(DateTime fromdate, DateTime todate)
        {
            List<YearQuarter> ywl = new List<YearQuarter>();
            DateTime lo = fromdate.Date;
            DateTime hi = todate.Date;
            int comp = DateTime.Compare(fromdate, todate);

            if (comp == 0)
            {
                ywl.Add(new YearQuarter(lo));
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
                ywl.Add(new YearQuarter(curwf));
                if (DateTime.Compare(hi, curwf) <= 0)
                    done = true;
                else
                    curwf = curwf.AddMonths(3);
            }

            return ywl;
        }

        public override YearQuarter NextPeriodItem()
        {
            return AddPeriod(1);
        }

        public override YearQuarter AddPeriod(int nperiods)
        {
            return new YearQuarter(BeginDate.AddMonths(3 * nperiods));
        }

        public override YearQuarter AddYear(int nyears)
        {
            return new YearQuarter(BeginDate.AddYears(nyears));
        }

        public override int PeriodsBetween(IYearNumberedPeriodItem other)
        {
            if (other.Periodicity != PeriodicityType.Quarterly)
                return int.MaxValue;
            int nyrs = other.Year - Year;
            int nqtrs = other.PeriodNumber - PeriodNumber;

            return nyrs * 4 + nqtrs;
        }
    }

}
