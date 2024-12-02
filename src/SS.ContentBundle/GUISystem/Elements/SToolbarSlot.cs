using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.GUI;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.Elements
{
    public class SToolbarSlot
    {
        public SGUIImageElement Background { get; }
        public SGUIImageElement Icon { get; }

        public SToolbarSlot(ISGame gameInstance, SGUIImageElement parent, ISGUILayoutBuilder layout, SItem item, Vector2 margin)
        {
            this.Background = new SGUIImageElement(gameInstance)
            {
                Texture = parent.Texture,
                OriginPivot = SCardinalDirection.Center,
                Margin = margin,
                Size = new SSize2(96)
            };

            layout.AddElement(this.Background);
        }
    }
}
