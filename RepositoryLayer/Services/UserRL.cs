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
    /// <summary>
    /// User repository layer class for User CRUD operations
    /// </summary>
    /// <seealso cref="RepositoryLayer.Interface.IUserRL" />
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

        /// <summary>
        /// Registers the specified user.
        /// </summary>
        /// <param name="userReg">The user reg.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Logins in the specified user.
        /// </summary>
        /// <param name="userLogin">The user login.</param>
        /// <returns></returns>
        public LoginResponse UserLogin(UserLogin userLogin)
        {
            try
            {
                LoginResponse loginResponse = new LoginResponse();
                var loginResult = this.fundooContext.UserEntityTable.Where(user => user.Email == userLogin.Email).FirstOrDefault();
                if (loginResult != null)
                {
                    var passwordResult = this.fundooContext.UserEntityTable.Where(user => user.Email == userLogin.Email && user.Password == userLogin.Password).FirstOrDefault();
                    string decryptPass = DecryptPassword(loginResult.Password);
                    if (decryptPass == userLogin.Password || passwordResult != null)
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

        /// <summary>
        /// Generates the security token.
        /// Method to generate Security key for user (Generating Json Web token) 
        /// </summary>
        /// <param name="emailID">The email identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
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
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Method to get password reset link.
        /// </summary>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        public string ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var email = fundooContext.UserEntityTable.Where(x => x.Email == forgotPassword.Email).FirstOrDefault();
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

        /// <summary>
        /// Creates the ticket for password (RabbitMQ).
        /// </summary>
        /// <param name="emailId">The email identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public UserTicket CreateTicketForPassword(ForgotPassword forgotPassword, string token)
        {
            try
            {
                var userDetails = fundooContext.UserEntityTable.FirstOrDefault(user => user.Email == forgotPassword.Email);
                if (userDetails != null)
                {
                    UserTicket ticketResonse = new UserTicket
                    {
                        FirstName = userDetails.FirstName,
                        LastName = userDetails.LastName,
                        EmailId = forgotPassword.Email,
                        Token = token,
                        IssueAt = DateTime.Now
                    };
                    return ticketResonse;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reset Password for Authenticated emailID after authorization 
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        public string ResetPassword(ResetPassword resetPassword, string emailID)
        {
            try
            {
                if (resetPassword.NewPassword == resetPassword.ConfirmPassword)
                {
                    var userDetails = fundooContext.UserEntityTable.Where(x => x.Email == emailID).FirstOrDefault();
                    userDetails.Password = EncryptPassword(resetPassword.NewPassword);
                    int res = fundooContext.SaveChanges();
                    if (res > 0)
                    {
                        return "Congratulations! Your password has been changed successfully";
                    }
                    else
                        return "Failed to reset your password";
                }
                else
                    return "Make Sure your Passwords Match";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Decrypts the password.
        /// </summary>
        /// <param name="encodedPassword">The encoded password.</param>
        /// <returns></returns>
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
                    //return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
