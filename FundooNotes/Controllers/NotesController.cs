using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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

        public NotesController(INotesBL notesBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.notesBL = notesBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        [HttpPost("Create")]
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

        [HttpGet("View/noteId")]
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

        [HttpGet("GetAll")]
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

        [HttpPut("Update")]
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

        [HttpDelete("Delete")]
        public IActionResult DeleteNote(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.DeleteNote(noteId, userId);
                if (resNote.Contains("Success"))
                    return Ok(new { success = true, message = resNote });
                else
                    return BadRequest(new { success = false, message = resNote });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("IsArchive")]
        public IActionResult PatchArchieveOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.IsArchieveOrNot(noteId, userId);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Archive Status Changed Successfully", data = resNote });
                else
                    return BadRequest(new { Success = false, message = "Failed to change Archive Status" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("IsPinned")]
        public IActionResult PatchPinnedOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.IsPinnedOrNot(noteId, userId);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Pin Status Changed Successfully" , data = resNote });
                else
                    return BadRequest(new { Success = false, message = "Failed to change Pin Status" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("IsTrash")]
        public IActionResult PatchTrashOrNot(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.IsTrashOrNot(noteId, userId);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Trash Status Changed Successfully", data = resNote });
                else
                    return BadRequest(new { Success = false, message = "Failed to change Trash Status" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("ChangeColor")]
        public IActionResult PatchChangeColor(string newColor, long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.ChangeColor(newColor, noteId, userId);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Color Changed Successfully", data = resNote });
                else
                    return BadRequest(new { Success = false, message = "Failed to change Color" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("UploadImage/noteId")]
        public IActionResult PatchUploadImage(long noteId, IFormFile imagePath)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.UploadImage(noteId, userId, imagePath);
                if (resNote != null)
                    return Ok(new { Success = true, message = "Image Uploaded Successfully", data = resNote });
                else
                    return BadRequest(new { Success = false, message = "Failed to Upload Image" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpPatch("RemoveImage/noteId")]
        public IActionResult PatchRemoveImage(long noteId)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resNote = notesBL.RemoveImage(noteId, userId);
                if (resNote.Contains("Success"))
                    return Ok(new { success = true, message = resNote });
                else
                    return BadRequest(new { success = false, message = resNote });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
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
    }
}
