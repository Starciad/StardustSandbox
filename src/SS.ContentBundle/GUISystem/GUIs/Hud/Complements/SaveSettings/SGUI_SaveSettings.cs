﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.InputSystem;
using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Global;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Specials;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Messages;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;
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

        private readonly ISWorld world;
        private readonly SGUI_Input guiInput;

        private readonly SInputSettings nameInputBuilder;
        private readonly SInputSettings descriptionInputBuilder;

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_SaveSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_Input guiInput, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiSmallButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.guiLargeButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_3");
            this.guiFieldTexture = gameInstance.AssetDatabase.GetTexture("gui_field_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.pixelOperatorSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_9");

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("icon_gui_16"),
            ];

            this.menuButtons = [
                new(this.iconTextures[0], SLocalization_GUIs.Button_Exit_Name, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.fieldButtons = [
                new(null, "Name Field", string.Empty, NameFieldButtonAction),
                new(null, "Description Field", string.Empty, DescriptionFieldButtonAction)
            ];

            this.footerButtons = [
                new(null, SLocalization_Statements.Save, SLocalization_GUIs.HUD_Complements_SaveSettings_Button_Save_Description, SaveButtonAction),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
            this.fieldButtonSlots = new SSlot[this.fieldButtons.Length];
            this.footerButtonSlots = new SSlot[this.footerButtons.Length];

            this.world = this.SGameInstance.World;
            this.guiInput = guiInput;

            this.nameInputBuilder = new()
            {
                Synopsis = SLocalization_Messages.Input_World_Name,
                InputMode = SInputMode.Normal,
                InputRestriction = SInputRestriction.Alphanumeric,
                MaxCharacters = 50,

                OnSendCallback = (SArgumentResult result) =>
                {
                    this.world.Infos.Name = result.Content;
                },
            };

            this.descriptionInputBuilder = new()
            {
                Synopsis = SLocalization_Messages.Input_World_Description,
                InputMode = SInputMode.Normal,
                MaxCharacters = 500,

                OnSendCallback = (result) =>
                {
                    this.world.Infos.Description = result.Content;
                },
            };

            this.tooltipBoxElement = tooltipBoxElement;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateMenuButtons();
            UpdateFieldButtons();
            UpdateFooterButtons();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                Vector2 position = slot.BackgroundElement.Position;
                SSize2 size = new(SGUI_HUDConstants.SLOT_SIZE);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.menuButtons[i].ClickAction.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.menuButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.menuButtons[i].Description;

                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
            }
        }

        private void UpdateFieldButtons()
        {
            for (int i = 0; i < this.fieldButtons.Length; i++)
            {
                SSlot slot = this.fieldButtonSlots[i];

                SSize2 size = slot.BackgroundElement.Size / 2;
                Vector2 position = slot.BackgroundElement.Position + size.ToVector2();

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.fieldButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }

        private void UpdateFooterButtons()
        {
            for (int i = 0; i < this.footerButtons.Length; i++)
            {
                SSlot slot = this.footerButtonSlots[i];

                SSize2 size = slot.BackgroundElement.Size / 2;
                Vector2 position = slot.BackgroundElement.Position + size.ToVector2();

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    this.footerButtons[i].ClickAction.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = this.footerButtons[i].DisplayName;
                    SGUIGlobalTooltip.Description = this.footerButtons[i].Description;

                    slot.BackgroundElement.Color = SColorPalette.HoverColor;
                }
                else
                {
                    slot.BackgroundElement.Color = SColorPalette.White;
                }
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
