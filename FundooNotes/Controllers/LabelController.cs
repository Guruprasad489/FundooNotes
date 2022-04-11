using BusinessLayer.Interface;
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
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public LabelController(ILabelBL labelBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.labelBL = labelBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// Adds the label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns></returns>
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
                //NLog.ErrorInfo("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Edits the label.
        /// </summary>
        /// <param name="newName">The new name.</param>
        /// <param name="labelId">The label identifier.</param>
        /// <returns></returns>
        [HttpPatch("Edit")]
        public IActionResult EditLabel(string newName, long labelId)
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = labelBL.EditLabel(newName, labelId, userID);
                if (result != null)
                    return Ok(new { success = true, message = "Label Name modified successfully", data = result });
                else
                    return BadRequest(new { success = false, message = "Faild to Modify label name" });
            }
            catch (Exception ex)
            {
                //NLog.ErrorInfo("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes the label.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <param name="noteId">The note identifier.</param>
        /// <returns></returns>
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
                //NLog.ErrorInfo("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets the labels.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <returns></returns>
        [HttpGet("Get/noteId")]
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
                //NLog.ErrorInfo("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets all labels.
        /// </summary>
        /// <returns></returns>
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
                //NLog.ErrorInfo("Exception: " + ex.Message);
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets all labels using redis cache.
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllLabelsUsingRedisCache()
        {
            var cacheKey = "labelList";
            string serializedLabelList;
            var labelList = new List<LabelEntity>();
            var redisLabelList = await distributedCache.GetAsync(cacheKey);
            if (redisLabelList != null)
            {
                serializedLabelList = Encoding.UTF8.GetString(redisLabelList);
                labelList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
            }
            else
            {
                labelList = labelBL.GetAll();
                serializedLabelList = JsonConvert.SerializeObject(labelList);
                redisLabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLabelList, options);
            }
            return Ok(labelList);
        }
    }
}
