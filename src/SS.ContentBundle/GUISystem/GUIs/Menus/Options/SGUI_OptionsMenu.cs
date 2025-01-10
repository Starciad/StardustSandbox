using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Elements;
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

            this.plusIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_51");
            this.minusIconTexture = gameInstance.AssetDatabase.GetTexture("icon_gui_52");
            this.panelBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_13");
            this.colorButtonTexture = gameInstance.AssetDatabase.GetTexture("gui_button_4");
            this.bigApple3PMSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_2");
            this.digitalDiscoSpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont("font_8");

            this.tooltipBoxElement = tooltipBoxElement;

            this.systemButtons = [
                new(null, SLocalization_Statements.Return, "Closes the current menu and returns to the previous menu.", ReturnButtonAction),
                new(null, SLocalization_Statements.Save, "Saves and applies all modified options.", SaveButtonAction),
            ];

            this.root = new()
            {
                Sections = new Dictionary<string, SSection>()
                {
                    ["general"] = new("General", "Basic game settings, including language and accessibility.")
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["language"] = new SSelectorOption("Language", "Select the language for the game interface.",
                                Array.ConvertAll<SGameCulture, object>(SLocalizationConstants.AVAILABLE_GAME_CULTURES, x => x.Name)),
                        },
                    },

                    ["gameplay"] = new("Gameplay", "Adjust gameplay-related preferences.")
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["preview_area_color"] = new SColorOption("Preview Area Color", "Choose the color for the preview area displayed during gameplay."),
                            ["preview_area_opacity"] = new SValueOption("Preview Area Opacity", "Set the transparency level of the preview area.",
                                byte.MinValue, byte.MaxValue),
                        },
                    },

                    ["volume"] = new("Volume", "Control audio levels for different categories.")
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["master_volume"] = new SValueOption("Master Volume", "Adjust the overall game volume.", 0, 100),
                            ["music_volume"] = new SValueOption("Music Volume", "Set the volume for background music.", 0, 100),
                            ["sfx_volume"] = new SValueOption("SFX Volume", "Adjust the sound effects volume.", 0, 100),
                        }
                    },

                    ["video"] = new("Video", "Modify video-related settings, including resolution and display modes.")
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["resolution"] = new SSelectorOption("Resolution", "Select the screen resolution for the game.",
                                Array.ConvertAll<SSize2, object>(SScreenConstants.RESOLUTIONS, x => x)),
                            ["fullscreen"] = new SSelectorOption("Fullscreen", "Enable or disable fullscreen mode.", new object[] { false, true }),
                            ["vsync"] = new SSelectorOption("VSync", "Turn vertical synchronization on or off to avoid screen tearing.",
                                new object[] { false, true }),
                            ["borderless"] = new SSelectorOption("Borderless", "Enable borderless windowed mode.", new object[] { false, true }),
                        },
                    },

                    ["graphics"] = new("Graphics", "Configure graphical settings for visual quality.")
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["lighting"] = new SSelectorOption("Lighting", "Enable or disable advanced lighting effects.",
                                new object[] { false, true }),
                        }
                    },

                    ["cursor"] = new("Cursor", "Customize the appearance and behavior of the mouse cursor.")
                    {
                        Options = new Dictionary<string, SOption>()
                        {
                            ["color"] = new SColorOption("Color", "Set the color of the cursor."),
                            ["background_color"] = new SColorOption("Background Color", "Set the background color of the cursor."),
                            ["scale"] = new SSelectorOption("Scale", "Adjust the size of the cursor.",
                                new object[] { 0.5f, 1f, 1.5f, 2f, 2.5f, 3f }),
                            ["opacity"] = new SValueOption("Opacity", "Set the transparency level of the cursor.", byte.MinValue, byte.MaxValue),
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

                if (this.selectedSectionIdentififer.Equals(item.Key))
                {
                    labelElement.Color = SColorPalette.LemonYellow;
                }
                else if (onMouseOver)
                {
                    labelElement.Color = SColorPalette.LemonYellow;
                }
                else
                {
                    labelElement.Color = SColorPalette.White;
                }
            }
        }

        private void UpdateSystemButtons()
        {
            for (byte i = 0; i < this.systemButtons.Length; i++)
            {
                SGUILabelElement labelElement = this.systemButtonElements[i];
                SButton button =  this.systemButtons[i];

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
                    HandleOptionInteractivity(option, element);
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
        #endregion

        #region Handlers
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
            };

            this.guiColorPicker.Configure(this.colorPickerSettings);
            this.SGameInstance.GUIManager.OpenGUI(this.guiColorPicker.Identifier);
        }

        private static void HandleSelectorOption(SSelectorOption selectorOption, SGUIElement element)
        {
            selectorOption.Next();
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
