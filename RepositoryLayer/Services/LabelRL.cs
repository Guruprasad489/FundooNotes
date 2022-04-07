using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRL : ILabelRL
    {
        private readonly FundooContext fundooContext;

        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public LabelEntity AddLabel(NoteLabel label, long userId)
        {
            try
            {
                var duplicate = fundooContext.LabelTable.Where(x => x.LabelName == label.LabelName && x.NoteId == label.NoteId && x.UserId == userId).FirstOrDefault();
                var resNote = fundooContext.notesEntityTable.Where(x => x.NoteId == label.NoteId && x.UserId == userId).FirstOrDefault();

                if (duplicate == null && resNote != null)
                {
                    LabelEntity labelEntity = new LabelEntity()
                    {
                        LabelName = label.LabelName,
                        NoteId = label.NoteId,
                        UserId = userId
                    };
                    fundooContext.LabelTable.Add(labelEntity);
                    int res = fundooContext.SaveChanges();
                    if (res > 0)
                        return labelEntity;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LabelEntity EditLabel(string newName, long labelId, long userId)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(x => x.LabelId == labelId && x.UserId == userId).FirstOrDefault();

                if (result != null)
                {
                    result.LabelName = newName;
                    
                    fundooContext.LabelTable.Update(result);
                    int res = fundooContext.SaveChanges();
                    if (res > 0)
                        return result;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RemoveLabel(long labelId, long noteId, long userId)
        {
            try
            {
                var resLabel = fundooContext.LabelTable.Where(x => x.LabelId == labelId && x.NoteId == noteId && x.UserId == userId).FirstOrDefault();

                fundooContext.LabelTable.Remove(resLabel);
                int res = fundooContext.SaveChanges();
                if (res > 0)
                    return "Label removed successfully";
                else
                    return "Failed to remove the label";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<LabelEntity> GetLabels(long noteId, long userId)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(x => x.NoteId == noteId && x.UserId == userId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<LabelEntity> GetAllLabels(long userId)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(x => x.UserId == userId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LabelEntity> GetAll()
        {
            try
            {
                var getLabels = fundooContext.LabelTable.ToList();
                return getLabels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
