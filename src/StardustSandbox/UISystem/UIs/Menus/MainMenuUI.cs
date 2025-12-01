using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.AudioSystem;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.BackgroundSystem;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.Simulation;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.Randomness;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Information;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UISystem.UIs.Menus
{
    internal sealed class MainMenuUI : UI
    {
        private Image panelBackground;
        private Image gameTitle;

        private Vector2 originalGameTitleElementPosition;

        private float animationTime = 0f;
        private float[] buttonAnimationOffsets;

        private Dictionary<Label, Vector2> buttonOriginalPositions;

        private readonly Label[] menuButtons;
        private readonly ButtonInfo[] menuButtonInfos;

        private readonly InputController inputController;
        private readonly AmbientManager ambientManager;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;
        private readonly World world;

        private const float animationSpeed = 2f;
        private const float animationAmplitude = 10f;
        private const float buttonAnimationSpeed = 1.5f;
        private const float buttonAnimationAmplitude = 5f;

        internal MainMenuUI(
            AmbientManager ambientManager,
            InputController inputController,
            GameManager gameManager,
            UIIndex index,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.ambientManager = ambientManager;
            this.inputController = inputController;
            this.gameManager = gameManager;
            this.uiManager = uiManager;
            this.world = world;

            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_GUIs.Menu_Main_Button_Create, string.Empty, CreateMenuButtonAction),
                new(TextureIndex.None, null, Localization_GUIs.Menu_Main_Button_Play, string.Empty, PlayMenuButtonAction),
                new(TextureIndex.None, null, Localization_GUIs.Menu_Main_Button_Options, string.Empty, OptionsMenuButtonAction),
                new(TextureIndex.None, null, Localization_GUIs.Menu_Main_Button_Credits, string.Empty, CreditsMenuButtonAction),
                new(TextureIndex.None, null, Localization_GUIs.Menu_Main_Button_Quit, string.Empty, QuitMenuButtonAction),
            ];

            this.menuButtons = new Label[this.menuButtonInfos.Length];
        }

        #region INITIALIZATION

        private void ResetElementPositions()
        {
            this.panelBackground.AddChild(this.gameTitle);

            foreach (Label buttonLabelElement in this.menuButtons)
            {
                this.panelBackground.AddChild(buttonLabelElement);
            }
        }

        private void LoadAnimationValues()
        {
            // GameTitle
            this.originalGameTitleElementPosition = this.gameTitle.Position;

            // Buttons
            this.buttonOriginalPositions = [];
            this.buttonAnimationOffsets = new float[this.menuButtons.Length];

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                this.buttonOriginalPositions[this.menuButtons[i]] = this.menuButtons[i].Position;
                this.buttonAnimationOffsets[i] = (float)(SSRandom.GetDouble() * Math.PI * 2);
            }
        }

        private void LoadMainMenuWorld()
        {
            this.world.StartNew(WorldConstants.WORLD_SIZES_TEMPLATE[0]);
        }

        #endregion

        #region ACTIONS

        private void CreateMenuButtonAction()
        {
            this.gameManager.StartGame();
        }

        private void PlayMenuButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.PlayMenu);
        }

        private void OptionsMenuButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.OptionsMenu);
        }

        private void CreditsMenuButtonAction()
        {
            this.uiManager.OpenGUI(UIIndex.CreditsMenu);
        }

        private void QuitMenuButtonAction()
        {
            Program.Quit();
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildMainPanel(root);
            BuildDecorations();
            BuildGameTitle();
            BuildButtons();
            BuildInfos(root);
        }

        private void BuildMainPanel(Container root)
        {
            this.panelBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(487f, ScreenConstants.SCREEN_HEIGHT),
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 180),
            };

            root.AddChild(this.panelBackground);
        }

        private void BuildDecorations()
        {
            Image prosceniumCurtainElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.MiscellaneousTheatricalCurtains),
                Scale = new(2)
            };

            this.panelBackground.AddChild(prosceniumCurtainElement);
        }

        private static void BuildInfos(Container root)
        {
            Label gameVersionLabel = new()
            {
                Margin = new(-32f, -32f),
                Scale = new(0.08f),
                Color = AAP64ColorPalette.White,
                Alignment = CardinalDirection.Southeast,
                TextAlignment = CardinalDirection.West,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = $"Ver. {GameConstants.VERSION}",
            };

            Label copyrightLabel = new()
            {
                Margin = new(0f, -32),
                Scale = new(0.08f),
                Color = AAP64ColorPalette.White,
                Alignment = CardinalDirection.South,
                TextAlignment = CardinalDirection.Center,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = $"(c) {GameConstants.YEAR} {GameConstants.AUTHOR}",
            };

            root.AddChild(gameVersionLabel);
            root.AddChild(copyrightLabel);
        }

        private void BuildGameTitle()
        {
            this.gameTitle = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GameTitle),
                Scale = new(1.5f),
                Size = new(292, 112),
                Margin = new(0, 32),
                Alignment = CardinalDirection.North,
            };

            this.panelBackground.AddChild(this.gameTitle);
        }

        private void BuildButtons()
        {
            // BUTTONS
            Vector2 margin = new(0, 0);

            // Labels
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                Label label = new()
                {
                    Scale = new(0.15f),
                    Margin = margin,
                    Color = AAP64ColorPalette.White,
                    Alignment = CardinalDirection.Center,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    TextContent = this.menuButtonInfos[i].Name,
                    TextAlignment = CardinalDirection.Center,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 4f,
                    BorderThickness = 4f,
                };

                this.panelBackground.AddChild(label);
                this.menuButtons[i] = label;

                margin.Y += 75;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateAnimations(gameTime);
            UpdateButtons();
        }

        private void UpdateAnimations(GameTime gameTime)
        {
            UpdateGameTitleElementAnimation(gameTime);
            UpdateButtonElementsAnimation(gameTime);
        }

        private void UpdateGameTitleElementAnimation(GameTime gameTime)
        {
            this.animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            float offsetY = (float)Math.Sin(this.animationTime) * animationAmplitude;
            Vector2 newPosition = new(this.originalGameTitleElementPosition.X, this.originalGameTitleElementPosition.Y + offsetY);

            this.gameTitle.Position = newPosition;
        }

        private void UpdateButtonElementsAnimation(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                Label button = this.menuButtons[i];
                Vector2 originalPosition = this.buttonOriginalPositions[button];

                this.buttonAnimationOffsets[i] += elapsedTime * buttonAnimationSpeed;
                float offsetY = (float)Math.Sin(this.buttonAnimationOffsets[i]) * buttonAnimationAmplitude;

                button.Position = new(originalPosition.X, originalPosition.Y + offsetY);
            }
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                Label label = this.menuButtons[i];
                Vector2 labelElementSize = label.MeasuredText / 2f;

                if (Interaction.OnMouseClick(label.Position, labelElementSize))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                label.Color = Interaction.OnMouseOver(label.Position, labelElementSize) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.MainMenu);
            this.ambientManager.CloudHandler.IsActive = true;
            this.ambientManager.CelestialBodyHandler.IsActive = true;
            this.ambientManager.SkyHandler.IsActive = true;

            ResetElementPositions();

            this.inputController.Pen.Tool = PenTool.Visualization;
            this.inputController.Disable();

            LoadAnimationValues();
            LoadMainMenuWorld();

            this.gameManager.SetSimulationSpeed(SimulationSpeed.Normal);
            this.gameManager.RemoveState(GameStates.IsPaused);
            this.gameManager.RemoveState(GameStates.IsSimulationPaused);
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);

            this.world.Time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES;
            this.world.Time.IsFrozen = false;

            if (SongEngine.State != MediaState.Playing || SongEngine.CurrentSongIndex != SongIndex.V01_CanvasOfSilence)
            {
                SongEngine.Play(SongIndex.V01_CanvasOfSilence);
            }
        }

        protected override void OnClosed()
        {
            this.world.Clear();
            this.world.IsVisible = false;
        }

        #endregion
    }
}
