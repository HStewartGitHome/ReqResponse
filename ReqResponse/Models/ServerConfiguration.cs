using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqResponse.Models
{
    public class ServerConfiguration
    {
        public string PrimaryServer { get; set; }
        public int PrimaryPort { get; set; }
        public string BackupServer { get; set; }
        public int BackupPort { get; set; }
        public bool AllowBackup { get; set; }
        public bool PrimarySwitchBack { get; set; }
        public bool OnPrimary { get; set;  }
        public int NetLimit { get; set; }
        public int StayNetLimit { get; set; }
        public bool OutputConfiguration { get; set; }
        public int DebugOption { get; set; }
        public int ServerDebugOption { get; set; }
        public int TestOption { get; set; }
    }
}
