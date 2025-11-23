using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Saving;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Utilities;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UISystem.UIs.Menus
{
    internal sealed class WorldDetailsMenuUI : UI
    {
        private SaveFile worldSaveFile;

        private ImageUIElement headerBackgroundElement;

        private LabelUIElement worldTitleElement;
        private ImageUIElement worldThumbnailElement;
        private TextUIElement worldDescriptionElement;
        private LabelUIElement worldVersionElement;
        private LabelUIElement worldCreationTimestampElement;

        private readonly LabelUIElement[] worldButtonElements;

        private readonly UIButton[] worldButtons;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        private readonly World world;

        internal WorldDetailsMenuUI(
            GameManager gameManager,
            UIIndex index,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.uiManager = uiManager;
            this.world = world;

            this.worldButtons = [
                new(null, null, "Return", string.Empty, ReturnButtonAction),
                new(null, null, "Delete", string.Empty, DeleteButtonAction),
                new(null, null, "Play", string.Empty, PlayButtonAction),
            ];

            this.worldButtonElements = new LabelUIElement[this.worldButtons.Length];
        }

        #region ACTIONS

        private void ReturnButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        private void DeleteButtonAction()
        {
            WorldSavingHandler.DeleteSavedFile(this.worldSaveFile.Header.Metadata.Name);
            this.uiManager.CloseGUI();
        }

        private void PlayButtonAction()
        {
            this.uiManager.Reset();
            this.uiManager.OpenGUI(UIIndex.MainMenu);
            this.uiManager.OpenGUI(UIIndex.Hud);

            this.gameManager.StartGame();
            this.world.LoadFromWorldSaveFile(this.worldSaveFile);
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildHeader(layout);
            BuildThumbnail(layout);
            BuildDescription(layout);
            BuildCreationTimestamp(layout);
            BuildVersion(layout);
            BuildWorldButtons(layout);
        }

        private static void BuildBackground(Layout layout)
        {
            ImageUIElement guiBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2(),
                Color = new Color(AAP64ColorPalette.DarkGray, 160)
            };

            layout.AddElement(guiBackground);
        }

        private void BuildHeader(Layout layout)
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
            this.worldTitleElement = new()
            {
                Scale = new(0.15f),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                PositionAnchor = CardinalDirection.West,
                OriginPivot = CardinalDirection.East,
                Margin = new(32.0f, 0.0f),
            };

            this.worldTitleElement.SetTextualContent("Title");
            this.worldTitleElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2.0f));
            this.worldTitleElement.PositionRelativeToElement(this.headerBackgroundElement);

            layout.AddElement(this.headerBackgroundElement);
            layout.AddElement(this.worldTitleElement);
        }

        private void BuildThumbnail(Layout layout)
        {
            this.worldThumbnailElement = new()
            {
                Scale = new(12.0f),
                Size = WorldConstants.WORLD_THUMBNAIL_SIZE.ToVector2(),
                Margin = new(32f, 128f),
            };

            this.worldThumbnailElement.PositionRelativeToScreen();

            layout.AddElement(this.worldThumbnailElement);
        }

        private void BuildDescription(Layout layout)
        {
            this.worldDescriptionElement = new()
            {
                Scale = new(0.078f),
                Margin = new(32.0f, 0.0f),
                LineHeight = 1.25f,
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
                TextAreaSize = new(930.0f, 600.0f),
                PositionAnchor = CardinalDirection.Northeast,
            };

            this.worldDescriptionElement.SetTextualContent("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            this.worldDescriptionElement.PositionRelativeToElement(this.worldThumbnailElement);

            layout.AddElement(this.worldDescriptionElement);
        }

        private void BuildCreationTimestamp(Layout layout)
        {
            this.worldCreationTimestampElement = new()
            {
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Scale = new(0.075f),
                Margin = new(-8),
                PositionAnchor = CardinalDirection.Southeast,
                OriginPivot = CardinalDirection.Northwest,
            };

            this.worldCreationTimestampElement.SetTextualContent(DateTime.Now.ToString());
            this.worldCreationTimestampElement.PositionRelativeToScreen();

            layout.AddElement(this.worldCreationTimestampElement);
        }

        private void BuildVersion(Layout layout)
        {
            this.worldVersionElement = new()
            {
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Scale = new(0.075f),
                Margin = new(0.0f, this.worldCreationTimestampElement.GetStringSize().Y + (64.0f * -1.0f)),
                PositionAnchor = CardinalDirection.Northeast,
                OriginPivot = CardinalDirection.Northwest,
            };

            this.worldVersionElement.SetTextualContent("Version 1.0.0");
            this.worldVersionElement.PositionRelativeToElement(this.worldCreationTimestampElement);

            layout.AddElement(this.worldVersionElement);
        }

        private void BuildWorldButtons(Layout layout)
        {
            Vector2 margin = new(32.0f, -32.0f);

            for (int i = 0; i < this.worldButtons.Length; i++)
            {
                UIButton button = this.worldButtons[i];

                LabelUIElement buttonLabel = new()
                {
                    Scale = new(0.12f),
                    Margin = margin,
                    SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                    PositionAnchor = CardinalDirection.Southwest,
                    OriginPivot = CardinalDirection.East,
                };

                buttonLabel.SetTextualContent(button.Name);
                buttonLabel.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2.0f));
                buttonLabel.PositionRelativeToScreen();

                layout.AddElement(buttonLabel);
                margin.Y -= buttonLabel.GetStringSize().Y + 8.0f;

                this.worldButtonElements[i] = buttonLabel;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            // Buttons
            for (int i = 0; i < this.worldButtonElements.Length; i++)
            {
                LabelUIElement slotInfoElement = this.worldButtonElements[i];

                Vector2 buttonSize = slotInfoElement.GetStringSize() / 2.0f;
                Vector2 buttonPosition = new(slotInfoElement.Position.X + buttonSize.X, slotInfoElement.Position.Y - (buttonSize.Y / 4.0f));

                if (UIInteraction.OnMouseClick(buttonPosition, buttonSize))
                {
                    this.worldButtons[i].ClickAction?.Invoke();
                }

                slotInfoElement.Color = UIInteraction.OnMouseOver(buttonPosition, buttonSize) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        #endregion

        #region UTILITIES

        internal void SetWorldSaveFile(SaveFile worldSaveFile)
        {
            this.worldSaveFile = worldSaveFile;
            UpdateDisplay(worldSaveFile);
        }

        private void UpdateDisplay(SaveFile worldSaveFile)
        {
            this.worldThumbnailElement.Texture = worldSaveFile.Header.ThumbnailTexture;
            this.worldTitleElement.SetTextualContent(worldSaveFile.Header.Metadata.Name);
            this.worldDescriptionElement.SetTextualContent(worldSaveFile.Header.Metadata.Description);
            this.worldVersionElement.SetTextualContent(string.Concat('v', worldSaveFile.Header.Information.SaveVersion));
            this.worldCreationTimestampElement.SetTextualContent(worldSaveFile.Header.Information.CreationTimestamp.ToString());
        }

        #endregion
    }
}
