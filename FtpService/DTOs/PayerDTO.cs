using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpPollerService.DTOs
{
    public class PayerDTO
    {
        public Guid Id { get; set; }
        public string SrcServer { get; set; }
        public int SrcPort { get; set; }
        public string FolderDir { get; set; }
        public string SrcUserName { get; set; }
        public string SrcPassword { get; set; }
        public List<string> FilePaths { get; set; }
    }
}
