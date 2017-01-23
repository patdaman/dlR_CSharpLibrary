using CommonUtils.Reflection;
using System.Dynamic;

namespace CommonUtils.ExpandoUtils
{
    public class ExpandoUtils
    {
        public static ExpandoObject GetObjectCopyAsExpando(object item)
        {
            dynamic obj = new ExpandoObject();
            ReflectionUtils.CopyProperties(item, obj);
            return obj; 
        }
    }
}
