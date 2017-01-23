using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public partial class Event
    {
        public int id { get; set; }
        public int TypeId { get; set; }
        public string EventData { get; set; }
        public string Source { get; set; }
        public Nullable<System.DateTime> MessageSentDate { get; set; }

        public virtual EventType Type { get; set; }
    }
}
