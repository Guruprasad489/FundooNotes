using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
    /// Notes repository layer class for Notes CRUD operations
    /// </summary>
    /// <seealso cref="RepositoryLayer.Interface.INotesRL" />
    public class NotesRL : INotesRL
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;

        public NotesRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        /// <summary>
        /// Creates the note.
        /// </summary>
        /// <param name="createNotes">The create notes.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public NotesEntity CreateNote(Notes createNotes, long userID)
        {
            try
            {
                //var duplicate = fundooContext.notesEntityTable.Where(x => x.Title == createNotes.Title).FirstOrDefault();
                //if (duplicate == null)
                //{
                    NotesEntity notesEntity = new NotesEntity()
                    {
                        Title = createNotes.Title,
                        Description = createNotes.Description,
                        Color = createNotes.Color,
                        Image = createNotes.Image,
                        Reminder = createNotes.Reminder,
                        IsArchive = createNotes.IsArchive,
                        IsPin = createNotes.IsPin,
                        IsTrash = createNotes.IsTrash,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        UserId = userID
                    };
                    fundooContext.notesEntityTable.Add(notesEntity);
                    int res = fundooContext.SaveChanges();
                    if (res > 0)
                        return notesEntity;
                    else
                        return null;
                //}
                //else
                //    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// View the note by NoteID.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public NotesEntity ViewNote(long noteID, long userID)
        {
            try
            {
                NotesEntity noteRes = null;
                var collabRes = fundooContext.Collaborators.FirstOrDefault(x => x.UserId == userID && x.NoteId == noteID);
                if (collabRes != null)
                    noteRes = fundooContext.notesEntityTable.Where(n => n.NoteId == collabRes.NoteId).FirstOrDefault();
                else
                    noteRes = fundooContext.notesEntityTable.Where(n => n.UserId == userID && n.NoteId == noteID).FirstOrDefault();
                return noteRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// View all notes.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public List<NotesEntity> ViewAllNotes(long userID)
        {
            try
            {
                var getNotes = fundooContext.notesEntityTable.Where(x => x.UserId == userID).ToList();
                return getNotes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the note.
        /// </summary>
        /// <param name="updateNotes">The update notes.</param>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public NotesEntity UpdateNote(Notes updateNotes, long noteID, long userID)
        {
            try
            {
                var resNote = fundooContext.notesEntityTable.Where(n => n.NoteId == noteID && n.UserId == userID).FirstOrDefault();
                if (resNote != null)
                {
                    resNote.Title = string.IsNullOrEmpty(updateNotes.Title) ? resNote.Title : updateNotes.Title;
                    resNote.Description = string.IsNullOrEmpty(updateNotes.Description) ? resNote.Description : updateNotes.Description;
                    resNote.Reminder = updateNotes.Reminder.CompareTo(resNote.Reminder) == 0 ? resNote.Reminder : updateNotes.Reminder;
                    resNote.Color = string.IsNullOrEmpty(updateNotes.Color) ? resNote.Color : updateNotes.Color;
                    resNote.Image = string.IsNullOrEmpty(updateNotes.Image) ? resNote.Image : updateNotes.Image;
                    resNote.IsTrash = updateNotes.IsTrash.CompareTo(resNote.IsTrash) == 0 ? resNote.IsTrash : updateNotes.IsTrash;
                    resNote.IsArchive = updateNotes.IsArchive.CompareTo(resNote.IsArchive) == 0 ? resNote.IsArchive : updateNotes.IsArchive;
                    resNote.IsPin = updateNotes.IsPin.CompareTo(resNote.IsPin) == 0 ? resNote.IsPin : updateNotes.IsPin;
                    resNote.ModifiedAt = DateTime.Now;

                    // Update And Save The Changes In The Database For Given NoteId.
                    fundooContext.notesEntityTable.Update(resNote);
                    fundooContext.SaveChanges();
                    return resNote;
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
        /// Deletes the note.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public string DeleteNote(long noteID, long userID)
        {
            try
            {
                var resNote = fundooContext.notesEntityTable.Where(n => n.NoteId == noteID && n.UserId == userID).FirstOrDefault();
                if (resNote != null)
                {
                    fundooContext.notesEntityTable.Remove(resNote);
                    fundooContext.SaveChanges();
                    return "Note Deleted Successfully";
                }
                else
                    return "Failed To Delete The Note";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether [is archieve or not] [the specified note identifier].
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public NotesEntity IsArchieveOrNot(long noteID, long userID)
        {
            try
            {
                var resNote = fundooContext.notesEntityTable.Where(n => n.NoteId == noteID && n.UserId == userID).FirstOrDefault();
                if (resNote != null)
                {
                    if (resNote.IsArchive == false)
                        resNote.IsArchive = true;
                    else
                        resNote.IsArchive = false;
                    fundooContext.SaveChanges();
                    return resNote;
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
        /// Determines whether [is pinned or not] [the specified note identifier].
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public NotesEntity IsPinnedOrNot(long noteID, long userID)
        {
            try
            {
                var resNote = fundooContext.notesEntityTable.Where(n => n.NoteId == noteID && n.UserId == userID).FirstOrDefault();
                if (resNote != null)
                {
                    if (resNote.IsPin == false)
                        resNote.IsPin = true;
                    else
                        resNote.IsPin = false;
                    fundooContext.SaveChanges();
                    return resNote;
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
        /// Determines whether [is trash or not] [the specified note identifier].
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public NotesEntity IsTrashOrNot(long noteID, long userID)
        {
            try
            {
                var resNote = fundooContext.notesEntityTable.Where(n => n.NoteId == noteID && n.UserId == userID).FirstOrDefault();
                if (resNote != null)
                {
                    if (resNote.IsTrash == false)
                        resNote.IsTrash = true;
                    else
                        resNote.IsTrash = false;
                    fundooContext.SaveChanges();
                    return resNote;
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
        /// Changes the color.
        /// </summary>
        /// <param name="newColor">The new color.</param>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public NotesEntity ChangeColor(string newColor, long noteID, long userID)
        {
            try
            {
                var resNote = fundooContext.notesEntityTable.Where(n => n.NoteId == noteID && n.UserId == userID).FirstOrDefault();
                if (resNote != null)
                {
                    resNote.Color = string.IsNullOrEmpty(newColor) ? resNote.Color : newColor;
                    resNote.ModifiedAt = DateTime.Now;
                    //fundooContext.notesEntityTable.Update(resNote);
                    fundooContext.SaveChanges();
                    return resNote;
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
        /// Uploads the image.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public NotesEntity UploadImage(long noteID, long userID, IFormFile imagePath)
        {
            try
            {
                var resNote = fundooContext.notesEntityTable.Where(n => n.NoteId == noteID && n.UserId == userID).FirstOrDefault();
                if (resNote != null)
                {
                    //Account account = new Account("guruprasad489", "495828661761912", "WhoWnj9p7haOK6-X_DRuJVRTvJE");
                    Account account = new Account(configuration["Cloudinary:CloudName"], configuration["Cloudinary:ApiKey"], configuration["Cloudinary:ApiSecret"]);
                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imagePath.FileName, imagePath.OpenReadStream()),
                    };
                    var uploadResult = cloudinary.Upload(uploadParams);

                    if (uploadResult != null)
                    {
                        resNote.Image = uploadResult.Url.ToString();
                        resNote.ModifiedAt = DateTime.Now;
                        fundooContext.notesEntityTable.Update(resNote);
                        fundooContext.SaveChanges();
                        return resNote;
                    }
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
        /// Removes the image.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public string RemoveImage(long noteID, long userID)
        {
            try
            {
                var resNote = fundooContext.notesEntityTable.Where(x => x.NoteId == noteID && x.UserId == userID).FirstOrDefault();
                if (resNote != null)
                {
                    resNote.Image = null;
                    resNote.ModifiedAt = DateTime.Now;
                    fundooContext.SaveChanges();
                    return "Image Removed Successfully";
                }
                else
                    return "Failed to remove the image";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all data from NotesENtityTable
        /// </summary>
        /// <returns></returns>
        public List<NotesEntity> GetAll()
        {
            try
            {
                var getNotes = fundooContext.notesEntityTable.ToList();
                return getNotes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the notes by label.
        /// </summary>
        /// <param name="labelID">The label identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public List<NotesEntity> GetNotesByLabel(long labelID, long userID)
        {
            try
            {
                var labelList = fundooContext.LabelTable.Where(x => x.LabelId == labelID).ToList();
                if (labelList.Count() > 0)
                {
                    List<NotesEntity> getNotes = null;
                    foreach (var label in labelList)
                    {
                        getNotes = fundooContext.notesEntityTable.Where(x => x.NoteId == label.NoteId && x.UserId == userID).ToList();
                    }
                    return getNotes;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
