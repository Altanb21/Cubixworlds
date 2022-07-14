
using System;
using System.Collections.Generic;

namespace Shittopia_Server
{
    public class IPBans
    {
        public Dictionary<string, DateTime> ipBans { get; set; } = new Dictionary<string, DateTime>();

        public Dictionary<string, DateTime> guidBans { get; set; } = new Dictionary<string, DateTime>();
    }
}
