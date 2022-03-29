using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        UserEntity Register(UserReg userReg);
        LoginResponse UserLogin(UserLogin userLogin);
    }
}
