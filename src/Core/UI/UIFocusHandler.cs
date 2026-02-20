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

using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.UI.Elements;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.UI
{
    internal sealed class UIFocusHandler
    {
        internal bool HasFocusedElement => this.CurrentFocusedElement != null;
        internal UIElement CurrentFocusedElement { get; private set; }

        private readonly List<UIElement> focusableElements = [];

        internal void Initialize()
        {
            if (this.focusableElements.Count > 0)
            {
                SetFocus(this.focusableElements[0]);
            }
        }

        internal void Update()
        {
            foreach (UIElement element in this.focusableElements)
            {
                if (Interaction.OnMouseEnter(element))
                {
                    SetFocus(element);
                }

                if (Interaction.OnMouseLeftClick(element))
                {
                    SetFocus(element);
                    Select();
                }
            }
        }

        internal void AddFocusableElement(UIElement element)
        {
            if (!this.focusableElements.Contains(element))
            {
                this.focusableElements.Add(element);
            }
        }

        private void SetFocus(UIElement element)
        {
            if (this.CurrentFocusedElement == element)
            {
                return;
            }

            this.CurrentFocusedElement?.OnFocusLost?.Invoke(this.CurrentFocusedElement);
            this.CurrentFocusedElement = element;
            this.CurrentFocusedElement.OnFocusGained?.Invoke(element);
        }

        private static bool IsInDirection(Vector2 delta, Direction direction)
        {
            return direction switch
            {
                Direction.Right => delta.X > 0,
                Direction.Left => delta.X < 0,
                Direction.Down => delta.Y > 0,
                Direction.Up => delta.Y < 0,
                _ => false
            };
        }

        private static float ComputeScore(Vector2 delta, Direction direction)
        {
            float primary = direction is Direction.Left or Direction.Right
                ? Math.Abs(delta.X)
                : Math.Abs(delta.Y);

            float secondary = direction is Direction.Left or Direction.Right
                ? Math.Abs(delta.Y)
                : Math.Abs(delta.X);

            return primary + (secondary * 3f);
        }

        private UIElement FindNextFocusable(Direction direction)
        {
            RectangleF currentBounds = this.CurrentFocusedElement.Bounds;
            Vector2 currentCenter = currentBounds.Center;

            UIElement best = null;
            float bestScore = float.MaxValue;

            foreach (UIElement candidate in this.focusableElements)
            {
                if (candidate == this.CurrentFocusedElement || !candidate.CanDraw)
                {
                    continue;
                }

                Vector2 candidateCenter = candidate.Bounds.Center;
                Vector2 delta = candidateCenter - currentCenter;

                if (!IsInDirection(delta, direction))
                {
                    continue;
                }

                float score = ComputeScore(delta, direction);

                if (score < bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }

            return best;
        }

        internal void Move(Direction direction)
        {
            if (!this.HasFocusedElement)
            {
                return;
            }

            UIElement next = FindNextFocusable(direction);

            if (next != null)
            {
                SetFocus(next);
            }
        }

        internal void Select()
        {
            if (!this.HasFocusedElement)
            {
                return;
            }

            this.CurrentFocusedElement.OnSelected?.Invoke();
        }
    }
}
