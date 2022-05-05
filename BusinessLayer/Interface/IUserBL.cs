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
        string ForgotPassword(ForgotPassword forgotPassword);
        string ResetPassword(ResetPassword resetPassword, string emailID);
        UserTicket CreateTicketForPassword(ForgotPassword forgotPassword, string token);
    }
}
