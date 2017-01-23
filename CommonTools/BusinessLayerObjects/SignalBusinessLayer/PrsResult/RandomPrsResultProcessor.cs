using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using AppDataLib.Enums;

namespace BusinessLayer.PxPortal
{
    public class RandomPrsResultProcessor : IPrsResultsProcessor
    {

        private PrsRandomResultGenerator RandomResultGenerator { get; set; } = new PrsRandomResultGenerator(); 

        public PrsResult GetPrsResult(string caseNumber, string gep70AlgorithmVersion = null, string subtypeAlgorithmVersion = null, string vkAlgorithmVersion = null)
        {
            return RandomResultGenerator.GeneratePrsResult(caseNumber);
        }

        public List<PrsResult> GetPrsResults(IEnumerable<string> casenumbers, string gep70AlgorithmVersion = null, string subtypeAlgorithmVersion = null, string vkAlgorithmVersion = null)
        {
            List<PrsResult> list = new List<PrsResult>();
            foreach (string caseno in casenumbers)
                list.Add(GetPrsResult(caseno));
            return list;
        }

        public List<PrsResult> GetPrsResultsForUser(string username)
        {   
            int NumberToReturn = 20;
            List<PrsResult> list = new List<PrsResult>();
            for( int ind = 0; ind < NumberToReturn; ind++)
            {
                list.Add(RandomResultGenerator.GeneratePrsResult());
            }
            return list;
        }

        public PrsResultStatus GetPrsResultStatus(PrsResult prsResult)
        {
            return PrsResultStatus.OK;
        }
    }
}
