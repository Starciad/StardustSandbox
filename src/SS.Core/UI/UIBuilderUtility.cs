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

using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

namespace StardustSandbox.Core.UI
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
                    Alignment = UIDirection.Center,
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

        internal static SlotInfo[] BuildGridButtons(UIElement parent, ButtonInfo[] buttonInfo, int itemsPerRow, Vector2 start, Vector2 spacing, UIDirection backgroundAlignment)
        {
            SlotInfo[] slots = new SlotInfo[buttonInfo.Length];

            for (int i = 0; i < buttonInfo.Length; i++)
            {
                int col = i % itemsPerRow;
                int row = i / itemsPerRow;

                Vector2 position = new(start.X + (col * spacing.X), start.Y + (row * spacing.Y));
                SlotInfo slot = BuildButtonSlot(position, buttonInfo[i]);

                slot.Background.Alignment = backgroundAlignment;
                slot.Icon.Alignment = UIDirection.Center;

                parent.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                slots[i] = slot;
            }

            return slots;
        }

        internal static SlotInfo[] BuildHorizontalButtonLine(UIElement parent, ButtonInfo[] buttonInfo, Vector2 start, float spacingX, UIDirection backgroundAlignment)
        {
            SlotInfo[] slots = new SlotInfo[buttonInfo.Length];

            for (int i = 0; i < buttonInfo.Length; i++)
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

                slots[i] = slot;
            }

            return slots;
        }

        internal static SlotInfo[] BuildVerticalButtonLine(UIElement parent, ButtonInfo[] buttonInfo, Vector2 start, float spacingY, UIDirection backgroundAlignment)
        {
            SlotInfo[] slots = new SlotInfo[buttonInfo.Length];

            for (int i = 0; i < buttonInfo.Length; i++)
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

                slots[i] = slot;
            }

            return slots;
        }
    }
}
