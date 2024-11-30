using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SharpDX.Direct3D9;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Enums.GUI;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.GUISystem.Elements
{
    public abstract class SGUIElement(ISGame gameInstance) : SGameObject(gameInstance), ISPoolableObject
    {
        public bool ShouldUpdate { get; set; }
        public bool IsVisible { get; set; }
        public Vector2 Position => this.position;
        public SPositioningType PositioningType => this.positioningType;
        public SCardinalDirection PositionAnchor => this.positionAnchor;
        public SSize2F Size => new(this.size.Width * this.scale.X, this.size.Height * this.scale.Y);
        public Vector2 Margin => this.margin;
        public Vector2 Scale => this.scale;
        public SCardinalDirection OriginPivot => this.originPivot;
        public SpriteEffects SpriteEffects => this.spriteEffects;
        public float RotationAngle => this.rotationAngle;
        public Color Color => this.color;

        private SPositioningType positioningType = SPositioningType.Relative;
        private SCardinalDirection positionAnchor = SCardinalDirection.Northwest;
        private SSize2 size = SSize2.One;
        private Vector2 margin = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        private Vector2 scale = Vector2.Zero;
        private SCardinalDirection originPivot = SCardinalDirection.Northwest;
        private SpriteEffects spriteEffects = SpriteEffects.None;
        private float rotationAngle = 0f;
        private Color color = Color.White;

        private readonly Dictionary<string, object> data = [];

        private static SSize2 ScreenSize = new(SScreenConstants.DEFAULT_SCREEN_WIDTH, SScreenConstants.DEFAULT_SCREEN_HEIGHT);

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
