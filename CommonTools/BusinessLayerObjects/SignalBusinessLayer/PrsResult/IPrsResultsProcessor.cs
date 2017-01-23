using AppDataLib.Enums;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IPrsResultsProcessor
    {

        PrsResultStatus GetPrsResultStatus(PrsResult prsResult);

        PrsResult GetPrsResult(string caseNumber,
               string gep70AlgorithmVersion = null,
               string subtypeAlgorithmVersion = null,
               string vkAlgorithmVersion = null);

        List<PrsResult> GetPrsResults(IEnumerable<string> casenumbers,
               string gep70AlgorithmVersion = null,
               string subtypeAlgorithmVersion = null,
               string vkAlgorithmVersion = null);


    }


}
