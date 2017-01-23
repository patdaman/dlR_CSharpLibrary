using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DataTableUtils
{
    public class DataTableUtils
    {
        public static DataTable StackCols(DataTable fromDT, string[] cols)
        {
            DataTable newDT = new DataTable();
            foreach (string col in cols)
            {
                int colidx = fromDT.Columns.IndexOf(col);
                if (colidx > -1)
                {
                    DataColumn dcol = fromDT.Columns[colidx];
                    newDT.Columns.Add(dcol.ColumnName, dcol.DataType);
                }
            }
            foreach(DataRow row in fromDT.Rows)
            {
                var r = newDT.Rows.Add();
                foreach(DataColumn tcol in newDT.Columns)
                {
                    r[tcol.ColumnName] = row[tcol.ColumnName];
                }
            }

            return newDT;
        }

        public static DataTable GetUnique(DataTable dt, string colname)
        {
            DataView dv = new DataView(dt);
            return dv.ToTable(true, colname);
        }
    }




}
