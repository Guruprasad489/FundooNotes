using CommonLayer.Models;
using MassTransit;
using System.Threading.Tasks;

namespace TicketConsumer.Services
{
    /// <summary>
    /// User ticket for consumer
    /// </summary>
    /// <seealso cref="MassTransit.IConsumer&lt;CommonLayer.Models.UserTicket&gt;" />
    public class TicketUser : IConsumer<UserTicket>
    {
        public async Task Consume(ConsumeContext<UserTicket> context)
        {
            var data = context.Message;
            //Validate the Ticket Data
            //Store to Database
            //Notify the user via Email / SMS
        }
    }
}
