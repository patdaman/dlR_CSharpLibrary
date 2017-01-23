using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.BaiFile
{
    public class BaiFile
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public string RecordCode { get; set; }
        public string SenderIdentification { get; set; }
        public string ReceiverIdentification { get; set; }
        public string FileCreationDate { get; set; }
        public string FileCreationTime { get; set; }
        public string FileIdentificationNumber { get; set; }
        public string PhysicalRecordLength { get; set; }
        public string BlockSize { get; set; }
        public string VersionNumber { get; set; }
        public Boolean IsSelected { get; set; }

        public List<AccountGroup> AccountGroupList { get; set; }
    }
}
