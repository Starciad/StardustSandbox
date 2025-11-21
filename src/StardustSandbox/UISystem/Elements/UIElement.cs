using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Directions;

using System.Collections.Generic;

namespace StardustSandbox.UISystem.Elements
{
    internal abstract class UIElement
    {
        internal bool ShouldUpdate { get; set; }
        internal bool IsVisible { get; set; }

        internal CardinalDirection PositionAnchor { get; set; }
        internal CardinalDirection OriginPivot { get; set; }
        internal SpriteEffects SpriteEffects { get; set; }
        internal Vector2 Position { get; set; }
        internal Vector2 Size
        {
            get => new(this.size.X * this.Scale.X, this.size.Y * this.Scale.Y);
            set => this.size = value;
        }
        internal Vector2 Margin { get; set; }
        internal Vector2 Scale { get; set; }
        internal Color Color { get; set; }
        internal float RotationAngle { get; set; } = 0f;

        private Vector2 size = Vector2.Zero;

        private readonly Dictionary<string, object> data = [];

        internal UIElement()
        {
            this.PositionAnchor = CardinalDirection.Northwest;
            this.OriginPivot = CardinalDirection.Southeast;
            this.SpriteEffects = SpriteEffects.None;

            this.Size = Vector2.One;
            this.Margin = Vector2.Zero;
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Color = Color.White;

            this.RotationAngle = 0f;
        }

        internal abstract void Initialize();
        internal abstract void Update(GameTime gameTime);
        internal abstract void Draw(SpriteBatch spriteBatch);

        internal void PositionRelativeToElement(Vector2 otherElementPosition, Vector2 otherElementSize)
        {
            this.Position = GetAnchorPosition(otherElementPosition, otherElementSize);
        }

        internal void PositionRelativeToElement(UIElement reference)
        {
            PositionRelativeToElement(reference.Position, reference.Size);
        }

        internal void PositionRelativeToScreen()
        {
            PositionRelativeToElement(Vector2.Zero, ScreenConstants.SCREEN_DIMENSIONS.ToVector2());
        }

        private Vector2 GetAnchorPosition(Vector2 targetPosition, Vector2 targetSize)
        {
            return this.PositionAnchor switch
            {
                CardinalDirection.Center => targetPosition + this.Margin + (targetSize / 2),
                CardinalDirection.North => targetPosition + this.Margin + new Vector2(targetSize.X / 2, 0),
                CardinalDirection.Northeast => targetPosition + this.Margin + new Vector2(targetSize.X, 0),
                CardinalDirection.East => targetPosition + this.Margin + new Vector2(targetSize.X, targetSize.Y / 2),
                CardinalDirection.Southeast => targetPosition + this.Margin + new Vector2(targetSize.X, targetSize.Y),
                CardinalDirection.South => targetPosition + this.Margin + new Vector2(targetSize.X / 2, targetSize.Y),
                CardinalDirection.Southwest => targetPosition + this.Margin + new Vector2(0, targetSize.Y),
                CardinalDirection.West => targetPosition + this.Margin + new Vector2(0, targetSize.Y / 2),
                CardinalDirection.Northwest => targetPosition + this.Margin,
                _ => targetPosition + this.Margin,
            };
        }

        // [ Data ]
        internal bool ContainsData(string name)
        {
            return this.data.ContainsKey(name);
        }

        internal void AddData(string name, object value)
        {
            this.data.Add(name, value);
        }

        internal object GetData(string name)
        {
            return this.data[name];
        }

        internal void UpdateData(string name, object value)
        {
            this.data[name] = value;
        }

        internal void RemoveData(string name)
        {
            _ = this.data.Remove(name);
        }
    }
}
