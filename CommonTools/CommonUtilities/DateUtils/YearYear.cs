///-------------------------------------------------------------------------------------------------
// <date>20160224</date>
// <summary>Implements the year class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{
    public class YearYear : YearNumberedPeriodItem<YearYear>
    {

        public YearYear()
        {
            Periodicity = PeriodicityType.Yearly;
        }

        private void makeBegEndDate()
        {
            BeginDate = new DateTime(Year, 1, 1);
            EndDate = BeginDate.AddYears(1).AddDays(-1);
        }
        public YearYear(DateTime? dt)
        {
            Periodicity = PeriodicityType.Yearly;
            if (dt.HasValue)
            {
                try
                {
                    Year = dt.Value.Year;
                    PeriodNumber = 1;
                    makeBegEndDate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public YearYear(int yearp)
        {
            Periodicity = PeriodicityType.Yearly;
            if (yearp < 0) return;
            int year = yearp;
            if (yearp.ToString().Length < 4)
                year = 2000 + yearp;

            Year = year;
            PeriodNumber = 1;
            makeBegEndDate();
        }

        public override YearYear AddPeriod(int nperiods)
        {
            return new YearYear(BeginDate.AddYears(nperiods));
        }

        public override YearYear AddYear(int nyears)
        {
            return AddPeriod(nyears);
        }

        public override YearYear NextPeriodItem()
        {
            return AddPeriod(1);
        }

        public override int PeriodsBetween(IYearNumberedPeriodItem other)
        {
            if (other.Periodicity != PeriodicityType.Yearly)
                return int.MaxValue;
            return other.Year - Year;
        }

        public override string ToLongString()
        {
            return Year.ToString();
        }

        public override string ToShortString()
        {
            return Year.ToString();
        }

        public static List<YearYear> GetPeriodsCovering(DateTime fromdate, DateTime todate)
        {
            List<YearYear> ywl = new List<YearYear>();
            DateTime lo = fromdate.Date;
            DateTime hi = todate.Date;
            int comp = DateTime.Compare(fromdate, todate);

            if (comp == 0)
            {
                ywl.Add(new YearYear(lo));
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
                ywl.Add(new YearYear(curwf));
                if (DateTime.Compare(hi, curwf) <= 0)
                    done = true;
                else
                    curwf = curwf.AddYears(1);
            }
            return ywl;
        }
    }
}
