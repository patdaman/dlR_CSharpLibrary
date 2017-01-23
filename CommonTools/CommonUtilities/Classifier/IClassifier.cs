using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Classifier
{
    public interface IClassifier
    {
        List<ClassifierClassItem> GetOutputClasses();
        ClassifierOutput<T> RunClassifier<T>(ClassifierInputs parameters, List<T> inputData, Dictionary<String, String> outputGroups = null);
        ClassifierOutput<T> RunClassifier<T>(ClassifierInputs parameters, T item);
    }
}
