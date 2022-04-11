using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface For Label Business Layer Class
    /// </summary>
    public interface ILabelBL
    {
        LabelEntity AddLabel(NoteLabel label, long userId);
        LabelEntity EditLabel(string newName, long labelId, long userId);
        string RemoveLabel(long labelId, long noteId, long userId);
        IEnumerable<LabelEntity> GetLabels(long noteId, long userId);
        IEnumerable<LabelEntity> GetAllLabels(long userId);
        List<LabelEntity> GetAll();
    }
}
