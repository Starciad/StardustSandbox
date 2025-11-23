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
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Utilities;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UISystem.UIs.Menus
{
    internal sealed class MainMenuUI : UI
    {
        private const float animationSpeed = 2f;
        private const float animationAmplitude = 10f;
        private const float ButtonAnimationSpeed = 1.5f;
        private const float ButtonAnimationAmplitude = 5f;

        private ImageUIElement panelBackgroundElement;
        private ImageUIElement gameTitleElement;

        private Vector2 originalGameTitleElementPosition;
        private float animationTime = 0f;

        private Dictionary<LabelUIElement, Vector2> buttonOriginalPositions;
        private float[] buttonAnimationOffsets;

        private readonly LabelUIElement[] menuButtonElements;
        private readonly UIButton[] menuButtons;

        private readonly InputController inputController;

        private readonly AmbientManager ambientManager;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;
        private readonly World world;

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

            this.menuButtons = [
                new(null, null, Localization_GUIs.Menu_Main_Button_Create, string.Empty, CreateMenuButtonAction),
                new(null, null, Localization_GUIs.Menu_Main_Button_Play, string.Empty, PlayMenuButtonAction),
                new(null, null, Localization_GUIs.Menu_Main_Button_Options, string.Empty, OptionsMenuButtonAction),
                new(null, null, Localization_GUIs.Menu_Main_Button_Credits, string.Empty, CreditsMenuButtonAction),
                new(null, null, Localization_GUIs.Menu_Main_Button_Quit, string.Empty, QuitMenuButtonAction),
            ];

            this.menuButtonElements = new LabelUIElement[this.menuButtons.Length];
        }

        #region INITIALIZATION

        private void ResetElementPositions()
        {
            this.gameTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            foreach (LabelUIElement buttonLabelElement in this.menuButtonElements)
            {
                buttonLabelElement.PositionRelativeToElement(this.panelBackgroundElement);
            }
        }

        private void LoadAnimationValues()
        {
            // GameTitle
            this.originalGameTitleElementPosition = this.gameTitleElement.Position;

            // Buttons
            this.buttonOriginalPositions = [];
            this.buttonAnimationOffsets = new float[this.menuButtonElements.Length];

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                this.buttonOriginalPositions[this.menuButtonElements[i]] = this.menuButtonElements[i].Position;
                this.buttonAnimationOffsets[i] = (float)(SSRandom.GetDouble() * Math.PI * 2);
            }
        }

        private void LoadMainMenuWorld()
        {
            this.world.StartNew(WorldConstants.WORLD_SIZES_TEMPLATE[0]);
        }

        private static void LoadMagicCursor()
        {
            // if (this.world.ActiveEntitiesCount > 0)
            // {
            //     return;
            // }
            // 
            // _ = this.world.InstantiateEntity(EntityConstants.MAGIC_CURSOR_IDENTIFIER, null);
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
        protected override void OnBuild(Layout layout)
        {
            BuildMainPanel(layout);
            BuildDecorations(layout);
            BuildGameTitle(layout);
            BuildButtons(layout);
            BuildInfos(layout);
        }

        private void BuildMainPanel(Layout layout)
        {
            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(487f, ScreenConstants.SCREEN_HEIGHT),
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 180),
            };

            layout.AddElement(this.panelBackgroundElement);
        }

        private static void BuildDecorations(Layout layout)
        {
            ImageUIElement prosceniumCurtainElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.MiscellaneousTheatricalCurtains),
                Scale = new(2)
            };

            layout.AddElement(prosceniumCurtainElement);
        }

        private static void BuildInfos(Layout layout)
        {
            LabelUIElement gameVersionLabel = new()
            {
                Margin = new(-32f, -32f),
                Scale = new(0.08f),
                Color = AAP64ColorPalette.White,
                PositionAnchor = CardinalDirection.Southeast,
                OriginPivot = CardinalDirection.West,
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
            };

            LabelUIElement copyrightLabel = new()
            {
                Margin = new(0f, -32),
                Scale = new(0.08f),
                Color = AAP64ColorPalette.White,
                PositionAnchor = CardinalDirection.South,
                OriginPivot = CardinalDirection.Center,
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
            };

            gameVersionLabel.SetTextualContent($"Ver. {GameConstants.VERSION}");
            gameVersionLabel.PositionRelativeToScreen();

            copyrightLabel.SetTextualContent($"(c) {GameConstants.YEAR} {GameConstants.AUTHOR}");
            copyrightLabel.PositionRelativeToScreen();

            layout.AddElement(gameVersionLabel);
            layout.AddElement(copyrightLabel);
        }

        private void BuildGameTitle(Layout layout)
        {
            this.gameTitleElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GameTitle),
                Scale = new(1.5f),
                Size = new(292, 112),
                Margin = new(0, 96),
                PositionAnchor = CardinalDirection.North,
                OriginPivot = CardinalDirection.Center
            };

            this.gameTitleElement.PositionRelativeToElement(this.panelBackgroundElement);

            layout.AddElement(this.gameTitleElement);
        }

        private void BuildButtons(Layout layout)
        {
            // BUTTONS
            Vector2 margin = new(0, 0);

            // Labels
            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                LabelUIElement labelElement = new()
                {
                    Scale = new(0.15f),
                    Margin = margin,
                    Color = AAP64ColorPalette.White,
                    PositionAnchor = CardinalDirection.Center,
                    OriginPivot = CardinalDirection.Center,
                    SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                };

                labelElement.SetTextualContent(this.menuButtons[i].Name);
                labelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(4f));
                labelElement.PositionRelativeToElement(this.panelBackgroundElement);

                this.menuButtonElements[i] = labelElement;
                margin.Y += 75;
            }

            layout.AddElement(this.gameTitleElement);

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                layout.AddElement(this.menuButtonElements[i]);
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

            this.gameTitleElement.Position = newPosition;
        }

        private void UpdateButtonElementsAnimation(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                LabelUIElement button = this.menuButtonElements[i];
                Vector2 originalPosition = this.buttonOriginalPositions[button];

                this.buttonAnimationOffsets[i] += elapsedTime * ButtonAnimationSpeed;
                float offsetY = (float)Math.Sin(this.buttonAnimationOffsets[i]) * ButtonAnimationAmplitude;

                button.Position = new(originalPosition.X, originalPosition.Y + offsetY);
            }
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                LabelUIElement labelElement = this.menuButtonElements[i];
                Vector2 labelElementSize = labelElement.GetStringSize() / 2f;

                if (UIInteraction.OnMouseClick(labelElement.Position, labelElementSize))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = UIInteraction.OnMouseOver(labelElement.Position, labelElementSize) ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
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
            LoadMagicCursor();

            this.gameManager.SetSimulationSpeed(SimulationSpeed.Normal);
            this.gameManager.RemoveState(GameStates.IsPaused);
            this.gameManager.RemoveState(GameStates.IsSimulationPaused);
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);

            this.world.Time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES;
            this.world.Time.IsFrozen = false;

            if (SongEngine.State != MediaState.Playing)
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
