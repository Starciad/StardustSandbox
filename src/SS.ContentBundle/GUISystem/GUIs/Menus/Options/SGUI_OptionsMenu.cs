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
using StardustSandbox.Core.IO.Handlers;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;

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

        private readonly SGeneralSettings generalSettings;
        private readonly SGameplaySettings gameplaySettings;
        private readonly SVolumeSettings volumeSettings;
        private readonly SVideoSettings videoSettings;
        private readonly SGraphicsSettings graphicsSettings;
        private readonly SCursorSettings cursorSettings;

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
            this.generalSettings = SSettingsHandler.LoadSettings<SGeneralSettings>();
            this.gameplaySettings = SSettingsHandler.LoadSettings<SGameplaySettings>();
            this.volumeSettings = SSettingsHandler.LoadSettings<SVolumeSettings>();
            this.videoSettings = SSettingsHandler.LoadSettings<SVideoSettings>();
            this.graphicsSettings = SSettingsHandler.LoadSettings<SGraphicsSettings>();
            this.cursorSettings = SSettingsHandler.LoadSettings<SCursorSettings>();

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
                Sections = new() {
                    ["general"] = new("General", string.Empty)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["language"] = new SSelectorOption("Language", string.Empty, Array.ConvertAll<SGameCulture, object>(SLocalizationConstants.AVAILABLE_GAME_CULTURES, x => x.CultureInfo.NativeName.FirstCharToUpper())),
                        },
                    },

                    ["gameplay"] = new("Gameplay", string.Empty)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["preview_area_color"] = new SColorOption("Preview Area Color", string.Empty),
                            ["preview_area_opacity"] = new SSliderOption("Preview Area Opacity", string.Empty, new(0, 100)),
                        },
                    },

                    ["volume"] = new("Volume", string.Empty)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["master_volume"] = new SSliderOption("Master Volume", string.Empty, new(000, 100)),
                            ["music_volume"] = new SSliderOption("Music Volume", string.Empty, new(000, 100)),
                            ["sfx_volume"] = new SSliderOption("SFX Volume", string.Empty, new(000, 100))
                        }
                    },

                    ["video"] = new("Video", string.Empty)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["resolution"] = new SSelectorOption("Resolution", string.Empty, Array.ConvertAll<SSize2, object>(SScreenConstants.RESOLUTIONS, x => x)),
                            ["fullscreen"] = new SSelectorOption("Fullscreen", string.Empty, [SLocalization_Statements.False, SLocalization_Statements.True]),
                            ["vsync"] = new SSelectorOption("VSync", string.Empty, [SLocalization_Statements.False, SLocalization_Statements.True]),
                            ["borderless"] = new SSelectorOption("Borderless", string.Empty, [SLocalization_Statements.False, SLocalization_Statements.True]),
                        },
                    },

                    ["graphics"] = new("Graphics", string.Empty)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["lighting"] = new SSelectorOption("Lighting", string.Empty, [SLocalization_Statements.False, SLocalization_Statements.True]),
                        }
                    },

                    ["cursor"] = new("Cursor", string.Empty)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["color"] = new SColorOption("Color", string.Empty),
                            ["background_color"] = new SColorOption("Background Color", string.Empty),
                            ["opacity"] = new SSliderOption("Opacity", string.Empty, new(0, 100)),
                            ["scale"] = new SSelectorOption("Scale", string.Empty, ["Very Small", "Small", "Medium", "Large", "Very Large"]),
                        }
                    },
                },
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
