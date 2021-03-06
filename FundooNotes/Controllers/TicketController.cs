using BusinessLayer.Interface;
using CommonLayer.Models;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IUserBL userBL;

        public TicketController(IBus bus, IUserBL userBL)
        {
            this._bus = bus;
            this.userBL = userBL;
        }

        /// <summary>
        /// Creates the ticket for password.
        /// </summary>
        /// <param name="emailId">The email identifier.</param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> CreateTicketForPassword(ForgotPassword forgotPassword)
        {
            try
            {
                if (forgotPassword.Email != null)
                {
                    var token = userBL.ForgotPassword(forgotPassword);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var ticketResponse = userBL.CreateTicketForPassword(forgotPassword, token);
                        Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                        var endPoint = await _bus.GetSendEndpoint(uri);
                        await endPoint.Send(ticketResponse);
                        return Ok(new { success = true, message = "Email Sent Successfully" });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Email Id Is Not Registered" });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something Went Wrong" });
                }
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}