using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface INotesBL
    {
        NotesEntity CreateNote(Notes createNotes, long userID);
        NotesEntity ViewNote(long noteID, long userID);
        List<NotesEntity> ViewAllNotes(long userID);
        NotesEntity UpdateNote(Notes createNotes, long noteID, long userID);
        string DeleteNote(long noteID, long userID);
        NotesEntity IsArchieveOrNot(long noteID, long userID);
        NotesEntity IsPinnedOrNot(long noteID, long userID);
        NotesEntity IsTrashOrNot(long noteID, long userID);
        NotesEntity ChangeColor(string newColor, long noteID, long userID);
        NotesEntity UploadImage(long noteID, long userID, IFormFile imagePath);
        string RemoveImage(long noteID, long userID);
        List<NotesEntity> GetAll();
    }
}
