using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollaboratorRL
    {
        bool IsRegUser(Collaborator collaborator);
        CollaboratorEntity AddCollaborator(Collaborator collaborator, long noteID, long userID);
        string RemoveCollaborator(long collabID, long noteID, long userID);
        IEnumerable<CollaboratorEntity> GetAllCollaborators(long noteID, long userID);
    }
}
