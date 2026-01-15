using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;

using System;

namespace StardustSandbox.UI
{
    internal static class UIBuilderUtility
    {
        internal static SlotInfo BuildButtonSlot(Vector2 margin, TextureIndex iconTextureIndex, Rectangle? iconTextureSourceRectangle)
        {
            return new(
                background: new()
                {
                    TextureIndex = TextureIndex.UIButtons,
                    SourceRectangle = new(320, 140, 32, 32),
                    Scale = new(2.0f),
                    Size = new(32.0f),
                    Margin = margin,
                },

                icon: new()
                {
                    TextureIndex = iconTextureIndex,
                    SourceRectangle = iconTextureSourceRectangle,
                    Scale = new(1.5f),
                    Size = new(32.0f)
                }
            );
        }

        internal static SlotInfo BuildButtonSlot(Vector2 margin, ButtonInfo button)
        {
            return BuildButtonSlot(margin, button.TextureIndex, button.TextureSourceRectangle);
        }

        internal static void BuildGridButtons(UIElement parent, SlotInfo[] slotInfos, ButtonInfo[] buttonInfo, int itemsPerRow, Vector2 start, Vector2 spacing, UIDirection backgroundAlignment)
        {
            if (slotInfos.Length != buttonInfo.Length)
            {
                throw new ArgumentException($"{nameof(slotInfos)} and {nameof(buttonInfo)} arrays must have the same length.");
            }

            for (int i = 0; i < slotInfos.Length; i++)
            {
                int col = i % itemsPerRow;
                int row = i / itemsPerRow;

                Vector2 position = new(start.X + (col * spacing.X), start.Y + (row * spacing.Y));
                SlotInfo slot = BuildButtonSlot(position, buttonInfo[i]);

                slot.Background.Alignment = backgroundAlignment;
                slot.Icon.Alignment = UIDirection.Center;

                parent.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                slotInfos[i] = slot;
            }
        }

        internal static void BuildHorizontalButtonLine(UIElement parent, SlotInfo[] slotInfos, ButtonInfo[] buttonInfo, Vector2 start, float spacingX, UIDirection backgroundAlignment)
        {
            if (slotInfos.Length != buttonInfo.Length)
            {
                throw new ArgumentException($"{nameof(slotInfos)} and {nameof(buttonInfo)} arrays must have the same length.");
            }

            for (int i = 0; i < slotInfos.Length; i++)
            {
                Vector2 position = new(
                    start.X + (i * spacingX),
                    start.Y
                );

                SlotInfo slot = BuildButtonSlot(position, buttonInfo[i]);

                slot.Background.Alignment = backgroundAlignment;
                slot.Icon.Alignment = UIDirection.Center;

                parent.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                slotInfos[i] = slot;
            }
        }

        internal static void BuildVerticalButtonLine(UIElement parent, SlotInfo[] slotInfos, ButtonInfo[] buttonInfo, Vector2 start, float spacingY, UIDirection backgroundAlignment)
        {
            if (slotInfos.Length != buttonInfo.Length)
            {
                throw new ArgumentException($"{nameof(slotInfos)} and {nameof(buttonInfo)} arrays must have the same length.");
            }

            for (int i = 0; i < slotInfos.Length; i++)
            {
                Vector2 position = new(
                    start.X,
                    start.Y + (i * spacingY)
                );

                SlotInfo slot = BuildButtonSlot(position, buttonInfo[i]);

                slot.Background.Alignment = backgroundAlignment;
                slot.Icon.Alignment = UIDirection.Center;

                parent.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                slotInfos[i] = slot;
            }
        }
    }
}
