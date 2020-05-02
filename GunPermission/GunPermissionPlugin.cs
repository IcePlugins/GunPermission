using System;
using System.Linq;
using System.Reflection;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace GunPermission
{
    public class GunPermissionPlugin : RocketPlugin<GunPermissionConfiguration>
    {

        public static GunPermissionPlugin Instance;

        private static bool _overrided;
        
        

        public override void LoadPlugin()
        {
            Instance = this;
            if (_overrided)
                return;
            MethodInfo originalMethod = typeof(PlayerEquipment).GetMethod("askEquip", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo newMethod =
                typeof(OverrideMethods).GetMethod("askEquip", BindingFlags.Static | BindingFlags.Public);
            RedirectionHelper.RedirectCalls(originalMethod, newMethod);
            _overrided = true;
        }

        public override void UnloadPlugin(PluginState state = PluginState.Unloaded)
        {
            Instance = null;
        }


        //Scrapped code
        /*
        public void FixedUpdate() => CheckEquips();
        public static void CheckEquips()
        {
            foreach (SteamPlayer pla in Provider.clients)
            {
                UnturnedPlayer rPlayer = UnturnedPlayer.FromSteamPlayer(pla);
                if ((rPlayer.Player.equipment.useable as UseableGun) == null || !rPlayer.Player.equipment.isEquipped)
                    continue;

                bool isAllowed = false;
                
                foreach (var permission in Instance.Configuration.Instance.Permissions.Where(permission => rPlayer.HasPermission(permission)))
                    isAllowed = true;

                if (Instance.Configuration.Instance.BlacklistedGunIds.Contains(
                    ((ItemGunAsset) rPlayer.Player.equipment.asset).id))
                    isAllowed = false;
                        
                
                if (isAllowed)
                    return;
                
                rPlayer.Player.equipment.dequip();
                
            }
        }
        */

        public static bool CanEquip(UnturnedPlayer rPlayer)
        {
            bool isAllowed = false;

            if (Instance.Configuration.Instance.IgnoreAdmin && rPlayer.IsAdmin)
                return true;
            
            if (Instance.Configuration.Instance.WhitelistedSteams.Contains(rPlayer.CSteamID.m_SteamID))
                return true;
                
            foreach (var permission in Instance.Configuration.Instance.Permissions.Where(permission => rPlayer.HasPermission(permission)))
                isAllowed = true;

            //TODO: Add individual permissions for guns but will need to get UseableGun because apparently Nolson can't pass an id
            
            
            

            if (Instance.Configuration.Instance.BlacklistedGunIds.Contains(
                ((ItemGunAsset) rPlayer.Player.equipment.asset).id))
                isAllowed = false;

            
            
            

            return isAllowed;
        }
        
    }
}