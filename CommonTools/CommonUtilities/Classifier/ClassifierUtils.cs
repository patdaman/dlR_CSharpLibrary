using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Classifier
{
     public class ClassifierUtils
    {
        public static Dictionary<String, List<T>> CreateClassifierOutputBuckets<T>()
        {
            return new Dictionary<string, List<T>>();
        }

        public static Expression<Func<T, bool>> BuildMultipleOrClauseExpression<T>(List<String> values, String propertyName)
        {

            if (values == null || values.Count == 0) return null;
            List<Filter> fl = new List<Filter>();

            for (int i = 0; i < values.Count; i++)
            {
                fl.Add(
                    new Filter
                    {
                        ConnClause = Filter.ConnectionClause.OrElse,
                        Operation = Filter.Op.Equals,
                        PropertyName = propertyName,
                        Value = values[i]
                    }
                    );
            }

            return ExpressionBuilder.GetComplexExpression<T>(fl);
        }

        public static Dictionary<String, List<T>> CreateOutputGroups<T>(ClassifierInputs ci, List<T> OutputData, Dictionary<String, String> outputGroups)
        {
            if (outputGroups == null || outputGroups.Count == 0) return null;
            Dictionary<String, List<T>> groupings = new Dictionary<string, List<T>>();

            foreach (String grpname in outputGroups.Keys)
            {
                List<String> categ = outputGroups[grpname].Split(',').Select(p => p.Trim()).ToList();

                if (categ != null && categ.Count > 0)
                {
                    int ncateg = categ.Count;
                    IList<Filter> filt = new List<Filter>();


                    for (int i = 0; i < ncateg; i++)
                    {
                        filt.Add(new Filter
                        {
                            ConnClause = Filter.ConnectionClause.OrElse,
                            Operation = Filter.Op.Equals,
                            PropertyName = ci.CurrentClassifierPropertyName,
                            Value = categ[i]
                        }
                        );
                    }

                    Expression<Func<T, bool>> exp = ExpressionBuilder.GetComplexExpression<T>(filt);
                    List<T> sellist = OutputData.Where(exp.Compile()).ToList();
                    groupings.Add(grpname, sellist);
                }
            }
            return groupings;

        }

        public static IEnumerable<T> Subtract<T>(List<T> baseList, List<T> removelist, String IndexName)
        {
            ListComparerOnSingleProperty<T> comp = new ListComparerOnSingleProperty<T>();
            comp.PropertyName = IndexName;
            return Subtract(baseList, removelist, comp);
        }

        public static IEnumerable<T> Update<T>(List<T> baseList, List<T> updateList, String IndexName, String updateField, object newValue)
        {
            ListComparerOnSingleProperty<T> comp = new ListComparerOnSingleProperty<T>();
            comp.PropertyName = IndexName;
            return Update(baseList, updateList, comp, updateField, newValue);
        }

         public static IEnumerable<T> StringAppend<T>(List<T> baseList, List<T> updateList, String IndexName, String updateField, String addValue)
        {
            ListComparerOnSingleProperty<T> comp = new ListComparerOnSingleProperty<T>();
            comp.PropertyName = IndexName;
            return StringAppend(baseList, updateList, comp, updateField, addValue);
        }


        public static IEnumerable<T> Subtract<T>(IEnumerable<T> baseList, IEnumerable<T> removelist)
        {
            return Subtract(baseList, removelist, EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Subtract<T>(IEnumerable<T> baseList, IEnumerable<T> removelist, IEqualityComparer<T> comp)
        {
            Dictionary<T, object> dict = new Dictionary<T, object>(comp);
            foreach (T item in baseList)
            {
                dict[item] = null;
            }

            foreach (T item in removelist)
            {
                dict.Remove(item);
            }

            return dict.Keys;
        }

        public static IEnumerable<T> Update<T>(IEnumerable<T> baseList, IEnumerable<T> updateList, IEqualityComparer<T> comp, String updateField, object newValue)
        {
            Dictionary<T, T> dict = new Dictionary<T, T>(comp);
            foreach (T item in baseList)
            {
                dict[item] = item;
            }

            foreach (T item in updateList)
            {

                object obj = dict[item];
                obj.GetType().GetProperty(updateField).SetValue(obj, newValue);
            }

            return dict.Values;
        }

        public static IEnumerable<T> StringAppend<T>(IEnumerable<T> baseList, IEnumerable<T> updateList, IEqualityComparer<T> comp, String updateField, String addValue)
        {
            Dictionary<T, T> dict = new Dictionary<T, T>(comp);
            foreach (T item in baseList)
            {
                dict[item] = item;
            }

            foreach (T item in updateList)
            {

                object obj = dict[item];
                String curval = obj.GetType().GetProperty(updateField).GetValue(obj) as String;
                obj.GetType().GetProperty(updateField).SetValue(obj, curval + addValue);
            }

            return dict.Values;
        }

        

    }

    public class ListComparerOnSingleProperty<T> : IEqualityComparer<T>
    {
        public String PropertyName { get; set; }

        public bool Equals(T x, T y)
        {
            return (x.GetType().GetProperty(PropertyName).GetValue(x) == x.GetType().GetProperty(PropertyName).GetValue(y));
        }

        public int GetHashCode(T obj)
        {
            return (obj.GetType().GetProperty(PropertyName).GetValue(obj).GetHashCode());
        }
    }
}
