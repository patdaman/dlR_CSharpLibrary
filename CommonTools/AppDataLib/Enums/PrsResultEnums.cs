using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDataLib.Enums
{
    public enum PrsResultStatus
    {
        Unknown,
        OK,
        ResultsNotReady,
        QCFailure,
        QnsFailure,
        ResultsPx_ResultDiscrepency,
        ResultsPx_QCDiscrepency,
        CaseCancelled
    };

    public enum PrsResultsWorkflowStep
    {
        Complete,
        Post_Sort_Flow_Analysis,
        Additional_Purification,
        IVT,
        cDNA_Synthesis,
        Distribution,
        Wash_Stain_Scan,
        Professional,
        RNA_Isolation,
        cRNA_Cleanup,
        Received,
        Cancelled,
        Pre_Sort_Flow_Analysis,
        Hybridization,
        cRNA_Fragmentation,
        NA_Isolation
    }
}
