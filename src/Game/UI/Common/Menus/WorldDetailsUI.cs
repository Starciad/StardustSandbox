using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Serialization;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem.Game;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Saving;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class WorldDetailsUI : UIBase
    {
        private SaveFile saveFile;

        private Image headerBackground;

        private Image worldThumbnail;
        private Label worldTitle, worldVersion, worldCreationTimestamp;
        private Text worldDescription;

        private readonly Label[] worldButtonLabels;
        private readonly ButtonInfo[] worldButtonInfos;

        private readonly UIManager uiManager;

        private readonly World world;

        internal WorldDetailsUI(
            AmbientManager ambientManager,
            UIIndex index,
            InputController inputController,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.uiManager = uiManager;
            this.world = world;

            this.worldButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Return, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.uiManager.CloseGUI();
                }),
                new(TextureIndex.None, null, Localization_Statements.Delete, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    SavingSerializer.Delete(this.saveFile.Metadata.Name);
                    this.uiManager.CloseGUI();
                }),
                new(TextureIndex.None, null, Localization_Statements.Play, string.Empty, () =>
                {
                    this.uiManager.Reset();
                    this.uiManager.OpenGUI(UIIndex.MainMenu);
                    this.uiManager.OpenGUI(UIIndex.Hud);

                    GameHandler.StartGame(ambientManager, inputController, uiManager, world);
                    this.world.LoadFromSaveFile(this.saveFile.Metadata.Name);

                    SoundEngine.Play(SoundEffectIndex.GUI_World_Loaded);
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
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new Color(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            root.AddChild(background);
        }

        private void BuildHeader(Container root)
        {
            // Background
            this.headerBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Scale = new(ScreenConstants.SCREEN_WIDTH, 96.0f),
                Size = Vector2.One,
            };

            // Title
            this.worldTitle = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
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
            this.worldDescription = new()
            {
                Scale = new(0.078f),
                Margin = new((WorldConstants.WORLD_THUMBNAIL_SIZE.X * this.worldThumbnail.Scale.X) + 16.0f, 0.0f),
                LineHeight = 1.25f,
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                TextAreaSize = new(930.0f, 600.0f),
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
                Alignment = UIDirection.Southeast,
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
                Alignment = UIDirection.Northeast,
            };

            this.worldCreationTimestamp.AddChild(this.worldVersion);
        }

        private void BuildWorldButtons(Container root)
        {
            float marginY = -32.0f;

            for (int i = 0; i < this.worldButtonInfos.Length; i++)
            {
                ButtonInfo button = this.worldButtonInfos[i];

                Label buttonLabel = new()
                {
                    Scale = new(0.12f),
                    Margin = new(32.0f, marginY),
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = UIDirection.Southwest,
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
            for (int i = 0; i < this.worldButtonLabels.Length; i++)
            {
                Label slotInfoElement = this.worldButtonLabels[i];

                if (Interaction.OnMouseEnter(slotInfoElement))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slotInfoElement))
                {
                    this.worldButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slotInfoElement.Color = Interaction.OnMouseOver(slotInfoElement) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }

            base.Update(gameTime);
        }

        #endregion

        #region UTILITIES

        internal void SetSaveFile(GraphicsDevice graphicsDevice, string saveFilename)
        {
            this.saveFile = SavingSerializer.Load(saveFilename, LoadFlags.Metadata | LoadFlags.Manifest | LoadFlags.Thumbnail);
            UpdateDisplay(graphicsDevice, this.saveFile);
        }

        private void UpdateDisplay(GraphicsDevice graphicsDevice, SaveFile saveFile)
        {
            this.worldThumbnail.Texture = saveFile.ThumbnailTextureData.ToTexture2D(graphicsDevice);

            this.worldTitle.TextContent = saveFile.Metadata.Name;
            this.worldDescription.TextContent = saveFile.Metadata.Description;
            this.worldVersion.TextContent = string.Concat('v', saveFile.Manifest.FormatVersion);
            this.worldCreationTimestamp.TextContent = saveFile.Manifest.CreationTimestamp.ToString();
        }

        #endregion

        protected override void OnClosed()
        {
            this.worldThumbnail.Texture?.Dispose();
        }
    }
}
