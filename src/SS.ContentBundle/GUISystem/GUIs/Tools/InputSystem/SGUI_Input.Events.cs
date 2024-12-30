using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.GUISystem;

using System;

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

        private void AddCharacter(char character)
        {
            if (this.userInputStringBuilder.Length > this.inputSettings.MaxCharacters)
            {
                return;
            }

            if (!char.IsControl(character))
            {
                _ = this.userInputStringBuilder.Insert(this.cursorPosition, character);
                this.cursorPosition++;
            }
        }

        private void UpdateDisplayedText()
        {
            this.userInputElement.SetTextualContent(this.userInputStringBuilder);
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            _ = this.userInputStringBuilder.Insert(this.cursorPosition, '|');
            this.userInputElement.SetTextualContent(this.userInputStringBuilder);
            _ = this.userInputStringBuilder.Remove(this.cursorPosition, 1);
        }
    }
}
