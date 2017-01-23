using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public partial class ReportType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReportType()
        {
            this.FieldMappings = new HashSet<FieldMapping>();
        }

        public int id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public byte[] Report { get; set; }
        public bool IsActive { get; set; }
        public System.DateTimeOffset CreatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FieldMapping> FieldMappings { get; set; }
    }
}
