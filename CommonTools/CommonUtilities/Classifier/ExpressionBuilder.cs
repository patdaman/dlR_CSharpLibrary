using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Classifier
{
     public static class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static MethodInfo startsWithMethod =
        typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod =
        typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });


        public static Expression<Func<T, bool>> GetComplexExpression<T>(IList<Filter> flist)
        {
            if (flist.Count == 0) return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Filter.ConnectionClause curconn = Filter.ConnectionClause.AndAlso;
            int filtitem = 0;
            Expression totExp = null;
            foreach (Filter filt in flist)
            {
                curconn = filt.ConnClause;
                Expression exp = GetExpression<T>(param, filt);
                if (filtitem == 0)
                    totExp = exp;
                else
                    totExp = ConnectClause<T>(totExp, exp, curconn);
                filtitem++;
            }

            return Expression.Lambda<Func<T, bool>>(totExp, param);

        }

        public static Expression<Func<T, bool>> GetSingleExpression<T>(Filter filt)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            return Expression.Lambda<Func<T, bool>>(GetExpression<T>(param, filt), param);
        }

        private static Expression ConnectClause<T>(Expression exp1, Expression exp2, Filter.ConnectionClause clause)
        {

            switch (clause)
            {
                case Filter.ConnectionClause.AndAlso:
                    return Expression.AndAlso(exp1, exp2);
                    break;
                case Filter.ConnectionClause.OrElse:
                    return Expression.OrElse(exp1, exp2);
                    break;
            }

            return null;
        }

        private static Expression GetExpression<T>(ParameterExpression param, Filter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value);

            switch (filter.Operation)
            {
                case Filter.Op.Equals:
                    return Expression.Equal(member, constant);

                case Filter.Op.NotEqual:
                    return Expression.NotEqual(member, constant);

                case Filter.Op.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Filter.Op.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Filter.Op.LessThan:
                    return Expression.LessThan(member, constant);

                case Filter.Op.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Filter.Op.Contains:
                    return Expression.Call(member, containsMethod, constant);

                case Filter.Op.DoesNotContain:
                    return Expression.Not(Expression.Call(member, containsMethod, constant));

                case Filter.Op.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Filter.Op.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>
        (ParameterExpression param, Filter filter1, Filter filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }
    }
}
