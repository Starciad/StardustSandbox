using Microsoft.Xna.Framework;

using PixelDust.Game.Constants;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.GUI.Elements
{
    public struct PGUIElementStyle
    {
        public readonly PPositioningType PositioningType => this.positioningType;
        public readonly PCardinalDirection PositionAnchor => this.positionAnchor;
        public readonly Size2 Size => this.size;
        public readonly Vector2 Margin => this.margin;

        private readonly PGUIElement _element;

        private PPositioningType positioningType;
        private PCardinalDirection positionAnchor;
        private Size2 size;
        private Vector2 margin;

        public PGUIElementStyle(PGUIElement element)
        {
            this._element = element;

            SetPositioningType(PPositioningType.Relative);
            SetPositionAnchor(PCardinalDirection.Northwest);
            SetSize(Size2.Zero);
            SetMargin(Vector2.Zero);
        }

        public readonly Vector2 GetPosition()
        {
            Size2 screenSize = new(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT);

            Size2 targetSize = screenSize;
            Vector2 targetPosition = Vector2.Zero;

            if (this.PositioningType == PPositioningType.Relative && this._element.Parent != null)
            {
                targetSize = this._element.Parent.Style.Size;
                targetPosition = this._element.Parent.Position;
            }

            return this.PositionAnchor switch
            {
                PCardinalDirection.Center => targetPosition + this.Margin + (new Vector2(targetSize.Width, targetSize.Height) / 2),
                PCardinalDirection.North => targetPosition + this.Margin + new Vector2(targetSize.Width / 2, 0),
                PCardinalDirection.Northeast => targetPosition + this.Margin + new Vector2(targetSize.Width, 0),
                PCardinalDirection.East => targetPosition + this.Margin + new Vector2(targetSize.Width, targetSize.Height / 2),
                PCardinalDirection.Southeast => targetPosition + this.Margin + new Vector2(targetSize.Width, targetSize.Height),
                PCardinalDirection.South => targetPosition + this.Margin + new Vector2(targetSize.Width / 2, targetSize.Height),
                PCardinalDirection.Southwest => targetPosition + this.Margin + new Vector2(0, targetSize.Height),
                PCardinalDirection.West => targetPosition + this.Margin + new Vector2(0, targetSize.Height / 2),
                PCardinalDirection.Northwest => targetPosition + this.Margin,
                _ => targetPosition,
            };
        }

        public void SetPositioningType(PPositioningType type)
        {
            this.positioningType = type;
        }
        public void SetPositionAnchor(PCardinalDirection cardinalDirection)
        {
            this.positionAnchor = cardinalDirection;
        }
        public void SetSize(Size2 size)
        {
            this.size = size;
        }
        public void SetMargin(Vector2 margin)
        {
            this.margin = margin;
        }
    }
}
