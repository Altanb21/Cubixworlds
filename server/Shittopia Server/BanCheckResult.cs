

using System;

namespace Shittopia_Server
{
    public class BanCheckResult
    {
        public bool isIPBanned;
        public bool isGuidBanned;
        public DateTime guidExpireTime;
        public DateTime ipExpireTime;
    }
}
