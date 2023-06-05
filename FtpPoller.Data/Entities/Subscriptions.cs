using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpPoller.Data.Entities
{
    public class Subscriptions
    {
        public Guid Id { get; set; }
        public Guid PayerId { get; set; }
        public Guid RecipientId { get; set; }
    }
}
