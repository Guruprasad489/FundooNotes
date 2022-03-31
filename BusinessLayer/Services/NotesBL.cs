using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NotesBL : INotesBL
    {
        private readonly INotesRL notesRL;
        public NotesBL(INotesRL notesRL)
        {
            this.notesRL = notesRL;
        }

        public NotesEntity CreateNote(Notes createNotes, long userID)
        {
            try
            {
                return notesRL.CreateNote(createNotes, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotesEntity ViewNote(long noteID, long userID)
        {
            try
            {
                return notesRL.ViewNote(noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<NotesEntity> ViewAllNotes(long userID)
        {
            try
            {
                return notesRL.ViewAllNotes(userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotesEntity UpdateNote(Notes updateNotes, long noteID, long userID)
        {
            return notesRL.UpdateNote(updateNotes, noteID, userID);
        }

        public string DeleteNote(long noteID, long userID)
        {
            return notesRL.DeleteNote(noteID, userID);
        }

        public bool IsArchieveOrNot(long noteID, long userID)
        {
            return notesRL.IsArchieveOrNot(noteID, userID);
        }

        public bool IsPinnedOrNot(long noteID, long userID)
        {
            return notesRL.IsPinnedOrNot(noteID, userID);
        }

        public bool IsTrashOrNot(long noteID, long userID)
        {
            return notesRL.IsTrashOrNot(noteID, userID);
        }
    }
}
