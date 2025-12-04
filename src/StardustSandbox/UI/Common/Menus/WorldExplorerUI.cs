using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.Extensions;
using StardustSandbox.IO;
using StardustSandbox.Managers;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Saving;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class WorldExplorerUI : UIBase
    {
        private int currentPage = 0;
        private int totalPages = 1;

        private List<SaveFile> savedWorldFilesLoaded;
        private Image headerBackground;
        private Label pageIndexLabel;

        private readonly ButtonInfo[] headerButtonInfos, footerButtonInfos;

        private readonly Image[] headerButtonImages;
        private readonly Label[] footerButtonLabels;
        private readonly SlotInfo[] itemSlotInfos;

        private readonly WorldDetailsUI worldDetailsMenuUI;

        private readonly GraphicsDevice graphicsDevice;
        private readonly UIManager uiManager;

        internal WorldExplorerUI(
            GraphicsDevice graphicsDevice,
            UIIndex index,
            UIManager uiManager,
            WorldDetailsUI worldDetailsMenuUI
        ) : base(index)
        {
            this.graphicsDevice = graphicsDevice;
            this.uiManager = uiManager;
            this.worldDetailsMenuUI = worldDetailsMenuUI;

            this.itemSlotInfos = new SlotInfo[UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE];

            this.headerButtonInfos = [
                new(TextureIndex.IconUI, new(192, 0, 32, 32), "Exit", string.Empty, this.uiManager.CloseGUI),
                new(TextureIndex.IconUI, new(160, 192, 32, 32), "Reload", string.Empty, () =>
                {
                    LoadAllLocalSavedWorlds();
                    this.currentPage = 0;
                    UpdatePagination();
                    ChangeWorldsCatalog();
                }),
                new(TextureIndex.IconUI, new(32, 32, 32, 32), "Open Directory in Explorer", string.Empty, () =>
                {
                    SSDirectory.OpenDirectoryInFileExplorer(SSDirectory.Worlds);
                }),
            ];

            this.footerButtonInfos = [
                new(TextureIndex.None, null, "Previous", string.Empty, () =>
                {
                    if (this.currentPage > 0)
                    {
                        this.currentPage--;
                    }
                    else
                    {
                        this.currentPage = this.totalPages - 1;
                    }

                    ChangeWorldsCatalog();
                }),
                new(TextureIndex.None, null, "Next", string.Empty, () =>
                {
                    if (this.currentPage < this.totalPages - 1)
                    {
                        this.currentPage++;
                    }
                    else
                    {
                        this.currentPage = 0;
                    }

                    ChangeWorldsCatalog();
                }),
            ];

            this.headerButtonImages = new Image[this.headerButtonInfos.Length];
            this.footerButtonLabels = new Label[this.footerButtonInfos.Length];

            UpdatePagination();
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildHeader(root);
            BuildFooter(root);

            BuildingWorldDisplaySlots();
        }

        private void BuildHeader(Container root)
        {
            // Background
            this.headerBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Size = Vector2.One,
                Scale = new(ScreenConstants.SCREEN_WIDTH, 96.0f),
            };

            root.AddChild(headerBackground);

            // Title
            Label titleLabelElement = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.West,
                Margin = new(32.0f, 0.0f),
                TextContent = "World Explorer",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.headerBackground.AddChild(titleLabelElement);

            // Buttons
            float marginX = -32.0f;

            for (byte i = 0; i < this.headerButtonInfos.Length; i++)
            {
                ButtonInfo button = this.headerButtonInfos[i];

                Image buttonBackgroundElement = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Alignment = CardinalDirection.East,
                    Margin = new(marginX, 0.0f),
                    Scale = new(2.0f),
                    Size = new(32.0f),
                };

                Image buttonIconElement = new()
                {
                    Alignment = CardinalDirection.Center,
                    Texture = button.Texture,
                    SourceRectangle = button.TextureSourceRectangle,
                    Scale = new(1.5f),
                    Size = new(32.0f),
                };

                this.headerBackground.AddChild(buttonBackgroundElement);
                buttonBackgroundElement.AddChild(buttonIconElement);

                this.headerButtonImages[i] = buttonBackgroundElement;

                marginX -= buttonBackgroundElement.Size.X + 16.0f;
            }
        }

        private void BuildFooter(Container root)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Size = Vector2.One,
                Scale = new(ScreenConstants.SCREEN_WIDTH, 96.0f),
                Alignment = CardinalDirection.Southwest,
            };

            Label pageIndexTitleLabel = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Center,
                TextContent = "Current Page",
                Margin = new(0.0f, -18.0f),

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.pageIndexLabel = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Center,
                TextContent = "1 / 1",
                Margin = new(0.0f, pageIndexTitleLabel.Size.Y),

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            Label previousButtonLabel = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.West,
                TextContent = "Previous",
                Margin = new(32.0f, 0.0f),

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            Label nextButtonLabel = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.East,
                TextContent = "Next",
                Margin = new(-32.0f, 0.0f),

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.footerButtonLabels[0] = previousButtonLabel;
            this.footerButtonLabels[1] = nextButtonLabel;

            background.AddChild(pageIndexTitleLabel);
            background.AddChild(previousButtonLabel);
            background.AddChild(nextButtonLabel);

            pageIndexTitleLabel.AddChild(this.pageIndexLabel);

            root.AddChild(background);
        }

        // ========================================================================== //

        private void BuildingWorldDisplaySlots()
        {
            Vector2 margin = new(32.0f, 118.0f);

            int rows = UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_ROW;
            int columns = UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_COLUMN;

            int index = 0;

            for (byte col = 0; col < columns; col++)
            {
                for (byte row = 0; row < rows; row++)
                {
                    Image background = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                        SourceRectangle = new(0, 0, 386, 140),
                        Size = new(386.0f, 140.0f),
                        Margin = margin
                    };

                    Image thumbnail = new()
                    {
                        Scale = new(5.1f),
                        Size = WorldConstants.WORLD_THUMBNAIL_SIZE.ToVector2(),
                        Alignment = CardinalDirection.West,
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
                    this.headerBackground.AddChild(background);
                    background.AddChild(thumbnail);
                    background.AddChild(title);

                    // Spacing
                    margin.X += 418.0f;

                    this.itemSlotInfos[index] = new(background, thumbnail, title);

                    index++;
                }

                margin.X = 32.0f;
                margin.Y += 172.0f;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            // HEADER
            for (byte i = 0; i < this.headerButtonImages.Length; i++)
            {
                Image buttonBackgroundElement = this.headerButtonImages[i];

                if (Interaction.OnMouseLeftClick(buttonBackgroundElement))
                {
                    this.headerButtonInfos[i].ClickAction?.Invoke();
                }

                buttonBackgroundElement.Color = Interaction.OnMouseOver(buttonBackgroundElement) ? AAP64ColorPalette.LightGrayBlue : AAP64ColorPalette.White;
            }

            // FOOTER
            for (byte i = 0; i < this.footerButtonLabels.Length; i++)
            {
                Label label = this.footerButtonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.footerButtonInfos[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }

            // SLOTS
            for (byte i = 0; i < this.itemSlotInfos.Length; i++)
            {
                SlotInfo slotInfoElement = this.itemSlotInfos[i];

                if (!this.itemSlotInfos[i].Background.CanDraw)
                {
                    break;
                }

                if (Interaction.OnMouseLeftClick(slotInfoElement.Background))
                {
                    this.worldDetailsMenuUI.SetWorldSaveFile(this.savedWorldFilesLoaded[(this.currentPage * UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE) + i]);
                    this.uiManager.OpenGUI(UIIndex.WorldDetailsMenu);
                }

                slotInfoElement.Background.Color = Interaction.OnMouseOver(slotInfoElement.Background) ? AAP64ColorPalette.LightGrayBlue : AAP64ColorPalette.White;
            }

            base.Update(gameTime);
        }

        private void UpdatePagination()
        {
            this.totalPages = Math.Max(1, (int)Math.Ceiling((float)(this.savedWorldFilesLoaded?.Count ?? 0) / UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE));
            this.currentPage = Math.Clamp(this.currentPage, 0, this.totalPages - 1);

            if (this.pageIndexLabel != null)
            {
                this.pageIndexLabel.TextContent = string.Concat(this.currentPage + 1, " / ", Math.Max(this.totalPages, 1));
            }
        }

        private void ChangeWorldsCatalog()
        {
            int startIndex = this.currentPage * UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE;

            for (byte i = 0; i < this.itemSlotInfos.Length; i++)
            {
                SlotInfo slotInfoElement = this.itemSlotInfos[i];
                int worldIndex = startIndex + i;

                if (worldIndex < this.savedWorldFilesLoaded?.Count)
                {
                    SaveFile saveFile = this.savedWorldFilesLoaded[worldIndex];

                    slotInfoElement.Background.CanDraw = true;

                    slotInfoElement.Icon.Texture = saveFile.ThumbnailTexture;
                    slotInfoElement.Label.TextContent = saveFile.Metadata.Name.Truncate(10);
                }
                else
                {
                    slotInfoElement.Background.CanDraw = false;
                }
            }

            UpdatePagination();
        }

        #endregion

        protected override void OnOpened()
        {
            this.headerButtonInfos[1].ClickAction?.Invoke();
            ChangeWorldsCatalog();
        }

        protected override void OnClosed()
        {
            this.savedWorldFilesLoaded.Clear();
        }

        private void LoadAllLocalSavedWorlds()
        {
            this.savedWorldFilesLoaded = [.. SavingSerializer.LoadAllSavedWorldData(this.graphicsDevice)];
        }
    }
}
