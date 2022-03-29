using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;


        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
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

        public LoginResponse UserLogin(UserLogin userLogin)
        {
            try
            {
                LoginResponse loginResponse = new LoginResponse();
                var loginResult = this.fundooContext.UserEntityTable.Where(user => user.Email == userLogin.Email 
                          && user.Password == userLogin.Password).FirstOrDefault();
                if (loginResult != null)
                {
                    loginResponse.Token = GenerateSecurityToken(loginResult.Email, loginResult.UserId);
                    loginResponse.Email = loginResult.Email;
                    return loginResponse;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method to generate Security key for user (Generating Json Web token)
        public string GenerateSecurityToken(string emailID, long userId)
        {
            var SecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this.configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, emailID),
                new Claim("UserId", userId.ToString())
            };
            var token = new JwtSecurityToken(
                this.configuration["Jwt:Issuer"],
                this.configuration["jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
