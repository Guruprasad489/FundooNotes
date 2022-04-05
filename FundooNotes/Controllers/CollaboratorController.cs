using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBL collaboratorBL;
        public CollaboratorController(ICollaboratorBL collaboratorBL)
        {
            this.collaboratorBL = collaboratorBL;
        }

        [HttpPost("Add")]
        public IActionResult AddCollab(Collaborator collaborator, long noteID)
        {
            try
            {
                bool regUser = collaboratorBL.IsRegUser(collaborator);
                if (regUser == true)
                {
                    //Id Of Authorized User Using JWT Claims
                    long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                    var resCollab = collaboratorBL.AddCollaborator(collaborator, noteID, userID);
                    if (resCollab != null)
                        return Ok(new { success = true, message = "Collaboration successfull", data = resCollab });
                    else
                        return BadRequest(new { success = false, message = "Faild to Collaborate" });
                }
                else
                    return BadRequest(new { success = false, message = "Can't to Collaborate with non-Regestered User" });

            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("Remove")]
        public IActionResult RemoveCollab(long collabID, long noteID)
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resCollab = collaboratorBL.RemoveCollaborator(collabID, noteID, userID);

                if (resCollab.ToLower().Contains("success"))
                    return Ok(new { success = true, message = resCollab });
                else
                    return BadRequest(new { success = false, message = resCollab });
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
