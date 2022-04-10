using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppSystem.Text.RegularExpressions;
using MelonLoader;
using ReMod.Core;
using ReMod.Core.Managers;
using ReMod.Core.UI.QuickMenu;
using ReModCE.Loader;
using ReMod.Core.VRChat;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using ReModCE.EvilEyeSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace ReModCE.Components
{
    internal class UserSpoofComponent : ModComponent
    {
        private ConfigValue<bool> UserSpoofEnabled;
        private static ReMenuToggle _UserSpoofToggled;
        private static MelonPreferences_Entry<bool> _UserSpoof;
        private static MelonPreferences_Entry<string> _UserIdToSpoof;
        private ReMenuButton _UserSpoofButton;

        public UserSpoofComponent()
        {
            UserSpoofEnabled = new ConfigValue<bool>(nameof(UserSpoofEnabled), false);
            UserSpoofEnabled.OnValueChanged += () => _UserSpoofToggled.Toggle(UserSpoofEnabled);
        }

        public override void OnUiManagerInit(UiManager uiManager)
        {
            var spoofMenu = uiManager.MainMenu.GetCategoryPage("Spoofing").GetCategory("User");

            _UserSpoofToggled = spoofMenu.AddToggle("User spoof", "Spoofs you to user id", b => { UserSpoofEnabled.SetValue(b); UserSpoof(b); }, UserSpoofEnabled);

            _UserSpoofButton = spoofMenu.AddButton("User ID to spoof", "Set user id to spoof", () => {
                PopupTextInput(_UserIdToSpoof);
            });

            ReModCE.WingExploitsMenu.AddToggle("User spoof", "Spoofs you to user id", b => { UserSpoofEnabled.SetValue(b); UserSpoof(b); }, UserSpoofEnabled);
        }

        private void PopupTextInput(MelonPreferences_Entry<string> configValue)
        {
            var category = MelonPreferences.GetCategory("ReModCE");
            _UserIdToSpoof = (MelonPreferences_Entry<string>)category.GetEntry("UserIdToSpoof");

            VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowInputPopupWithCancel("Input User ID to spoof",
                $"", InputField.InputType.Standard, false, "Submit",
                (s, k, t) =>
                {

                    if (string.IsNullOrEmpty(s))
                        return;

                    if (s.Contains("userId="))
                    {
                        var userIdIndex = s.IndexOf("userId=");
                        var userId = s.Substring(userIdIndex + "userId=".Length);

                        if (configValue.Identifier.Contains("_UserIdToSpoof"))
                        {
                            s = $"{userIdIndex}".Trim().TrimEnd('\r', '\n');
                            ReLogger.Msg($"parsed vrc join link to {s}");
                        }
                    }

                    if (!s.Contains("usr_"))
                    {
                        ReLogger.Msg($"Not setting UserID due to it not containing usr_");
                        return;
                    }

                    _UserIdToSpoof.Value = s;

                }, null);
        }

        public void UserSpoof(bool enabled)
        {
            var category = MelonPreferences.GetCategory("ReModCE");
            _UserSpoof = (MelonPreferences_Entry<bool>)category.GetEntry("UserSpoofEnabled");

            if (enabled)
            {
                _UserSpoof.Value = true;
                MelonPreferences.Save();
            }
            else
            {
                _UserSpoof.Value = false;
                MelonPreferences.Save();
            }
        }
    }
}
