using CommonLayer.Models;
using MassTransit;
using System.Threading.Tasks;

namespace TicketConsumer.Services
{
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
