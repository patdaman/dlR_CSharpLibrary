///-------------------------------------------------------------------------------------------------
// <date>20160205</date>
// <summary>Implements the year numbered period item class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DateUtils
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A year numbered period item. </summary>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    ///-------------------------------------------------------------------------------------------------

    abstract public class YearNumberedPeriodItem<T> : IYearNumberedPeriodItem, IEquatable<YearNumberedPeriodItem<T>>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <value> The periodicity. </value>
        ///-------------------------------------------------------------------------------------------------

        public PeriodicityType Periodicity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <value> The year. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Year { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <value> The period number. </value>
        ///-------------------------------------------------------------------------------------------------

        public int PeriodNumber { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the begin date. </summary>
        ///
        /// <value> The begin date. </value>
        ///-------------------------------------------------------------------------------------------------

        public DateTime BeginDate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the end date. </summary>
        ///
        /// <value> The end date. </value>
        ///-------------------------------------------------------------------------------------------------

        public DateTime EndDate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the CommonUtils.DateUtils.YearNumberedPeriodItem&lt;T&gt;
        /// class.
        /// </summary>
        /// <remarks>   Ssur, 20160222. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public YearNumberedPeriodItem() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Next period item. </summary>
        /// <returns>   A T. </returns>
        ///-------------------------------------------------------------------------------------------------

        abstract public T NextPeriodItem();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a period. </summary>
        /// <param name="nperiods"> The nperiods. </param>
        /// <returns>   A T. </returns>
        ///-------------------------------------------------------------------------------------------------

        abstract public T AddPeriod(int nperiods);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a year. </summary>
        /// <param name="nyears">   The nyears. </param>
        /// <returns>   A T. </returns>
        ///-------------------------------------------------------------------------------------------------

        abstract public T AddYear(int nyears);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts this object to a short string. </summary>
        /// <returns>   This object as a string. </returns>
        ///-------------------------------------------------------------------------------------------------

        abstract public string ToShortString();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts this object to a long string. </summary>
        /// <returns>   This object as a string. </returns>
        ///-------------------------------------------------------------------------------------------------

        abstract public string ToLongString();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Periods between. </summary>
        /// <param name="other">    Another instance to compare. </param>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        abstract public int PeriodsBetween(IYearNumberedPeriodItem other);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts a fmt to a formatted string. </summary>
        /// <param name="fmt">  Describes the format to use. {0} is Year, {1} is the periodnumber. </param>
        /// <returns>   fmt as a string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string ToFormattedString(string fmt)
        {
            return String.Format(fmt, Year, PeriodNumber);
        }

        //public int CompareTo(YearNumberedPeriodItem other)
        //{
        //    return this.BeginDate.CompareTo(other.BeginDate);
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Serves as the default hash function. </summary>
        ///
        /// <remarks>   Ssur, 20160222. </remarks>
        ///
        /// <returns>   A hash code for the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int GetHashCode()
        {
            return this.BeginDate.GetHashCode() + this.Periodicity.GetHashCode();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determines whether the specified object is equal to the current object. </summary>
        /// <param name="obj">  The object to compare with the current object. </param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool Equals(object obj)
        {
            return (obj is YearNumberedPeriodItem<T>) && (this.Equals(obj as YearNumberedPeriodItem<T>));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">    An object to compare with this object. </param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise,
        /// false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Equals(YearNumberedPeriodItem<T> other)
        {
            return (this.Periodicity == other.Periodicity) && (this.BeginDate == other.BeginDate);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this IYearNumberedPeriodItem is considered equal to another. </summary>
        /// <param name="other">    Another instance to compare. </param>
        /// <returns>   true if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Equals(IYearNumberedPeriodItem other)
        {
             return (this.Periodicity == other.Periodicity) && (this.BeginDate == other.BeginDate); 
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the next period item. </summary>
        /// <returns>   The next period item. </returns>
        ///-------------------------------------------------------------------------------------------------

        public IYearNumberedPeriodItem GetNextPeriodItem()
        {
            return NextPeriodItem() as IYearNumberedPeriodItem;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Increment by periods. </summary>
        /// <param name="nperiods"> The nperiods. </param>
        /// <returns>   An IYearNumberedPeriodItem. </returns>
        ///-------------------------------------------------------------------------------------------------

        public IYearNumberedPeriodItem IncrementByPeriods(int nperiods)
        {
            return AddPeriod(nperiods) as IYearNumberedPeriodItem;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Increment by years. </summary>
        /// <param name="nyears">   The nyears. </param>
        /// <returns>   An IYearNumberedPeriodItem. </returns>
        ///-------------------------------------------------------------------------------------------------

        public IYearNumberedPeriodItem IncrementByYears(int nyears)
        {
            return AddYear(nyears) as IYearNumberedPeriodItem;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Compares this IYearNumberedPeriodItem object to another to determine their relative
        /// ordering.
        /// </summary>
        /// <param name="other">    Another instance to compare. </param>
        /// <returns>
        /// Negative if this object is less than the other, 0 if they are equal, or positive if this is
        /// greater.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------

        public int CompareTo(IYearNumberedPeriodItem other)
        {
            return this.BeginDate.CompareTo(other.BeginDate);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Equality operator. </summary>
        /// <param name="A">    The YearNumberedPeriodItem&lt;T&gt; to process. </param>
        /// <param name="B">    The YearNumberedPeriodItem&lt;T&gt; to process. </param>
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool operator ==(YearNumberedPeriodItem<T> A, YearNumberedPeriodItem<T> B)
        {
            return A.Equals(B);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Inequality operator. </summary>
        /// <param name="A">    The YearNumberedPeriodItem&lt;T&gt; to process. </param>
        /// <param name="B">    The YearNumberedPeriodItem&lt;T&gt; to process. </param>
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool operator !=(YearNumberedPeriodItem<T> A, YearNumberedPeriodItem<T> B)
        {
            return !(A.Equals(B));
        }
    }

}
