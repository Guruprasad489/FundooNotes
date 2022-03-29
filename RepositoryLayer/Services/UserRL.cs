using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;
        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public UserEntity Register(UserReg userReg)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userReg.FirstName;
                userEntity.LastName = userReg.LastName;
                userEntity.Email = userReg.Email;
                userEntity.Password = userReg.Password;
                fundooContext.Add(userEntity);
                int res = fundooContext.SaveChanges();
                if (res > 0)
                    return userEntity;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserEntity UserLogin(UserLogin userLogin)
        {
            try
            {
                var loginResult = this.fundooContext.UserEntityTable.Where(user => user.Email == userLogin.Email 
                          && user.Password == userLogin.Password).FirstOrDefault();
                if (loginResult != null)
                    return loginResult;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
