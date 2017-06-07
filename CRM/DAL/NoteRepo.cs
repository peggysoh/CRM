using OgsysCRM.Models;
using OgsysCRM.Models.DataTables;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OgsysCRM.DAL
{
    public class NoteRepo : INoteRepo
    {
        private AppContext context;

        public NoteRepo(AppContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Note> GetNotes()
        {
            return context.Notes.ToList();
        }

        /// <summary>
        /// Gets note by Id.
        /// </summary>
        /// <param name="noteId">The note ID.</param>
        public Note GetNoteById(int noteId)
        {
            return context.Notes.Find(noteId);
        }

        /// <summary>
        /// Inserts the note.
        /// </summary>
        /// <param name="note">The note.</param>
        public void InsertNote(Note note)
        {
            context.Notes.Add(note);
        }

        /// <summary>
        /// Deletes the note.
        /// </summary>
        /// <param name="noteId">The note ID.</param>
        public void DeleteNote(int noteId)
        {
            Note note = context.Notes.Find(noteId);
            context.Notes.Remove(note);
        }

        /// <summary>
        /// Updates the note.
        /// </summary>
        /// <param name="note">The note.</param>
        public void UpdateNote(Note note)
        {
            context.Entry(note).State = EntityState.Modified;
        }

        /// <summary>
        /// Saves context.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Gets Notes for datatable
        /// </summary>
        public IEnumerable<Note> GetAllNotesForTable(int? customerFilter, int? userFilter, ColumnViewModel sortedColumn = null, SearchViewModel searchBy = null)
        {
            var noteList = GetNotes();

            if (customerFilter != 0)
                noteList = noteList.Where(x => x.Customer.Id == customerFilter);

            if (userFilter != 0)
                noteList = noteList.Where(x => x.User.Id == userFilter);

            if (searchBy != null && !string.IsNullOrEmpty(searchBy.Value))
            {
                var term = searchBy.Value.ToLower();
                noteList = noteList.Where(x => x.Customer.FirstName.ToLower().Contains(term) ||
                                               x.Customer.LastName.ToLower().Contains(term) ||
                                               x.User.FirstName.ToLower().Contains(term) ||
                                               x.User.LastName.ToLower().Contains(term) ||
                                               x.Body.ToLower().Contains(term));
            }

            // sorted by column and order
            if (sortedColumn == null)
                return noteList.OrderBy(x => x.DateCreated);

            switch (sortedColumn.Data)
            {
                case "Id":
                    {
                        noteList = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? noteList.OrderBy(x => x.Id)
                            : noteList.OrderByDescending(x => x.Id);
                    }
                    break;

                case "DateCreated":
                    {
                        noteList = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? noteList.OrderBy(x => x.DateCreated)
                            : noteList.OrderByDescending(x => x.DateCreated);
                    }
                    break;

                case "Customer":
                    {
                        noteList = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? noteList.OrderBy(x => x.Customer.FirstName)
                            : noteList.OrderByDescending(x => x.Customer.FirstName);
                    }
                    break;

                case "CreatedBy":
                    {
                        noteList = sortedColumn.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? noteList.OrderBy(x => x.User.FirstName)
                            : noteList.OrderByDescending(x => x.User.FirstName);
                    }
                    break;
            }

            return noteList;
        }
    }
}