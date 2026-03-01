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

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Serialization;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Saving;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class WorldExplorerUI : UIBase
    {
        private int currentPage = 0, totalPages = 1;

        private Image panelBackground;
        private Label title, pageIndexLabel;

        private SlotInfo[] menuButtonSlotInfos;

        private readonly SlotInfo[] worldButtonSlotInfos, paginationButtonSlotInfos;
        private readonly ButtonInfo[] menuButtonInfos, paginationButtonInfos;

        private readonly List<SaveFile> loadedSaveFiles = [];

        private readonly WorldDetailsUI worldDetailsMenuUI;
        private readonly GraphicsDevice graphicsDevice;
        private readonly UIManager uiManager;

        internal WorldExplorerUI(
            GraphicsDevice graphicsDevice,
            UIManager uiManager,
            WorldDetailsUI worldDetailsMenuUI
        ) : base()
        {
            this.graphicsDevice = graphicsDevice;
            this.uiManager = uiManager;
            this.worldDetailsMenuUI = worldDetailsMenuUI;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(192, 0, 32, 32), Localization_Statements.Exit, string.Empty, this.uiManager.CloseUI),
                new(TextureIndex.IconUI, new(160, 192, 32, 32), Localization_Statements.Reload, string.Empty, Reload),
                new(TextureIndex.IconUI, new(32, 32, 32, 32), Localization_GUIs.WorldExplorer_OpenInDirectory_Name, string.Empty, () =>
                {
                    Directory.OpenDirectoryInFileExplorer(Directory.Worlds);
                }),
            ];

            this.paginationButtonInfos = [
                new(TextureIndex.IconUI, new(128, 160, 32, 32), Localization_Statements.Previous, string.Empty, () =>
                {
                    if (this.currentPage > 0)
                    {
                        this.currentPage--;
                    }
                    else
                    {
                        this.currentPage = this.totalPages - 1;
                    }

                    RefreshWorldsCatalog();
                    RecalculatePagination();
                }),
                new(TextureIndex.IconUI, new(64, 160, 32, 32), Localization_Statements.Next, string.Empty, () =>
                {
                    if (this.currentPage < this.totalPages - 1)
                    {
                        this.currentPage++;
                    }
                    else
                    {
                        this.currentPage = 0;
                    }

                    RefreshWorldsCatalog();
                    RecalculatePagination();
                }),
            ];

            this.worldButtonSlotInfos = new SlotInfo[UIConstants.WORLD_EXPLORER_ITEMS_PER_PAGE];
            this.paginationButtonSlotInfos = new SlotInfo[this.paginationButtonInfos.Length];
        }

        private void RecalculatePagination()
        {
            this.totalPages = (int)MathF.Max(1.0f, MathF.Ceiling(this.loadedSaveFiles.Count / (float)UIConstants.WORLD_EXPLORER_ITEMS_PER_PAGE));
            this.currentPage = Math.Clamp(this.currentPage, 0, this.totalPages - 1);

            this.pageIndexLabel.TextContent = string.Concat(this.currentPage + 1, " / ", Math.Max(this.totalPages, 1));
        }

        private void RefreshWorldsCatalog()
        {
            int startIndex = this.currentPage * UIConstants.WORLD_EXPLORER_ITEMS_PER_PAGE;

            for (int i = 0; i < this.worldButtonSlotInfos.Length; i++)
            {
                SlotInfo slotInfoElement = this.worldButtonSlotInfos[i];
                int worldIndex = startIndex + i;

                if (worldIndex < this.loadedSaveFiles?.Count)
                {
                    SaveFile saveFile = this.loadedSaveFiles[worldIndex];

                    slotInfoElement.Background.CanDraw = true;

                    slotInfoElement.Icon.DisposeTexture();
                    slotInfoElement.Icon.Texture = saveFile.ThumbnailTextureData.ToTexture2D(this.graphicsDevice);
                    slotInfoElement.Label.TextContent = saveFile.Metadata.Name.Truncate(10);
                }
                else
                {
                    slotInfoElement.Background.CanDraw = false;
                }
            }
        }

        private void LoadAllSaveFiles()
        {
            this.loadedSaveFiles.Clear();

            foreach (SaveFile saveFile in SavingSerializer.LoadAll(LoadFlags.Thumbnail | LoadFlags.Metadata))
            {
                this.loadedSaveFiles.Add(saveFile);
            }
        }

        private void Reload()
        {
            LoadAllSaveFiles();
            this.currentPage = 0;

            RefreshWorldsCatalog();
            RecalculatePagination();
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildMenuButtons();
            BuildWorldDisplaySlots();
            BuildPagination();

            RefreshWorldsCatalog();
            RecalculatePagination();
        }

        private void BuildBackground(Container root)
        {
            // Background
            this.panelBackground = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundWorldExplorer,
                Size = new(823.0f, 629.0f),
            };

            // Title
            this.title = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.1f),
                Margin = new(16.0f, 15.0f),
                TextContent = Localization_GUIs.WorldExplorer_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            root.AddChild(this.panelBackground);
            this.panelBackground.AddChild(this.title);
        }

        private void BuildMenuButtons()
        {
            this.menuButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(
                this.panelBackground,
                this.menuButtonInfos,
                new(-4.0f, 6.5f),
                -76.0f,
                UIDirection.Northeast
            );
        }

        private void BuildWorldDisplaySlots()
        {
            Vector2 margin = new(17.0f, 91.0f);

            int index = 0;

            for (int row = 0; row < UIConstants.WORLD_EXPLORER_ITEMS_PER_ROW; row++)
            {
                for (int col = 0; col < UIConstants.WORLD_EXPLORER_ITEMS_PER_COLUMN; col++)
                {
                    Image background = new()
                    {
                        TextureIndex = TextureIndex.UIButtons,
                        SourceRectangle = new(0, 0, 386, 140),
                        Size = new(386.0f, 140.0f),
                        Margin = margin
                    };

                    Image thumbnail = new()
                    {
                        Scale = new(5.1f),
                        Size = WorldConstants.WORLD_THUMBNAIL_SIZE.ToVector2(),
                        Alignment = UIDirection.West,
                        Margin = new(11.5f, 0.0f),
                    };

                    Label title = new()
                    {
                        SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                        Scale = new(0.1f),
                        Margin = new((WorldConstants.WORLD_THUMBNAIL_SIZE.X * thumbnail.Scale.X) + 22.0f, 5.0f),
                        TextContent = "Title",

                        BorderColor = AAP64ColorPalette.DarkGray,
                        BorderDirections = LabelBorderDirection.All,
                        BorderOffset = 2.0f,
                        BorderThickness = 2.0f,
                    };

                    // Position
                    this.panelBackground.AddChild(background);
                    background.AddChild(thumbnail);
                    background.AddChild(title);

                    // Spacing
                    margin.X += 402.0f;

                    this.worldButtonSlotInfos[index] = new(background, thumbnail, title);

                    index++;
                }

                margin.X = 17.0f;
                margin.Y += 156.0f;
            }
        }

        private void BuildPagination()
        {
            this.pageIndexLabel = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = UIDirection.South,
                Margin = new(0.0f, -12.0f),
                TextContent = "1 / 1",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.panelBackground.AddChild(this.pageIndexLabel);

            for (int i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = new(
                    new()
                    {
                        TextureIndex = TextureIndex.UIButtons,
                        SourceRectangle = new(320, 140, 32, 32),
                        Scale = new(1.6f),
                        Size = new(32.0f),
                    },

                    new()
                    {
                        TextureIndex = this.paginationButtonInfos[i].TextureIndex,
                        SourceRectangle = this.paginationButtonInfos[i].TextureSourceRectangle,
                        Alignment = UIDirection.Center,
                        Size = new(32.0f)
                    }
                );

                // Spacing
                this.paginationButtonSlotInfos[i] = slot;

                // Adding
                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }

            SlotInfo left = this.paginationButtonSlotInfos[0];
            left.Background.Alignment = UIDirection.Southwest;
            left.Background.Margin = new(10.0f, -10.0f);

            SlotInfo right = this.paginationButtonSlotInfos[1];
            right.Background.Alignment = UIDirection.Southeast;
            right.Background.Margin = new(-10.0f);

            for (int i = 0; i < this.paginationButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateMenuButtons();
            UpdateSlotButtons();
            UpdatePagination();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateSlotButtons()
        {
            for (int i = 0; i < this.worldButtonSlotInfos.Length; i++)
            {
                SlotInfo slotInfoElement = this.worldButtonSlotInfos[i];

                if (!this.worldButtonSlotInfos[i].Background.CanDraw)
                {
                    break;
                }

                if (Interaction.OnMouseEnter(slotInfoElement.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slotInfoElement.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.worldDetailsMenuUI.SetSaveFile(this.graphicsDevice, this.loadedSaveFiles[(this.currentPage * UIConstants.WORLD_EXPLORER_ITEMS_PER_PAGE) + i].Metadata.Name);
                    this.uiManager.OpenUI(UIIndex.WorldDetails);
                    break;
                }

                slotInfoElement.Background.Color = Interaction.OnMouseOver(slotInfoElement.Background) ? AAP64ColorPalette.LightGrayBlue : AAP64ColorPalette.White;
            }
        }

        private void UpdatePagination()
        {
            for (int i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.paginationButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        protected override void OnOpened()
        {
            Reload();
        }

        protected override void OnClosed()
        {
            this.loadedSaveFiles.Clear();

            for (int i = 0; i < this.worldButtonSlotInfos.Length; i++)
            {
                this.worldButtonSlotInfos[i].Icon.DisposeTexture();
            }
        }
    }
}

