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
                var duplicate = fundooContext.Collaborators.Where(x => x.EmailId == collaborator.EmailId).FirstOrDefault();
                bool regUser = IsRegUser(collaborator);

                if (duplicate == null && regUser == true)
                {
                    var resNote = fundooContext.notesEntityTable.Where(x => x.NoteId == noteID && x.UserId == userID).FirstOrDefault();

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

        public CollaboratorEntity RemoveCollaborator(long collabID, long noteID, long userID)
        {
            throw new NotImplementedException();
        }

        public CollaboratorEntity GetAllCollaborators(long noteID, long userID)
        {
            throw new NotImplementedException();
        }
    }
}
