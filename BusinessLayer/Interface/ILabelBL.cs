using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ILabelBL
    {
        LabelEntity AddLabel(NoteLabel label, long userId);
        string RemoveLabel(long labelId, long noteId, long userId);
        IEnumerable<LabelEntity> GetLabels(long noteId, long userId);
        IEnumerable<LabelEntity> GetAllLabels(long userId);
    }
}
