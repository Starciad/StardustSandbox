using System.Collections.Generic;

using Microsoft.Xna.Framework;

using PixelDust.Game.Objects;
using PixelDust.Game.Enums.General;
using PixelDust.Game.Enums.GUI;
using PixelDust.Game.Constants;
using PixelDust.Game.Mathematics;

namespace PixelDust.Game.GUI.Elements
{
    public abstract class PGUIElement : PGameObject
    {
        // Settings
        public string Id { get; set; }

        // Readonly
        public Vector2 Position => this.position;

        // Parental
        public PGUIElement Parent => this.parent;
        public PGUIElement[] Children => [.. this.children];
        public bool HasChildren => this.children.Count > 0;
        public PPositioningType PositioningType => this.positioningType;
        public PCardinalDirection PositionAnchor => this.positionAnchor;
        public Size2 Size => this.size;
        public Vector2 Margin => this.margin;

        private readonly List<PGUIElement> children = [];
        private readonly Dictionary<string, object> data = [];

        private PPositioningType positioningType;
        private PCardinalDirection positionAnchor;
        private Size2 size;
        private Vector2 margin;
        private Vector2 position;

        private PGUIElement parent;

        public PGUIElement()
        {
            this.Id = string.Empty;
        }

        public PGUIElement(string id)
        {
            this.Id = id;
        }

        #region Settings
        public Vector2 GetPosition()
        {
            Size2 screenSize = new(PScreenConstants.DEFAULT_SCREEN_WIDTH, PScreenConstants.DEFAULT_SCREEN_HEIGHT);

            Size2 targetSize = screenSize;
            Vector2 targetPosition = Vector2.Zero;

            if (this.positioningType == PPositioningType.Relative && this.parent != null)
            {
                targetSize = this.parent.size;
                targetPosition = this.parent.position;
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
        #endregion

        #region Parental
        public void AppendChild(PGUIElement element)
        {
            element.parent?.RemoveChild(element);
            element.parent = this;
            element.position = element.GetPosition();

            this.children.Add(element);
        }

        public void RemoveAllChildren()
        {
            foreach (PGUIElement element in this.Children)
            {
                RemoveChild(element);
            }
        }

        public void RemoveChild(PGUIElement element)
        {
            _ = this.children.Remove(element);
        }
        #endregion

        #region Data
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
        #endregion
    }
}
