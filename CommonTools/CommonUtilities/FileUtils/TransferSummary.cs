using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.FileUtils
{
    public class TransferSummary
    {
        public int TotalFilesTransferred { get; set; } 
        public int TransferSuccessfully { get; set; }
        public int TransferSkipped { get; set; }
        public int TransferFailed { get; set; }
        public string ElapsedTime { get; set; }
    }
}
