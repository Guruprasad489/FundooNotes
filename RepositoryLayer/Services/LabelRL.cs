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
    /// <summary>
    /// Label repository layer class for Label CRUD operations
    /// </summary>
    /// <seealso cref="RepositoryLayer.Interface.ILabelRL" />
    public class LabelRL : ILabelRL
    {
        private readonly FundooContext fundooContext;

        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        /// <summary>
        /// Adds the label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Edits the label.
        /// </summary>
        /// <param name="newName">The new name.</param>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes the label.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="noteId">The note identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the labels by NoteID.
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all labels.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all data from Labeltable
        /// </summary>
        /// <returns></returns>
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
