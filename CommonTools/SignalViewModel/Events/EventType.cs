using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Events
{
    public class EventType
    {
        public int id { get; set; }
        public string EventType1 { get; set; }
        public string EventName { get; set; }
        public string EventDefinition { get; set; }
        public string Priority { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateTimeAdded { get; set; }
    }
}
