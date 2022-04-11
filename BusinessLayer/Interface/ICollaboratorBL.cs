using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface For Colaborator Business Layer Class
    /// </summary>
    public interface ICollaboratorBL
    {
        bool IsRegUser(Collaborator collaborator);
        CollaboratorEntity AddCollaborator(Collaborator collaborator, long noteID, long userID);
        string RemoveCollaborator(long collabID, long noteID, long userID);
        IEnumerable<CollaboratorEntity> GetAllCollaborators(long noteID, long userID);
        List<CollaboratorEntity> GetAll();
    }
}
