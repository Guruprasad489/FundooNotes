using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public LabelEntity AddLabel(NoteLabel label, long userId)
        {
            return labelRL.AddLabel(label, userId);
        }

        public LabelEntity EditLabel(string newName, long labelId, long userId)
        {
            return labelRL.EditLabel(newName,labelId, userId);
        }

        public string RemoveLabel(long labelId, long noteId, long userId)
        {
            return labelRL.RemoveLabel(labelId, noteId, userId);
        }

        public IEnumerable<LabelEntity> GetLabels(long noteId, long userId)
        {
            return labelRL.GetLabels(noteId, userId);
        }

        public IEnumerable<LabelEntity> GetAllLabels(long userId)
        {
            return labelRL.GetAllLabels(userId);
        }
    }
}
