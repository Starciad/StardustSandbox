using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.InputSystem.GameInput;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Results;
using StardustSandbox.UI.Settings;
using StardustSandbox.UI.States;

using System;
using System.Text;

namespace StardustSandbox.UI.Common.Tools
{
    internal sealed class TextInputUI : UIBase
    {
        private int cursorPosition = 0;

        private Vector2 userInputBackgroundElementPosition = Vector2.Zero;
        private Vector2 characterCountElementPosition = Vector2.Zero;

        private TextInputSettings inputSettings;

        private Image userInputBackground;

        private Label characterCount;
        private Text synopsis, userInput;

        private readonly Label[] menuButtonLabels;
        private readonly ButtonInfo[] menuButtonInfos;

        private readonly StringBuilder userInputStringBuilder = new();
        private readonly StringBuilder userInputPasswordMaskedStringBuilder = new();

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

            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Cancel, string.Empty, this.uiManager.CloseGUI),
                new(TextureIndex.None, null, Localization_Statements.Send, string.Empty, () =>
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
                }),
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
        }

        internal void Configure(TextInputSettings settings)
        {
            this.inputSettings = settings;
            ApplySettings(settings);
        }

        private void ApplySettings(TextInputSettings settings)
        {
            // Setting Synopsis
            this.synopsis.TextContent = settings.Synopsis;

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
            this.characterCount.CanDraw = settings.MaxCharacters != 0;
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            // Shadow
            root.AddChild(new Image()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160)
            });

            BuildSynopsis(root);
            BuildUserInput(root);
            BuildCharacterCount(root);
            BuildMenuButtons(root);
        }

        private void BuildSynopsis(Container root)
        {
            this.synopsis = new()
            {
                Scale = new(0.1f),
                Margin = new(0.0f, 128.0f),
                LineHeight = 1.25f,
                TextAreaSize = new(850.0f, 1000.0f),
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Alignment = CardinalDirection.North,
            };

            root.AddChild(this.synopsis);
        }

        private void BuildUserInput(Container root)
        {
            this.userInput = new()
            {
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Scale = new(0.085f),
                TextAreaSize = new(1000.0f, 1000.0f),
                Margin = new(0.0f, -32.0f),
                Alignment = CardinalDirection.Center,
            };

            this.userInputBackground = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UITextInputOrnament),
                Scale = new(1.5f),
                Size = new(632.0f, 50.0f),
                Margin = new(0.0f, 64.0f),
                Alignment = CardinalDirection.Center,
            };

            root.AddChild(this.userInput);
            root.AddChild(this.userInputBackground);
        }

        private void BuildCharacterCount(Container root)
        {
            this.characterCount = new()
            {
                SpriteFontIndex = SpriteFontIndex.PixelOperator,
                Scale = new(0.08f),
                Margin = new(-212.0f, -16.0f),
                Alignment = CardinalDirection.East,
            };

            root.AddChild(this.characterCount);
        }

        private void BuildMenuButtons(Container root)
        {
            float marginY = -48.0f;

            for (byte i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.125f),
                    Margin = new(0.0f, marginY),
                    Alignment = CardinalDirection.South,
                    TextContent = button.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                marginY -= 72;

                root.AddChild(label);

                this.menuButtonLabels[i] = label;
            }
        }

        #endregion

        internal override void Update(GameTime gameTime)
        {
            UpdateMenuButtons();
            UpdateElementPositionAccordingToUserInput();

            base.Update(gameTime);
        }

        private void UpdateMenuButtons()
        {
            for (byte i = 0; i < this.menuButtonInfos.Length; i++)
            {
                Label label = this.menuButtonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateElementPositionAccordingToUserInput()
        {
            float screenCenterYPosition = (ScreenConstants.SCREEN_HEIGHT / 2.0f) + (this.userInput.Size.Y / 2.0f);

            // Background
            this.userInputBackgroundElementPosition.X = this.userInputBackground.Position.X;
            this.userInputBackgroundElementPosition.Y = screenCenterYPosition;

            // Count
            this.characterCountElementPosition.X = this.characterCount.Position.X;
            this.characterCountElementPosition.Y = screenCenterYPosition - 32.0f;

            // Apply
            this.userInputBackground.Position = this.userInputBackgroundElementPosition;
            this.characterCount.Position = this.characterCountElementPosition;
        }

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

        #region INPUT EVENTS

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

        #endregion

        #region KEY CHECKERS

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

        #endregion

        #region CURSOR MOVEMENT

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

        #endregion

        #region SPECIAL KEY HANDLERS

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

        #endregion

        #region CHARACTER HANDLER

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

        #endregion

        #region DISPLAY UPDATER

        private void UpdateDisplayedText()
        {
            this.userInput.TextContent = this.userInputStringBuilder.ToString();

            if (this.characterCount.CanDraw)
            {
                this.characterCount.TextContent = string.Concat(this.userInputStringBuilder.Length, '/', this.inputSettings.MaxCharacters);
            }

            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            _ = this.userInputStringBuilder.Insert(this.cursorPosition, '|');

            switch (this.inputSettings.InputMode)
            {
                case InputMode.Normal:
                    this.userInput.TextContent = this.userInputStringBuilder.ToString();
                    break;

                case InputMode.Password:
                    UpdatePasswordMask(this.cursorPosition);
                    this.userInput.TextContent = this.userInputPasswordMaskedStringBuilder.ToString();
                    break;

                default:
                    this.userInput.TextContent = this.userInputStringBuilder.ToString();
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

            for (byte i = 0; i < this.userInputStringBuilder.Length; i++)
            {
                _ = this.userInputPasswordMaskedStringBuilder.Append(i == cursorPosition ? '|' : '*');
            }
        }

        #endregion

        #endregion
    }
}
