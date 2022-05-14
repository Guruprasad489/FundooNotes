using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INotesBL notesBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<UserController> _logger;

        public NotesController(INotesBL notesBL, IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<UserController> logger)
        {
            this.notesBL = notesBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this._logger = logger;
        }

        /// <summary>
        /// Creates the note.
        /// </summary>
        /// <param name="createNotes">The create notes.</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public IActionResult CreateNote(Notes createNotes)
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = notesBL.CreateNote(createNotes, userID);
                if (res != null)
                {
                    _logger.LogInformation("Note Created successfully: " + res.Title);
                    return Ok(new { success = true, message = "Note Created successfully", data = res });
                }
                else
                {
                    _logger.LogError("Faild to Create Note: " + res.Title);
                    return BadRequest(new { success = false, message = "Faild to Create Note" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: "+ ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// View the note by note ID.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <returns></returns>
        [HttpGet("View/noteId")]
        public IActionResult ViewNote(long noteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = notesBL.ViewNote(noteID, userID);
                if (res != null)
                {
                    _logger.LogInformation("Note Display successfull"+ res.Title);
                    return Ok(new { success = true, message = "Note Display successfull", data = res });
                }
                else
                {
                    _logger.LogError("Faild to Display Note"+ res.Title);
                    return BadRequest(new { success = false, message = "Faild to Display Note" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// View all notes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult ViewAllNotes()
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = notesBL.ViewAllNotes(userID);
                if (res != null)
                {
                    _logger.LogInformation("All Notes Displayed successfully");
                    return Ok(new { success = true, message = "All Notes Displayed successfully", data = res });
                }
                else
                {
                    _logger.LogError("Faild to Display Notes");
                    return BadRequest(new { success = false, message = "Faild to Display Notes" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Updates the note.
        /// </summary>
        /// <param name="updateNotes">The update notes.</param>
        /// <param name="noteId">The note identifier.</param>
        /// <returns></returns>
        [HttpPut("Update")]
        public IActionResult UpdateNote(Notes updateNotes, long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.UpdateNote(updateNotes, noteId, userId);
                if (resNote != null)
                {
                    _logger.LogInformation("Note Updated Successfully"+ resNote.Title);
                    return Ok(new { success = true, message = "Note Updated Successfully", data = resNote });
                }
                else
                {
                    _logger.LogError("Faild to Update Note"+ resNote.Title);
                    return BadRequest(new { success = false, message = "Faild to Update Note" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes the note.
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public IActionResult DeleteNote(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.DeleteNote(noteId, userId);
                if (resNote.Contains("Success"))
                {
                    _logger.LogInformation(resNote);
                    return Ok(new { success = true, message = resNote });
                }
                else
                {
                    _logger.LogError(resNote);
                    return BadRequest(new { success = false, message = resNote });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Determines whether [is archieve or not] [the specified note identifier].
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <returns></returns>
        [HttpPatch("IsArchive")]
        public IActionResult IsArchieveOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.IsArchieveOrNot(noteId, userId);
                if (resNote != null)
                {
                    _logger.LogInformation("Archive Status Changed Successfully: "+ resNote.Title);
                    return Ok(new { Success = true, message = "Archive Status Changed Successfully", data = resNote });
                }
                else
                {
                    _logger.LogError("Failed to change Archive Status: "+ resNote.Title);
                    return BadRequest(new { Success = false, message = "Failed to change Archive Status" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Determines whether [is pinned or not] [the specified note identifier].
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <returns></returns>
        [HttpPatch("IsPinned")]
        public IActionResult IsPinnedOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.IsPinnedOrNot(noteId, userId);
                if (resNote != null)
                {
                    _logger.LogInformation("Pin Status Changed Successfully: "+ resNote.Title);
                    return Ok(new { Success = true, message = "Pin Status Changed Successfully" , data = resNote });
                }
                else
                {
                    _logger.LogError("Failed to change Pin Status: "+ resNote.Title);
                    return BadRequest(new { Success = false, message = "Failed to change Pin Status" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Determines whether [is trash or not] [the specified note identifier].
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <returns></returns>
        [HttpPatch("IsTrash")]
        public IActionResult IsTrashOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.IsTrashOrNot(noteId, userId);
                if (resNote != null)
                {
                    _logger.LogInformation("Trash Status Changed Successfully: "+ resNote.Title);
                    return Ok(new { Success = true, message = "Trash Status Changed Successfully", data = resNote });
                }
                else
                {
                    _logger.LogError("Failed to change Trash Status: "+ resNote.Title);
                    return BadRequest(new { Success = false, message = "Failed to change Trash Status" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Changes the color.
        /// </summary>
        /// <param name="newColor">The new color.</param>
        /// <param name="noteId">The note identifier.</param>
        /// <returns></returns>
        [HttpPatch("ChangeColor")]
        public IActionResult ChangeColor(ChangeColor color)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.ChangeColor(color.NewColor , color.NoteId, userId);
                if (resNote != null)
                {
                    _logger.LogInformation("Color Changed Successfully: "+ resNote.Title);
                    return Ok(new { Success = true, message = "Color Changed Successfully", data = resNote });
                }
                else
                {
                    _logger.LogError("Failed to change Color: "+ resNote.Title);
                    return BadRequest(new { Success = false, message = "Failed to change Color" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        [HttpPatch("UploadImage/noteId")]
        public IActionResult UploadImage(long noteId, IFormFile imagePath)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.UploadImage(noteId, userId, imagePath);
                if (resNote != null)
                {
                    _logger.LogInformation("Image Uploaded Successfully: "+ resNote.Title);
                    return Ok(new { Success = true, message = "Image Uploaded Successfully", data = resNote });
                }
                else
                {
                    _logger.LogError("Failed to Upload Image: "+ resNote.Title);
                    return BadRequest(new { Success = false, message = "Failed to Upload Image" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Removes the image.
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <returns></returns>
        [HttpPatch("RemoveImage/noteId")]
        public IActionResult RemoveImage(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.RemoveImage(noteId, userId);
                if (resNote.Contains("Success"))
                {
                    _logger.LogInformation(resNote);
                    return Ok(new { success = true, message = resNote });
                }
                else
                {
                    _logger.LogError(resNote);
                    return BadRequest(new { success = false, message = resNote });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets all notes using redis cache.
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "notesList";
            string serializedNotesList;
            var notesList = new List<NotesEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                notesList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
            }
            else
            {
                notesList = notesBL.GetAll();
                serializedNotesList = JsonConvert.SerializeObject(notesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(notesList);
        }

        /// <summary>
        /// Gets the notes by label.
        /// </summary>
        /// <param name="labelID">The label identifier.</param>
        /// <returns></returns>
        [HttpGet("View/labelId")]
        public IActionResult GetNotesByLabel(long labelID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var res = notesBL.GetNotesByLabel(labelID, userID);
                if (res != null)
                {
                    _logger.LogInformation("Note By Label Display successfull");
                    return Ok(new { success = true, message = "Note By Label Display successfull", data = res });
                }
                else
                {
                    _logger.LogError("Faild to Display Note By Label");
                    return BadRequest(new { success = false, message = "Faild to Display Note By Label" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
