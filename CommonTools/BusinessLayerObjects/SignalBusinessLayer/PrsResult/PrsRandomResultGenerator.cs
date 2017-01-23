using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppDataLib.Enums.CAEnums;

namespace BusinessLayer
{
    public class PrsRandomResultGenerator
    {

        private static List<string> Subtypes { get; set; }

        static PrsRandomResultGenerator()
        {
            Subtypes = new List<string>(new string[]{
                "PR",
                "LB",
                "MS",
                "HY",
                "CD-1",
                "CD-2",
                "MF"});
        }

        public PrsRandomResultGenerator()
        {

        }


        public PrsResult GeneratePrsResult(int? seed = null)
        {
            Random randomGenerator = seed.HasValue ? new Random(seed.Value) : new Random();
            int number = randomGenerator.Next(0, 2000);
            string casenumber = "CL16-" + string.Format("{0:D6}", number);
            return GeneratePrsResult(casenumber);
        }



        public PrsResult GeneratePrsResult(string casenumber)
        {
            Random randomGenerator = new Random(casenumber.GetHashCode());

            double riskScore = randomGenerator.NextDouble() * 100;
            string borderClassification = "";
            if (riskScore < 40)
                borderClassification = BorderClassification_GEP70Values.LowRisk;
            else if (40 <= riskScore && riskScore < 45.2) //<- THESE ARE NOT ACTUAL DECISION RULES, just for test data 
                borderClassification = BorderClassification_GEP70Values.LowRiskBorderline;
            else if (45.2 <= riskScore && riskScore < 51) //<- THESE ARE NOT ACTUAL DECISION RULES, just for test data 
                borderClassification = BorderClassification_GEP70Values.HighRiskBorderline;
            else
                borderClassification = BorderClassification_GEP70Values.HighRisk;

            string classificationGep70 = (borderClassification.Substring(0, 8) == "Low Risk") ? "Low Risk" : "High Risk";


            var result = new PrsResult();
            result.GEP70_AlgorithmVersion = "0.0randgen";
            result.Subtype_AlgorithmVersion = "0.0randgen";
            result.VK_AlgorithmVersion = "0.0randgen";
            result.TP53_AlgorithmVersion = "0.0randgen";
            result.CaseNumber = casenumber;
            result.RiskScore_GEP70 = riskScore;
            result.Classification_GEP70 = classificationGep70;
            result.BorderClassification_GEP70 = borderClassification;
            //result.NormalizationType = ;
            //result.TestType = ;
            result.chr1p = "normal";
            result.chr1q = "normal";
            result.chr1q21 = "normal";
            result.chr3 = "gain";
            result.chr5 = "gain";
            result.chr6q = "normal";
            result.chr7 = "normal";
            result.chr9 = "del";
            result.chr11 = "normal";
            result.chr13 = "normal";
            result.chr15 = "gain";
            result.chr19 = "normal";
            result.chr21 = "normal";
            result.Trisomies = "Yes (4)";
            result.t4_14 = "Yes";
            result.t14_16 = "No";
            result.t11_14 = "Yes";
            result.t14_20 = "No";
            result.Expression_TP53 = 520;
            result.Category_TP53 = "Low";
            result.Subtype = Subtypes[randomGenerator.Next(Subtypes.Count())];
            result.SubtypeDescription = @"Lorem ipsum dolor sit amet, ut nibh interesset mei. Tempor voluptua qui ea, eu eam movet explicari, nec ut nisl elaboraret consectetuer. Mei at cetero minimum. Eu amet omnis officiis sed, vis ei latine dolores. Ne sea quod wisi exerci.";

            result.Comments = "** These results were randomly generated for a test environment. **";
            result.QNSComments = "** These results were randomly generated for a test environment. **";

            result.ChipQCMetricsPass = null;


            return result;

        }
    }
}
