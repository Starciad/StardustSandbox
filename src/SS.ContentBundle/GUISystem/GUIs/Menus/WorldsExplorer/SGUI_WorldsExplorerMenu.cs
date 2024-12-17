using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

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

        private int currentPage = 0;
        private int totalPages = 1;

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
            this.guiButtonTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_2");
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

            this.headerButtonElements = new SGUILabelElement[this.headerButtons.Length];
            this.footerButtonElements = new SGUILabelElement[this.footerButtons.Length];

            UpdatePagination();
        }

        public override void Update(GameTime gameTime)
        {
            // Buttons
            for (int i = 0; i < this.headerButtons.Length; i++)
            {
                // {It will still be added.}
            }

            for (int i = 0; i < this.footerButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.footerButtonElements[i];
                SSize2F labelElementSize = labelElement.GetStringSize() / 2f;

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElementSize))
                {
                    this.footerButtons[i].ClickAction.Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElementSize) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        private void UpdatePagination()
        {
            this.totalPages = Math.Max(1, (int)Math.Ceiling((double)(this.savedWorldFilesLoaded?.Length ?? 0) / SWorldsExplorerConstants.ITEMS_PER_PAGE));
            this.currentPage = Math.Clamp(this.currentPage, 0, this.totalPages - 1);
            this.pageIndexLabel?.SetTextualContent(string.Concat(this.currentPage + 1, " / ", Math.Max(this.totalPages, 1)));
        }

        private void ChangeWorldsCatalog()
        {
            int startIndex = this.currentPage * SWorldsExplorerConstants.ITEMS_PER_PAGE;

            for (int i = 0; i < this.slotInfoElements.Length; i++)
            {
                SSlotInfoElement slotInfoElement = this.slotInfoElements[i];
                int worldIndex = startIndex + i;

                if (worldIndex < this.savedWorldFilesLoaded?.Length)
                {
                    SWorldSaveFile worldSaveFile = this.savedWorldFilesLoaded[worldIndex];

                    slotInfoElement.EnableVisibility();
                    slotInfoElement.ThumbnailElement.Texture = worldSaveFile.ThumbnailTexture;
                    slotInfoElement.TitleElement.SetTextualContent(worldSaveFile.Metadata.Name.Truncate(14));
                }
                else
                {
                    slotInfoElement.DisableVisibility();
                }
            }

            UpdatePagination();
        }
    }
}
