using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Mathematics;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;

using Microsoft.Xna.Framework;
using PixelDust.Game.Enums.General;

namespace PixelDust.Game.GUI.Common
{
    public sealed class PHudGUI : PGUISystem
    {
        private Texture2D particleTexture;

        private readonly static (PCardinalDirection pivot, Color color)[] anchorages = [
            (PCardinalDirection.Center, Color.Red),
            (PCardinalDirection.North, Color.Blue),
            (PCardinalDirection.Northeast, Color.Purple),
            (PCardinalDirection.East, Color.Pink),
            (PCardinalDirection.Southeast, Color.Red),
            (PCardinalDirection.South, Color.Green),
            (PCardinalDirection.Southwest, Color.Yellow),
            (PCardinalDirection.West, Color.Gray),
            (PCardinalDirection.Northwest, Color.Cyan)
        ];

        protected override void OnAwake()
        {
            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            base.OnAwake();
        }

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            foreach ((PCardinalDirection pivot, Color color) in anchorages)
            {
                PGUIImageElement imageElement = layout.CreateElement<PGUIImageElement>();

                imageElement.SetTexture(this.particleTexture);
                imageElement.SetOriginPivot(PCardinalDirection.Center);
                imageElement.Style.Margin = new(-32, 0);
                imageElement.Style.Size = new(32);
                imageElement.Style.Color = color;
                imageElement.Style.PositionAnchor = pivot;
            }
        }
    }
}
