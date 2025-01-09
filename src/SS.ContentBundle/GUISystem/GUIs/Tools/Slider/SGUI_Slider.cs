using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.Settings;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.TextInput
{
    internal sealed partial class SGUI_Slider : SGUISystem
    {
        private readonly float currentValue;

        private SSliderSettings sliderSettings;

        private readonly Texture2D particleTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont pixelOperatorSpriteFont;

        private readonly SButton[] menuButtons;

        internal SGUI_Slider(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_9");

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

        // ====================================== //

        internal void Configure(SSliderSettings settings)
        {
            this.sliderSettings = settings;
            this.synopsisElement.SetTextualContent(settings.Synopsis);
        }
    }
}
