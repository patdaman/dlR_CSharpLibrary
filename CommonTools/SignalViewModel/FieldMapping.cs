using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public partial class FieldMapping
    {
        public int ReportTypeId { get; set; }
        public string Field { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public bool IsActive { get; set; }
        public System.DateTimeOffset CreatedDate { get; set; }

        public virtual ReportType ReportType { get; set; }
    }
}
