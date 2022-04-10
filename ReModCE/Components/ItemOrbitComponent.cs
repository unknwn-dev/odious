using ReMod.Core;
using ReMod.Core.Managers;
using ReMod.Core.UI;
using ReMod.Core.UI.QuickMenu;
using ReMod.Core.VRChat;
using ReModCE.Loader;
using ReModCE.Managers;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.DataModel;
using VRC.SDKBase;
using VRC.Udon;
using VRC.UI;

namespace ReModCE.Components
{
    internal sealed class ItemOrbitComponent : ModComponent
    {
        public static VRC_Pickup[] vrc_Pickups;

        private ConfigValue<bool> ItemOrbitEnabled;
        private ReMenuToggle _itemOrbitEnabled;
        private GameObject target;
        private Player TargetPlayer;

        public ItemOrbitComponent()
        {
            ItemOrbitEnabled = new ConfigValue<bool>(nameof(ItemOrbitEnabled), false);
            ItemOrbitEnabled.OnValueChanged += () => _itemOrbitEnabled.Toggle(ItemOrbitEnabled);
        }

        public override void OnUiManagerInit(UiManager uiManager)
        {
            base.OnUiManagerInit(uiManager);

            var menu = uiManager.MainMenu.GetCategoryPage("Exploits").GetCategory("ItemOrbit");
            _itemOrbitEnabled = menu.AddToggle("Item Orbit",
                "Makes all Pickups spin arround selected player.", ItemOrbit,
                ItemOrbitEnabled);

            ReModCE.WingExploitsMenu.AddToggle("Item Orbit",
                "Makes all Pickups spin arround selected player.", ItemOrbit,
                ItemOrbitEnabled);

            uiManager.TargetMenu.AddButton("Item Orbit To", "Select player as target to item orbit", SelectTarget);
        }

        private void SelectTarget()
        {
            var user = QuickMenuEx.SelectedUserLocal.field_Private_IUser_0;
            if (user == null)
                return;
            TargetPlayer = PlayerManager.field_Private_Static_PlayerManager_0.GetPlayer(user.prop_String_0);
        }

        private void ItemOrbit(bool enable)
        {
            if (enable)
                initWorldProps();

            ItemOrbitEnabled.SetValue(enable);
        }

        public override void OnUpdate()
        {
            if (ItemOrbitEnabled)
            {
                if (target == null)
                {
                    target = new GameObject();
                }

                if(TargetPlayer == null && Player.prop_Player_0 != null)
                {
                    TargetPlayer = Player.prop_Player_0;
                }

                target.transform.position = TargetPlayer.transform.position + new Vector3(0f, 1f, 0f);
                target.transform.Rotate(new Vector3(0f, 380f * Time.time * 1.5f, 0f));

                for (int i = 0; i < vrc_Pickups.Length; i++)
                {
                    VRC_Pickup vrc_Pickup = vrc_Pickups[i];
                    if (Networking.GetOwner(vrc_Pickup.gameObject) != Networking.LocalPlayer)
                    {
                        Networking.SetOwner(Networking.LocalPlayer, vrc_Pickup.gameObject);
                    }
                    vrc_Pickup.transform.position = target.transform.position + target.transform.forward * 1.5f;
                    target.transform.Rotate(new Vector3(0f, 380 / vrc_Pickups.Length, 0f));
                }
            }
        }

        public override void OnPlayerJoined(Player player)
        {
            if (player == TargetPlayer)
            {
                initWorldProps();
            }
        }

        private void initWorldProps()
        {
            vrc_Pickups = Object.FindObjectsOfType<VRC_Pickup>();
        }
    }
}

