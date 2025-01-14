using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements;
using StardustSandbox.ContentBundle.GUISystem.Elements.Graphics;
using StardustSandbox.ContentBundle.GUISystem.Elements.Informational;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.GUISystem.Global;
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

        private string selectedSectionIdentififer;
        private bool restartMessageAppeared;

        private readonly SGeneralSettings generalSettings;
        private readonly SGameplaySettings gameplaySettings;
        private readonly SVolumeSettings volumeSettings;
        private readonly SVideoSettings videoSettings;
        private readonly SGraphicsSettings graphicsSettings;
        private readonly SCursorSettings cursorSettings;

        private readonly Texture2D toggleButtonTexture;
        private readonly Texture2D plusIconTexture;
        private readonly Texture2D minusIconTexture;
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

        private readonly SGUITooltipBoxElement tooltipBoxElement;

        internal SGUI_OptionsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents, SGUI_ColorPicker guiColorPicker, SGUI_Message guiMessage, SGUITooltipBoxElement tooltipBoxElement) : base(gameInstance, identifier, guiEvents)
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

            this.toggleButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_5");
            this.plusIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_51");
            this.minusIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_52");
            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_13");
            this.colorButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_4");
            this.bigApple3PMSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.digitalDiscoSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_8");

            this.tooltipBoxElement = tooltipBoxElement;

            this.systemButtons = [
                new(null, SLocalization_Statements.Return, SLocalization_GUIs.Button_Exit_Description, ReturnButtonAction),
                new(null, SLocalization_Statements.Save, SLocalization_GUIs.Menu_Options_Button_Save_Description, SaveButtonAction),
            ];

            this.root = new()
            {
                Sections = new Dictionary<string, SSection>()
                {
                    ["general"] = new(SLocalization_GUIs.Menu_Options_Section_General_Name, SLocalization_GUIs.Menu_Options_Section_General_Description)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["language"] = new SSelectorOption(SLocalization_GUIs.Menu_Options_Section_General_Option_Language_Name, SLocalization_GUIs.Menu_Options_Section_General_Option_Language_Description, Array.ConvertAll<SGameCulture, object>(SLocalizationConstants.AVAILABLE_GAME_CULTURES, x => x.Name)),
                        },
                    },

                    ["gameplay"] = new(SLocalization_GUIs.Menu_Options_Section_Gameplay_Name, SLocalization_GUIs.Menu_Options_Section_Gameplay_Description)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["preview_area_color"] = new SColorOption(SLocalization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaColor_Name, SLocalization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaColor_Description),
                            ["preview_area_opacity"] = new SValueOption(SLocalization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaOpacity_Name, SLocalization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaOpacity_Description, byte.MinValue, byte.MaxValue),
                        },
                    },

                    ["volume"] = new(SLocalization_GUIs.Menu_Options_Section_Volume_Name, SLocalization_GUIs.Menu_Options_Section_Volume_Description)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["master_volume"] = new SValueOption(SLocalization_GUIs.Menu_Options_Section_Volume_Option_MasterVolume_Name, SLocalization_GUIs.Menu_Options_Section_Volume_Option_MasterVolume_Description, 0, 100),
                            ["music_volume"] = new SValueOption(SLocalization_GUIs.Menu_Options_Section_Volume_Option_MusicVolume_Name, SLocalization_GUIs.Menu_Options_Section_Volume_Option_MusicVolume_Description, 0, 100),
                            ["sfx_volume"] = new SValueOption(SLocalization_GUIs.Menu_Options_Section_Volume_Option_SFXVolume_Name, SLocalization_GUIs.Menu_Options_Section_Volume_Option_SFXVolume_Description, 0, 100),
                        }
                    },

                    ["video"] = new(SLocalization_GUIs.Menu_Options_Section_Video_Name, SLocalization_GUIs.Menu_Options_Section_Video_Description)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["resolution"] = new SSelectorOption(SLocalization_GUIs.Menu_Options_Section_Video_Option_Resolution_Name, SLocalization_GUIs.Menu_Options_Section_Video_Option_Resolution_Description, Array.ConvertAll<SSize2, object>(SScreenConstants.RESOLUTIONS, x => x)),
                            ["fullscreen"] = new SToggleOption(SLocalization_GUIs.Menu_Options_Section_Video_Option_Fullscreen_Name, SLocalization_GUIs.Menu_Options_Section_Video_Option_Fullscreen_Description),
                            ["vsync"] = new SToggleOption(SLocalization_GUIs.Menu_Options_Section_Video_Option_VSync_Name, SLocalization_GUIs.Menu_Options_Section_Video_Option_VSync_Description),
                            ["borderless"] = new SToggleOption(SLocalization_GUIs.Menu_Options_Section_Video_Option_Borderless_Name, SLocalization_GUIs.Menu_Options_Section_Video_Option_Borderless_Description),
                        },
                    },

                    // ["graphics"] = new(SLocalization_GUIs.Menu_Options_Section_Graphics_Name, SLocalization_GUIs.Menu_Options_Section_Graphics_Description)
                    // {
                    //     Options = new Dictionary<string, SOption>()
                    //     {
                    //         ["lighting"] = new SToggleOption(SLocalization_GUIs.Menu_Options_Section_Graphics_Option_Lighting_Name, SLocalization_GUIs.Menu_Options_Section_Graphics_Option_Lighting_Description),
                    //     }
                    // },

                    ["cursor"] = new(SLocalization_GUIs.Menu_Options_Section_Cursor_Name, SLocalization_GUIs.Menu_Options_Section_Cursor_Description)
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["color"] = new SColorOption(SLocalization_GUIs.Menu_Options_Section_Cursor_Option_Color_Name, SLocalization_GUIs.Menu_Options_Section_Cursor_Option_Color_Description),
                            ["background_color"] = new SColorOption(SLocalization_GUIs.Menu_Options_Section_Cursor_Option_BackgroundColor_Name, SLocalization_GUIs.Menu_Options_Section_Cursor_Option_BackgroundColor_Description),
                            ["scale"] = new SSelectorOption(SLocalization_GUIs.Menu_Options_Section_Cursor_Option_Scale_Name, SLocalization_GUIs.Menu_Options_Section_Cursor_Option_Scale_Description, [0.5f, 1f, 1.5f, 2f, 2.5f, 3f]),
                            ["opacity"] = new SValueOption(SLocalization_GUIs.Menu_Options_Section_Cursor_Option_Opacity_Name, SLocalization_GUIs.Menu_Options_Section_Cursor_Option_Opacity_Description, byte.MinValue, byte.MaxValue),
                        }
                    },
                },
            };

            this.systemButtonElements = new SGUILabelElement[this.systemButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.IsVisible = false;

            UpdateSectionButtons();
            UpdateSystemButtons();
            UpdateSectionOptions();

            this.tooltipBoxElement.RefreshDisplay(SGUIGlobalTooltip.Title, SGUIGlobalTooltip.Description);
        }

        private void UpdateSectionButtons()
        {
            foreach (KeyValuePair<string, SGUILabelElement> item in this.sectionButtonElements)
            {
                SGUILabelElement labelElement = item.Value;

                SSize2 size = labelElement.GetStringSize() / 2f;
                Vector2 position = labelElement.Position;

                bool onMouseOver = this.GUIEvents.OnMouseOver(position, size);

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    SelectSection(item.Key);
                }

                if (onMouseOver)
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SSection section = this.root.Sections[item.Key];

                    SGUIGlobalTooltip.Title = section.Name;
                    SGUIGlobalTooltip.Description = section.Description;
                }

                labelElement.Color = this.selectedSectionIdentififer.Equals(item.Key)
                    ? SColorPalette.LemonYellow
                    : onMouseOver ? SColorPalette.LemonYellow : SColorPalette.White;
            }
        }

        private void UpdateSystemButtons()
        {
            for (byte i = 0; i < this.systemButtons.Length; i++)
            {
                SGUILabelElement labelElement = this.systemButtonElements[i];
                SButton button = this.systemButtons[i];

                SSize2 size = labelElement.GetStringSize() / 2f;
                Vector2 position = labelElement.Position;

                if (this.GUIEvents.OnMouseClick(position, size))
                {
                    button.ClickAction?.Invoke();
                }

                if (this.GUIEvents.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = button.Name;
                    SGUIGlobalTooltip.Description = button.Description;

                    labelElement.Color = SColorPalette.LemonYellow;
                }
                else
                {
                    labelElement.Color = SColorPalette.White;
                }
            }
        }

        private void UpdateSectionOptions()
        {
            SSize2 interactiveAreaSize = new(295, 18);
            SSize2 plusAndMinusButtonAreaSize = new(32);

            foreach (SGUIElement element in this.sectionContents[this.selectedSectionIdentififer])
            {
                Vector2 position = new(element.Position.X + interactiveAreaSize.Width, element.Position.Y - 6);
                SOption option = (SOption)element.GetData("option");

                if (this.GUIEvents.OnMouseClick(position, interactiveAreaSize))
                {
                    HandleOptionInteractivity(option);
                }

                UpdateOptionSync(option, element);

                if (this.GUIEvents.OnMouseOver(position, interactiveAreaSize))
                {
                    element.Color = SColorPalette.LemonYellow;

                    this.tooltipBoxElement.IsVisible = true;

                    SGUIGlobalTooltip.Title = option.Name;
                    SGUIGlobalTooltip.Description = option.Description;
                }
                else
                {
                    element.Color = SColorPalette.White;
                }
            }

            foreach ((SGUIElement plusElement, SGUIElement minusElement) in this.plusAndMinusButtons)
            {
                if (this.GUIEvents.OnMouseDown(plusElement.Position, plusAndMinusButtonAreaSize))
                {
                    ((SValueOption)plusElement.GetData("option")).Increment();
                }
                else if (this.GUIEvents.OnMouseDown(minusElement.Position, plusAndMinusButtonAreaSize))
                {
                    ((SValueOption)minusElement.GetData("option")).Decrement();
                }
            }
        }

        #region Sync
        private static void UpdateOptionSync(SOption option, SGUIElement element)
        {
            switch (option)
            {
                case SColorOption colorOption:
                    UpdateColorOption(colorOption, element.GetData("color_slot") as SColorSlot);
                    break;

                case SSelectorOption selectorOption:
                    UpdateSelectorOption(selectorOption, element as SGUILabelElement);
                    break;

                case SValueOption valueOption:
                    UpdateValueOption(valueOption, element as SGUILabelElement);
                    break;

                case SToggleOption toggleOption:
                    UpdateToggleOption(toggleOption, element.GetData("toogle_preview") as SGUIImageElement);
                    break;

                default:
                    break;
            }
        }

        private static void UpdateColorOption(SColorOption colorOption, SColorSlot colorSlot)
        {
            colorSlot.BackgroundElement.Color = colorOption.CurrentColor;
        }

        private static void UpdateSelectorOption(SSelectorOption selectorOption, SGUILabelElement labelElement)
        {
            labelElement.SetTextualContent(string.Concat(selectorOption.Name, ": ", selectorOption.GetValue()));
        }

        private static void UpdateValueOption(SValueOption valueOption, SGUILabelElement labelElement)
        {
            labelElement.SetTextualContent(string.Concat(valueOption.Name, ": ", valueOption.CurrentValue.ToString($"D{valueOption.MaximumValue.ToString().Length}")));
            SSize2 labelElementSize = labelElement.GetStringSize();

            SGUIElement plusElement = (SGUIElement)labelElement.GetData("plus_element");
            SGUIElement minusElement = (SGUIElement)labelElement.GetData("minus_element");

            minusElement.Margin = new(labelElementSize.Width + 8f, labelElementSize.Height / 2 * -1);

            minusElement.PositionRelativeToElement(labelElement);
            plusElement.PositionRelativeToElement(minusElement);
        }

        private static void UpdateToggleOption(SToggleOption toggleOption, SGUIImageElement toggleStateElement)
        {
            toggleStateElement.TextureClipArea = toggleOption.State ? new(new(0, 32), new(32)) : new(new(0), new(32));
        }
        #endregion

        #region Handlers
        private void HandleOptionInteractivity(SOption option)
        {
            switch (option)
            {
                case SButtonOption buttonOption:
                    HandleButtonOption(buttonOption);
                    break;

                case SColorOption colorOption:
                    HandleColorOption(colorOption);
                    break;

                case SSelectorOption selectorOption:
                    HandleSelectorOption(selectorOption);
                    break;

                case SToggleOption toggleOption:
                    HandleToggleOption(toggleOption);
                    break;

                default:
                    break;
            }
        }

        private static void HandleButtonOption(SButtonOption buttonOption)
        {
            buttonOption.OnClickCallback?.Invoke();
        }

        private void HandleColorOption(SColorOption colorOption)
        {
            this.colorPickerSettings.OnSelectCallback = (SColorPickerResult result) =>
            {
                colorOption.SetValue(result.SelectedColor);
            };

            this.guiColorPicker.Configure(this.colorPickerSettings);
            this.SGameInstance.GUIManager.OpenGUI(this.guiColorPicker.Identifier);
        }

        private static void HandleSelectorOption(SSelectorOption selectorOption)
        {
            selectorOption.Next();
        }

        private static void HandleToggleOption(SToggleOption toggleOption)
        {
            toggleOption.Toggle();
        }
        #endregion

        // ========================================================= //

        private void SelectSection(string identififer)
        {
            this.selectedSectionIdentififer = identififer;

            foreach (KeyValuePair<string, SGUIContainerElement> item in this.sectionContainerElements)
            {
                if (item.Key.Equals(identififer))
                {
                    item.Value.Active();
                    continue;
                }

                item.Value.Disable();
            }
        }
    }
}
