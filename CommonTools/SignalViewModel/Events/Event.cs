using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Events
{
    public class Event
    {
        public int TypeId { get; set; }
        public string EventName { get; set; }
        public string EventDefinition { get; set; }
        public string Source { get; set; }
        public string EventContent { get; set; }
        public string ContentType { get; set; }
        public string Priority { get; set; }
        public Nullable<System.DateTime> MessageSentDate { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public object IdField { get; set; }
    }
}
