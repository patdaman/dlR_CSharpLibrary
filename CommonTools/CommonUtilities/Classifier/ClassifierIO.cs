using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Classifier
{
    public class ClassifierOutput<T>
    {
        public List<ClassifierClassItem> OutputClasses { get; set; }
        public List<T> OutputDataList { set; get; }
        public Dictionary<String, List<T>> Groupings { set; get; }

    }

    public class ClassifierInputs
    {
        public Dictionary<String, String> PropertyNameMapper { get; set; }
        public String IndexObjectName { get; set; }
        public String ClassifierChainPropertyName { get; set; }
        public String CurrentClassifierPropertyName { get; set; }
    }

    public class ClassifierClassItem
    {
        public String ClassName;
        public string ClassDescription;
        public int Count=0;
    }
}
