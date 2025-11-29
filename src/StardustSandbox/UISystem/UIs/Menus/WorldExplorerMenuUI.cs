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
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Utilities;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UISystem.UIs.Menus
{
    internal sealed class WorldExplorerMenuUI : UI
    {
        private sealed class SSlotInfoElement
        {
            internal bool IsVisible { get; private set; }

            internal ImageUIElement BackgroundElement { get; set; }
            internal ImageUIElement ThumbnailElement { get; set; }
            internal LabelUIElement TitleElement { get; set; }

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
        private ImageUIElement headerBackgroundElement;
        private LabelUIElement pageIndexLabelElement;

        private readonly UIButton[] headerButtons;
        private readonly UIButton[] footerButtons;

        private readonly ImageUIElement[] headerButtonElements;
        private readonly LabelUIElement[] footerButtonElements;
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

            this.headerButtonElements = new ImageUIElement[this.headerButtons.Length];
            this.footerButtonElements = new LabelUIElement[this.footerButtons.Length];

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

        protected override void OnBuild(Layout layout)
        {
            BuildHeader(layout);
            BuildFooter(layout);

            BuildingWorldDisplaySlots(layout);
        }

        private void BuildHeader(Layout layout)
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
            LabelUIElement titleLabelElement = new()
            {
                Scale = new(0.15f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Alignment = CardinalDirection.West,
                Margin = new(32f, 0f),
            };

            titleLabelElement.SetTextualContent("Worlds Explorer");
            titleLabelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2f));
            titleLabelElement.RepositionRelativeToElement(this.headerBackgroundElement);

            layout.AddElement(this.headerBackgroundElement);
            layout.AddElement(titleLabelElement);

            // Buttons
            Vector2 margin = new(-64f, 0);

            for (int i = 0; i < this.headerButtons.Length; i++)
            {
                UIButton button = this.headerButtons[i];

                ImageUIElement buttonBackgroundElement = new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                    TextureRectangle = new(320, 140, 32, 32),
                    Alignment = CardinalDirection.East,
                    Margin = margin,
                    Scale = new(2f),
                    Size = new(32f),
                };

                ImageUIElement buttonIconElement = new()
                {
                    Texture = button.IconTexture,
                    Scale = new(1.5f),
                    Size = new(32f),
                };

                buttonBackgroundElement.RepositionRelativeToElement(this.headerBackgroundElement);
                buttonIconElement.RepositionRelativeToElement(buttonBackgroundElement);

                layout.AddElement(buttonBackgroundElement);
                layout.AddElement(buttonIconElement);

                this.headerButtonElements[i] = buttonBackgroundElement;

                margin.X -= buttonBackgroundElement.Size.X + 16.0f;
            }
        }

        private void BuildFooter(Layout layout)
        {
            ImageUIElement backgroundImage = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Size = Vector2.One,
                Scale = new(ScreenConstants.SCREEN_WIDTH, 96.0f),
                Alignment = CardinalDirection.Southwest,
                Margin = new(0.0f, -96.0f),
            };

            LabelUIElement pageIndexTitleLabel = new()
            {
                Scale = new(0.1f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Alignment = CardinalDirection.Center,
            };

            this.pageIndexLabelElement = new()
            {
                Scale = new(0.1f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Alignment = CardinalDirection.Center,
            };

            LabelUIElement previousButtonLabel = new()
            {
                Scale = new(0.15f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Alignment = CardinalDirection.West,
            };

            LabelUIElement nextButtonLabel = new()
            {
                Scale = new(0.15f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Alignment = CardinalDirection.East,
            };

            this.footerButtonElements[0] = previousButtonLabel;
            this.footerButtonElements[1] = nextButtonLabel;

            pageIndexTitleLabel.SetTextualContent("Current Page");
            this.pageIndexLabelElement.SetTextualContent("1 / 1");
            previousButtonLabel.SetTextualContent("Previous");
            nextButtonLabel.SetTextualContent("Next");

            pageIndexTitleLabel.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2f));
            this.pageIndexLabelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2f));
            previousButtonLabel.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2f));
            nextButtonLabel.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2f));

            pageIndexTitleLabel.Margin = new(0f, -16f);
            this.pageIndexLabelElement.Margin = new(0f, pageIndexTitleLabel.GetStringSize().Y);
            previousButtonLabel.Margin = new(previousButtonLabel.GetStringSize().X + 32f, 0f);
            nextButtonLabel.Margin = new((nextButtonLabel.GetStringSize().X + 32f) * -1, 0f);

            backgroundImage.RepositionRelativeToScreen();
            pageIndexTitleLabel.RepositionRelativeToElement(backgroundImage);
            this.pageIndexLabelElement.RepositionRelativeToElement(pageIndexTitleLabel);
            previousButtonLabel.RepositionRelativeToElement(backgroundImage);
            nextButtonLabel.RepositionRelativeToElement(backgroundImage);

            layout.AddElement(backgroundImage);
            layout.AddElement(pageIndexTitleLabel);
            layout.AddElement(this.pageIndexLabelElement);
            layout.AddElement(previousButtonLabel);
            layout.AddElement(nextButtonLabel);
        }

        // ========================================================================== //

        private void BuildingWorldDisplaySlots(Layout layout)
        {
            Vector2 slotMargin = new(32, (UIConstants.HUD_WORLD_EXPLORER_SLOT_HEIGHT_SPACING / 2) + 32);

            int rows = UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_ROW;
            int columns = UIConstants.HUD_WORLD_EXPLORER_ITEMS_PER_COLUMN;

            int index = 0;
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    ImageUIElement backgroundImageElement = new()
                    {
                        Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                        TextureRectangle = new(0, 0, 386, 140),
                        Size = new(UIConstants.HUD_WORLD_EXPLORER_SLOT_WIDTH, UIConstants.HUD_WORLD_EXPLORER_SLOT_HEIGHT),
                        Margin = slotMargin
                    };

                    ImageUIElement thumbnailImageElement = new()
                    {
                        Scale = new(5.1f),
                        Size = WorldConstants.WORLD_THUMBNAIL_SIZE.ToVector2(),
                        Alignment = CardinalDirection.West,
                        Margin = new(11.5f, 0f),
                    };

                    LabelUIElement titleLabelElement = new()
                    {
                        Color = AAP64ColorPalette.White,
                        SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                        Alignment = CardinalDirection.North,
                        Scale = new(0.1f),
                        Margin = new(-52.5f, 23f),
                    };

                    // Setting
                    titleLabelElement.SetTextualContent("Title");

                    // Position
                    backgroundImageElement.RepositionRelativeToElement(this.headerBackgroundElement);
                    thumbnailImageElement.RepositionRelativeToElement(backgroundImageElement);
                    titleLabelElement.RepositionRelativeToElement(backgroundImageElement);

                    // Spacing
                    slotMargin.X += UIConstants.HUD_WORLD_EXPLORER_SLOT_WIDTH_SPACING;

                    this.slotInfoElements[index] = new()
                    {
                        BackgroundElement = backgroundImageElement,
                        ThumbnailElement = thumbnailImageElement,
                        TitleElement = titleLabelElement
                    };

                    index++;

                    // Adding
                    layout.AddElement(backgroundImageElement);
                    layout.AddElement(thumbnailImageElement);
                    layout.AddElement(titleLabelElement);
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
                ImageUIElement buttonBackgroundElement = this.headerButtonElements[i];

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
                LabelUIElement labelElement = this.footerButtonElements[i];
                Vector2 labelElementSize = labelElement.GetStringSize() / 2f;

                if (Interaction.OnMouseClick(labelElement.Position, labelElementSize))
                {
                    this.footerButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = Interaction.OnMouseOver(labelElement.Position, labelElementSize) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
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
            this.pageIndexLabelElement?.SetTextualContent(string.Concat(this.currentPage + 1, " / ", Math.Max(this.totalPages, 1)));
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
                    slotInfoElement.TitleElement.SetTextualContent(worldSaveFile.Header.Metadata.Name.Truncate(10));
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

        protected override void OnBuild(ContainerUIElement root)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
