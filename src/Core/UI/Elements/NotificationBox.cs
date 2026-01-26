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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class NotificationBox : UIElement
    {
        private readonly struct NotificationInformation(TextureIndex iconTextureIndex, Rectangle? iconSourceRectangle, string message)
        {
            internal readonly TextureIndex IconTextureIndex => iconTextureIndex;
            internal readonly Rectangle? IconSourceRectangle => iconSourceRectangle;
            internal readonly string Message => message;
        }

        private bool isShowingProcess;
        private bool isShowing;

        private readonly Image background;
        private readonly Image icon;
        private readonly Label label;

        private readonly Queue<NotificationInformation> notificationQueue = [];

        private static readonly Vector2 HiddenMargin = new(0.0f, 96.0f);
        private static readonly Vector2 VisibleMargin = new(0.0f, -48.0f);

        internal NotificationBox()
        {
            this.Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2();

            this.background = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 120),
                Alignment = UIDirection.South,
            };

            this.icon = new()
            {
                Size = new(32f),
                Scale = new(2.0f),
                Color = AAP64ColorPalette.White,
                Alignment = UIDirection.West,
                Margin = new(16.0f, 0.0f),
            };

            this.label = new()
            {
                Scale = new(0.11f),
                SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                Color = AAP64ColorPalette.White,
                Alignment = UIDirection.West,
                Margin = new(this.icon.Size.X + this.icon.Margin.X + 16.0f, 0.0f),
            };

            AddChild(this.background);
            this.background.AddChild(this.icon);
            this.background.AddChild(this.label);
        }

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.Margin = Vector2.Lerp(this.Margin, this.isShowing ? VisibleMargin : HiddenMargin, 0.2f);

            if (!this.isShowingProcess && this.notificationQueue.Count > 0)
            {
                _ = Task.Run(ShowNextNotificationAsync);
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            return;
        }

        internal void AppendNotification(TextureIndex iconTextureIndex, Rectangle? sourceRectangle, string message)
        {
            this.notificationQueue.Enqueue(new(iconTextureIndex, sourceRectangle, message));
        }

        private async Task ShowNextNotificationAsync()
        {
            if (this.notificationQueue.Count == 0)
            {
                return;
            }

            this.isShowingProcess = true;

            while (this.notificationQueue.TryDequeue(out NotificationInformation information))
            {
                this.isShowing = true;

                this.icon.TextureIndex = information.IconTextureIndex;
                this.icon.SourceRectangle = information.IconSourceRectangle;
                this.label.TextContent = information.Message;
                this.background.Scale = new(this.label.Size.X + this.icon.Size.X + 48.0f, 88.0f);

                await Task.Delay(TimeSpan.FromSeconds(5.0f));

                this.isShowing = false;

                await Task.Delay(TimeSpan.FromSeconds(0.5f));
            }

            this.isShowingProcess = false;

            await Task.CompletedTask;
        }
    }
}
