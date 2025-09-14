using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.Core.GUISystem.GUIs.Tools.Message
{
    internal sealed partial class SGUI_Message(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : SGUISystem(gameInstance, identifier, guiEvents)
    {
        private readonly Texture2D particleTexture = gameInstance.AssetDatabase.GetTexture("texture_particle_1");
        private readonly SpriteFont bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");
        private readonly SpriteFont pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_9");

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateContinueButton();
        }

        private void UpdateContinueButton()
        {
            Vector2 position = this.continueButtonElement.Position;
            SSize2 size = this.continueButtonElement.GetStringSize() / 2;

            if (this.GUIEvents.OnMouseClick(position, size))
            {
                this.SGameInstance.GUIManager.CloseGUI();
            }

            this.continueButtonElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.HoverColor : SColorPalette.White;
        }

        internal void SetContent(string text)
        {
            this.messageElement.SetTextualContent(text);
        }
    }
}
