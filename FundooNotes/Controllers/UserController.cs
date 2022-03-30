using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                    return Ok(new { success = true, message = "Data posted successfully", data = res });
                else
                    return BadRequest(new { success = false, message = "Faild to post the data" });
            }
            catch (System.Exception ex)
            {

                return NotFound(new { success = false, message = ex.Message});
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

        [HttpPost("ForgetPassword")]
        public IActionResult PostForgetPassword(string emailID)
        {
            try
            {
                var res = userBL.ForgetPassword(emailID);
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
    }
}
