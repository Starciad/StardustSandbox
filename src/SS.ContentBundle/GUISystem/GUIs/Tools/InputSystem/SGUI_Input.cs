using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Specials
{
    internal sealed partial class SGUI_Input : SGUISystem
    {
        private int cursorPosition = 0;

        private Vector2 userInputBackgroundElementPosition = Vector2.Zero;

        private SInputSettings inputSettings;

        private readonly Texture2D particleTexture;
        private readonly Texture2D typingFieldTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont pixelOperatorSpriteFont;

        private readonly StringBuilder userInputStringBuilder = new();

        private readonly SButton[] menuButtons;

        internal SGUI_Input(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.typingFieldTexture = gameInstance.AssetDatabase.GetTexture("gui_field_2");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_9");

            this.menuButtons = [
                new(null, "Cancel", string.Empty, CancelButtonAction),
                new(null, "Send", string.Empty, SendButtonAction),
            ];

            this.menuButtonElements = new SGUILabelElement[this.menuButtons.Length];
        }

        internal void Configure(SInputSettings settings)
        {
            this.inputSettings = settings;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateMenuButtons();
            UpdateInputBackgroundPosition();
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
                    this.menuButtons[i].ClickAction.Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateInputBackgroundPosition()
        {
            this.userInputBackgroundElementPosition.X = this.userInputBackgroundElement.Position.X;
            this.userInputBackgroundElementPosition.Y = (SScreenConstants.DEFAULT_SCREEN_HEIGHT / 2) + this.userInputElement.GetStringSize().Height - 8;
            this.userInputBackgroundElement.Position = this.userInputBackgroundElementPosition;
        }

        // ====================================== //

        private void ApplySettings()
        {

        }
    }
}
