using OgsysCRM.Models;
using OgsysCRM.Models.DataTables;
using System.Collections.Generic;

namespace OgsysCRM.DAL
{
    public interface INoteRepo
    {
        IEnumerable<Note> GetNotes();
        Note GetNoteById(int noteId);
        void InsertNote(Note note);
        void DeleteNote(int noteId);
        void UpdateNote(Note note);
        void Save();
        IEnumerable<Note> GetAllNotesForTable(int? customerFilter, int? userFilter, ColumnViewModel sortedColumn = null, SearchViewModel searchBy = null);
    }
}
