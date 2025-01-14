using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.GUISystem.Elements
{
    public abstract class SGUIElement : SGameObject
    {
        public bool ShouldUpdate { get; set; }
        public bool IsVisible { get; set; }

        public SCardinalDirection PositionAnchor { get; set; }
        public SCardinalDirection OriginPivot { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public Vector2 Position { get; set; }
        public SSize2F Size
        {
            get => new(this.size.Width * this.Scale.X, this.size.Height * this.Scale.Y);

            set => this.size = value;
        }
        public Vector2 Margin { get; set; }
        public Vector2 Scale { get; set; }
        public Color Color { get; set; }
        public float RotationAngle { get; set; } = 0f;

        private SSize2 size = SSize2.Zero;

        private readonly Dictionary<string, object> data = [];

        public SGUIElement(ISGame gameInstance) : base(gameInstance)
        {
            this.PositionAnchor = SCardinalDirection.Northwest;
            this.OriginPivot = SCardinalDirection.Southeast;
            this.SpriteEffects = SpriteEffects.None;

            this.Size = SSize2.One;
            this.Margin = Vector2.Zero;
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Color = Color.White;

            this.RotationAngle = 0f;
        }

        // [ Settings ]
        public void PositionRelativeToScreen()
        {
            PositionRelativeToElement(Vector2.Zero, SScreenConstants.DEFAULT_SCREEN_SIZE);
        }

        public void PositionRelativeToElement(SGUIElement reference)
        {
            PositionRelativeToElement(reference.Position, reference.Size);
        }

        public void PositionRelativeToElement(Vector2 otherElementPosition, SSize2 otherElementSize)
        {
            this.Position = GetAnchorPosition(otherElementPosition, otherElementSize);
        }

        private Vector2 GetAnchorPosition(Vector2 targetPosition, SSize2 targetSize)
        {
            return this.PositionAnchor switch
            {
                SCardinalDirection.Center => targetPosition + this.Margin + (targetSize / 2).ToVector2(),
                SCardinalDirection.North => targetPosition + this.Margin + new Vector2(targetSize.Width / 2, 0),
                SCardinalDirection.Northeast => targetPosition + this.Margin + new Vector2(targetSize.Width, 0),
                SCardinalDirection.East => targetPosition + this.Margin + new Vector2(targetSize.Width, targetSize.Height / 2),
                SCardinalDirection.Southeast => targetPosition + this.Margin + new Vector2(targetSize.Width, targetSize.Height),
                SCardinalDirection.South => targetPosition + this.Margin + new Vector2(targetSize.Width / 2, targetSize.Height),
                SCardinalDirection.Southwest => targetPosition + this.Margin + new Vector2(0, targetSize.Height),
                SCardinalDirection.West => targetPosition + this.Margin + new Vector2(0, targetSize.Height / 2),
                SCardinalDirection.Northwest => targetPosition + this.Margin,
                _ => targetPosition + this.Margin,
            };
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
    }
}
