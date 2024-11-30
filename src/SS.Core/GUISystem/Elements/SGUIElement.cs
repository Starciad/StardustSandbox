using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.GUISystem.Elements
{
    public abstract class SGUIElement(ISGame gameInstance) : SGameObject(gameInstance)
    {
        public bool ShouldUpdate { get; set; }
        public bool IsVisible { get; set; }
        public SCardinalDirection PositionAnchor => this.positionAnchor;
        public SCardinalDirection OriginPivot => this.originPivot;
        public SpriteEffects SpriteEffects => this.spriteEffects;
        public Vector2 Position => this.position;
        public SSize2F Size => new(this.size.Width * this.scale.X, this.size.Height * this.scale.Y);
        public Vector2 Margin => this.margin;
        public Vector2 Scale => this.scale;
        public Color Color => this.color;
        public float RotationAngle => this.rotationAngle;

        private SCardinalDirection positionAnchor = SCardinalDirection.Northwest;
        private SCardinalDirection originPivot = SCardinalDirection.Southeast;
        private SpriteEffects spriteEffects = SpriteEffects.None;

        private SSize2 size = SSize2.Zero;
        private Vector2 margin = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        private Vector2 scale = Vector2.One;
        private Color color = Color.White;

        private float rotationAngle = 0f;

        private readonly Dictionary<string, object> data = [];

        // [ Settings ]
        public void PositionRelativeToScreen()
        {
            PositionRelativeToElement(Vector2.Zero, SScreenConstants.DEFAULT_SCREEN_SIZE);
        }

        public void PositionRelativeToElement(SGUIElement reference)
        {
            PositionRelativeToElement(reference.position, reference.Size);
        }

        public void PositionRelativeToElement(Vector2 otherElementPosition, SSize2 otherElementSize)
        {
            this.position = GetPosition(otherElementPosition, otherElementSize);
        }

        private Vector2 GetPosition(Vector2 targetPosition, SSize2 targetSize)
        {
            return this.positionAnchor switch
            {
                SCardinalDirection.Center => targetPosition + this.margin + (targetSize / 2).ToVector2(),
                SCardinalDirection.North => targetPosition + this.margin + new Vector2(targetSize.Width / 2, 0),
                SCardinalDirection.Northeast => targetPosition + this.margin + new Vector2(targetSize.Width, 0),
                SCardinalDirection.East => targetPosition + this.margin + new Vector2(targetSize.Width, targetSize.Height / 2),
                SCardinalDirection.Southeast => targetPosition + this.margin + new Vector2(targetSize.Width, targetSize.Height),
                SCardinalDirection.South => targetPosition + this.margin + new Vector2(targetSize.Width / 2, targetSize.Height),
                SCardinalDirection.Southwest => targetPosition + this.margin + new Vector2(0, targetSize.Height),
                SCardinalDirection.West => targetPosition + this.margin + new Vector2(0, targetSize.Height / 2),
                SCardinalDirection.Northwest => targetPosition + this.margin,
                _ => targetPosition + this.margin,
            };
        }

        public void SetPositionAnchor(SCardinalDirection direction)
        {
            this.positionAnchor = direction;
        }

        public void SetSize(SSize2F value)
        {
            this.size = value;
        }

        public void SetMargin(Vector2 value)
        {
            this.margin = value;
        }

        public void SetScale(Vector2 value)
        {
            this.scale = value;
        }

        public void SetColor(Color value)
        {
            this.color = value;
        }

        public void SetOriginPivot(SCardinalDirection direction)
        {
            this.originPivot = direction;
        }

        public void SetSpriteEffects(SpriteEffects value)
        {
            this.spriteEffects = value;
        }

        public void SetRotationAngle(float value)
        {
            this.rotationAngle = value;
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
