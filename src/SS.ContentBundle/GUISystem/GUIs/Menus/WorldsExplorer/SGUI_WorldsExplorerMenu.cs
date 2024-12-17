using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.Items;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    internal sealed partial class SGUI_WorldsExplorerMenu : SGUISystem
    {
        private sealed class SSlotInfoElement
        {
            public SGUIImageElement BackgroundElement { get; set; }
            public SGUIImageElement ThumbnailElement { get; set; }
            public SGUILabelElement TitleElement { get; set; }

            public void EnableVisibility()
            {
                this.BackgroundElement.IsVisible = true;
                this.ThumbnailElement.IsVisible = true;
                this.TitleElement.IsVisible = true;
            }

            public void DisableVisibility()
            {
                this.BackgroundElement.IsVisible = false;
                this.ThumbnailElement.IsVisible = false;
                this.TitleElement.IsVisible = false;
            }
        }

        private SWorldSaveFile[] savedWorldFilesLoaded;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiButtonTexture;
        private readonly Texture2D reloadIconTexture;
        private readonly Texture2D exitIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] headerButtons;
        private readonly SButton[] footerButtons;

        public SGUI_WorldsExplorerMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiButtonTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.reloadIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_5");
            this.exitIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_15");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);

            this.slotInfoElements = new SSlotInfoElement[SWorldsExplorerConstants.ITEMS_PER_PAGE];

            this.headerButtons = [
                new(this.reloadIconTexture, "Reload", ReloadButtonAction),
                new(this.exitIconTexture, "Exit", ExitButtonAction),
            ];

            this.footerButtons = [
                new(this.reloadIconTexture, "Previous", PreviousButtonAction),
                new(this.exitIconTexture, "Next", NextButtonAction),
            ];
        }

        private void ChangeWorldsCatalog()
        {
            for (int i = 0; i < this.slotInfoElements.Length; i++)
            {
                SSlotInfoElement slotInfoElement = this.slotInfoElements[i];

                if (i < this.savedWorldFilesLoaded.Length)
                {
                    SWorldSaveFile worldSaveFile = this.savedWorldFilesLoaded[i];

                    slotInfoElement.EnableVisibility();
                    slotInfoElement.ThumbnailElement.Texture = worldSaveFile.ThumbnailTexture;
                    slotInfoElement.TitleElement.SetTextualContent(worldSaveFile.Metadata.Name.Truncate(14));
                }
                else
                {
                    slotInfoElement.DisableVisibility();
                }
            }
        }
    }
}
