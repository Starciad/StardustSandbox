using Microsoft.Xna.Framework;

using PixelDust.Game.Constants;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Objects;

using System.Collections.Generic;

namespace PixelDust.Game.GUI.Elements
{
    public abstract class PGUIElement : PGameObject
    {
        public Vector2 Position => this.position;
        public PPositioningType PositioningType => this.positioningType;
        public PCardinalDirection PositionAnchor => this.positionAnchor;
        public Size2 Size => this.size;
        public Vector2 Margin => this.margin;

        private readonly Dictionary<string, object> data = [];

        private PPositioningType positioningType = PPositioningType.Relative;
        private PCardinalDirection positionAnchor = PCardinalDirection.Northwest;
        private Size2 size = Size2.One;
        private Vector2 margin = Vector2.Zero;
        private Vector2 position = Vector2.Zero;

        // [ Settings ]
        public void PositionRelativeToElement(PGUIElement reference)
        {
            PositionRelativeToElement(reference.position, reference.size);
        }

        public void PositionRelativeToElement(Vector2 position, Size2 size)
        {
            Size2 screenSize = new(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT);

            Vector2 targetPosition = Vector2.Zero;
            Size2 targetSize = screenSize;

            if (this.positioningType == PPositioningType.Relative)
            {
                targetPosition = position;
                targetSize = size;
            }

            this.position = CalculatePosition(targetPosition, targetSize);
        }

        private Vector2 CalculatePosition(Vector2 targetPosition, Size2 targetSize)
        {
            return this.positionAnchor switch
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

        // [ Data ]
        public void AddData(string name, object value)
        {
            this.data.Add(name, value);
        }

        public object GetData(string name)
        {
            return this.data[name];
        }

        public void UpdateData(string name, object value)
        {
            this.data[name] = value;
        }

        public void RemoveData(string name)
        {
            _ = this.data.Remove(name);
        }
    }
}
