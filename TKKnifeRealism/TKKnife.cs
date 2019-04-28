using System;
using System.Collections.Generic;
using System.Linq;
using InfinityScript;

namespace TKKnife
{
    public class TKKnife : BaseScript
    {
        public int currentClipAmmo;
        public int currentStockAmmo;
        public TKKnife()
        {
            PlayerConnected += TK_PlayerConnected;
        }
        void TK_PlayerConnected(Entity player)
        {
            player.NotifyOnPlayerCommand("knifeThrow", "+frag");
            player.NotifyOnPlayerCommand("knifePickup", "+activate");
            player.OnNotify("knifeThrow", (p) =>
                    {
                            if (player.CurrentWeapon.Contains("_tactical") && player.HasWeapon("throwingknife_mp"))
                            {
                            AfterDelay(250, () =>
                                {
                                    OnInterval(500, () =>
                                        {
                                            if (!player.IsAlive) return false;
                                            if (player.GetAmmoCount("throwingknife_mp") == 0)
                                            {
                                                string weapon = player.CurrentWeapon;
                                                player.GiveWeapon(weapon.Replace("_tactical", ""));
                                                AfterDelay(100, () =>
                                                        player.SwitchToWeaponImmediate(weapon.Replace("_tactical", "")));
                                                currentClipAmmo = player.GetWeaponAmmoClip(weapon);
                                                currentStockAmmo = player.GetWeaponAmmoStock(weapon);
                                                player.SetWeaponAmmoClip(weapon.Replace("_tactical", ""), currentClipAmmo);
                                                player.SetWeaponAmmoStock(weapon.Replace("_tactical", ""), currentStockAmmo);
                                                player.TakeWeapon(weapon);
                                                return false;
                                            }
                                            return true;
                                        });
                                });
                            }
                    });
            OnNotify("knifePickup", () =>
            {
                AfterDelay(50, () =>
                    {
                        if (player.HasWeapon("throwingknife_mp") && player.GetAmmoCount("throwingknife_mp") == 1 && !player.CurrentWeapon.Contains("_tactical") && GSCFunctions.WeaponClass(player.CurrentWeapon) == "pistol")
                        {
                            string weapon = player.CurrentWeapon;
                            player.GiveWeapon(weapon + "_tactical");
                            AfterDelay(100, () =>
                                    player.SwitchToWeaponImmediate(weapon + "_tactical"));
                            currentClipAmmo = player.GetWeaponAmmoClip(weapon);
                            currentStockAmmo = player.GetWeaponAmmoStock(weapon);
                            player.SetWeaponAmmoClip(weapon + "_tactical", currentClipAmmo);
                            player.SetWeaponAmmoStock(weapon + "_tactical", currentStockAmmo);
                            AfterDelay(150, () => player.TakeWeapon(weapon));
                        }
                    });
            });
        }
    }
}
