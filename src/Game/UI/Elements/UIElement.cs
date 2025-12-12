using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Constants;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Mathematics.Primitives;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UI.Elements
{
    internal abstract class UIElement
    {
        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        internal IReadOnlyList<UIElement> Children => this.children.AsReadOnly();
        internal FloatRectangle SelfFloatRectangle => new(this.Position, this.Size);

        internal UIElement Parent
        {
            get => this.parent;
            set
            {
                if (this.parent != value)
                {
                    this.parent = value;
                    RepositionRelativeToParent();
                }
            }
        }
        internal Vector2 Position
        {
            get => this.position;
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    RepositionChildren();
                }
            }
        }
        internal virtual Vector2 Size
        {
            get => this.rawSize * this.scale;
            set
            {
                if (this.rawSize != value)
                {
                    this.rawSize = value;
                    RepositionRelativeToParent();
                }
            }
        }
        internal Vector2 Margin
        {
            get => this.margin;
            set
            {
                if (this.margin != value)
                {
                    this.margin = value;
                    RepositionRelativeToParent();
                }
            }
        }
        internal Vector2 Scale
        {
            get => this.scale;
            set
            {
                if (this.scale != value)
                {
                    this.scale = value;
                    RepositionRelativeToParent();
                }
            }
        }
        internal UIDirection Alignment
        {
            get => this.alignment;
            set
            {
                if (this.alignment != value)
                {
                    this.alignment = value;
                    RepositionRelativeToParent();
                }
            }
        }

        private UIElement parent;

        private Vector2 position;
        private Vector2 rawSize;
        private Vector2 margin;
        private Vector2 scale;
        private UIDirection alignment;

        private readonly List<UIElement> children = [];
        private readonly Queue<UIElement> childrenToAdd = new();
        private readonly Queue<UIElement> childrenToRemove = new();

        private readonly Dictionary<string, object> data = [];

        internal UIElement()
        {
            this.alignment = UIDirection.Northwest;

            this.position = Vector2.Zero;
            this.rawSize = Vector2.Zero;
            this.margin = Vector2.Zero;
            this.scale = Vector2.One;

            this.CanDraw = true;
            this.CanUpdate = true;
        }

        private void FlushPendingHierarchyChanges()
        {
            while (this.childrenToAdd.Count > 0)
            {
                UIElement actor = this.childrenToAdd.Dequeue();
                actor.parent = this;
                actor.RepositionRelativeToParent();
                this.children.Add(actor);
            }

            while (this.childrenToRemove.Count > 0)
            {
                UIElement actor = this.childrenToRemove.Dequeue();

                if (this.children.Remove(actor))
                {
                    actor.parent = null;
                }
            }
        }

        internal virtual void Initialize()
        {
            FlushPendingHierarchyChanges();

            OnInitialize();

            foreach (UIElement childElement in this.children)
            {
                childElement.Initialize();
            }
        }

        internal void Update(in GameTime gameTime)
        {
            if (!this.CanUpdate)
            {
                return;
            }

            FlushPendingHierarchyChanges();

            OnUpdate(gameTime);

            foreach (UIElement childElement in this.children)
            {
                childElement.Update(gameTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!this.CanDraw)
            {
                return;
            }

            OnDraw(spriteBatch);

            foreach (UIElement childElement in this.children)
            {
                childElement.Draw(spriteBatch);
            }
        }

        protected abstract void OnInitialize();
        protected abstract void OnUpdate(in GameTime gameTime);
        protected abstract void OnDraw(SpriteBatch spriteBatch);

        #region Positioning

        private static Vector2 GetAnchoredPosition(FloatRectangle rect1, FloatRectangle rect2, UIDirection anchor, Vector2 margin)
        {
            float x = rect2.Location.X;
            float y = rect2.Location.Y;

            switch (anchor)
            {
                case UIDirection.Center:
                    x += (rect2.Size.X - rect1.Size.X) / 2f;
                    y += (rect2.Size.Y - rect1.Size.Y) / 2f;
                    break;
                case UIDirection.North:
                    x += (rect2.Size.X - rect1.Size.X) / 2f;
                    y += 0f;
                    break;
                case UIDirection.Northeast:
                    x += rect2.Size.X - rect1.Size.X;
                    y += 0f;
                    break;
                case UIDirection.East:
                    x += rect2.Size.X - rect1.Size.X;
                    y += (rect2.Size.Y - rect1.Size.Y) / 2f;
                    break;
                case UIDirection.Southeast:
                    x += rect2.Size.X - rect1.Size.X;
                    y += rect2.Size.Y - rect1.Size.Y;
                    break;
                case UIDirection.South:
                    x += (rect2.Size.X - rect1.Size.X) / 2f;
                    y += rect2.Size.Y - rect1.Size.Y;
                    break;
                case UIDirection.Southwest:
                    x += 0f;
                    y += rect2.Size.Y - rect1.Size.Y;
                    break;
                case UIDirection.West:
                    x += 0f;
                    y += (rect2.Size.Y - rect1.Size.Y) / 2f;
                    break;
                case UIDirection.Northwest:
                default:
                    x += 0f;
                    y += 0f;
                    break;
            }

            // clamp to keep within target rectangle
            x = Math.Clamp(x, rect2.Location.X, rect2.Location.X + Math.Max(0, rect2.Size.X - rect1.Size.X));
            y = Math.Clamp(y, rect2.Location.Y, rect2.Location.Y + Math.Max(0, rect2.Size.Y - rect1.Size.Y));

            return new Vector2(x + margin.X, y + margin.Y);
        }

        private void RepositionRelativeToElement(FloatRectangle targetRectangle)
        {
            this.position = GetAnchoredPosition(this.SelfFloatRectangle, targetRectangle, this.Alignment, this.Margin);
            RepositionChildren();
        }

        private void RepositionRelativeToElement(UIElement targetElement)
        {
            RepositionRelativeToElement(targetElement.SelfFloatRectangle);
        }

        private void RepositionRelativeToScreen()
        {
            RepositionRelativeToElement(new FloatRectangle(Vector2.Zero, new Vector2(ScreenConstants.SCREEN_DIMENSIONS.X, ScreenConstants.SCREEN_DIMENSIONS.Y)));
        }

        protected void RepositionRelativeToParent()
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

        private void RepositionChildren()
        {
            foreach (UIElement child in this.children)
            {
                child.RepositionRelativeToParent();
            }
        }

        #endregion

        #region Hierarchy Management

        internal void AddChild(UIElement element)
        {
            if (element == null)
            {
                return;
            }

            this.childrenToAdd.Enqueue(element);
        }

        internal void RemoveChild(UIElement element)
        {
            if (element == null)
            {
                return;
            }

            this.childrenToRemove.Enqueue(element);
        }

        internal void RemoveAllChildren()
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
