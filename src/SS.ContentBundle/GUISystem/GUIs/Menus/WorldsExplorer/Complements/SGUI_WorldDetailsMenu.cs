using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.IO.Files.World;

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

        internal SGUI_WorldDetailsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiButton3Texture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.returnIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_16");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.PIXEL_OPERATOR);
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
