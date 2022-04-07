using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ILabelRL
    {
        LabelEntity AddLabel(NoteLabel label, long userId);
        LabelEntity EditLabel(string newName, long labelId, long userId);
        string RemoveLabel(long labelId, long noteId, long userId);
        IEnumerable<LabelEntity> GetLabels(long noteId, long userId);
        IEnumerable<LabelEntity> GetAllLabels(long userId);
        List<LabelEntity> GetAll();
    }
}
