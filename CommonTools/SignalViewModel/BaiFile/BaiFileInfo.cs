using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.BaiFile
{
    public class BaiFileInfo
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public DateTimeOffset UploadDate { get; set; }
        public string UploadUser { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string FileCreationDate { get; set; }
        public string FileIdentificationNumber { get; set; }
        public string OriginatorIdentification { get; set; }
        public string UltimateReceiverIdentification { get; set; }
    }
}
