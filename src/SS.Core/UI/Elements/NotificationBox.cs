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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Indexers;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.UI.Elements
{
    internal sealed class NotificationBox : UIElement
    {
        private readonly struct NotificationEntry(TextureIndex iconTextureIndex, Rectangle? iconSourceRectangle, string message)
        {
            internal TextureIndex IconTextureIndex => iconTextureIndex;
            internal Rectangle? IconSourceRectangle => iconSourceRectangle;
            internal string Message => message;
        }

        private enum DisplayState : byte
        {
            Idle,
            Showing,
            Hiding
        }

        // State machine
        private DisplayState state = DisplayState.Idle;
        private float stateTimerSeconds;

        // UI children
        private readonly Image background;
        private readonly Image icon;
        private readonly Text text;

        // Queue
        private readonly Queue<NotificationEntry> notifications = new();
        private readonly object queueLock = new();

        // Dependencies
        private readonly AssetDatabase assetDatabase;

        // Visual / timing configuration
        private static readonly Vector2 HIDDEN_MARGIN = new(0.0f, 96.0f);
        private static readonly Vector2 VISIBLE_MARGIN = new(0.0f, -48.0f);

        internal NotificationBox(AssetDatabase assetDatabase, GameScreen gameScreen)
        {
            this.assetDatabase = assetDatabase;
            this.Size = gameScreen.Viewport;

            this.background = new(assetDatabase.GetTexture(TextureIndex.Pixel))
            {
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 120),
                Alignment = UIAlignment.South,
            };

            this.icon = new()
            {
                Size = new(32f),
                Scale = new(2.0f),
                Color = AAP64ColorPalette.White,
                Alignment = UIAlignment.West,
                Margin = new(16.0f, 0.0f),
            };

            this.text = new()
            {
                SpriteFont = assetDatabase.GetSpriteFont(SpriteFontIndex.Font_01),
                Scale = new(0.11f),
                Alignment = UIAlignment.West,
                Margin = new(this.icon.Size.X + this.icon.Margin.X + 16.0f, 0.0f),
            };

            this.background.AddChild(this.icon);
            this.background.AddChild(this.text);
            AddChild(this.background);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            // Smoothly animate visibility margin
            this.Margin = Vector2.Lerp(this.Margin, this.state == DisplayState.Showing ? VISIBLE_MARGIN : HIDDEN_MARGIN, UIConstants.NOTIFICATION_MARGIN_LERP_FACTOR);

            float deltaSeconds = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // If idle and there is a pending notification, start it immediately (safely)
            if (this.state == DisplayState.Idle)
            {
                NotificationEntry next;
                lock (this.queueLock)
                {
                    next = this.notifications.Count == 0 ? default : this.notifications.Dequeue();
                }

                if (!Equals(next, default(NotificationEntry)))
                {
                    StartNotification(next);
                }
            }

            // State machine timing
            switch (this.state)
            {
                case DisplayState.Showing:
                    this.stateTimerSeconds -= deltaSeconds;
                    if (this.stateTimerSeconds <= 0f)
                    {
                        // Begin hiding
                        this.state = DisplayState.Hiding;
                        this.stateTimerSeconds = UIConstants.NOTIFICATION_HIDE_DURATION_SECONDS;
                        // Toggle visibility; margin lerp above will animate it out
                        // keep currentNotification until hiding finishes
                    }

                    break;

                case DisplayState.Hiding:
                    this.stateTimerSeconds -= deltaSeconds;
                    if (this.stateTimerSeconds <= 0f)
                    {
                        // Finish current notification
                        this.state = DisplayState.Idle;
                        this.stateTimerSeconds = 0f;
                    }

                    break;

                case DisplayState.Idle:
                default:
                    // nothing to do
                    break;
            }
        }

        internal void EnqueueNotification(TextureIndex iconTextureIndex, Rectangle? sourceRectangle, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            NotificationEntry entry = new(iconTextureIndex, sourceRectangle, message);

            lock (this.queueLock)
            {
                this.notifications.Enqueue(entry);
            }
        }

        private void StartNotification(NotificationEntry entry)
        {
            // Apply UI values immediately on the game/main thread
            this.icon.Texture = this.assetDatabase.GetTexture(entry.IconTextureIndex);
            this.icon.SourceRectangle = entry.IconSourceRectangle;
            this.text.TextContent = entry.Message;

            // Recompute background size based on label/icon sizes.
            this.background.Scale = new(this.text.Size.X + this.icon.Size.X + 48.0f, 88.0f);

            // Set state to showing and start timer
            this.state = DisplayState.Showing;
            this.stateTimerSeconds = UIConstants.NOTIFICATION_DISPLAY_DURATION_SECONDS;
        }

        internal void OnScreenResize(Vector2 newSize)
        {
            this.Size = newSize;
        }
    }
}
