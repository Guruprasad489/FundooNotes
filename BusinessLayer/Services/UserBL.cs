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
    ///  User Business Layer Class To Implement methods of IUserBL
    /// </summary>
    /// <seealso cref="BusinessLayer.Interface.IUserBL" />
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public UserEntity Register(UserReg userReg)
        {
            try
            {
                return userRL.Register(userReg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LoginResponse UserLogin(UserLogin userLogin)
        {
            try
            {
                return userRL.UserLogin(userLogin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ForgotPassword(string emailID)
        {
            try
            {
                return userRL.ForgotPassword(emailID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string ResetPassword(ResetPassword resetPassword, string emailID)
        {
            try
            {
                return userRL.ResetPassword(resetPassword, emailID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public UserTicket CreateTicketForPassword(string emailId, string token)
        {
            try
            {
                return this.userRL.CreateTicketForPassword(emailId, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
