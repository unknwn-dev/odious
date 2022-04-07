using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReMod.Core;
using ReMod.Core.Managers;
using ReMod.Core.UI.QuickMenu;
using ReModCE.Loader;
using ReModCE.SDK;
using UnityEngine;

namespace ReModCE.Components
{
    internal class CameraFixComponent : ModComponent
    {

        public override void OnUiManagerInit(UiManager uiManager)
        {
            ReMenuCategory visualMenu = uiManager.MainMenu.GetCategoryPage("Visuals").AddCategory("Camera");

            visualMenu.AddButton(
                "Distance fix",
                "Camera distance draw fix.",
                FixCameraDistance
                );

            ReModCE.WingMenu.AddButton("Cam Dist", "Camera distance draw fix.", FixCameraDistance, ResourceManager.GetSprite("remodce.eye"), false);
        }

        private void FixCameraDistance()
        {
            Camera.main.nearClipPlane = 0.01f;
        }
    }
}
