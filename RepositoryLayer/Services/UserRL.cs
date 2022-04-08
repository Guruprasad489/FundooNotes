﻿using CommonLayer.Models;
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
        private static string Key = "36c53aa7571c33d2f98d02a4313c4ba1ea15e45c18794eb564b21c19591805g";

        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        public UserEntity Register(UserReg userReg)
        {
            try
            {
                var duplicate = fundooContext.UserEntityTable.Where(x => x.Email == userReg.Email).FirstOrDefault();
                if (duplicate == null)
                {
                    UserEntity userEntity = new UserEntity();
                    userEntity.FirstName = userReg.FirstName;
                    userEntity.LastName = userReg.LastName;
                    userEntity.Email = userReg.Email;
                    userEntity.Password = EncryptPassword(userReg.Password);
                    fundooContext.UserEntityTable.Add(userEntity);
                    int res = fundooContext.SaveChanges();
                    if (res > 0)
                        return userEntity;
                    else
                        return null;
                }
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
                var loginResult = this.fundooContext.UserEntityTable.Where(user => user.Email == userLogin.Email).FirstOrDefault();
                if (loginResult != null)
                {
                    string decryptPass = DecryptPassword(loginResult.Password);
                    if (decryptPass == userLogin.Password)
                    {
                        loginResponse.Token = GenerateSecurityToken(loginResult.Email, loginResult.UserId);
                        loginResponse.Email = loginResult.Email;
                        return loginResponse;
                    }
                    else
                        return null;
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
                this.configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ForgotPassword(string emailID)
        {
            try
            {
                var email = fundooContext.UserEntityTable.Where(x => x.Email == emailID).FirstOrDefault();
                if (email != null)
                {
                    var token = GenerateSecurityToken(email.Email, email.UserId);
                    new Msmq().SendMessage(token);
                    return token;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Reset Password for Authenticated emailID after authorization
        public string ResetPassword(ResetPassword resetPassword, string emailID)
        {
            try
            {
                if (resetPassword.NewPassword == resetPassword.ConfirmPassword)
                {
                    var userDetails = fundooContext.UserEntityTable.Where(x => x.Email == emailID).FirstOrDefault();
                    userDetails.Password = EncryptPassword(resetPassword.NewPassword);
                    fundooContext.SaveChanges();
                    return "Congratulations! Your password has been changed successfully";
                }
                else
                    return "Make Sure your Passwords Match";

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Method To Encrypt The Password
        public static string EncryptPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                    return null;
                else
                {
                    password += Key;
                    var passwordBytes = Encoding.UTF8.GetBytes(password);
                    return Convert.ToBase64String(passwordBytes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Decrypt The Password
        public static string DecryptPassword(string encodedPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(encodedPassword))
                    return null;
                else
                {
                    var encodedBytes = Convert.FromBase64String(encodedPassword);
                    var res = Encoding.UTF8.GetString(encodedBytes);
                    var resPass = res.Substring(0, res.Length - Key.Length);
                    return resPass;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
