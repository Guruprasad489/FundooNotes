using BusinessLayer.Interface;
using CommonLayer.Models;
using FundooNotes.CustomException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserBL userBL, ILogger<UserController> logger)
        {
            this.userBL = userBL;
            this._logger = logger;
        }

        /// <summary>
        /// Post request for register.
        /// Registers the specified user reg.
        /// </summary>
        /// <param name="userReg">The user reg.</param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register(UserReg userReg)
        {
            try
            {
                var res = userBL.Register(userReg);
                if (res != null)
                {
                    _logger.LogInformation("Registration successfull");
                    return Created("User Registration sucessfull", new { success = true, data = res });
                    //return Ok(new { success = true, message = "Registration successfull", data = res });
                }
                else
                {
                    _logger.LogError("Failed to Register");
                    throw new CustomAppException("Faild to Register");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post request for login
        /// Logins the specified user login.
        /// </summary>
        /// <param name="userLogin">The user login.</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            try
            {
                var res = userBL.UserLogin(userLogin);
                if (res != null)
                {
                    _logger.LogInformation("Login successfull: "+ userLogin.Email);
                    return Ok(new { success = true, message = "Logged in successfully", Email = res.Email, Token = res.Token });
                }
                else
                {
                    _logger.LogError("Failed to login: "+ userLogin.Email);
                    throw new CustomAppException("Faild to Login");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Post request for Forgot password.
        /// </summary>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var res = userBL.ForgotPassword(forgotPassword);
                if (res != null)
                {
                    _logger.LogInformation("Reset link sent successfully to: "+ forgotPassword.Email);
                    return Ok(new { success = true, message = "Reset link sent successfully"  });
                }
                else
                {
                    _logger.LogError("Failed to send reset link:");
                    throw new CustomAppException("Failed to send reset link");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Put request for Reset password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPut("ResetPassword")]
        [Authorize]
        public IActionResult ResetPassword(ResetPassword resetPassword)
        {
            try
            {
                var emailID = User.FindFirst(ClaimTypes.Email).Value;
                var res = userBL.ResetPassword(resetPassword, emailID);
                if (res.ToLower().Contains("match"))
                {
                    _logger.LogError(res);
                    throw new CustomAppException(res);
                }
                else if (res.ToLower().Contains("success"))
                {
                    _logger.LogInformation(res);
                    return Ok(new { success = true, message = res, });
                }
                else
                {
                    _logger.LogError("Failed to Reset Password");
                    throw new CustomAppException(res);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
