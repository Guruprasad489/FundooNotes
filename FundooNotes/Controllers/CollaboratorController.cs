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
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBL collaboratorBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public CollaboratorController(ICollaboratorBL collaboratorBL, ILabelBL labelBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.collaboratorBL = collaboratorBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        /// <summary>
        /// Adds the collab.
        /// </summary>
        /// <param name="collaborator">The collaborator.</param>
        /// <param name="noteID">The note identifier.</param>
        /// <returns></returns>
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
                    return NotFound(new { success = false, message = "Can't to Collaborate with non-Regestered User" });

            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Removes the collab.
        /// </summary>
        /// <param name="collabID">The collab identifier.</param>
        /// <param name="noteID">The note identifier.</param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets all collabs.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllCollabs(long noteID)
        {
            try
            {
                //Id Of Authorized User Using JWT Claims
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var resCollab = collaboratorBL.GetAllCollaborators(noteID, userID);

                if (resCollab != null)
                    return Ok(new { success = true, message = "Collabs Display successfull", data = resCollab });
                else
                    return BadRequest(new { success = false, message = "Faild to Display Collabs" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets all collabs using redis cache.
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCollabsUsingRedisCache()
        {
            var cacheKey = "collabList";
            string serializedCollabList;
            var collabList = new List<CollaboratorEntity>();
            var redisCollabList = await distributedCache.GetAsync(cacheKey);
            if (redisCollabList != null)
            {
                serializedCollabList = Encoding.UTF8.GetString(redisCollabList);
                collabList = JsonConvert.DeserializeObject<List<CollaboratorEntity>>(serializedCollabList);
            }
            else
            {
                collabList = collaboratorBL.GetAll();
                serializedCollabList = JsonConvert.SerializeObject(collabList);
                redisCollabList = Encoding.UTF8.GetBytes(serializedCollabList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollabList, options);
            }
            return Ok(collabList);
        }
    }
}
