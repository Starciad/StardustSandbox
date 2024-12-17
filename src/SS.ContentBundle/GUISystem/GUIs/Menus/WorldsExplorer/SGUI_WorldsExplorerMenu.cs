using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Complements;
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
            public bool IsVisible { get; private set; }

            public SGUIImageElement BackgroundElement { get; set; }
            public SGUIImageElement ThumbnailElement { get; set; }
            public SGUILabelElement TitleElement { get; set; }

            public void EnableVisibility()
            {
                this.IsVisible = true;
                this.BackgroundElement.IsVisible = true;
                this.ThumbnailElement.IsVisible = true;
                this.TitleElement.IsVisible = true;
            }

            public void DisableVisibility()
            {
                this.IsVisible = false;
                this.BackgroundElement.IsVisible = false;
                this.ThumbnailElement.IsVisible = false;
                this.TitleElement.IsVisible = false;
            }
        }

        private int currentPage = 0;
        private int totalPages = 1;

        private SWorldSaveFile[] savedWorldFilesLoaded;

        private readonly Texture2D particleTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D guiButton2Texture;
        private readonly Texture2D exitIconTexture;
        private readonly Texture2D reloadIconTexture;
        private readonly Texture2D folderIconTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] headerButtons;
        private readonly SButton[] footerButtons;

        private readonly SGUI_WorldDetailsMenu detailsMenu;

        internal SGUI_WorldsExplorerMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_WorldDetailsMenu detailsMenu) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiButton1Texture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.guiButton2Texture = this.SGameInstance.AssetDatabase.GetTexture("gui_button_2");
            this.exitIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_15");
            this.reloadIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_5");
            this.folderIconTexture = this.SGameInstance.AssetDatabase.GetTexture("icon_gui_18");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont(SFontFamilyConstants.BIG_APPLE_3PM);

            this.slotInfoElements = new SSlotInfoElement[SWorldsExplorerConstants.ITEMS_PER_PAGE];

            this.headerButtons = [
                new(this.exitIconTexture, "Exit", ExitButtonAction),
                new(this.reloadIconTexture, "Reload", ReloadButtonAction),
                new(this.folderIconTexture, "Open Directory in Explorer", OpenDirectoryInExplorerAction),
            ];

            this.footerButtons = [
                new(null, "Previous", PreviousButtonAction),
                new(null, "Next", NextButtonAction),
            ];

            this.headerButtonElements = new SGUIImageElement[this.headerButtons.Length];
            this.footerButtonElements = new SGUILabelElement[this.footerButtons.Length];

            this.detailsMenu = detailsMenu;

            UpdatePagination();
        }

        public override void Update(GameTime gameTime)
        {
            #region BUTTONS
            // HEADER
            for (int i = 0; i < this.headerButtonElements.Length; i++)
            {
                SGUIImageElement buttonBackgroundElement = this.headerButtonElements[i];
                SSize2 buttonSize = buttonBackgroundElement.Size / 2;

                if (this.GUIEvents.OnMouseClick(buttonBackgroundElement.Position, buttonSize))
                {
                    this.headerButtons[i].ClickAction.Invoke();
                }

                buttonBackgroundElement.Color = this.GUIEvents.OnMouseOver(buttonBackgroundElement.Position, buttonSize) ? SColorPalette.LightGrayBlue : SColorPalette.White;
            }

            // FOOTER
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
            #endregion

            // SLOTS
            for (int i = 0; i < this.slotInfoElements.Length; i++)
            {
                SSlotInfoElement slotInfoElement = this.slotInfoElements[i];
    
                if (!this.slotInfoElements[i].IsVisible)
                {
                    break;
                }

                SSize2 backgroundSize = slotInfoElement.BackgroundElement.Size / 2;
                Vector2 backgroundPosition = slotInfoElement.BackgroundElement.Position + backgroundSize.ToVector2();

                if (this.GUIEvents.OnMouseClick(backgroundPosition, backgroundSize))
                {
                    this.detailsMenu.SetWorldSaveFile(this.savedWorldFilesLoaded[this.currentPage + i]);
                    this.SGameInstance.GUIManager.OpenGUI(this.detailsMenu.Identifier);
                }

                slotInfoElement.BackgroundElement.Color = this.GUIEvents.OnMouseOver(backgroundPosition, backgroundSize) ? SColorPalette.LightGrayBlue : SColorPalette.White;
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
