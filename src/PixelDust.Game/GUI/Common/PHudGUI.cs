using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.GUI.Common
{
    public sealed class PHudGUI : PGUISystem
    {
        private Texture2D backgroundTexture;

        protected override void OnAwake()
        {
            backgroundTexture = this.Game.AssetDatabase.GetTexture("gui_background_6");
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

            PGUISliceImageElement sliceImageElement2 = layout.CreateElement<PGUISliceImageElement>();
            sliceImageElement2.SetTexture(this.backgroundTexture);
            sliceImageElement2.Style.Color = Color.Cyan;
            sliceImageElement2.Style.Size = new Size2(5, 12);
            sliceImageElement2.Style.Margin = new Vector2(-360, 0);
            sliceImageElement2.Style.PositionAnchor = PCardinalDirection.Center;

            PGUISliceImageElement sliceImageElement3 = layout.CreateElement<PGUISliceImageElement>();
            sliceImageElement3.SetTexture(this.backgroundTexture);
            sliceImageElement3.Style.Color = Color.Cyan;
            sliceImageElement3.Style.Size = new Size2(16, 16);
            sliceImageElement3.Style.Margin = new Vector2(96, 32);
            sliceImageElement3.Style.PositionAnchor = PCardinalDirection.Center;
        }
    }
}
