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
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Saving;
using StardustSandbox.Managers;
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
        private Image headerBackgroundElement;
        private Label pageIndexLabelElement;

        private readonly ButtonInfo[] headerButtons;
        private readonly ButtonInfo[] footerButtons;

        private readonly Image[] headerButtonElements;
        private readonly Label[] footerButtonElements;
        private readonly SlotInfo[] slotInfoElements;

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

            this.slotInfoElements = new SlotInfo[UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE];

            this.headerButtons = [
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

            this.footerButtons = [
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

            this.headerButtonElements = new Image[this.headerButtons.Length];
            this.footerButtonElements = new Label[this.footerButtons.Length];

            UpdatePagination();
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildHeader();
            BuildFooter(root);

            BuildingWorldDisplaySlots();
        }

        private void BuildHeader()
        {
            // Background
            this.headerBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Size = Vector2.One,
                Scale = new(ScreenConstants.SCREEN_WIDTH, 96.0f),
            };

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

            this.headerBackgroundElement.AddChild(titleLabelElement);

            // Buttons
            float marginX = -64.0f;

            for (byte i = 0; i < this.headerButtons.Length; i++)
            {
                ButtonInfo button = this.headerButtons[i];

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
                    Texture = button.Texture,
                    Scale = new(1.5f),
                    Size = new(32.0f),
                };

                this.headerBackgroundElement.AddChild(buttonBackgroundElement);
                buttonBackgroundElement.AddChild(buttonIconElement);

                this.headerButtonElements[i] = buttonBackgroundElement;

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
                Margin = new(0.0f, -96.0f),
            };

            Label pageIndexTitleLabel = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Center,
                TextContent = "Current Page",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.pageIndexLabelElement = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Center,
                TextContent = "1 / 1",

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

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.footerButtonElements[0] = previousButtonLabel;
            this.footerButtonElements[1] = nextButtonLabel;

            pageIndexTitleLabel.Margin = new(0.0f, -16.0f);
            this.pageIndexLabelElement.Margin = new(0.0f, pageIndexTitleLabel.Size.Y);
            previousButtonLabel.Margin = new(previousButtonLabel.Size.X + 32.0f, 0.0f);
            nextButtonLabel.Margin = new((nextButtonLabel.Size.X + 32.0f) * -1.0f, 0.0f);

            background.AddChild(pageIndexTitleLabel);
            background.AddChild(previousButtonLabel);
            background.AddChild(nextButtonLabel);

            pageIndexTitleLabel.AddChild(this.pageIndexLabelElement);

            root.AddChild(background);
        }

        // ========================================================================== //

        private void BuildingWorldDisplaySlots()
        {
            Vector2 slotMargin = new(32.0f, 118.0f);

            int rows = UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_ROW;
            int columns = UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_COLUMN;

            int index = 0;

            for (byte col = 0; col < columns; col++)
            {
                for (byte row = 0; row < rows; row++)
                {
                    Image backgroundImageElement = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                        SourceRectangle = new(0, 0, 386, 140),
                        Size = new(386.0f, 140.0f),
                        Margin = slotMargin
                    };

                    Image thumbnailImageElement = new()
                    {
                        Scale = new(5.1f),
                        Size = WorldConstants.WORLD_THUMBNAIL_SIZE.ToVector2(),
                        Alignment = CardinalDirection.West,
                        Margin = new(11.5f, 0.0f),
                    };

                    Label titleLabelElement = new()
                    {
                        Color = AAP64ColorPalette.White,
                        SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                        Alignment = CardinalDirection.North,
                        Scale = new(0.1f),
                        Margin = new(-52.5f, 23.0f),
                        TextContent = "Title"
                    };

                    // Position
                    this.headerBackgroundElement.AddChild(backgroundImageElement);
                    backgroundImageElement.AddChild(thumbnailImageElement);
                    thumbnailImageElement.AddChild(titleLabelElement);

                    // Spacing
                    slotMargin.X += 418.0f;

                    this.slotInfoElements[index] = new(backgroundImageElement, thumbnailImageElement, titleLabelElement);

                    index++;
                }

                slotMargin.X = 32.0f;
                slotMargin.Y += 172.0f;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            #region BUTTONS

            // HEADER
            for (byte i = 0; i < this.headerButtonElements.Length; i++)
            {
                Image buttonBackgroundElement = this.headerButtonElements[i];

                if (Interaction.OnMouseLeftClick(buttonBackgroundElement))
                {
                    this.headerButtons[i].ClickAction?.Invoke();
                }

                buttonBackgroundElement.Color = Interaction.OnMouseOver(buttonBackgroundElement) ? AAP64ColorPalette.LightGrayBlue : AAP64ColorPalette.White;
            }

            // FOOTER
            for (byte i = 0; i < this.footerButtonElements.Length; i++)
            {
                Label label = this.footerButtonElements[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.footerButtons[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }

            #endregion

            // SLOTS
            for (byte i = 0; i < this.slotInfoElements.Length; i++)
            {
                SlotInfo slotInfoElement = this.slotInfoElements[i];

                if (!this.slotInfoElements[i].Background.CanDraw)
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
        }

        private void UpdatePagination()
        {
            this.totalPages = Math.Max(1, (int)Math.Ceiling((float)(this.savedWorldFilesLoaded?.Count ?? 0) / UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE));
            this.currentPage = Math.Clamp(this.currentPage, 0, this.totalPages - 1);

            if (this.pageIndexLabelElement != null)
            {
                this.pageIndexLabelElement.TextContent = string.Concat(this.currentPage + 1, " / ", Math.Max(this.totalPages, 1));
            }
        }

        private void ChangeWorldsCatalog()
        {
            int startIndex = this.currentPage * UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE;

            for (byte i = 0; i < this.slotInfoElements.Length; i++)
            {
                SlotInfo slotInfoElement = this.slotInfoElements[i];
                int worldIndex = startIndex + i;

                if (worldIndex < this.savedWorldFilesLoaded?.Count)
                {
                    SaveFile worldSaveFile = this.savedWorldFilesLoaded[worldIndex];

                    slotInfoElement.Background.CanDraw = true;
                    
                    slotInfoElement.Icon.Texture = worldSaveFile.Header.ThumbnailTexture;
                    slotInfoElement.Label.TextContent = worldSaveFile.Header.Metadata.Name.Truncate(10);
                }
                else
                {
                    slotInfoElement.Background.CanDraw = false;
                }
            }

            UpdatePagination();
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            this.headerButtons[1].ClickAction?.Invoke();
            ChangeWorldsCatalog();
        }

        protected override void OnClosed()
        {
            this.savedWorldFilesLoaded.Clear();
        }

        private void LoadAllLocalSavedWorlds()
        {
            this.savedWorldFilesLoaded = [.. WorldSavingHandler.LoadAllSavedWorldData(this.graphicsDevice)];
        }

        #endregion
    }
}
