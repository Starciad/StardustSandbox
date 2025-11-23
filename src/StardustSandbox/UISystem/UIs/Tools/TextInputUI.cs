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
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Results;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.Utilities;

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

        private TextUIElement synopsisElement;
        private TextUIElement userInputElement;
        private LabelUIElement characterCountElement;

        private ImageUIElement userInputBackgroundElement;

        private readonly LabelUIElement[] menuButtonElements;

        private readonly StringBuilder userInputStringBuilder = new();
        private readonly StringBuilder userInputPasswordMaskedStringBuilder = new();

        private readonly UIButton[] menuButtons;

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

            this.menuButtonElements = new LabelUIElement[this.menuButtons.Length];
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
            this.synopsisElement.SetTextualContent(settings.Synopsis);

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
            this.characterCountElement.IsVisible = settings.MaxCharacters != 0;
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

        protected override void OnBuild(Layout layout)
        {
            BuildBackground(layout);
            BuildSynopsis(layout);
            BuildUserInput(layout);
            BuildCharacterCount(layout);
            BuildMenuButtons(layout);
        }

        private static void BuildBackground(Layout layout)
        {
            ImageUIElement guiBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Size = ScreenConstants.SCREEN_DIMENSIONS.ToVector2(),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            layout.AddElement(guiBackground);
        }

        private void BuildSynopsis(Layout layout)
        {
            this.synopsisElement = new()
            {
                Scale = new(0.1f),
                Margin = new(0, -128),
                LineHeight = 1.25f,
                TextAreaSize = new(850, 1000),
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
                PositionAnchor = CardinalDirection.Center,
                OriginPivot = CardinalDirection.Center,
            };

            this.synopsisElement.SetTextualContent("Synopsis");
            this.synopsisElement.PositionRelativeToScreen();

            layout.AddElement(this.synopsisElement);
        }

        private void BuildUserInput(Layout layout)
        {
            this.userInputBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiTextInputOrnament),
                Scale = new(1.5f),
                Size = new(632, 50),
                Margin = new(0, 64),
                PositionAnchor = CardinalDirection.Center,
                OriginPivot = CardinalDirection.Center,
            };

            this.userInputElement = new()
            {
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
                Scale = new(0.085f),
                TextAreaSize = new(1000, 1000),
                Margin = new(0, -32),
                PositionAnchor = CardinalDirection.Center,
                OriginPivot = CardinalDirection.Center,
            };

            this.userInputBackgroundElement.PositionRelativeToScreen();
            this.userInputElement.PositionRelativeToScreen();

            layout.AddElement(this.userInputBackgroundElement);
            layout.AddElement(this.userInputElement);
        }

        private void BuildCharacterCount(Layout layout)
        {
            this.characterCountElement = new()
            {
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.PixelOperator),
                Scale = new(0.08f),
                Margin = new(-212, -16),
                PositionAnchor = CardinalDirection.East,
                OriginPivot = CardinalDirection.West,
            };

            this.characterCountElement.SetTextualContent("000/000");
            this.characterCountElement.PositionRelativeToScreen();

            layout.AddElement(this.characterCountElement);
        }

        private void BuildMenuButtons(Layout layout)
        {
            Vector2 margin = new(0, -48);

            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                UIButton button = this.menuButtons[i];

                LabelUIElement labelElement = new()
                {
                    SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                    Scale = new(0.125f),
                    Margin = margin,
                    PositionAnchor = CardinalDirection.South,
                    OriginPivot = CardinalDirection.Center,
                };

                labelElement.SetTextualContent(button.Name);
                labelElement.PositionRelativeToScreen();
                labelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2));

                margin.Y -= 72;

                layout.AddElement(labelElement);

                this.menuButtonElements[i] = labelElement;
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
                LabelUIElement labelElement = this.menuButtonElements[i];

                Vector2 size = labelElement.GetStringSize() / 2;
                Vector2 position = labelElement.Position;

                if (UIInteraction.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = UIInteraction.OnMouseOver(position, size) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateElementPositionAccordingToUserInput()
        {
            float screenCenterYPosition = (ScreenConstants.SCREEN_HEIGHT / 2) + this.userInputElement.GetStringSize().Y;

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
            this.userInputElement.SetTextualContent(this.userInputStringBuilder);

            if (this.characterCountElement.IsVisible)
            {
                this.characterCountElement.SetTextualContent(string.Concat(this.userInputStringBuilder.Length, '/', this.inputSettings.MaxCharacters));
            }

            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            _ = this.userInputStringBuilder.Insert(this.cursorPosition, '|');

            switch (this.inputSettings.InputMode)
            {
                case InputMode.Normal:
                    this.userInputElement.SetTextualContent(this.userInputStringBuilder);
                    break;

                case InputMode.Password:
                    UpdatePasswordMask(this.cursorPosition);
                    this.userInputElement.SetTextualContent(this.userInputPasswordMaskedStringBuilder);
                    break;

                default:
                    this.userInputElement.SetTextualContent(this.userInputStringBuilder);
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
