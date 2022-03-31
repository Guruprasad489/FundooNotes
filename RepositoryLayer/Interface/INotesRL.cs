using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {
        NotesEntity CreateNote(Notes createNotes, long userID);
        NotesEntity ViewNote(long noteID, long userID);
        List<NotesEntity> ViewAllNotes(long userID);
        NotesEntity UpdateNote(Notes createNotes, long noteID, long userID);
        string DeleteNote(long noteID, long userID);
        bool IsArchieveOrNot(long noteID, long userID);
        bool IsPinnedOrNot(long noteID, long userID);
        bool IsTrashOrNot(long noteID, long userID);
    }
}
