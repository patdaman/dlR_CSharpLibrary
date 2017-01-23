using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{
    public class Periodics
    {

        // this method is borrowed from http://stackoverflow.com/a/11155102/284240
        public static int GetIso8601WeekOfYear(DateTime time, CultureInfo ci)
        {
            //DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            DayOfWeek day = ci.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            //return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return ci.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        //public static string GetIso8601YearWeek(DateTime time, CultureInfo ci, string wkTag = "W")
        //{
        //    return time.Year.ToString() + "-" + wkTag + GetIso8601WeekOfYear(time, ci).ToString();
        //}

        public static int GetWeekOfYear(DateTime dt, CultureInfo ci)
        {
            return ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
        }

        public static int GetWeekOfYear(DateTime dt, CalendarWeekRule cwr, CultureInfo ci)
        {
            return ci.Calendar.GetWeekOfYear(dt, cwr, ci.DateTimeFormat.FirstDayOfWeek);
        }

        public static YearWeek GetYearWeek(DateTime refdate, CalendarWeekRule cwr, CultureInfo ci)
        {
            return new YearWeek()
            {
                PeriodNumber = GetWeekOfYear(refdate, cwr, ci),
                Year = refdate.Year,
                BeginDate = FirstDateOfWeek(refdate, ci)
            };
        }
        //public static string GetYearWeek(DateTime dt, CultureInfo ci, string wkTag = "W")
        //{
        //    return dt.Year.ToString() + "-" + wkTag + GetWeekOfYear(dt, ci).ToString();
        //}

        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime FirstDateOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            //int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            int firstWeek = GetIso8601WeekOfYear(jan1, ci);
            if (firstWeek <= 1 || firstWeek > 50)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        public static DateTime LastDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            return FirstDateOfWeek(year, weekOfYear, ci).AddDays(6.0);
        }

        /*
        public static DateTime FirstDateOfWeek(DateTime refDate, System.Globalization.CultureInfo ci)
        {
            int year = refDate.Year;
            int weekOfYear = GetIso8601WeekOfYear(refDate, ci);
            return FirstDateOfWeek(year, weekOfYear, ci);
        }
        */
        public static DateTime LastDateOfWeek(DateTime refDate, System.Globalization.CultureInfo ci)
        {
            return LastDateOfWeek(refDate.Year, GetIso8601WeekOfYear(refDate, ci), ci);
        }

        


        public static IList<DateSubjectItem> GroupBySubject(System.Globalization.CultureInfo ci,
            DataTable dt, string datecol, string subjectcol,
            DateTime? refEndDate = null, DateTime? refBeginDate = null)
        {
            DateTime? ldt, bdt;
            bool isdate = Tools.IsDateColumn(dt, datecol, out bdt, out ldt);
            if (!isdate)
                return null;

            if (refEndDate != null)
                ldt = refEndDate;

            if (refBeginDate != null)
                bdt = refBeginDate;

            IList<DateSubjectItem> data = null;
            if (subjectcol != null)
            {
                int colidx = dt.Columns.IndexOf(subjectcol);
                if (colidx > -1)
                {

                    data = (from row in dt.AsEnumerable()
                            where ((row.Field<DateTime>(datecol).CompareTo(bdt) >= 0) && (row.Field<DateTime>(datecol).CompareTo(ldt) <= 0))
                            select new DateSubjectItem
                            {
                                Date = row.Field<DateTime>(datecol),
                                Subject = row[colidx],
                                YearWeekDate = FirstDateOfWeek(row.Field<DateTime>(datecol), ci),
                                YearWeek = GetWeekOfYear(row.Field<DateTime>(datecol), ci)
                            }
                                ).ToList();
                }
            }
            else
            {
                data = (from row in dt.AsEnumerable()
                        where ((row.Field<DateTime>(datecol).CompareTo(bdt) >= 0) && (row.Field<DateTime>(datecol).CompareTo(ldt) <= 0))
                        select new DateSubjectItem
                        {
                            Date = row.Field<DateTime>(datecol),
                            Subject = "Order",
                            YearWeekDate = FirstDateOfWeek(row.Field<DateTime>(datecol), ci),
                            YearWeek = GetIso8601WeekOfYear(row.Field<DateTime>(datecol), ci)
                        }
                               ).ToList();
            }
            return data;
        }

        public static IList<PeriodDataCount> GroupByPeriod(PeriodicityType pt, PeriodicityRefType rt, System.Globalization.CultureInfo ci,
            DataTable dt, string datecol, string subjectcol,
            DateTime? refEndDate = null, DateTime? refBeginDate = null)
        {
            DateTime? ldt, bdt;
            bool isdate = Tools.IsDateColumn(dt, datecol, out bdt, out ldt);
            if (!isdate)
                return null;

            if (refEndDate != null)
                ldt = refEndDate;

            if (refBeginDate != null)
                bdt = refBeginDate;

            IList<DateSubjectItem> data = GroupBySubject(ci, dt, datecol, subjectcol, refEndDate, refBeginDate);

            if (data != null)
            {


                switch (pt)
                {
                    case PeriodicityType.Daily:
                        IList<PeriodDataCount> dailyCounts = data
                            .GroupBy(x => new { x.Date.Date, x.Subject })
                            .Select(x => new PeriodDataCount
                            {
                                PeriodObject = x.Key,
                                PeriodBeginDate = x.Key.Date.Date,
                                PeriodEndDate = x.Key.Date.Date.AddDays(1.0),
                                Period = x.Key.Date.ToShortDateString(),
                                Subject = x.Key.Subject.ToString(),
                                Count = x.Count()
                            }).ToList();
                        return dailyCounts;
                        break;

                    case PeriodicityType.Weekly:
                        IList<PeriodDataCount> weeklyCounts = data
                             .GroupBy(x => new { x.Date.Year, x.YearWeek, x.YearWeekDate, x.Subject })
                             .Select(x => new PeriodDataCount
                             {
                                 PeriodObject = x.Key,
                                 PeriodBeginDate = x.Key.YearWeekDate,
                                 PeriodEndDate = x.Key.YearWeekDate.AddDays(7),
                                 Period = x.Key.Year.ToString() + "W" + x.Key.YearWeek.ToString(),
                                 Subject = (string)x.Key.Subject,
                                 Count = x.Count()

                             }).ToList();
                        return weeklyCounts;
                        break;

                    case PeriodicityType.Monthly:
                        IList<PeriodDataCount> monthlyCounts = data
                           .GroupBy(x => new { x.Date.Year, x.Date.Month, x.Subject })
                           .Select(x => new PeriodDataCount
                           {
                               PeriodObject = x.Key,
                               PeriodBeginDate = new DateTime(x.Key.Year, x.Key.Month, 1),
                               PeriodEndDate = new DateTime(x.Key.Year, x.Key.Month, 1).AddMonths(1),
                               Period = x.Key.Year.ToString() + "-" + x.Key.Month.ToString(),
                               Subject = x.Key.Subject.ToString(),
                               Count = x.Count()
                           }).ToList();
                        return monthlyCounts;
                        break;
                        break;
                    case PeriodicityType.Yearly:
                        IList<PeriodDataCount> yearlyCounts = data
                           .GroupBy(x => new { x.Date.Year, x.Subject })
                           .Select(x => new PeriodDataCount
                           {
                               PeriodObject = x.Key,
                               PeriodBeginDate = new DateTime(x.Key.Year, 1, 1),
                               PeriodEndDate = new DateTime(x.Key.Year + 1, 1, 1),
                               Period = x.Key.Year.ToString(),
                               Subject = x.Key.Subject.ToString(),
                               Count = x.Count()
                           }).ToList();
                        return yearlyCounts;
                        break;
                }

            }
            return null;


        }

    }

    public class DateSubjectItem
    {
        public DateTime Date { get; set; }
        public int YearWeek { get; set; }
        public DateTime YearWeekDate { get; set; }
        public object Subject { get; set; }
        public string SubjectName { get; set; }
    }

    public class PeriodDataCount
    {
        public string Subject { get; set; }
        public DateTime PeriodBeginDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public string Period { get; set; }
        public object PeriodObject { get; set; }
        public double Count { get; set; }
        public int PeriodIndex { get; set; }
    }

}
