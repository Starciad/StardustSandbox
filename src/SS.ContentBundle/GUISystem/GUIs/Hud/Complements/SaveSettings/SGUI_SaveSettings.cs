using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.Enums.GUISystem;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Specials;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.InputSystem.Settings;
using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Fonts;
using StardustSandbox.Core.Constants.GUI.Common;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.IO;

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

        internal SGUI_SaveSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_Input guiInput) : base(gameInstance, identifier, guiEvents)
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

            this.world = this.SGameInstance.World;
            this.guiInput = guiInput;

            this.nameInputBuilder = new()
            {
                InputType = SInputType.Text,
                MaxCharacters = 30,

                OnValidationCallback = (SValidationState state, SArgumentResult result) =>
                {
                    // Validation for empty world name
                    if (string.IsNullOrWhiteSpace(result.Content))
                    {
                        state.Status = SValidationStatus.Failure;
                        state.Message = "The name of the world cannot be empty.";
                        return;
                    }

                    // Validation for repeated world name
                    string[] files = Directory.GetFiles(SDirectory.Worlds, "*" + SFileExtensionConstants.WORLD, SearchOption.TopDirectoryOnly);

                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i] = Path.GetFileNameWithoutExtension(files[i]);
                    }

                    Array.Sort(files, StringComparer.OrdinalIgnoreCase);

                    if (Array.BinarySearch(files, result.Content, StringComparer.OrdinalIgnoreCase) >= 0)
                    {
                        state.Status = SValidationStatus.Failure;
                        state.Message = "The world name is already being used by another saved world.";
                    }
                },

                OnSendCallback = (SArgumentResult result) =>
                {
                    this.world.Infos.Name = result.Content;
                },
            };

            this.descriptionInputBuilder = new()
            {
                InputType = SInputType.Text,
                MaxCharacters = 300,

                OnSendCallback = (result) =>
                {
                    this.world.Infos.Description = result.Content;
                },
            };
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
            for (int i = 0; i < this.fieldButtonSlots.Length; i++)
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
