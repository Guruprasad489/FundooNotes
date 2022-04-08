using BusinessLayer.Interface;
using CommonLayer.Models;
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

        [HttpPost("Register")]
        public IActionResult Post(UserReg userReg)
        {
            try
            {
                var res = userBL.Register(userReg);
                if (res != null)
                {
                    _logger.LogInformation("Registration successfull");
                    return Ok(new { success = true, message = "Registration successfull", data = res });
                }
                else
                {
                    _logger.LogError("Failed to Register");
                    return BadRequest(new { success = false, message = "Faild to Register" });
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public IActionResult PostLogin(UserLogin userLogin)
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
                    return BadRequest(new { success = false, message = "Faild to login" });
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("ForgotPassword")]
        public IActionResult PostForgotPassword(string emailID)
        {
            try
            {
                var res = userBL.ForgotPassword(emailID);
                if (res != null)
                    return Ok(new { success = true, message = "Reset link sent successfully",  });
                else
                    return BadRequest(new { success = false, message = "Failed to sent reset link" });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("ResetPassword")]
        [Authorize]
        public IActionResult PutResetPassword(ResetPassword resetPassword)
        {
            try
            {
                var emailID = User.FindFirst(ClaimTypes.Email).Value;
                var res = userBL.ResetPassword(resetPassword, emailID);
                if (res != null)
                    return Ok(new { success = true, message = res, });
                else
                    return BadRequest(new { success = false, message = res });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
