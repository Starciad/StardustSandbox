using Microsoft.Xna.Framework;

using PixelDust.Game.Constants;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.GUI.Elements
{
    public sealed class PGUIElementStyle(PGUIElement element)
    {
        public PCardinalDirection PositionAnchor { get; set; } = PCardinalDirection.Northwest;
        public Size2 Size { get; set; } = Size2.Zero;
        public Color Color { get; set; } = Color.White;
        public Vector2 Margin { get; set; } = Vector2.Zero;

        private readonly PGUIElement _element = element;

        public Vector2 GetPosition()
        {
            Size2 screenSize = new(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT);

            return this.PositionAnchor switch
            {
                PCardinalDirection.Center => this.Margin + (new Vector2(screenSize.Width, screenSize.Height) / 2),
                PCardinalDirection.North => this.Margin + new Vector2(screenSize.Width / 2, 0),
                PCardinalDirection.Northeast => this.Margin + new Vector2(screenSize.Width, 0),
                PCardinalDirection.East => this.Margin + new Vector2(screenSize.Width, screenSize.Height / 2),
                PCardinalDirection.Southeast => this.Margin + new Vector2(screenSize.Width, screenSize.Height),
                PCardinalDirection.South => this.Margin + new Vector2(screenSize.Width / 2, screenSize.Height),
                PCardinalDirection.Southwest => this.Margin + new Vector2(0, screenSize.Height),
                PCardinalDirection.West => this.Margin + new Vector2(0, screenSize.Height / 2),
                PCardinalDirection.Northwest => this.Margin,
                _ => Vector2.Zero,
            };
        }
    }
}
