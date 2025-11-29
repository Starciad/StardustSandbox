using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Enums.Directions;

using System;

namespace StardustSandbox.UISystem.Elements
{
    internal sealed class StackPanel : UIElement
    {
        internal enum Orientation
        {
            Vertical,
            Horizontal
        }
        
        internal Orientation PanelOrientation { get; set; } = Orientation.Vertical;
        internal float Spacing { get; set; } = 0f;
        internal Vector2 Padding { get; set; } = Vector2.Zero;

        private const string FLEX_DATA_KEY = "stackpanel:flex";
        private const string CROSS_ALIGN_DATA_KEY = "stackpanel:cross_align";

        internal StackPanel()
        {
            this.CanDraw = true;
            this.CanUpdate = true;
        }

        internal override void Initialize()
        {
            base.Initialize();
            LayoutChildren();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            LayoutChildren();
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        internal static void SetChildFlex(UIElement child, float flex)
        {
            if (child == null)
            {
                return;
            }

            if (flex <= 0f)
            {
                if (child.ContainsData(FLEX_DATA_KEY))
                {
                    child.RemoveData(FLEX_DATA_KEY);
                }

                return;
            }

            if (child.ContainsData(FLEX_DATA_KEY))
            {
                child.UpdateData(FLEX_DATA_KEY, flex);
            }
            else
            {
                child.AddData(FLEX_DATA_KEY, flex);
            }
        }

        internal static float GetChildFlex(UIElement child)
        {
            return child == null ? 0f : !child.ContainsData(FLEX_DATA_KEY) ? 0f : Convert.ToSingle(child.GetData(FLEX_DATA_KEY));
        }

        internal static void SetChildCrossAlignment(UIElement child, CardinalDirection direction)
        {
            if (child == null)
            {
                return;
            }

            if (child.ContainsData(CROSS_ALIGN_DATA_KEY))
            {
                child.UpdateData(CROSS_ALIGN_DATA_KEY, direction);
            }
            else
            {
                child.AddData(CROSS_ALIGN_DATA_KEY, direction);
            }
        }

        internal static CardinalDirection GetChildCrossAlignment(UIElement child)
        {
            return child == null
                ? CardinalDirection.Northwest
                : !child.ContainsData(CROSS_ALIGN_DATA_KEY) ? CardinalDirection.Northwest : (CardinalDirection)child.GetData(CROSS_ALIGN_DATA_KEY);
        }

        private void LayoutChildren()
        {
            int childCount = this.Children.Count;

            if (childCount == 0)
            {
                return;
            }

            // Available content rectangle inside padding
            Vector2 panelPos = this.Position;
            Vector2 panelSize = this.Size;
            float contentWidth = Math.Max(0f, panelSize.X - (this.Padding.X * 2f));
            float contentHeight = Math.Max(0f, panelSize.Y - (this.Padding.Y * 2f));

            bool vertical = this.PanelOrientation == Orientation.Vertical;

            // Main axis available
            float availableMain = vertical ? contentHeight : contentWidth;

            // Sum fixed sizes and compute total flex
            float totalFixed = 0f;
            float totalFlex = 0f;
            float totalSpacing = Math.Max(0, childCount - 1) * this.Spacing;

            // Compute fixed contribution (use child's current Size on main axis)
            foreach (UIElement child in this.Children)
            {
                float childMainSize = vertical ? child.Size.Y : child.Size.X;
                float childMarginMain = vertical ? child.Margin.Y : child.Margin.X;
                float flex = GetChildFlex(child);

                if (flex > 0f)
                {
                    totalFlex += flex;
                }
                else
                {
                    totalFixed += childMainSize + childMarginMain;
                }
            }

            float remaining = availableMain - totalFixed - totalSpacing;

            if (remaining < 0f)
            {
                remaining = 0f;
            }

            // Starting offset (top-left inside padding)
            Vector2 contentOrigin = new(panelPos.X + this.Padding.X, panelPos.Y + this.Padding.Y);
            float offsetMain = 0f;

            for (int i = 0; i < childCount; i++)
            {
                UIElement child = this.Children[i];

                float childMarginMainStart = vertical ? child.Margin.Y : child.Margin.X; // Using same margin value on both sides (simple)
                float childMarginMainEnd = childMarginMainStart;

                // Decide main axis size
                float flex = GetChildFlex(child);
                float childMainSize = flex > 0f && totalFlex > 0f ? flex / totalFlex * remaining : vertical ? child.Size.Y : child.Size.X;

                // Decide cross axis size
                float availableCross = vertical ? contentWidth : contentHeight;
                float childCrossSize = vertical ? child.Size.X : child.Size.Y;
                float childMarginCross = vertical ? child.Margin.X : child.Margin.Y;

                if (childCrossSize <= 0f)
                {
                    // Stretch to fill cross axis (respecting margin)
                    childCrossSize = Math.Max(0f, availableCross - childMarginCross);
                }

                // Final size vector
                Vector2 finalSize = vertical ? new Vector2(childCrossSize, childMainSize) : new Vector2(childMainSize, childCrossSize);

                // Apply size to child (this will trigger child's RepositionRelativeToParent internally)
                child.Size = finalSize;

                // Compute child's position
                float posMain = (vertical ? contentOrigin.Y : contentOrigin.X) + offsetMain + childMarginMainStart;
                float posCrossBase = vertical ? contentOrigin.X : contentOrigin.Y;

                // Compute cross-axis alignment
                float crossAvailable = availableCross - childMarginCross;
                float posCross;
                CardinalDirection crossDir = GetChildCrossAlignment(child);

                bool alignStart = IsCrossAlignStart(crossDir, vertical);
                _ = IsCrossAlignCenter(crossDir);
                bool alignEnd = IsCrossAlignEnd(crossDir, vertical);

                posCross = alignStart
                    ? posCrossBase + (vertical ? child.Margin.X : child.Margin.Y)
                    : alignEnd
                        ? posCrossBase + (crossAvailable - (vertical ? child.Size.X : child.Size.Y)) + (vertical ? child.Margin.X : child.Margin.Y)
                        : posCrossBase + ((crossAvailable - (vertical ? child.Size.X : child.Size.Y)) / 2f) + (vertical ? child.Margin.X : child.Margin.Y);

                Vector2 finalPosition = vertical ? new Vector2(posCross, posMain) : new Vector2(posMain, posCross);

                // Set child's Position (this will call RepositionChildren on that child)
                child.Position = finalPosition;

                // Advance offset
                offsetMain += childMarginMainStart + childMainSize + childMarginMainEnd;

                if (i < childCount - 1)
                {
                    offsetMain += this.Spacing;
                }
            }
        }

        private static bool IsCrossAlignStart(CardinalDirection dir, bool vertical)
        {
            // vertical -> cross axis is horizontal -> start = West variants
            // horizontal -> cross axis is vertical -> start = North variants
            return dir != CardinalDirection.Center && (vertical
                ? dir == CardinalDirection.West
                    || dir == CardinalDirection.Northwest
                    || dir == CardinalDirection.Southwest
                : dir == CardinalDirection.North
                    || dir == CardinalDirection.Northeast
                    || dir == CardinalDirection.Northwest);
        }

        private static bool IsCrossAlignEnd(CardinalDirection dir, bool vertical)
        {
            // vertical -> cross axis is horizontal -> end = East variants
            // horizontal -> cross axis is vertical -> end = South variants
            return dir != CardinalDirection.Center && (vertical
                ? dir == CardinalDirection.East
                    || dir == CardinalDirection.Northeast
                    || dir == CardinalDirection.Southeast
                : dir == CardinalDirection.South
                    || dir == CardinalDirection.Southeast
                    || dir == CardinalDirection.Southwest);
        }

        private static bool IsCrossAlignCenter(CardinalDirection dir)
        {
            return dir == CardinalDirection.Center;
        }
    }
}
