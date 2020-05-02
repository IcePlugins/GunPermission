using System.Collections.Generic;
using Rocket.API;

namespace GunPermission
{
    public class GunPermissionConfiguration : IRocketPluginConfiguration
    {

        public List<string> Permissions;
        public List<ushort> BlacklistedGunIds;


        public Dictionary<string, ushort> BlacklistedBypassPermissions;

        public List<ulong> WhitelistedSteams;
        
        public bool IgnoreAdmin;
        
        public void LoadDefaults()
        {
            BlacklistedBypassPermissions = new Dictionary<string, ushort>()
            {
                {"permission", 1}
            };
            WhitelistedSteams = new List<ulong>(){00000000000000000};
            Permissions = new List<string>();
            BlacklistedGunIds = new List<ushort>();
            IgnoreAdmin = true;
        }
    }
}