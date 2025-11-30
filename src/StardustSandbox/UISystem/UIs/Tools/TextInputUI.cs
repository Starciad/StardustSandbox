using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.Enums.UISystem.Tools;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Information;
using StardustSandbox.UISystem.Results;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.States;

using System;
using System.Text;

namespace StardustSandbox.UISystem.UIs.Tools
{
    internal sealed class TextInputUI : UI
    {
        private int cursorPosition = 0;

        private Vector2 userInputBackgroundElementPosition = Vector2.Zero;
        private Vector2 characterCountElementPosition = Vector2.Zero;

        private TextInputSettings inputSettings;

        private Text synopsisElement;
        private Text userInputElement;
        private Label characterCountElement;

        private Image userInputBackgroundElement;

        private readonly Label[] menuButtonElements;

        private readonly StringBuilder userInputStringBuilder = new();
        private readonly StringBuilder userInputPasswordMaskedStringBuilder = new();

        private readonly ButtonInfo[] menuButtons;

        private readonly GameManager gameManager;
        private readonly GameWindow gameWindow;
        private readonly InputController inputController;
        private readonly MessageUI messageUI;
        private readonly UIManager uiManager;

        internal TextInputUI(
            GameManager gameManager,
            GameWindow gameWindow,
            UIIndex index,
            InputController inputController,
            MessageUI messageUI,
            UIManager uiManager
        ) : base(index)
        {
            this.gameManager = gameManager;
            this.gameWindow = gameWindow;
            this.inputController = inputController;
            this.messageUI = messageUI;
            this.uiManager = uiManager;

            this.menuButtons = [
                new(null, null, Localization_Statements.Cancel, string.Empty, CancelButtonAction),
                new(null, null, Localization_Statements.Send, string.Empty, SendButtonAction),
            ];

            this.menuButtonElements = new Label[this.menuButtons.Length];
        }

        #region INITIALIZE

        internal void Configure(TextInputSettings settings)
        {
            this.inputSettings = settings;
            ApplySettings(settings);
        }

        private void ApplySettings(TextInputSettings settings)
        {
            // Setting Synopsis
            this.synopsisElement.TextContent = settings.Synopsis;

            // Setting Content
            _ = this.userInputStringBuilder.Clear();

            if (string.IsNullOrWhiteSpace(settings.Content))
            {
                this.cursorPosition = 0;
            }
            else
            {
                _ = this.userInputStringBuilder.Append(settings.Content);
                this.cursorPosition = settings.Content.Length;
            }

            // Count
            this.characterCountElement.CanDraw = settings.MaxCharacters != 0;
        }

        #endregion

        #region ACTIONS

        private void CancelButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        private void SendButtonAction()
        {
            this.inputSettings?.OnSendCallback?.Invoke(new(this.userInputStringBuilder.ToString()));

            if (this.inputSettings != null)
            {
                TextValidationState validationState = new();
                TextArgumentResult argumentResult = new(this.userInputStringBuilder.ToString());

                this.inputSettings.OnValidationCallback?.Invoke(validationState, argumentResult);

                if (validationState.Status == ValidationStatus.Failure)
                {
                    this.messageUI.SetContent(validationState.Message);
                    this.uiManager.OpenGUI(UIIndex.Message);
                    return;
                }

                this.inputSettings.OnSendCallback?.Invoke(argumentResult);
            }

            this.uiManager.CloseGUI();
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildSynopsis(root);
            BuildUserInput(root);
            BuildCharacterCount(root);
            BuildMenuButtons(root);
        }

        private static void BuildBackground(Container root)
        {
            Image guiBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2(),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            root.AddChild(guiBackground);
        }

        private void BuildSynopsis(Container root)
        {
            this.synopsisElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, -128),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.Center,
                TextContent = "Synopsis"
            };

            root.AddChild(this.synopsisElement);
        }

        private void BuildUserInput(Container root)
        {
            this.userInputBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiTextInputOrnament),
                Scale = new(1.5f),
                Size = new(632, 50),
                Margin = new(0, 64),
                Alignment = CardinalDirection.Center,
            };

            this.userInputElement = new()
            {
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Scale = new(0.085f),
                TextAreaSize = new(1000, 1000),
                Margin = new(0, -32),
                Alignment = CardinalDirection.Center,
            };

            root.AddChild(this.userInputBackgroundElement);
            root.AddChild(this.userInputElement);
        }

        private void BuildCharacterCount(Container root)
        {
            this.characterCountElement = new()
            {
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Scale = new(0.08f),
                Margin = new(-212, -16),
                Alignment = CardinalDirection.East,
                TextContent = "000/000"
            };

            root.AddChild(this.characterCountElement);
        }

        private void BuildMenuButtons(Container root)
        {
            Vector2 margin = new(0, -48);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                ButtonInfo button = this.menuButtons[i];

                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.125f),
                    Margin = margin,
                    Alignment = CardinalDirection.South,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2f,
                    BorderThickness = 2f,
                };

                margin.Y -= 72;

                root.AddChild(label);

                this.menuButtonElements[i] = label;
            }
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateMenuButtons();
            UpdateElementPositionAccordingToUserInput();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                Label label = this.menuButtonElements[i];

                Vector2 size = label.MeasuredText / 2;
                Vector2 position = label.Position;

                if (Interaction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateElementPositionAccordingToUserInput()
        {
            float screenCenterYPosition = (ScreenConstants.SCREEN_HEIGHT / 2) + this.userInputElement.MeasuredText.Y;

            // Background
            this.userInputBackgroundElementPosition.X = this.userInputBackgroundElement.Position.X;
            this.userInputBackgroundElementPosition.Y = screenCenterYPosition;

            // Count
            this.characterCountElementPosition.X = this.characterCountElement.Position.X;
            this.characterCountElementPosition.Y = screenCenterYPosition - 32;

            // Apply
            this.userInputBackgroundElement.Position = this.userInputBackgroundElementPosition;
            this.characterCountElement.Position = this.characterCountElementPosition;
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            UpdateDisplayedText();

            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            this.inputController.Disable();

            this.gameWindow.KeyDown += OnKeyDown;
            this.gameWindow.TextInput += OnTextInput;
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
            this.inputController.Activate();

            this.gameWindow.KeyDown -= OnKeyDown;
            this.gameWindow.TextInput -= OnTextInput;
        }

        // ======================================================================== //

        private void OnKeyDown(object sender, InputKeyEventArgs inputKeyEventArgs)
        {
            if (IsSpecialKey(inputKeyEventArgs.Key, out Action specialKeyAction))
            {
                specialKeyAction?.Invoke();
                UpdateDisplayedText();
            }
        }

        private void OnTextInput(object sender, TextInputEventArgs textInputEventArgs)
        {
            if (IsTextSpecialKey(textInputEventArgs.Key, out Action specialKeyAction))
            {
                specialKeyAction?.Invoke();
            }
            else
            {
                AddCharacter(textInputEventArgs.Character);
            }

            UpdateDisplayedText();
        }

        // ========================================================= //

        private bool IsSpecialKey(Keys key, out Action action)
        {
            action = key switch
            {
                Keys.Left => MoveCursorLeft,
                Keys.Right => MoveCursorRight,
                Keys.Home => HandleHomeKey,
                Keys.End => HandleEndKey,
                Keys.Space => HandleSpaceKey,
                _ => null,
            };

            return action != null;
        }

        private bool IsTextSpecialKey(Keys key, out Action action)
        {
            action = key switch
            {
                Keys.Back => HandleBackspaceKey,
                _ => null,
            };

            return action != null;
        }

        // ========================================================= //

        private void MoveCursorLeft()
        {
            if (this.cursorPosition > 0)
            {
                this.cursorPosition--;
            }
        }

        private void MoveCursorRight()
        {
            if (this.cursorPosition < this.userInputStringBuilder.Length)
            {
                this.cursorPosition++;
            }
        }

        private void HandleBackspaceKey()
        {
            if (this.userInputStringBuilder.Length == 0)
            {
                return;
            }

            int index = this.cursorPosition - 1;

            if (index < 0)
            {
                return;
            }

            _ = this.userInputStringBuilder.Remove(index, 1);
            this.cursorPosition = index;
        }

        private void HandleHomeKey()
        {
            this.cursorPosition = 0;
        }

        private void HandleEndKey()
        {
            this.cursorPosition = this.userInputStringBuilder.Length;
        }

        private void HandleSpaceKey()
        {
            if (this.userInputStringBuilder.Length >= this.inputSettings.MaxCharacters || !this.inputSettings.AllowSpaces)
            {
                return;
            }

            _ = this.userInputStringBuilder.Insert(this.cursorPosition, ' ');
            this.cursorPosition++;
        }

        private void AddCharacter(char character)
        {
            if (this.userInputStringBuilder.Length >= this.inputSettings.MaxCharacters)
            {
                return;
            }

            if (char.IsControl(character))
            {
                return;
            }

            switch (this.inputSettings.InputRestriction)
            {
                case InputRestriction.None:
                    break;

                case InputRestriction.LettersOnly:
                    if (!char.IsLetter(character))
                    {
                        return;
                    }

                    break;

                case InputRestriction.NumbersOnly:
                    if (!char.IsDigit(character))
                    {
                        return;
                    }

                    break;

                case InputRestriction.Alphanumeric:
                    if (!char.IsLetterOrDigit(character))
                    {
                        return;
                    }

                    break;

                default:
                    break;
            }

            _ = this.userInputStringBuilder.Insert(this.cursorPosition, character);
            this.cursorPosition++;
        }

        private void UpdateDisplayedText()
        {
            this.userInputElement.TextContent = this.userInputStringBuilder.ToString();

            if (this.characterCountElement.CanDraw)
            {
                this.characterCountElement.TextContent = string.Concat(this.userInputStringBuilder.Length, '/', this.inputSettings.MaxCharacters);
            }

            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            _ = this.userInputStringBuilder.Insert(this.cursorPosition, '|');

            switch (this.inputSettings.InputMode)
            {
                case InputMode.Normal:
                    this.userInputElement.TextContent = this.userInputStringBuilder.ToString();
                    break;

                case InputMode.Password:
                    UpdatePasswordMask(this.cursorPosition);
                    this.userInputElement.TextContent = this.userInputPasswordMaskedStringBuilder.ToString();
                    break;

                default:
                    this.userInputElement.TextContent = this.userInputStringBuilder.ToString();
                    break;
            }

            _ = this.userInputStringBuilder.Remove(this.cursorPosition, 1);
        }

        private void UpdatePasswordMask(int cursorPosition)
        {
            if (this.userInputPasswordMaskedStringBuilder.Capacity < this.userInputStringBuilder.Length)
            {
                this.userInputPasswordMaskedStringBuilder.Capacity = this.userInputStringBuilder.Length;
            }

            _ = this.userInputPasswordMaskedStringBuilder.Clear();

            for (int i = 0; i < this.userInputStringBuilder.Length; i++)
            {
                _ = this.userInputPasswordMaskedStringBuilder.Append(i == cursorPosition ? '|' : '*');
            }
        }

        #endregion
    }
}
