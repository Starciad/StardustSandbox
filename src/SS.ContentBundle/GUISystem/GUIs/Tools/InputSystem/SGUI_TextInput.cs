using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.Modals;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_TextInput : SGUISystem
    {
        private int cursorPosition = 0;

        private Vector2 userInputBackgroundElementPosition = Vector2.Zero;
        private Vector2 characterCountElementPosition = Vector2.Zero;

        private STextInputSettings inputSettings;

        private readonly Texture2D particleTexture;
        private readonly Texture2D typingFieldTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont pixelOperatorSpriteFont;

        private readonly StringBuilder userInputStringBuilder = new();
        private readonly StringBuilder userInputPasswordMaskedStringBuilder = new();

        private readonly SButton[] menuButtons;

        private readonly SGUI_Message guiMessage;

        internal SGUI_TextInput(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_Message guiMessage) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.typingFieldTexture = gameInstance.AssetDatabase.GetTexture("gui_field_2");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_9");

            this.guiMessage = guiMessage;

            this.menuButtons = [
                new(null, SLocalization_Statements.Cancel, string.Empty, CancelButtonAction),
                new(null, SLocalization_Statements.Send, string.Empty, SendButtonAction),
            ];

            this.menuButtonElements = new SGUILabelElement[this.menuButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateMenuButtons();
            UpdateElementPositionAccordingToUserInput();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SGUILabelElement labelElement = this.menuButtonElements[i];

                SSize2 size = labelElement.GetStringSize() / 2;
                Vector2 position = labelElement.Position;

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateElementPositionAccordingToUserInput()
        {
            float screenCenterYPosition = (SScreenConstants.DEFAULT_SCREEN_HEIGHT / 2) + this.userInputElement.GetStringSize().Height;

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

        // ====================================== //

        internal void Configure(STextInputSettings settings)
        {
            this.inputSettings = settings;
            ApplySettings(settings);
        }

        private void ApplySettings(STextInputSettings settings)
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
    }
}
