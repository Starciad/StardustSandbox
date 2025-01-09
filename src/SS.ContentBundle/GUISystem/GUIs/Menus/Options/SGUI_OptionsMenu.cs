using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options.Structure;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.ColorPicker;
using StardustSandbox.ContentBundle.GUISystem.GUIs.Tools.Message;
using StardustSandbox.ContentBundle.GUISystem.Helpers.General;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Interactive;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Options;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.ColorPicker;
using StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.Settings;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.ContentBundle.Localization.Statements;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Mathematics.Primitives;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Options
{
    internal sealed partial class SGUI_OptionsMenu : SGUISystem
    {
        private enum SSystemButton : byte
        {
            Return = 0,
            Save = 1
        }

        private byte selectedSectionIndex;
        private bool restartMessageAppeared;

        private readonly SVideoSettings videoSettings;
        private readonly SLanguageSettings languageSettings;

        private readonly Texture2D panelBackgroundTexture;
        private readonly Texture2D colorButtonTexture;
        private readonly SpriteFont bigApple3PMSpriteFont;
        private readonly SpriteFont digitalDiscoSpriteFont;

        private readonly string titleName = SLocalization_GUIs.Menu_Options_Title;

        private readonly SButton[] systemButtons;

        private readonly SGUI_ColorPicker guiColorPicker;
        private readonly SGUI_Message guiMessage;
        private readonly SRoot root;

        private readonly SColorPickerSettings colorPickerSettings;

        internal SGUI_OptionsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_ColorPicker guiColorPicker, SGUI_Message guiMessage) : base(gameInstance, identifier, guiEvents)
        {
            this.guiColorPicker = guiColorPicker;
            this.guiMessage = guiMessage;

            this.colorPickerSettings = new();

            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_13");
            this.colorButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_4");
            this.bigApple3PMSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.digitalDiscoSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_8");

            this.systemButtons = [
                new(null, SLocalization_Statements.Return, string.Empty, ReturnButtonAction),
                new(null, SLocalization_Statements.Save, string.Empty, SaveButtonAction),
            ];

            this.root = new()
            {
                Sections = [
                    new("general", "General", string.Empty, [
                        new SSelectorOption("language", "Language", string.Empty, Array.ConvertAll<SGameCulture, object>(SLocalizationConstants.AVAILABLE_GAME_CULTURES, x => x.CultureInfo.NativeName.FirstCharToUpper()), 0),
                    ]),

                    new("gameplay", "Gameplay", string.Empty, [
                        new SColorOption("preview_area_color", "Preview Area Color", string.Empty, SColorPalette.White),
                        new SSliderOption("preview_area_opacity", "Preview Area Opacity", string.Empty, new(0, 100), 50),
                    ]),

                    new("volume", "Volume", string.Empty, [
                        new SSliderOption("master_volume", "Master Volume", string.Empty, new(000, 100), 100),
                        new SSliderOption("music_volume", "Music Volume", string.Empty, new(000, 100), 50),
                        new SSliderOption("sfx_volume", "SFX Volume", string.Empty, new(000, 100), 50)
                    ]),

                    new("video", "Video", string.Empty, [
                        new SSelectorOption("resolution", "Resolution", string.Empty, Array.ConvertAll<SSize2, object>(SScreenConstants.RESOLUTIONS, x => x)),
                        new SSelectorOption("fullscreen", "Fullscreen", string.Empty, [SLocalization_Statements.False, SLocalization_Statements.True]),
                        new SSelectorOption("vsync", "VSync", string.Empty, [SLocalization_Statements.False, SLocalization_Statements.True]),
                        new SSelectorOption("borderless", "Borderless", string.Empty, [SLocalization_Statements.False, SLocalization_Statements.True]),
                    ]),

                    new("graphics", "Graphics", string.Empty, [
                        new SSelectorOption("lighting", "Lighting", string.Empty, [SLocalization_Statements.False, SLocalization_Statements.True], 1),
                    ]),

                    new("cursor", "Cursor", string.Empty, [
                        new SColorOption("border_color", "Border Color", string.Empty, SColorPalette.OrangeRed),
                        new SColorOption("background_color", "Background Color", string.Empty, SColorPalette.White),
                        new SSliderOption("opacity", "Opacity", string.Empty, new(0, 100), 0),
                        new SSelectorOption("scale", "Scale", string.Empty, ["Very Small", "Small", "Medium", "Large", "Very Large"], 3),
                    ]),
                ],
            };

            this.systemButtonElements = new SGUILabelElement[this.systemButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateSectionButtons();
            UpdateSystemButtons();
            UpdateSectionOptions();
        }

        private void UpdateSectionButtons()
        {
            for (byte i = 0; i < this.sectionButtonElements.Count; i++)
            {
                SGUILabelElement labelElement = this.sectionButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize() / 2f))
                {
                    SelectSection(i);
                }

                if (this.selectedSectionIndex.Equals(i))
                {
                    labelElement.Color = SColorPalette.LemonYellow;
                    continue;
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize() / 2f) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        private void UpdateSystemButtons()
        {
            for (byte i = 0; i < this.systemButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.systemButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize() / 2f))
                {
                    this.systemButtons[i].ClickAction?.Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize() / 2f) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        private void UpdateSectionOptions()
        {
            SSize2 size = new(295, 18);

            foreach (SGUIElement element in this.sectionContents[this.selectedSectionIndex])
            {
                Vector2 position = new(element.Position.X + size.Width, element.Position.Y - 6);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    HandleOptionInteractivity((SOption)element.GetData("option"), element);
                }

                element.Color = this.GUIEvents.OnMouseOver(position, size) ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        private void HandleOptionInteractivity(SOption option, SGUIElement element)
        {
            switch (option)
            {
                case SButtonOption buttonOption:
                    HandleButtonOption(buttonOption);
                    break;

                case SColorOption colorOption:
                    HandleColorOption(colorOption, element);
                    break;

                case SSelectorOption selectorOption:
                    HandleSelectorOption(selectorOption, element);
                    break;

                default:
                    break;
            }
        }

        private static void HandleButtonOption(SButtonOption buttonOption)
        {
            buttonOption.OnClickCallback?.Invoke();
        }

        private void HandleColorOption(SColorOption colorOption, SGUIElement element)
        {
            this.colorPickerSettings.OnSelectCallback = (SColorPickerResult result) =>
            {
                colorOption.SetValue(result.SelectedColor);
                ((SColorSlot)element.GetData("color_slot")).BackgroundElement.Color = result.SelectedColor;
            };

            this.guiColorPicker.Configure(this.colorPickerSettings);
            this.SGameInstance.GUIManager.OpenGUI(this.guiColorPicker.Identifier);
        }

        private static void HandleSelectorOption(SSelectorOption selectorOption, SGUIElement element)
        {
            selectorOption.Next();

            if (element is SGUILabelElement labelElement)
            {
                labelElement.SetTextualContent(string.Concat(selectorOption.Name, ": ", selectorOption.GetValue()));
            }
        }

        private void SelectSection(byte index)
        {
            this.selectedSectionIndex = byte.Clamp(index, 0, (byte)(this.sectionButtonElements.Count - 1));

            for (byte i = 0; i < this.sectionContainerElements.Count; i++)
            {
                if (this.selectedSectionIndex.Equals(i))
                {
                    this.sectionContainerElements[i].Active();
                    continue;
                }

                this.sectionContainerElements[i].Disable();
            }
        }
    }
}
