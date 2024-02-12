using PixelDust.Game.Mathematics;
using PixelDust.Game.Enums.General;
using Microsoft.Xna.Framework;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.Constants;

namespace PixelDust.Game.GUI.Elements
{
    public sealed class PGUIElementStyle(PGUIElement element)
    {
        public PPositioningType PositioningType { get; set; } = PPositioningType.Relative;
        public Size2 Size { get; set; } = Size2.Zero;
        public PCardinalDirection PositionAnchor { get; set; } = PCardinalDirection.Northwest;
        public PCardinalDirection OriginPivot { get; set; } = PCardinalDirection.Northwest;
        public Color Color { get; set; } = Color.White;
        public float RotationAngle { get; set; } = 0f;

        private readonly PGUIElement _element = element;

        public Vector2 GetPosition()
        {
            Size2 targetSize;
            Size2 screenSize = new(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT);

            switch (this.PositioningType)
            {
                case PPositioningType.Relative:
                    if (this._element.Parent != null)
                    {
                        targetSize = this._element.Parent.Style.Size;
                    }
                    else
                    {
                        targetSize = screenSize;
                    }
                    break;

                case PPositioningType.Fixed:
                    targetSize = screenSize;
                    break;

                default:
                    targetSize = screenSize;
                    break;
            }

            Vector2 elementPosition = this._element.Position;

            return this.PositionAnchor switch
            {
                PCardinalDirection.Center    => elementPosition + new Vector2(targetSize.Width / 2, targetSize.Height / 2),
                PCardinalDirection.North     => elementPosition + new Vector2(targetSize.Width / 2, 0),
                PCardinalDirection.Northeast => elementPosition + new Vector2(targetSize.Width, 0),
                PCardinalDirection.East      => elementPosition + new Vector2(targetSize.Width, targetSize.Height / 2),
                PCardinalDirection.Southeast => elementPosition + new Vector2(targetSize.Width, targetSize.Height),
                PCardinalDirection.South     => elementPosition + new Vector2(targetSize.Width / 2, targetSize.Height),
                PCardinalDirection.Southwest => elementPosition + new Vector2(0, targetSize.Height),
                PCardinalDirection.West      => elementPosition + new Vector2(0, targetSize.Height / 2),
                PCardinalDirection.Northwest => elementPosition,
                _ => elementPosition,
            };
        }
    }
}
