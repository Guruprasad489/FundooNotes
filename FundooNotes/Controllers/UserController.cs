using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpPost("Register")]
        public IActionResult Post(UserReg userReg)
        {
            try
            {
                var res = userBL.Register(userReg);
                if (res != null)
                    return Ok(new { success = true, message = "Registration successfull", data = res });
                else
                    return BadRequest(new { success = false, message = "Faild to Register" });
            }
            catch (System.Exception ex)
            {

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
                    return Ok(new { success = true, message = "Logged in successfully", Email = res.Email, Token = res.Token });
                else
                    return BadRequest(new { success = false, message = "Faild to login" });
            }
            catch (System.Exception ex)
            {
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
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
