using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonUtils.MathUtils
{
    public static class MathUtil
    {
        public static Double StdDev(this IEnumerable<double> values, bool NARM = false)
        {

            double ret = 0;
            if (values.Count() > 0)
            {  
                double avg = values.Average();  
                double sum = values.Sum(d => Math.Pow(d - avg, 2));    
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }
    }
}
