using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Enums.General;
using StardustSandbox.Game.Enums.GUI;
using StardustSandbox.Game.Interfaces.General;
using StardustSandbox.Game.Mathematics;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Game.GUISystem.Elements
{
    public abstract class SGUIElement : SGameObject, ISPoolableObject
    {
        public bool ShouldUpdate { get; set; }
        public bool IsVisible { get; set; }
        public Vector2 Position => this.position;
        public SPositioningType PositioningType => this.positioningType;
        public SCardinalDirection PositionAnchor => this.positionAnchor;
        public SSize2 Size => this.size;
        public Vector2 Margin => this.margin;

        private readonly Dictionary<string, object> data = [];

        private SPositioningType positioningType = SPositioningType.Relative;
        private SCardinalDirection positionAnchor = SCardinalDirection.Northwest;
        private SSize2 size = SSize2.One;
        private Vector2 margin = Vector2.Zero;
        private Vector2 position = Vector2.Zero;

        private static SSize2 ScreenSize = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT);

        public SGUIElement(SGame gameInstance) : base(gameInstance)
        {

        }

        // [ Settings ]
        public void PositionRelativeToElement(SGUIElement reference)
        {
            PositionRelativeToElement(reference.position, reference.size);
        }

        public void PositionRelativeToElement(Vector2 position, SSize2 size)
        {
            Vector2 targetPosition = Vector2.Zero;
            SSize2 targetSize = ScreenSize;

            if (this.positioningType == SPositioningType.Relative)
            {
                targetPosition = position;
                targetSize = size;
            }

            this.position = CalculatePosition(targetPosition, targetSize);
        }

        private Vector2 CalculatePosition(Vector2 targetPosition, SSize2 targetSize)
        {
            return this.positionAnchor switch
            {
                SCardinalDirection.Center => targetPosition + this.Margin + (new Vector2(targetSize.Width, targetSize.Height) / 2),
                SCardinalDirection.North => targetPosition + this.Margin + new Vector2(targetSize.Width / 2, 0),
                SCardinalDirection.Northeast => targetPosition + this.Margin + new Vector2(targetSize.Width, 0),
                SCardinalDirection.East => targetPosition + this.Margin + new Vector2(targetSize.Width, targetSize.Height / 2),
                SCardinalDirection.Southeast => targetPosition + this.Margin + new Vector2(targetSize.Width, targetSize.Height),
                SCardinalDirection.South => targetPosition + this.Margin + new Vector2(targetSize.Width / 2, targetSize.Height),
                SCardinalDirection.Southwest => targetPosition + this.Margin + new Vector2(0, targetSize.Height),
                SCardinalDirection.West => targetPosition + this.Margin + new Vector2(0, targetSize.Height / 2),
                SCardinalDirection.Northwest => targetPosition + this.Margin,
                _ => targetPosition,
            };
        }

        public void SetPositioningType(SPositioningType type)
        {
            this.positioningType = type;
        }

        public void SetPositionAnchor(SCardinalDirection cardinalDirection)
        {
            this.positionAnchor = cardinalDirection;
        }

        public void SetSize(SSize2 size)
        {
            this.size = size;
        }

        public void SetMargin(Vector2 margin)
        {
            this.margin = margin;
        }

        // [ Data ]
        public bool ContainsData(string name)
        {
            return this.data.ContainsKey(name);
        }

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

        public void Reset()
        {
            this.positioningType = SPositioningType.Relative;
            this.positionAnchor = SCardinalDirection.Northwest;
            this.size = SSize2.One;
            this.margin = Vector2.Zero;
            this.position = Vector2.Zero;
        }
    }
}
