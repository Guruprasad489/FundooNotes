using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface For User Business Layer Class
    /// </summary>
    public interface IUserBL
    {
        UserEntity Register(UserReg userReg);
        LoginResponse UserLogin(UserLogin userLogin);
        string ForgotPassword(string emailID);
        string ResetPassword(ResetPassword resetPassword, string emailID);
        UserTicket CreateTicketForPassword(string emailId, string token);
    }
}
