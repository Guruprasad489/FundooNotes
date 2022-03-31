﻿using BusinessLayer.Interface;
using BusinessLayer.Services;
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
    public class NotesController : ControllerBase
    {
        private readonly INotesBL notesBL;
        public NotesController(INotesBL notesBL)
        {
            this.notesBL = notesBL;
        }

        [HttpPost("CreateNote")]
        public IActionResult PostCreateNote(Notes createNotes)
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = notesBL.CreateNote(createNotes, userID);
                if (res != null)
                    return Ok(new { success = true, message = "Note Created successfull", data = res });
                else
                    return BadRequest(new { success = false, message = "Faild to Create Note" });
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("ViewNote/NoteId")]
        public IActionResult GetViewNote(long noteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = notesBL.ViewNote(noteID, userID);
                if (res != null)
                    return Ok(new { success = true, message = "Note Display successfull", data = res });
                else
                    return BadRequest(new { success = false, message = "Faild to Display Note" });
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("ViewAllNotes")]
        public IActionResult GetViewAllNotes()
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = notesBL.ViewAllNotes(userID);
                if (res != null)
                    return Ok(new { success = true, message = "Notes Display successfull", data = res });
                else
                    return BadRequest(new { success = false, message = "Faild to Display Notes" });
            }
            catch (System.Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("UpdateNote")]
        public IActionResult PutUpdateNote(Notes updateNotes, long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.UpdateNote(updateNotes, noteId, userId);
                if (resNote != null)
                    return Ok(new { success = true, message = "Notes Updated Successfully", data = resNote });
                else
                    return BadRequest(new { success = false, message = "Faild to Update Notes" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("DeleteNote")]
        public IActionResult DeleteNote(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.DeleteNote(noteId, userId);
                if (string.IsNullOrEmpty(resNote))
                    return Ok(new { success = true, message = resNote });
                else
                    return BadRequest(new { success = false, message = resNote });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("IsArchiveOrNot")]
        public IActionResult PatchArchieveOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.IsArchieveOrNot(noteId, userId);
                if (resNote == true)
                    return this.Ok(new { Success = true, message = "Archive Status Changed Successfully" });
                else
                    return this.BadRequest(new { Success = false, message = "Failed to change Archive Status" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("IsPinnedOrNot")]
        public IActionResult PatchPinnedOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.IsPinnedOrNot(noteId, userId);
                if (resNote == true)
                    return this.Ok(new { Success = true, message = "Pin Status Changed Successfully" });
                else
                    return this.BadRequest(new { Success = false, message = "Failed to change Pin Status" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("IsTrashOrNot")]
        public IActionResult PatchTrashOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = this.notesBL.IsTrashOrNot(noteId, userId);
                if (resNote == true)
                    return this.Ok(new { Success = true, message = "Trash Status Changed Successfully", data = resNote });
                else
                    return this.BadRequest(new { Success = false, message = "Failed to change Trash Status" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}