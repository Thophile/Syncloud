using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syncloud.Model
{
    public class SyncFolder
    {
        public SyncFolder()
        {
            this.Status = Status.New;
        }
        public string Name { get; set; }
        public string Path { get; set; }

        [JsonIgnore]
        public Status Status { get; set; }
    }
    
}
