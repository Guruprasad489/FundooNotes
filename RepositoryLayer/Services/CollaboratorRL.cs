using CommonLayer.Models;
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
    public class CollaboratorRL : ICollaboratorRL
    {
        private readonly FundooContext fundooContext;

        public CollaboratorRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
        }

        public bool IsRegUser(Collaborator collaborator)
        {
            try
            {
                var regUser = fundooContext.UserEntityTable.Where(x => x.Email == collaborator.EmailId).FirstOrDefault();
                if (regUser != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CollaboratorEntity AddCollaborator(Collaborator collaborator, long noteID, long userID)
        {
            try
            {
                var duplicate = fundooContext.Collaborators.Where(x => x.EmailId == collaborator.EmailId && x.NoteId == noteID && x.UserId == userID).FirstOrDefault();
                var resNote = fundooContext.notesEntityTable.Where(x => x.NoteId == noteID && x.UserId == userID).FirstOrDefault();

                if (duplicate == null && resNote != null)
                {
                    CollaboratorEntity collaboratorEntity = new CollaboratorEntity()
                    {
                        EmailId = collaborator.EmailId,
                        NoteId = noteID,
                        UserId = userID
                    };
                    fundooContext.Collaborators.Add(collaboratorEntity);
                    int res = fundooContext.SaveChanges();
                    if (res > 0)
                        return collaboratorEntity;
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

        public string RemoveCollaborator(long collabID, long noteID, long userID)
        {
            try
            {
                var resCollab = fundooContext.Collaborators.Where(x => x.CollaboratorId == collabID && x.NoteId == noteID && x.UserId == userID).FirstOrDefault();
                if (resCollab != null)
                {
                    fundooContext.Collaborators.Remove(resCollab);
                    int res = fundooContext.SaveChanges();
                    if (res > 0)
                        return "Collaborator removed Successfully";
                    else
                        return "Failed to remove collaborator";
                }
                else
                    return "Failed to remove collaborator";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CollaboratorEntity> GetAllCollaborators(long noteID, long userID)
        {
            try
            {
                var getCollabs = fundooContext.Collaborators.Where(x => x.NoteId == noteID && x.UserId == userID).ToList();
                return getCollabs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
