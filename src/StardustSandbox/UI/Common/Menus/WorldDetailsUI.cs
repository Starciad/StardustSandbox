using Microsoft.Xna.Framework;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Saving;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class WorldDetailsUI : UIBase
    {
        private SaveFile worldSaveFile;

        private Image headerBackground;

        private Image worldThumbnail;
        private Label worldTitle, worldVersion, worldCreationTimestamp;
        private Text worldDescription;

        private readonly Label[] worldButtonLabels;
        private readonly ButtonInfo[] worldButtonInfos;

        private readonly GameManager gameManager;
        private readonly UIManager uiManager;

        private readonly World world;

        internal WorldDetailsUI(
            GameManager gameManager,
            UIIndex index,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.uiManager = uiManager;
            this.world = world;

            this.worldButtonInfos = [
                new(TextureIndex.None, null, "Return", string.Empty, this.uiManager.CloseGUI),
                new(TextureIndex.None, null, "Delete", string.Empty, () =>
                {
                    WorldSavingHandler.DeleteSavedFile(this.worldSaveFile.Header.Metadata.Name);
                    this.uiManager.CloseGUI();
                }),
                new(TextureIndex.None, null, "Play", string.Empty, () =>
                {
                    this.uiManager.Reset();
                    this.uiManager.OpenGUI(UIIndex.MainMenu);
                    this.uiManager.OpenGUI(UIIndex.Hud);

                    this.gameManager.StartGame();
                    this.world.LoadFromWorldSaveFile(this.worldSaveFile);
                }),
            ];

            this.worldButtonLabels = new Label[this.worldButtonInfos.Length];
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildHeader(root);
            BuildThumbnail(root);
            BuildDescription();
            BuildCreationTimestamp(root);
            BuildVersion();
            BuildWorldButtons(root);
        }

        private static void BuildBackground(Container root)
        {
            Image guiBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2(),
                Color = new Color(AAP64ColorPalette.DarkGray, 160)
            };

            root.AddChild(guiBackground);
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

            // Title
            this.worldTitle = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = CardinalDirection.West,
                Margin = new(32.0f, 0.0f),
                TextContent = "Title",

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
            this.worldDescription = new()
            {
                Scale = new(0.078f),
                Margin = new(32.0f, 0.0f),
                LineHeight = 1.25f,
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                TextAreaSize = new(930.0f, 600.0f),
                Alignment = CardinalDirection.Northeast,
                TextContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            };

            this.worldThumbnail.AddChild(this.worldDescription);
        }

        private void BuildCreationTimestamp(Container root)
        {
            this.worldCreationTimestamp = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.075f),
                Margin = new(-8.0f),
                Alignment = CardinalDirection.Southeast,
                TextContent = DateTime.Now.ToString(),
            };

            root.AddChild(this.worldCreationTimestamp);
        }

        private void BuildVersion()
        {
            this.worldVersion = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.075f),
                Margin = new(0.0f, this.worldCreationTimestamp.Size.Y + (64.0f * -1.0f)),
                Alignment = CardinalDirection.Northeast,
                TextContent = "Version 1.0.0",
            };

            this.worldCreationTimestamp.AddChild(this.worldVersion);
        }

        private void BuildWorldButtons(Container root)
        {
            float marginY = -32.0f;

            for (byte i = 0; i < this.worldButtonInfos.Length; i++)
            {
                ButtonInfo button = this.worldButtonInfos[i];

                Label buttonLabel = new()
                {
                    Scale = new(0.12f),
                    Margin = new(32.0f, -32.0f),
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = CardinalDirection.Southwest,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                root.AddChild(buttonLabel);
                marginY -= buttonLabel.Size.Y + 8.0f;

                this.worldButtonLabels[i] = buttonLabel;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            for (byte i = 0; i < this.worldButtonLabels.Length; i++)
            {
                Label slotInfoElement = this.worldButtonLabels[i];

                if (Interaction.OnMouseLeftClick(slotInfoElement))
                {
                    this.worldButtonInfos[i].ClickAction?.Invoke();
                }

                slotInfoElement.Color = Interaction.OnMouseOver(slotInfoElement) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }

            base.Update(gameTime);
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
            this.worldThumbnail.Texture = worldSaveFile.Header.ThumbnailTexture;
            this.worldTitle.TextContent = worldSaveFile.Header.Metadata.Name;
            this.worldDescription.TextContent = worldSaveFile.Header.Metadata.Description;
            this.worldVersion.TextContent = string.Concat('v', worldSaveFile.Header.Information.SaveVersion);
            this.worldCreationTimestamp.TextContent = worldSaveFile.Header.Information.CreationTimestamp.ToString();
        }

        #endregion
    }
}
