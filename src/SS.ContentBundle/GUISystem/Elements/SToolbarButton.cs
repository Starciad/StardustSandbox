using StardustSandbox.ContentBundle.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.Elements
{
    public class SToolbarButton
    {
        public SToolbarButton(ISGame gameInstance, ISGUILayoutBuilder layout, string name, Action action, SGUIImageElement parent, SCardinalDirection anchor, int index)
        {
            SGUIImageElement buttonBackground = new(gameInstance)
            {
                Texture = parent.Texture,
                OriginPivot = SCardinalDirection.Center,
                Size = new SSize2(SGUI_HUD.SLOT_SIZE)
            };

            buttonBackground.PositionRelativeToElement(parent);
            layout.AddElement(buttonBackground);
        }
    }
}
