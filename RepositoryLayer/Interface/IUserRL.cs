using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// User Interface
    /// </summary>
    public interface IUserRL
    {
        UserEntity Register(UserReg userReg);
        LoginResponse UserLogin(UserLogin userLogin);
        string ForgotPassword(ForgotPassword forgotPassword);
        string ResetPassword(ResetPassword resetPassword, string emailID);
        UserTicket CreateTicketForPassword(ForgotPassword forgotPassword, string token);
    }
}
