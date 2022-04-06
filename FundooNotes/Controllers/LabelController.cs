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
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        public LabelController(ILabelBL labelBL)
        {
            this.labelBL = labelBL;
        }

        [HttpPost("Add")]
        public IActionResult AddLabel(NoteLabel label)
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = labelBL.AddLabel(label, userID);
                if (result != null)
                    return Ok(new { success = true, message = "Label added successfully", data = result });
                else
                    return BadRequest(new { success = false, message = "Faild to Add label" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("Remove")]
        public IActionResult DeleteLabel(long labelId, long noteId)
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = labelBL.RemoveLabel(labelId, noteId, userID);

                if (result.ToLower().Contains("success"))
                    return Ok(new { success = true, message = result });
                else
                    return BadRequest(new { success = false, message = result });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("Get/{noteId}")]
        public IActionResult GetLabels(long noteID)
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = labelBL.GetLabels(noteID, userID);

                if (result != null)
                    return Ok(new { success = true, message = "Labels Display successfull", data = result });
                else
                    return BadRequest(new { success = false, message = "Faild to Display Labels" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllLabels()
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = labelBL.GetAllLabels(userID);

                if (result != null)
                    return Ok(new { success = true, message = "Labels Display successfull", data = result });
                else
                    return BadRequest(new { success = false, message = "Faild to Display Labels" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
