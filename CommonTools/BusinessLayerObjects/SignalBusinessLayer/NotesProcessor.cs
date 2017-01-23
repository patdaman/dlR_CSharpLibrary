using DataLayer;
using EF = SignalEFDataModel;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class NotesProcessor : BehaviorBase
    {
        IRepository<EF.SGNL_LIS.Note> repository;

        public NotesProcessor()
        {
            repository = new ConcreteRepository<EF.SGNL_LIS.Note>(LISDbContext);
        }

        public List<Note> GetNotes(string caseno, string noteType)
        {
            var noteList = repository.Query<EF.SGNL_LIS.Note>(e => e.NoteType == noteType & e.NoteId == caseno).OrderByDescending(e => e.CreateDate).ToList<EF.SGNL_LIS.Note>();
            return BusinessLayerConverter.CreateNoteList(noteList);
           
        }

    }
}
