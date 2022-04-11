using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    /// <summary>
    /// Collaborator Business Layer Class To Implement methods of IColabBL
    /// </summary>
    public class CollaboratorBL : ICollaboratorBL
    {
        private readonly ICollaboratorRL collaboratorRL;
        public CollaboratorBL(ICollaboratorRL collaboratorRL)
        {
            this.collaboratorRL = collaboratorRL;
        }

        public bool IsRegUser(Collaborator collaborator)
        {
            try
            {
                return collaboratorRL.IsRegUser(collaborator);
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
                return collaboratorRL.AddCollaborator(collaborator, noteID, userID);
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
                return collaboratorRL.RemoveCollaborator(collabID, noteID, userID);
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
                return collaboratorRL.GetAllCollaborators(noteID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CollaboratorEntity> GetAll()
        {
            try
            {
                return collaboratorRL.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
