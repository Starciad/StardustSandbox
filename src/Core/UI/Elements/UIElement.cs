/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.UI.Elements
{
    internal abstract class UIElement
    {
        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        internal int ChildCount => this.children.Count;
        internal UIElement FirstChild => this.children.Count > 0 ? this.children[0] : null;
        internal UIElement LastChild => this.children.Count > 0 ? this.children[^1] : null;
        internal RectangleF Bounds => new(this.Position, this.Size);
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

        internal virtual void Initialize()
        {
            OnInitialize();

            foreach (UIElement childElement in this.children)
            {
                childElement.Initialize();
            }
        }

        internal void Update(GameTime gameTime)
        {
            if (!this.CanUpdate)
            {
                return;
            }

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
        protected abstract void OnUpdate(GameTime gameTime);
        protected abstract void OnDraw(SpriteBatch spriteBatch);

        #region Positioning

        private static Vector2 GetAnchoredPosition(in RectangleF rect1, in RectangleF rect2, in UIDirection anchor, in Vector2 margin)
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

            return new(x + margin.X, y + margin.Y);
        }

        private void RepositionRelativeToElement(in RectangleF targetRectangle)
        {
            this.position = GetAnchoredPosition(this.Bounds, targetRectangle, this.Alignment, this.Margin);
            RepositionChildren();
        }

        private void RepositionRelativeToElement(UIElement targetElement)
        {
            RepositionRelativeToElement(targetElement.Bounds);
        }

        private void RepositionRelativeToScreen()
        {
            RepositionRelativeToElement(new RectangleF(Vector2.Zero, GameScreen.GetViewport()));
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

        #region Size Management

        private static RectangleF CalculateTotalBounds(UIElement element)
        {
            RectangleF bounds = element.Bounds;

            foreach (UIElement child in element.children)
            {
                RectangleF childBounds = CalculateTotalBounds(child);

                Vector2 min = new(
                    MathF.Min(bounds.Location.X, childBounds.Location.X),
                    MathF.Min(bounds.Location.Y, childBounds.Location.Y)
                );

                Vector2 max = new(
                    MathF.Max(bounds.Location.X + bounds.Size.X, childBounds.Location.X + childBounds.Size.X),
                    MathF.Max(bounds.Location.Y + bounds.Size.Y, childBounds.Location.Y + childBounds.Size.Y)
                );

                bounds = new(min, max - min);
            }

            return bounds;
        }

        internal RectangleF GetLayoutBounds()
        {
            return CalculateTotalBounds(this);
        }

        #endregion

        #region Hierarchy Management

        internal void AddChild(UIElement element)
        {
            if (element == null)
            {
                return;
            }

            element.parent = this;
            element.RepositionRelativeToParent();
            this.children.Add(element);
        }

        #endregion

        #region Data

        internal bool ContainsData(string name)
        {
            return this.data.ContainsKey(name);
        }

        internal void SetData(string name, object value)
        {
            this.data[name] = value;
        }

        internal object GetData(string name)
        {
            return this.data[name];
        }

        #endregion
    }
}

