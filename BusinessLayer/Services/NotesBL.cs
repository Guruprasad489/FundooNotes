using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    /// <summary>
    /// Notes Business Layer Class To Implement methods of INotesBL
    /// </summary>
    /// <seealso cref="BusinessLayer.Interface.INotesBL" />
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
            try
            {
                return notesRL.UpdateNote(updateNotes, noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteNote(long noteID, long userID)
        {
            try
            {
                return notesRL.DeleteNote(noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotesEntity IsArchieveOrNot(long noteID, long userID)
        {
            try
            {
                return notesRL.IsArchieveOrNot(noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotesEntity IsPinnedOrNot(long noteID, long userID)
        {
            try
            {
                return notesRL.IsPinnedOrNot(noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotesEntity IsTrashOrNot(long noteID, long userID)
        {
            try
            {
                return notesRL.IsTrashOrNot(noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotesEntity ChangeColor(string newColor, long noteID, long userID)
        {
            try
            {
                return notesRL.ChangeColor(newColor, noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NotesEntity UploadImage(long noteID, long userID, IFormFile imagePath)
        {
            try
            {
                return notesRL.UploadImage(noteID, userID, imagePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RemoveImage(long noteID, long userID)
        {
            try
            {
                return notesRL.RemoveImage(noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<NotesEntity> GetAll()
        {
            try
            {
                return notesRL.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<NotesEntity> GetNotesByLabel(long labelID, long userID)
        {
            try
            {
                return notesRL.GetNotesByLabel(labelID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
