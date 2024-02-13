using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Enums.General;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.GUI.Common
{
    public sealed class PGUI_HUD : PGUISystem
    {
        private Texture2D backgroundTexture;

        protected override void OnAwake()
        {
            this.backgroundTexture = this.Game.AssetDatabase.GetTexture("gui_background_6");
            base.OnAwake();
        }

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            PGUISliceImageElement sliceImageElement = layout.CreateElement<PGUISliceImageElement>();
            sliceImageElement.SetTexture(this.backgroundTexture);
            sliceImageElement.Style.Color = Color.Cyan;
            sliceImageElement.Style.Size = new Size2(16, 5);
            sliceImageElement.Style.Margin = new Vector2(-96, -280);
            sliceImageElement.Style.PositionAnchor = PCardinalDirection.Center;

            layout.RootElement.AppendChild(sliceImageElement);
        }
    }
}
