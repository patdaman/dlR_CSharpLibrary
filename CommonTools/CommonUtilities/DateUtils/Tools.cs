using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{
    public class Tools
    {
        public static Boolean IsDateColumn(DataTable dt, string dateCol, out DateTime? mindate, out DateTime? maxdate)
        {
            int colidx = dt.Columns.IndexOf(dateCol);
            mindate = null;
            maxdate = null;

            if (colidx > -1)
            {
                DataColumn dcol = dt.Columns[colidx];
                if (dcol.DataType == System.Type.GetType("System.DateTime"))
                {
                    mindate = DateTime.MaxValue;
                    maxdate = DateTime.MinValue;
                    // this is faster than linq (single pass)
                    foreach (DataRow dr in dt.Rows)
                    {
                        DateTime rdt = dr.Field<DateTime>(colidx);
                        mindate = rdt.CompareTo(mindate) < 0 ? rdt : mindate;
                        maxdate = rdt.CompareTo(maxdate) > 0 ? rdt : maxdate;
                    }
                    return true;
                }
                return false;
            }

            return false;
        }

        public static Boolean IsDateProperty<T>(IList<T> dl, string datecol, out DateTime? mindate, out DateTime? maxdate)
        {
            mindate = null;
            maxdate = null;

            Type typeParameterType = typeof(T);
            PropertyInfo pi = typeParameterType.GetProperty(datecol);
            if (pi == null)
                return false;

            if (pi.GetType() == System.Type.GetType("System.DateTime"))
            {
                mindate = DateTime.MaxValue;
                maxdate = DateTime.MinValue;
                // this is faster than linq (single pass)
                foreach (var dr in dl)
                {
                    DateTime rdt = (DateTime)dr.GetType().GetProperty(datecol).GetValue(dr, null);
                    mindate = rdt.CompareTo(mindate) < 0 ? rdt : mindate;
                    maxdate = rdt.CompareTo(maxdate) > 0 ? rdt : maxdate;
                }
                return true;
            }
            return false;
        }

        public static DateTime DateTimeZoneConversion(DateTime LocalDate, string newZone)
        {
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(newZone);
            DateTime  newDateTime = TimeZoneInfo.ConvertTime(LocalDate, TimeZoneInfo.Local, timeInfo);
            return newDateTime;
        }

    }
}
