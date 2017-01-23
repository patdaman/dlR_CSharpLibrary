using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CommonUtils.Collections
{
    public static class SortObservableCollection
    {
     /// <summary>
     /// Extension for sorting Observable collection
     /// </summary>
     /// <typeparam name="TSource"></typeparam>
     /// <typeparam name="TKey"></typeparam>
     /// <param name="source"></param>
     /// <param name="keySelector"></param>
        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector)
        {
            var sortedList = source.OrderBy(keySelector).ToList();
            source.Clear();
            foreach (var sortedItem in sortedList)
                source.Add(sortedItem);
        }
    }
}
