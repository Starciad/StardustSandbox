using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_SaveSettings : SGUISystem
    {
        private Texture2D worldThumbnailTexture;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiSmallButtonTexture;
        private readonly Texture2D guiLargeButtonTexture;
        private readonly Texture2D guiFieldTexture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont pixelOperatorSpriteFont;

        private readonly SButton[] menuButtons;
        private readonly SButton[] fieldButtons;
        private readonly SButton[] footerButtons;

        internal SGUI_SaveSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiSmallButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.guiLargeButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_3");
            this.guiFieldTexture = gameInstance.AssetDatabase.GetTexture("gui_field_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.PIXEL_OPERATOR);

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("icon_gui_16"),
            ];

            this.menuButtons = [
                new(this.iconTextures[0], "Exit", string.Empty, ExitButtonAction),
            ];

            this.fieldButtons = [
                new(null, "Name Field", string.Empty, NameFieldButtonAction),
                new(null, "Description Field", string.Empty, DescriptionFieldButtonAction)
            ];

            this.footerButtons = [
                new(null, "Save", string.Empty, SaveButtonAction),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
            this.fieldButtonSlots = new SSlot[this.fieldButtons.Length];
            this.footerButtonSlots = new SSlot[this.footerButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateMenuButtons();
            UpdateFieldButtons();
            UpdateFooterButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlots.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE)))
                {
                    this.menuButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SHUDConstants.SLOT_SIZE)) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateFieldButtons()
        {

        }

        private void UpdateFooterButtons()
        {
            for (int i = 0; i < this.footerButtonSlots.Length; i++)
            {
                SSlot slot = this.footerButtonSlots[i];

                SSize2 size = slot.BackgroundElement.Size / 2;
                Vector2 position = slot.BackgroundElement.Position + size.ToVector2();

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.footerButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateInfos()
        {
            this.worldThumbnailTexture = this.SGameInstance.World.CreateThumbnail(this.SGameInstance.GraphicsManager.GraphicsDevice);
            this.thumbnailPreviewElement.Texture = this.worldThumbnailTexture;

            this.titleTextualContentElement.SetTextualContent(this.SGameInstance.World.Infos.Name.Truncate(19));
            this.descriptionTextualContentElement.SetTextualContent(this.SGameInstance.World.Infos.Description.Truncate(19));
        }
    }
}
