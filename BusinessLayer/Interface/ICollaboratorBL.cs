﻿using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ICollaboratorBL
    {
        bool IsRegUser(Collaborator collaborator);
        CollaboratorEntity AddCollaborator(Collaborator collaborator, long noteID, long userID);
        string RemoveCollaborator(long collabID, long noteID, long userID);
        CollaboratorEntity GetAllCollaborators(long noteID, long userID);
    }
}