using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Mathematics;
using PixelDust.Game.GUI.Elements.Common;
using PixelDust.Game.GUI.Interfaces;
using PixelDust.Game.Enums.General;

using Microsoft.Xna.Framework;

namespace PixelDust.Game.GUI.Common
{
    public sealed class PExperimentalGUI : PGUISystem
    {
        private Texture2D particleTexture;
        private readonly static (PCardinalDirection direction, Color color)[] directions = [
            (PCardinalDirection.Center, Color.Yellow),
            (PCardinalDirection.North, Color.Red),
            (PCardinalDirection.Northeast, Color.Orange),
            (PCardinalDirection.East, Color.Green),
            (PCardinalDirection.Southeast, Color.Blue),
            (PCardinalDirection.South, Color.Purple),
            (PCardinalDirection.Southwest, Color.Pink),
            (PCardinalDirection.West, Color.Cyan),
            (PCardinalDirection.Northwest, Color.Black)
        ];

        protected override void OnAwake()
        {
            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            base.OnAwake();
        }

        protected override void OnBuild(IPGUILayoutBuilder layout)
        {
            PGUIImageElement canvas = layout.CreateElement<PGUIImageElement>();

            canvas.Style.PositionAnchor = PCardinalDirection.Northwest;
            canvas.Style.Size = new Size2(
                PPercentageMath.PercentageOfValue(layout.Root.Style.Size.Width, 50),
                PPercentageMath.PercentageOfValue(layout.Root.Style.Size.Height, 50)
            );

            canvas.SetTexture(this.particleTexture);

            for (int i = 0; i < directions.Length; i++)
            {
                PGUIImageElement square = layout.CreateElement<PGUIImageElement>();

                square.Style.Size = new Size2(32);
                square.Style.PositionAnchor = directions[i].direction;
                square.Style.Color = directions[i].color;
                square.SetOriginPivot(PCardinalDirection.Center);
                square.SetTexture(this.particleTexture);

                canvas.AppendChild(square);
            }

            layout.Root.AppendChild(canvas);
        }
    }
}
