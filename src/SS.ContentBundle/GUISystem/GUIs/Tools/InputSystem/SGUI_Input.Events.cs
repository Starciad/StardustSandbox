using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.InputSystem;
using StardustSandbox.Core.GUISystem;

using System;
using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input : SGUISystem
    {
        protected override void OnOpened()
        {
            UpdateDisplayedText();

            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;
            this.SGameInstance.GameInputController.Disable();

            this.SGameInstance.GraphicsManager.GameWindow.KeyDown += OnKeyDown;
            this.SGameInstance.GraphicsManager.GameWindow.TextInput += OnTextInput;
        }

        protected override void OnClosed()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
            this.SGameInstance.GameInputController.Activate();

            this.SGameInstance.GraphicsManager.GameWindow.KeyDown -= OnKeyDown;
            this.SGameInstance.GraphicsManager.GameWindow.TextInput -= OnTextInput;
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
                Keys.Enter => HandleEnterKey,
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
            if (this.userInputStringBuilder.Length >= this.inputSettings.MaxCharacters)
            {
                return;
            }

            this.userInputStringBuilder.Insert(this.cursorPosition, ' ');
            this.cursorPosition++;
        }

        private void HandleEnterKey()
        {
            if (this.userInputStringBuilder.Length >= this.inputSettings.MaxCharacters)
            {
                return;
            }

            this.userInputStringBuilder.Insert(this.cursorPosition, Environment.NewLine);
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
                case SInputRestriction.None:
                    break;

                case SInputRestriction.LettersOnly:
                    if (!char.IsLetter(character))
                    {
                        return;
                    }
                    break;

                case SInputRestriction.NumbersOnly:
                    if (!char.IsDigit(character))
                    {
                        return;
                    }
                    break;

                case SInputRestriction.Alphanumeric:
                    if (!char.IsLetterOrDigit(character))
                    {
                        return;
                    }
                    break;

                default:
                    break;
            }

            this.userInputStringBuilder.Insert(this.cursorPosition, character);
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
                case SInputMode.Normal:
                    this.userInputElement.SetTextualContent(this.userInputStringBuilder);
                    break;

                case SInputMode.Password:
                    UpdatePasswordMask(cursorPosition);
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

            this.userInputPasswordMaskedStringBuilder.Clear();

            for (int i = 0; i < this.userInputStringBuilder.Length; i++)
            {
                this.userInputPasswordMaskedStringBuilder.Append(i == cursorPosition ? '|' : '*');
            }
        }
    }
}
