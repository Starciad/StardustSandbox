using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal partial class SGUI_PlayMenu : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D returnIconTexture;
        private readonly Texture2D worldIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;

        internal SGUI_PlayMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.returnIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_16");
            this.worldIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_17");
            this.bigApple3PMSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.menuButtons = [
                new(this.worldIconTexture, "Worlds", string.Empty, WorldsButtonAction),
                new(this.returnIconTexture, "Return", string.Empty, ReturnButtonAction),
            ];

            this.menuButtonElements = new SGUILabelElement[this.menuButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.menuButtonElements[i];
                SSize2F labelElementSize = labelElement.GetStringSize() / 2f;

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElementSize))
                {
                    this.menuButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElementSize) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }
    }
}
