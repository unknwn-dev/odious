﻿using ReMod.Core;
using ReMod.Core.Managers;
using ReMod.Core.UI.QuickMenu;
using ReModCE.Core;
using ReModCE.EvilEyeSDK;
using ReModCE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC;
using ReMod.Core.VRChat;
using VRC.Core;

namespace ReModCE.Components
{
    internal class NamePlatesComponent : ModComponent
    {
        private ConfigValue<bool> NameplatesEnabled;
        private static ReMenuToggle _NameplatesToggled;


        public NamePlatesComponent()
        {
            NameplatesEnabled = new ConfigValue<bool>(nameof(NameplatesEnabled), false);
        }
        public override void OnUiManagerInit(UiManager uiManager)
        {
            var menu = uiManager.MainMenu.GetCategoryPage("Visuals").GetCategory("Nametags");
            if(NameplatesEnabled == null)
            {
                MelonLoader.MelonLogger.Msg($"bruh wtf");
            }
            _NameplatesToggled = menu.AddToggle("Nametags", "Displays information about players above their heads.", b => { NameplatesEnabled.SetValue(b); Nameplates(b); }, NameplatesEnabled);
        }

        public void Nameplates(bool enabled)
        {
            this.RunOnce(enabled);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public void RunOnce(bool enabled)
        {
            if(enabled == false)
            {
                return;
            }

            try
            {
                List<Player> Players = new List<Player>(PlayerWrapper.GetAllPlayers());

                for (int i = 0; i < Players.Count; i++)
                {
                    Player player = Players[i];
                    NamePlates customNameplate = player.transform.Find("Player Nameplate/Canvas/Nameplate").gameObject.AddComponent<NamePlates>();
                    customNameplate.player = player;
                    bool flag = i >= Players.Count;
                    if (flag)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLoader.MelonLogger.Msg(ex.Message);
            }
        }

        public override void OnPlayerJoined(Player player)
        {
            if (!NameplatesEnabled)
                return;

            NamePlates customNameplate = player.transform.Find("Player Nameplate/Canvas/Nameplate").gameObject.AddComponent<NamePlates>();
            customNameplate.player = player;
        }            
    }
}
