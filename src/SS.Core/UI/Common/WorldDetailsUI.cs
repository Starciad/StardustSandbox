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
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.Serialization;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Worlds;
using StardustSandbox.Core.UI.Builders;
using StardustSandbox.Core.UI.Dependencies;
using StardustSandbox.Core.UI.Elements.Common;
using StardustSandbox.Core.UI.Handlers;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.UI.Models;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class WorldDetailsUI : UIBase<WorldDetailsUIDependencies, WorldDetailsUIModel>
    {
        private Image headerBackground, shadowBackground;

        private Image worldThumbnail;
        private Label worldTitle, worldVersion, worldCreationTimestamp;
        private Text worldDescription;

        private readonly Label[] worldButtonLabels;
        private readonly ButtonInfo[] worldButtonInfos;

        internal WorldDetailsUI(WorldDetailsUIDependencies dependencies, UIElementHandler elementHandler) : base(dependencies, elementHandler)
        {
            this.worldButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Return, string.Empty, () =>
                {
                    soundEffectManager.Play(SoundEffectIndex.GUI_Click);
                    uiManager.CloseUI();
                }),
                new(TextureIndex.None, null, Localization_Statements.Delete, string.Empty, () =>
                {
                    soundEffectManager.Play(SoundEffectIndex.GUI_Click);
                    worldSerializer.Delete(this.saveFile.Metadata.Name);
                    uiManager.CloseUI();
                }),
                new(TextureIndex.None, null, Localization_Statements.Play, string.Empty, () =>
                {
                    uiManager.Reset();
                    uiManager.OpenUI(UIIndex.Main);
                    uiManager.OpenUI(UIIndex.Hud);

                    gameHandler.StartGame();
                    gameHandler.LoadSaveFile(this.saveFile.Metadata.Name);
                    soundEffectManager.Play(SoundEffectIndex.GUI_World_Loaded);
                }),
            ];

            this.worldButtonLabels = new Label[this.worldButtonInfos.Length];
        }

        protected override void OnBuild(UIBuildContext context, WorldDetailsUIModel model)
        {
            BuildBackground(root);
            BuildHeader(root);
            BuildThumbnail(root);
            BuildDescription();
            BuildCreationTimestamp(root);
            BuildVersion();
            BuildWorldButtons(root);
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new(this.assetDatabase.GetTexture(TextureIndex.Pixel))
            {
                Scale = this.GameScreen.Viewport,
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            root.AddChild(this.shadowBackground);
        }

        private void BuildHeader(Container root)
        {
            // Background
            this.headerBackground = new(this.assetDatabase.GetTexture(TextureIndex.Pixel))
            {
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Scale = new(this.GameScreen.Viewport.X, 96.0f),
                Size = Vector2.One,
            };

            // Title
            this.worldTitle = new(this.assetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm))
            {
                Scale = new(0.15f),
                Alignment = UIDirection.West,
                Margin = new(32.0f, 0.0f),

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.headerBackground.AddChild(this.worldTitle);

            root.AddChild(this.headerBackground);
        }

        private void BuildThumbnail(Container root)
        {
            this.worldThumbnail = new()
            {
                Scale = new(12.0f),
                Size = WorldConstants.WORLD_THUMBNAIL_SIZE.ToVector2(),
                Margin = new(32.0f, 128f),
            };

            root.AddChild(this.worldThumbnail);
        }

        private void BuildDescription()
        {
            this.worldDescription = new(this.assetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator))
            {
                Scale = new(0.078f),
                Margin = new((WorldConstants.WORLD_THUMBNAIL_SIZE.X * this.worldThumbnail.Scale.X) + 16.0f, 0.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(930.0f, 600.0f),
            };

            this.worldThumbnail.AddChild(this.worldDescription);
        }

        private void BuildCreationTimestamp(Container root)
        {
            this.worldCreationTimestamp = new(this.assetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm))
            {
                Scale = new(0.075f),
                Margin = new(-8.0f),
                Alignment = UIDirection.Southeast,
                TextContent = DateTime.Now.ToString(),
            };

            root.AddChild(this.worldCreationTimestamp);
        }

        private void BuildVersion()
        {
            this.worldVersion = new(this.assetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm))
            {
                Scale = new(0.075f),
                Margin = new(0.0f, this.worldCreationTimestamp.Size.Y + (64.0f * -1.0f)),
                Alignment = UIDirection.Northeast,
            };

            this.worldCreationTimestamp.AddChild(this.worldVersion);
        }

        private void BuildWorldButtons(Container root)
        {
            for (int i = 0; i < this.worldButtonInfos.Length; i++)
            {
                ButtonInfo button = this.worldButtonInfos[i];

                Label buttonLabel = new(this.assetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm))
                {
                    Scale = new(0.12f),
                    Margin = new(32.0f, -32.0f - (i * (64.0f + 8.0f))),
                    Alignment = UIDirection.Southwest,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                root.AddChild(buttonLabel);
                this.worldButtonLabels[i] = buttonLabel;
            }
        }

        protected override void OnScreenResize()
        {
            this.shadowBackground.Scale = this.GameScreen.Viewport;
            this.headerBackground.Scale = new(this.GameScreen.Viewport.X, this.headerBackground.Scale.Y);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = 0; i < this.worldButtonLabels.Length; i++)
            {
                Label slotInfoElement = this.worldButtonLabels[i];

                if (Interaction.OnMouseEnter(slotInfoElement))
                {
                    this.soundEffectManager.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slotInfoElement))
                {
                    this.worldButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slotInfoElement.Color = Interaction.OnMouseOver(slotInfoElement) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        internal void SetSaveFile(GraphicsDevice graphicsDevice, string saveFilename)
        {
            this.saveFile = this.worldSerializer.Load(saveFilename, LoadFlags.Metadata | LoadFlags.Manifest | LoadFlags.Thumbnail);
            UpdateDisplay(graphicsDevice, this.saveFile);
        }

        private void UpdateDisplay(GraphicsDevice graphicsDevice, WorldSaveFile saveFile)
        {
            this.worldThumbnail.Texture = saveFile.ThumbnailTextureData.ToTexture2D(graphicsDevice);

            this.worldTitle.TextContent = saveFile.Metadata.Name;
            this.worldDescription.TextContent = saveFile.Metadata.Description;
            this.worldVersion.TextContent = string.Concat('v', saveFile.Manifest.GameVersion);
            this.worldCreationTimestamp.TextContent = saveFile.Manifest.CreationTimestamp.ToString();
        }

        protected override void OnClosed()
        {
            this.worldThumbnail.DisposeTexture();
        }
    }
}

