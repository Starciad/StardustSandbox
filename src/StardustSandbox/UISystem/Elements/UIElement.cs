using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Mathematics.Primitives;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UISystem.Elements
{
    internal abstract class UIElement
    {
        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        internal UIElement Parent
        {
            get => this.parent;
            set
            {
                this.parent = value;
                RepositionRelativeToParent();
            }
        }

        public IReadOnlyList<UIElement> Children => this.children.AsReadOnly();

        internal FloatRectangle SelfFloatRectangle => new(this.Position, this.Size);

        internal Vector2 Position
        {
            get => this.position;
            set
            {
                this.position = value;
                RepositionChildren();
            }
        }

        internal Vector2 Size
        {
            get => this.rawSize * this.scale;
            set
            {
                this.rawSize = value;
                RepositionRelativeToParent();
            }
        }

        internal Vector2 Margin
        {
            get => this.margin;
            set
            {
                this.margin = value;
                RepositionRelativeToParent();
            }
        }

        internal Vector2 Scale
        {
            get => this.scale;
            set
            {
                this.scale = value;
                RepositionRelativeToParent();
            }
        }

        internal CardinalDirection Alignment
        {
            get => this.alignment;
            set
            {
                this.alignment = value;
                RepositionRelativeToParent();
            }
        }

        internal Color Color { get; set; }

        private UIElement parent;

        private Vector2 position;
        private Vector2 rawSize;
        private Vector2 margin;
        private Vector2 scale;
        private CardinalDirection alignment;

        private readonly List<UIElement> children = [];
        private readonly Queue<UIElement> childrenToAdd = new();
        private readonly Queue<UIElement> childrenToRemove = new();

        private readonly Dictionary<string, object> data = [];

        internal UIElement()
        {
            this.alignment = CardinalDirection.Northwest;

            this.position = Vector2.Zero;
            this.rawSize = Vector2.Zero;
            this.margin = Vector2.Zero;
            this.scale = Vector2.One;
            this.Color = Color.White;

            this.CanDraw = true;
            this.CanUpdate = true;
        }

        private void FlushPendingHierarchyChanges()
        {
            while (this.childrenToAdd.Count > 0)
            {
                UIElement actor = this.childrenToAdd.Dequeue();
                // assign parent reference directly to avoid re-queueing/resizing before being part of list
                actor.parent = this;
                actor.RepositionRelativeToParent();
                this.children.Add(actor);
            }

            while (this.childrenToRemove.Count > 0)
            {
                UIElement actor = this.childrenToRemove.Dequeue();
                bool removed = this.children.Remove(actor);
                if (removed)
                {
                    actor.parent = null;
                }
            }
        }

        internal virtual void Initialize()
        {
            FlushPendingHierarchyChanges();

            foreach (UIElement childElement in this.children)
            {
                childElement.Initialize();
            }
        }

        internal virtual void Update(GameTime gameTime)
        {
            if (!this.CanUpdate)
            {
                return;
            }

            FlushPendingHierarchyChanges();

            foreach (UIElement childElement in this.children)
            {
                childElement.Update(gameTime);
            }
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!this.CanDraw)
            {
                return;
            }

            foreach (UIElement childElement in this.children)
            {
                childElement.Draw(spriteBatch);
            }
        }

        #region Positioning

        private static Vector2 GetAnchoredPosition(FloatRectangle rect1, FloatRectangle rect2, CardinalDirection anchor, Vector2 margin)
        {
            Vector2 targetPosition = rect2.Location;
            Vector2 targetSize = rect2.Size;
            Vector2 rect1Size = rect1.Size;

            float x = targetPosition.X;
            float y = targetPosition.Y;

            switch (anchor)
            {
                case CardinalDirection.Center:
                    x += (targetSize.X - rect1Size.X) / 2f;
                    y += (targetSize.Y - rect1Size.Y) / 2f;
                    break;
                case CardinalDirection.North:
                    x += (targetSize.X - rect1Size.X) / 2f;
                    y += 0f;
                    break;
                case CardinalDirection.Northeast:
                    x += targetSize.X - rect1Size.X;
                    y += 0f;
                    break;
                case CardinalDirection.East:
                    x += targetSize.X - rect1Size.X;
                    y += (targetSize.Y - rect1Size.Y) / 2f;
                    break;
                case CardinalDirection.Southeast:
                    x += targetSize.X - rect1Size.X;
                    y += targetSize.Y - rect1Size.Y;
                    break;
                case CardinalDirection.South:
                    x += (targetSize.X - rect1Size.X) / 2f;
                    y += targetSize.Y - rect1Size.Y;
                    break;
                case CardinalDirection.Southwest:
                    x += 0f;
                    y += targetSize.Y - rect1Size.Y;
                    break;
                case CardinalDirection.West:
                    x += 0f;
                    y += (targetSize.Y - rect1Size.Y) / 2f;
                    break;
                case CardinalDirection.Northwest:
                default:
                    x += 0f;
                    y += 0f;
                    break;
            }

            // clamp to keep within target rectangle
            x = Math.Clamp(x, targetPosition.X, targetPosition.X + Math.Max(0, targetSize.X - rect1Size.X));
            y = Math.Clamp(y, targetPosition.Y, targetPosition.Y + Math.Max(0, targetSize.Y - rect1Size.Y));

            return new Vector2(x + margin.X, y + margin.Y);
        }

        internal void RepositionRelativeToElement(FloatRectangle targetRectangle)
        {
            // set position using anchored calculation
            this.position = GetAnchoredPosition(this.SelfFloatRectangle, targetRectangle, this.Alignment, this.Margin);
            RepositionChildren();
        }

        internal void RepositionRelativeToElement(UIElement targetElement)
        {
            RepositionRelativeToElement(targetElement.SelfFloatRectangle);
        }

        internal void RepositionRelativeToScreen()
        {
            RepositionRelativeToElement(new FloatRectangle(Vector2.Zero, new Vector2(ScreenConstants.SCREEN_DIMENSIONS.X, ScreenConstants.SCREEN_DIMENSIONS.Y)));
        }

        internal void RepositionRelativeToParent()
        {
            if (this.Parent == null)
            {
                RepositionRelativeToScreen();
            }
            else
            {
                RepositionRelativeToElement(this.Parent);
            }
        }

        internal void RepositionChildren()
        {
            foreach (UIElement child in this.children)
            {
                child.RepositionRelativeToParent();
            }
        }

        #endregion

        #region Hierarchy Management

        internal void SetParent(UIElement parent)
        {
            this.Parent = parent;
            // Parent setter triggers RepositionRelativeToParent
        }

        internal void AddChildElement(UIElement element)
        {
            if (element == null)
                return;
            this.childrenToAdd.Enqueue(element);
        }

        internal void RemoveChildElement(UIElement element)
        {
            if (element == null)
                return;
            this.childrenToRemove.Enqueue(element);
        }

        internal void RemoveAllChildElements()
        {
            foreach (UIElement child in this.children)
            {
                this.childrenToRemove.Enqueue(child);
            }
        }

        #endregion

        #region Data

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

        #endregion

        public override string ToString()
        {
            return UIElementTree.ToString(this);
        }
    }
}
