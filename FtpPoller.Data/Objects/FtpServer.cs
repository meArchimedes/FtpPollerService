using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpPollerService.Objects
{
    public class FtpServer
    {
        public Guid Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? FolderDir { get; set; }
    }
}
