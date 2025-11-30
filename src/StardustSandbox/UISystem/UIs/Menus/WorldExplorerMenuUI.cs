using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Extensions;
using StardustSandbox.IO;
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Saving;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Information;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UISystem.UIs.Menus
{
    internal sealed class WorldExplorerMenuUI : UI
    {
        private sealed class SSlotInfoElement
        {
            internal bool IsVisible { get; private set; }

            internal Image BackgroundElement { get; set; }
            internal Image ThumbnailElement { get; set; }
            internal Label TitleElement { get; set; }

            internal void EnableVisibility()
            {
                this.IsVisible = true;
                this.BackgroundElement.CanDraw = true;
                this.ThumbnailElement.CanDraw = true;
                this.TitleElement.CanDraw = true;
            }

            internal void DisableVisibility()
            {
                this.IsVisible = false;
                this.BackgroundElement.CanDraw = false;
                this.ThumbnailElement.CanDraw = false;
                this.TitleElement.CanDraw = false;
            }
        }

        private int currentPage = 0;
        private int totalPages = 1;

        private List<SaveFile> savedWorldFilesLoaded;
        private Image headerBackgroundElement;
        private Label pageIndexLabelElement;

        private readonly ButtonInfo[] headerButtons;
        private readonly ButtonInfo[] footerButtons;

        private readonly Image[] headerButtonElements;
        private readonly Label[] footerButtonElements;
        private readonly SSlotInfoElement[] slotInfoElements;

        private readonly WorldDetailsMenuUI worldDetailsMenuUI;

        private readonly GraphicsDevice graphicsDevice;
        private readonly UIManager uiManager;

        internal WorldExplorerMenuUI(
            GraphicsDevice graphicsDevice,
            UIIndex index,
            UIManager uiManager,
            WorldDetailsMenuUI worldDetailsMenuUI
        ) : base(index)
        {
            this.graphicsDevice = graphicsDevice;
            this.uiManager = uiManager;
            this.worldDetailsMenuUI = worldDetailsMenuUI;

            this.slotInfoElements = new SSlotInfoElement[UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE];

            this.headerButtons = [
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(192, 0, 32, 32), "Exit", string.Empty, ExitButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(160, 192, 32, 32), "Reload", string.Empty, ReloadButtonAction),
                new(AssetDatabase.GetTexture(TextureIndex.IconUi), new(32, 32, 32, 32), "Open Directory in Explorer", string.Empty, OpenDirectoryInExplorerAction),
            ];

            this.footerButtons = [
                new(null, null, "Previous", string.Empty, PreviousButtonAction),
                new(null, null, "Next", string.Empty, NextButtonAction),
            ];

            this.headerButtonElements = new Image[this.headerButtons.Length];
            this.footerButtonElements = new Label[this.footerButtons.Length];

            UpdatePagination();
        }

        #region ACTIONS

        private void ReloadButtonAction()
        {
            LoadAllLocalSavedWorlds();
            this.currentPage = 0;
            UpdatePagination();
            ChangeWorldsCatalog();
        }

        private void ExitButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        private void PreviousButtonAction()
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
        }

        private void NextButtonAction()
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
        }

        private void OpenDirectoryInExplorerAction()
        {
            SSDirectory.OpenDirectoryInFileExplorer(SSDirectory.Worlds);
        }

        #endregion

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
                Scale = new(ScreenConstants.SCREEN_WIDTH, 96f),
            };

            // Title
            Label titleLabelElement = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.West,
                Margin = new(32f, 0f),
                TextContent = "World Explorer",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2f,
                BorderThickness = 2f,
            };

            this.headerBackgroundElement.AddChild(titleLabelElement);

            // Buttons
            Vector2 margin = new(-64f, 0);

            for (int i = 0; i < this.headerButtons.Length; i++)
            {
                ButtonInfo button = this.headerButtons[i];

                Image buttonBackgroundElement = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Alignment = CardinalDirection.East,
                    Margin = margin,
                    Scale = new(2f),
                    Size = new(32f),
                };

                Image buttonIconElement = new()
                {
                    Texture = button.IconTexture,
                    Scale = new(1.5f),
                    Size = new(32f),
                };

                this.headerBackgroundElement.AddChild(buttonBackgroundElement);
                buttonBackgroundElement.AddChild(buttonIconElement);

                this.headerButtonElements[i] = buttonBackgroundElement;

                margin.X -= buttonBackgroundElement.Size.X + 16.0f;
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
                BorderOffset = 2f,
                BorderThickness = 2f,
            };

            this.pageIndexLabelElement = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.Center,
                TextContent = "1 / 1",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2f,
                BorderThickness = 2f,
            };

            Label previousButtonLabel = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.West,
                TextContent = "Previous",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2f,
                BorderThickness = 2f,
            };

            Label nextButtonLabel = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.East,
                TextContent = "Next",

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2f,
                BorderThickness = 2f,
            };

            this.footerButtonElements[0] = previousButtonLabel;
            this.footerButtonElements[1] = nextButtonLabel;

            pageIndexTitleLabel.Margin = new(0f, -16f);
            this.pageIndexLabelElement.Margin = new(0f, pageIndexTitleLabel.MeasuredText.Y);
            previousButtonLabel.Margin = new(previousButtonLabel.MeasuredText.X + 32f, 0f);
            nextButtonLabel.Margin = new((nextButtonLabel.MeasuredText.X + 32f) * -1, 0f);

            background.AddChild(pageIndexTitleLabel);
            background.AddChild(previousButtonLabel);
            background.AddChild(nextButtonLabel);

            pageIndexTitleLabel.AddChild(this.pageIndexLabelElement);

            root.AddChild(background);
        }

        // ========================================================================== //

        private void BuildingWorldDisplaySlots()
        {
            Vector2 slotMargin = new(32, (UIConstants.HUD_WORLD_EXPLORER_SLOT_HEIGHT_SPACING / 2) + 32);

            int rows = UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_ROW;
            int columns = UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_COLUMN;

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    Image backgroundImageElement = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                        SourceRectangle = new(0, 0, 386, 140),
                        Size = new(UIConstants.HUD_WORLD_EXPLORER_SLOT_WIDTH, UIConstants.HUD_WORLD_EXPLORER_SLOT_HEIGHT),
                        Margin = slotMargin
                    };

                    Image thumbnailImageElement = new()
                    {
                        Scale = new(5.1f),
                        Size = WorldConstants.WORLD_THUMBNAIL_SIZE.ToVector2(),
                        Alignment = CardinalDirection.West,
                        Margin = new(11.5f, 0f),
                    };

                    Label titleLabelElement = new()
                    {
                        Color = AAP64ColorPalette.White,
                        SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                        Alignment = CardinalDirection.North,
                        Scale = new(0.1f),
                        Margin = new(-52.5f, 23f),
                        TextContent = "Title"
                    };

                    // Position
                    this.headerBackgroundElement.AddChild(backgroundImageElement);
                    backgroundImageElement.AddChild(thumbnailImageElement);
                    thumbnailImageElement.AddChild(titleLabelElement);

                    // Spacing
                    slotMargin.X += UIConstants.HUD_WORLD_EXPLORER_SLOT_WIDTH_SPACING;

                    this.slotInfoElements[index] = new()
                    {
                        BackgroundElement = backgroundImageElement,
                        ThumbnailElement = thumbnailImageElement,
                        TitleElement = titleLabelElement
                    };

                    index++;
                }

                slotMargin.X = 32;
                slotMargin.Y += UIConstants.HUD_WORLD_EXPLORER_SLOT_HEIGHT_SPACING;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            #region BUTTONS
            // HEADER
            for (int i = 0; i < this.headerButtonElements.Length; i++)
            {
                Image buttonBackgroundElement = this.headerButtonElements[i];

                Vector2 buttonSize = buttonBackgroundElement.Size / 2.0f;

                if (Interaction.OnMouseClick(buttonBackgroundElement.Position, buttonSize))
                {
                    this.headerButtons[i].ClickAction?.Invoke();
                }

                buttonBackgroundElement.Color = Interaction.OnMouseOver(buttonBackgroundElement.Position, buttonSize) ? AAP64ColorPalette.LightGrayBlue : AAP64ColorPalette.White;
            }

            // FOOTER
            for (int i = 0; i < this.footerButtonElements.Length; i++)
            {
                Label label = this.footerButtonElements[i];
                Vector2 labelElementSize = label.MeasuredText / 2f;

                if (Interaction.OnMouseClick(label.Position, labelElementSize))
                {
                    this.footerButtons[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label.Position, labelElementSize) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
            #endregion

            // SLOTS
            for (int i = 0; i < this.slotInfoElements.Length; i++)
            {
                SSlotInfoElement slotInfoElement = this.slotInfoElements[i];

                if (!this.slotInfoElements[i].IsVisible)
                {
                    break;
                }

                Vector2 backgroundSize = slotInfoElement.BackgroundElement.Size / 2.0f;
                Vector2 backgroundPosition = slotInfoElement.BackgroundElement.Position + backgroundSize;

                if (Interaction.OnMouseClick(backgroundPosition, backgroundSize))
                {
                    this.worldDetailsMenuUI.SetWorldSaveFile(this.savedWorldFilesLoaded[(this.currentPage * UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_PAGE) + i]);
                    this.uiManager.OpenGUI(UIIndex.WorldDetailsMenu);
                }

                slotInfoElement.BackgroundElement.Color = Interaction.OnMouseOver(backgroundPosition, backgroundSize) ? AAP64ColorPalette.LightGrayBlue : AAP64ColorPalette.White;
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

            for (int i = 0; i < this.slotInfoElements.Length; i++)
            {
                SSlotInfoElement slotInfoElement = this.slotInfoElements[i];
                int worldIndex = startIndex + i;

                if (worldIndex < this.savedWorldFilesLoaded?.Count)
                {
                    SaveFile worldSaveFile = this.savedWorldFilesLoaded[worldIndex];

                    slotInfoElement.EnableVisibility();
                    slotInfoElement.ThumbnailElement.Texture = worldSaveFile.Header.ThumbnailTexture;
                    slotInfoElement.TitleElement.TextContent = worldSaveFile.Header.Metadata.Name.Truncate(10);
                }
                else
                {
                    slotInfoElement.DisableVisibility();
                }
            }

            UpdatePagination();
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            ReloadButtonAction();
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
