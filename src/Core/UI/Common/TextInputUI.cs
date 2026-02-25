/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Enums.UI.Tools;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

using System;
using System.Text;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class TextInputUI : UIBase
    {
        private bool allowSpaces;
        private uint maxCharacters;
        private InputMode inputMode;
        private InputRestriction inputRestriction;
        private Func<string, ValidationState> validateCallback;
        private Action<string> sendCallback;

        private int cursorPosition = 0;

        private Vector2 userInputBackgroundElementPosition = Vector2.Zero;
        private Vector2 characterCountElementPosition = Vector2.Zero;

        private Image shadowBackground, userInputBackground;

        private Label characterCount;
        private Text synopsis, userInput;

        private readonly Label[] menuButtonLabels;
        private readonly ButtonInfo[] menuButtonInfos;

        private readonly StringBuilder userInputStringBuilder = new();
        private readonly StringBuilder userInputPasswordMaskedStringBuilder = new();

        private readonly GameWindow gameWindow;
        private readonly PlayerInputController playerInputController;

        internal TextInputUI(
            GameWindow gameWindow,
            MessageUI messageUI,
            PlayerInputController playerInputController,
            UIManager uiManager
        ) : base()
        {
            this.gameWindow = gameWindow;
            this.playerInputController = playerInputController;

            this.menuButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Cancel, string.Empty, () =>
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    uiManager.CloseUI();
                }),
                new(TextureIndex.None, null, Localization_Statements.Send, string.Empty, () =>
                {
                    string content = this.userInputStringBuilder.ToString();
                    this.sendCallback?.Invoke(content);

                    if (this.validateCallback != null)
                    {
                        ValidationState validationState = this.validateCallback.Invoke(content);

                        if (validationState.Status is ValidationStatus.Failure)
                        {
                            SoundEngine.Play(SoundEffectIndex.GUI_Error);
                            messageUI.SetContent(validationState.Message);
                            uiManager.OpenUI(UIIndex.Message);
                            return;
                        }
                    }

                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    uiManager.CloseUI();
                }),
            ];

            this.menuButtonLabels = new Label[this.menuButtonInfos.Length];
        }

        internal void Setup(string synopsis, string content, bool allowSpaces, InputMode inputMode, InputRestriction inputRestriction, uint maxCharacters, Func<string, ValidationState> validateCallback, Action<string> sendCallback)
        {
            // Setting Synopsis
            this.synopsis.TextContent = synopsis;

            // Setting Content
            _ = this.userInputStringBuilder.Clear();

            if (string.IsNullOrWhiteSpace(content))
            {
                this.cursorPosition = 0;
            }
            else
            {
                _ = this.userInputStringBuilder.Append(content);
                this.cursorPosition = content.Length;
            }

            // Count
            this.characterCount.CanDraw = maxCharacters != 0;

            // Settings
            this.allowSpaces = allowSpaces;
            this.maxCharacters = maxCharacters;
            this.inputMode = inputMode;
            this.inputRestriction = inputRestriction;

            // Callbacks
            this.validateCallback = validateCallback;
            this.sendCallback = sendCallback;
        }

        protected override void OnBuild(Container root)
        {
            // Shadow
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            root.AddChild(this.shadowBackground);

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
                Alignment = UIDirection.North,
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
                Alignment = UIDirection.Center,
            };

            this.userInputBackground = new()
            {
                TextureIndex = TextureIndex.UITextInputOrnament,
                Scale = new(1.5f),
                Size = new(632.0f, 50.0f),
                Margin = new(0.0f, 64.0f),
                Alignment = UIDirection.Center,
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
                Alignment = UIDirection.East,
            };

            root.AddChild(this.characterCount);
        }

        private void BuildMenuButtons(Container root)
        {
            float marginY = -48.0f;

            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                ButtonInfo button = this.menuButtonInfos[i];

                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.125f),
                    Margin = new(0.0f, marginY),
                    Alignment = UIDirection.South,
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

        protected override void OnScreenResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateMenuButtons();
            UpdateElementPositionAccordingToUserInput();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                Label label = this.menuButtonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                label.Color = Interaction.OnMouseOver(label) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private void UpdateElementPositionAccordingToUserInput()
        {
            float screenCenterYPosition = GameScreen.GetViewportCenter().Y + (this.userInput.Size.Y / 2.0f);

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

            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
            this.playerInputController.Disable();

            this.gameWindow.KeyDown += OnKeyDown;
            this.gameWindow.TextInput += OnTextInput;
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
            this.playerInputController.Enable();

            this.gameWindow.KeyDown -= OnKeyDown;
            this.gameWindow.TextInput -= OnTextInput;
        }

        #region INPUT EVENTS

        private static void PlayTypingSound()
        {
            SoundEngine.Play((SoundEffectIndex)Randomness.Random.Range((int)SoundEffectIndex.GUI_Typing_1, (int)SoundEffectIndex.GUI_Typing_5));
        }

        private void OnKeyDown(object sender, InputKeyEventArgs inputKeyEventArgs)
        {
            if (!IsSpecialKey(inputKeyEventArgs.Key, out Action specialKeyAction))
            {
                return;
            }

            PlayTypingSound();
            specialKeyAction?.Invoke();

            UpdateDisplayedText();
        }

        private void OnTextInput(object sender, TextInputEventArgs textInputEventArgs)
        {
            if (IsSpecialKey(textInputEventArgs.Key, out Action specialKeyAction))
            {
                return;
            }

            PlayTypingSound();
            AddCharacter(textInputEventArgs.Character);

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
            if (this.userInputStringBuilder.Length >= this.maxCharacters || !this.allowSpaces)
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
            if (char.IsControl(character) || this.userInputStringBuilder.Length >= this.maxCharacters)
            {
                return;
            }

            switch (this.inputRestriction)
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
                this.characterCount.TextContent = string.Concat(this.userInputStringBuilder.Length, '/', this.maxCharacters);
            }

            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            _ = this.userInputStringBuilder.Insert(this.cursorPosition, '|');

            switch (this.inputMode)
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

            for (int i = 0; i < this.userInputStringBuilder.Length; i++)
            {
                _ = this.userInputPasswordMaskedStringBuilder.Append(i == cursorPosition ? '|' : '*');
            }
        }

        #endregion

        #endregion
    }
}

