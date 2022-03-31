﻿using CommonLayer.Models;
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
    public class NotesRL : INotesRL
    {
        private readonly FundooContext fundooContext;

        public NotesRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
        }

        public NotesEntity CreateNote(Notes createNotes, long userID)
        {
            try
            {
                var duplicate = fundooContext.notesEntityTable.Where(x => x.Title == createNotes.Title).FirstOrDefault();
                if (duplicate == null)
                {
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
                }
                else
                    return null;
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
                var getNote = fundooContext.notesEntityTable.Where(x => x.NoteId == noteID).FirstOrDefault();
                //getNote.Reminder.ToString("dd-MM-yyyy hh:mm:ss tt");
                //getNote.CreatedAt.ToString("dd-MM-yyyy hh:mm:ss tt");
                //getNote.ModifiedAt.ToString("dd-MM-yyyy hh:mm:ss tt");
                return getNote;
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
                var getNotes = fundooContext.notesEntityTable.Where(x => x.UserId == userID).ToList();
                return getNotes;
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

        public bool IsArchieveOrNot(long noteID, long userID)
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
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsPinnedOrNot(long noteID, long userID)
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
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsTrashOrNot(long noteID, long userID)
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
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}