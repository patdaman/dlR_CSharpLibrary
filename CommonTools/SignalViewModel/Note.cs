using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Note
    {
        public int id { get; set; }
        public string NoteId { get; set; }
        public string Note1 { get; set; }
        public string NoteType { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserName { get; set; }
    }
}
