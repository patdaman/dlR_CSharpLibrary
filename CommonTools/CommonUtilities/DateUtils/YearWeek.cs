using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A year week. Week number in a year. </summary>
    ///-------------------------------------------------------------------------------------------------

    public class YearWeek : YearNumberedPeriodItem<YearWeek>
    {

        public CalendarWeekRule CalendarWeekRule
        {
            get; set;
        }

        public CultureInfo Culture { get; set; }

        public YearWeek() { Periodicity = PeriodicityType.Weekly; }
        public YearWeek(DateTime? refDate, CalendarWeekRule cwr, CultureInfo ci)
        {
            Periodicity = PeriodicityType.Weekly;
            if (refDate.HasValue)
            {

                PeriodNumber = Periodics.GetWeekOfYear(refDate.Value, cwr, ci);
                CalendarWeekRule = cwr;
                Culture = ci;
                Year = refDate.Value.Year;
                BeginDate = Periodics.FirstDateOfWeek(refDate.Value, ci);
                EndDate = BeginDate.AddDays(6);
            }
        }

        public YearWeek(int year, int week, CalendarWeekRule cwr, CultureInfo ci)
        {

        }

        public override string ToLongString()
        {
            return Year.ToString() + "-Week " + PeriodNumber;
        }

        public override string ToShortString()
        {
            return Year.ToString().Substring(Math.Max(0, Year.ToString().Length - 2)) + "W" + PeriodNumber;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets year weeks between two dates. </summary>
        /// <param name="fromdate"> The fromdate Date/Time. </param>
        /// <param name="todate">   The todate Date/Time. </param>
        /// <param name="cwr">      The CalendarWeekRule. </param>
        /// <param name="ci">       The CultureInfo. </param>
        /// <returns>   The year weeks between. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static List<YearWeek> GetPeriodsCovering(DateTime fromdate, DateTime todate, CalendarWeekRule cwr, CultureInfo ci)
        {
            List<YearWeek> ywl = new List<YearWeek>();
            DateTime lo = fromdate.Date;
            DateTime hi = todate.Date;
            int comp = DateTime.Compare(fromdate, todate);

            if (comp == 0)
            {
                ywl.Add(Periodics.GetYearWeek(lo, cwr, ci));
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
                ywl.Add(Periodics.GetYearWeek(curwf, cwr, ci));
                curwf = curwf.AddDays(7);

                if (DateTime.Compare(hi, curwf) <= 0)
                    done = true;

            }

            return ywl;
        }

        public override YearWeek NextPeriodItem()
        {
            return AddPeriod(1);
        }

        public override YearWeek AddPeriod(int nperiods)
        {
            return new YearWeek(BeginDate.AddDays(7 * nperiods), CalendarWeekRule, Culture);
        }

        public override YearWeek AddYear(int nyears)
        {
            return new YearWeek(BeginDate.AddYears(nyears), CalendarWeekRule, Culture);
        }

        public override int PeriodsBetween(IYearNumberedPeriodItem other)
        {
             if (other.Periodicity != PeriodicityType.Weekly)
                return int.MaxValue;
            // have to enumerate weeks now as the formula is not guaranteed to be accurate
            List<YearWeek> ywks = GetPeriodsCovering(this.BeginDate, other.BeginDate, CalendarWeekRule, Culture);
            return ywks.Count - 1;
        }
    }
}
