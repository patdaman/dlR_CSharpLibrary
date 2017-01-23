using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Classifier
{
   public class Filter
    {
        public enum Op
        {
            Equals,
            NotEqual,
            GreaterThan,
            LessThan,
            GreaterThanOrEqual,
            LessThanOrEqual,
            Contains,
            DoesNotContain,
            StartsWith,
            EndsWith
        };

        public enum ConnectionClause
        {
            AndAlso,
            OrElse

        };

        public string PropertyName { get; set; }
        public object Value { get; set; }
        public Op Operation { get; set; }
        public ConnectionClause ConnClause { get; set; }
    }

}
