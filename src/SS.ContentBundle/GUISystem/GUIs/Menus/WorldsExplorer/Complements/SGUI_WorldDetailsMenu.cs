using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Complements
{
    internal sealed partial class SGUI_WorldDetailsMenu : SGUISystem
    {
        private SWorldSaveFile worldSaveFile;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiButton3Texture;
        private readonly Texture2D returnIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont pixelOperatorSpriteFont;

        private readonly SButton[] worldButtons;

        internal SGUI_WorldDetailsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiButton3Texture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.returnIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_16");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.PIXEL_OPERATOR);

            this.worldButtons = [
                new(null, "Return", ReturnButtonAction),
                new(null, "Delete", DeleteButtonAction),
                new(null, "Play", PlayButtonAction),
            ];

            this.worldButtonElements = new SGUILabelElement[this.worldButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            // Buttons
            for (int i = 0; i < this.worldButtonElements.Length; i++)
            {
                SGUILabelElement slotInfoElement = this.worldButtonElements[i];

                SSize2 buttonSize = slotInfoElement.GetStringSize() / 2;
                Vector2 buttonPosition = new(slotInfoElement.Position.X + buttonSize.Width, slotInfoElement.Position.Y - buttonSize.Height / 4);

                if (this.GUIEvents.OnMouseClick(buttonPosition, buttonSize))
                {
                    this.worldButtons[i].ClickAction.Invoke();
                }

                slotInfoElement.Color = this.GUIEvents.OnMouseOver(buttonPosition, buttonSize) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        internal void SetWorldSaveFile(SWorldSaveFile worldSaveFile)
        {
            this.worldSaveFile = worldSaveFile;
            UpdateDisplay(worldSaveFile);
        }

        private void UpdateDisplay(SWorldSaveFile worldSaveFile)
        {
            this.worldThumbnailElement.Texture = worldSaveFile.ThumbnailTexture;
        }
    }
}
