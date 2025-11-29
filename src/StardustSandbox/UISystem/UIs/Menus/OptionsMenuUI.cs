using Microsoft.Xna.Framework;

using StardustSandbox.AudioSystem;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UISystem;
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Settings;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UISystem.Elements;
using StardustSandbox.UISystem.Elements.Graphics;
using StardustSandbox.UISystem.Elements.Textual;
using StardustSandbox.UISystem.Options;
using StardustSandbox.UISystem.Settings;
using StardustSandbox.UISystem.UIs.Tools;
using StardustSandbox.UISystem.Utilities;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UISystem.UIs.Menus
{
    internal sealed class OptionsMenuUI : UI
    {
        private enum SystemButton : byte
        {
            Return = 0,
            Save = 1
        }

        private sealed class Root
        {
            internal IReadOnlyDictionary<string, Section> Sections { get; init; }
        }

        private sealed class Section(string name, string description)
        {
            internal string Name => name;
            internal string Description => description;
            internal IReadOnlyDictionary<string, Option> Options { get; init; }
        }

        private string selectedSectionIdentififer;
        private bool restartMessageAppeared;

        private LabelUIElement titleLabelElement;
        private ImageUIElement panelBackgroundElement;

        private readonly Root root;
        private readonly ColorPickerSettings colorPickerSettings;

        private readonly ColorPickerUI colorPickerUI;
        private readonly MessageUI messageUI;

        private readonly string titleName = Localization_GUIs.Menu_Options_Title;
        private readonly List<(UIElement, UIElement)> plusAndMinusButtons = [];

        private readonly TooltipBox tooltipBoxElement;

        private readonly LabelUIElement[] systemButtonElements;
        private readonly Dictionary<string, IEnumerable<LabelUIElement>> sectionContents = [];
        private readonly Dictionary<string, ContainerUIElement> sectionContainerElements = [];
        private readonly Dictionary<string, LabelUIElement> sectionButtonElements = [];

        private readonly UIButton[] systemButtons;

        private readonly CursorManager cursorManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;

        private readonly GeneralSettings generalSettings;
        private readonly GameplaySettings gameplaySettings;
        private readonly VolumeSettings volumeSettings;
        private readonly VideoSettings videoSettings;
        private readonly CursorSettings cursorSettings;

        private static readonly Vector2 defaultRightPanelMargin = new(-112.0f, 64.0f);
        private static readonly Vector2 defaultButtonScale = new(0.11f);
        private static readonly Vector2 defaultButtonBorderOffset = new(2.0f);
        private static readonly float leftPanelMarginVerticalSpacing = 48.0f;
        private static readonly float rightPanelMarginVerticalSpacing = 48.0f;

        internal OptionsMenuUI(
            ColorPickerUI colorPickerUI,
            CursorManager cursorManager,
            UIIndex index,
            MessageUI messageUI,
            TooltipBox tooltipBoxElement,
            UIManager uiManager,
            VideoManager videoManager
        ) : base(index)
        {
            this.colorPickerUI = colorPickerUI;
            this.cursorManager = cursorManager;
            this.messageUI = messageUI;
            this.tooltipBoxElement = tooltipBoxElement;
            this.uiManager = uiManager;
            this.videoManager = videoManager;

            this.colorPickerSettings = new();

            this.generalSettings = SettingsHandler.LoadSettings<GeneralSettings>();
            this.gameplaySettings = SettingsHandler.LoadSettings<GameplaySettings>();
            this.volumeSettings = SettingsHandler.LoadSettings<VolumeSettings>();
            this.videoSettings = SettingsHandler.LoadSettings<VideoSettings>();
            this.cursorSettings = SettingsHandler.LoadSettings<CursorSettings>();

            this.systemButtons = [
                new(null, null, Localization_Statements.Return, Localization_GUIs.Button_Exit_Description, ReturnButtonAction),
                new(null, null, Localization_Statements.Save, Localization_GUIs.Menu_Options_Button_Save_Description, SaveButtonAction),
            ];

            this.root = new()
            {
                Sections = new Dictionary<string, Section>()
                {
                    ["general"] = new(Localization_GUIs.Menu_Options_Section_General_Name, Localization_GUIs.Menu_Options_Section_General_Description)
                    {
                        Options = new Dictionary<string, Option>()
                        {
                            ["language"] = new SelectorOption(Localization_GUIs.Menu_Options_Section_General_Option_Language_Name, Localization_GUIs.Menu_Options_Section_General_Option_Language_Description, Array.ConvertAll<GameCulture, object>(LocalizationConstants.AVAILABLE_GAME_CULTURES, x => x.Name)),
                        },
                    },

                    ["gameplay"] = new(Localization_GUIs.Menu_Options_Section_Gameplay_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Description)
                    {
                        Options = new Dictionary<string, Option>()
                        {
                            ["preview_area_color"] = new ColorOption(Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaColor_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaColor_Description),
                            ["preview_area_opacity"] = new ValueOption(Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaOpacity_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaOpacity_Description, byte.MinValue, byte.MaxValue),
                        },
                    },

                    ["volume"] = new(Localization_GUIs.Menu_Options_Section_Volume_Name, Localization_GUIs.Menu_Options_Section_Volume_Description)
                    {
                        Options = new Dictionary<string, Option>()
                        {
                            ["master_volume"] = new ValueOption(Localization_GUIs.Menu_Options_Section_Volume_Option_MasterVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_MasterVolume_Description, 0, 100),
                            ["music_volume"] = new ValueOption(Localization_GUIs.Menu_Options_Section_Volume_Option_MusicVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_MusicVolume_Description, 0, 100),
                            ["sfx_volume"] = new ValueOption(Localization_GUIs.Menu_Options_Section_Volume_Option_SFXVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_SFXVolume_Description, 0, 100),
                        }
                    },

                    ["video"] = new(Localization_GUIs.Menu_Options_Section_Video_Name, Localization_GUIs.Menu_Options_Section_Video_Description)
                    {
                        Options = new Dictionary<string, Option>()
                        {
                            ["framerate"] = new SelectorOption("Framerate", "Description", Array.ConvertAll<double, object>(ScreenConstants.FRAMERATES, x => x)),
                            ["resolution"] = new SelectorOption(Localization_GUIs.Menu_Options_Section_Video_Option_Resolution_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Resolution_Description, Array.ConvertAll<Point, object>(ScreenConstants.RESOLUTIONS, x => x)),
                            ["fullscreen"] = new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_Fullscreen_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Fullscreen_Description),
                            ["vsync"] = new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_VSync_Name, Localization_GUIs.Menu_Options_Section_Video_Option_VSync_Description),
                            ["borderless"] = new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_Borderless_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Borderless_Description),
                        },
                    },

                    ["cursor"] = new(Localization_GUIs.Menu_Options_Section_Cursor_Name, Localization_GUIs.Menu_Options_Section_Cursor_Description)
                    {
                        Options = new Dictionary<string, Option>()
                        {
                            ["color"] = new ColorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Color_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Color_Description),
                            ["background_color"] = new ColorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_BackgroundColor_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_BackgroundColor_Description),
                            ["scale"] = new SelectorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Scale_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Scale_Description, [0.5f, 1f, 1.5f, 2f, 2.5f, 3f]),
                            ["opacity"] = new ValueOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Opacity_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Opacity_Description, byte.MinValue, byte.MaxValue),
                        }
                    },
                },
            };

            this.systemButtonElements = new LabelUIElement[this.systemButtons.Length];
        }

        #region ACTIONS

        // ================================== //
        // Button Methods

        private void SaveButtonAction()
        {
            SaveSettings();
            ApplySettings();

            if (!this.restartMessageAppeared)
            {
                this.messageUI.SetContent(Localization_Messages.Settings_RestartRequired);
                this.uiManager.OpenGUI(UIIndex.Message);

                this.restartMessageAppeared = true;
            }
        }

        #region SETTINGS HANDLING

        #region SAVE SETTINGS

        private void SaveSettings()
        {
            SaveGeneralSettings();
            SaveGameplaySettings();
            SaveVolumeSettings();
            SaveVideoSettings();
            SaveCursorSettings();
        }

        private void SaveGeneralSettings()
        {
            Section generalSection = this.root.Sections["general"];
            GameCulture gameCulture = LocalizationConstants.GetGameCulture(Convert.ToString(generalSection.Options["language"].GetValue()));

            this.generalSettings.Language = gameCulture.Language;
            this.generalSettings.Region = gameCulture.Region;

            SettingsHandler.SaveSettings(this.generalSettings);
        }

        private void SaveGameplaySettings()
        {
            Section gameplaySection = this.root.Sections["gameplay"];

            this.gameplaySettings.PreviewAreaColor = (Color)gameplaySection.Options["preview_area_color"].GetValue();
            this.gameplaySettings.PreviewAreaColorA = Convert.ToByte(gameplaySection.Options["preview_area_opacity"].GetValue());

            SettingsHandler.SaveSettings(this.gameplaySettings);
        }

        private void SaveVolumeSettings()
        {
            Section volumeSection = this.root.Sections["volume"];

            this.volumeSettings.MasterVolume = Convert.ToSingle(volumeSection.Options["master_volume"].GetValue()) / 100.0f;
            this.volumeSettings.MusicVolume = Convert.ToSingle(volumeSection.Options["music_volume"].GetValue()) / 100.0f;
            this.volumeSettings.SFXVolume = Convert.ToSingle(volumeSection.Options["sfx_volume"].GetValue()) / 100.0f;

            SettingsHandler.SaveSettings(this.volumeSettings);
        }

        private void SaveVideoSettings()
        {
            Section videoSection = this.root.Sections["video"];

            this.videoSettings.Framerate = (double)videoSection.Options["framerate"].GetValue();
            this.videoSettings.Resolution = (Point)videoSection.Options["resolution"].GetValue();
            this.videoSettings.FullScreen = Convert.ToBoolean(videoSection.Options["fullscreen"].GetValue());
            this.videoSettings.VSync = Convert.ToBoolean(videoSection.Options["vsync"].GetValue());
            this.videoSettings.Borderless = Convert.ToBoolean(videoSection.Options["borderless"].GetValue());

            SettingsHandler.SaveSettings(this.videoSettings);
        }

        private void SaveCursorSettings()
        {
            Section cursorSettings = this.root.Sections["cursor"];

            this.cursorSettings.Color = (Color)cursorSettings.Options["color"].GetValue();
            this.cursorSettings.BackgroundColor = (Color)cursorSettings.Options["background_color"].GetValue();
            this.cursorSettings.Alpha = Convert.ToByte(cursorSettings.Options["opacity"].GetValue());
            this.cursorSettings.Scale = Convert.ToSingle(cursorSettings.Options["scale"].GetValue());

            SettingsHandler.SaveSettings(this.cursorSettings);
        }

        #endregion

        #region SYNC SETTINGS

        private void SyncSettingElements()
        {
            SyncGeneralSettings();
            SyncGameplaySettings();
            SyncVolumeSettings();
            SyncVideoSettings();
            SyncCursorSettings();
        }

        private void SyncGeneralSettings()
        {
            Section generalSection = this.root.Sections["general"];

            generalSection.Options["language"].SetValue(this.generalSettings.GameCulture.Name);
        }

        private void SyncGameplaySettings()
        {
            Section gameplaySection = this.root.Sections["gameplay"];

            gameplaySection.Options["preview_area_color"].SetValue(new Color(this.gameplaySettings.PreviewAreaColor, 255));
            gameplaySection.Options["preview_area_opacity"].SetValue(this.gameplaySettings.PreviewAreaColorA);
        }

        private void SyncVolumeSettings()
        {
            Section volumeSection = this.root.Sections["volume"];

            volumeSection.Options["master_volume"].SetValue(this.volumeSettings.MasterVolume * 100.0f);
            volumeSection.Options["music_volume"].SetValue(this.volumeSettings.MusicVolume * 100.0f);
            volumeSection.Options["sfx_volume"].SetValue(this.volumeSettings.SFXVolume * 100.0f);
        }

        private void SyncVideoSettings()
        {
            Section videoSection = this.root.Sections["video"];

            videoSection.Options["framerate"].SetValue(this.videoSettings.Framerate);
            videoSection.Options["resolution"].SetValue(this.videoSettings.Resolution);
            videoSection.Options["fullscreen"].SetValue(this.videoSettings.FullScreen);
            videoSection.Options["vsync"].SetValue(this.videoSettings.VSync);
            videoSection.Options["borderless"].SetValue(this.videoSettings.Borderless);
        }

        private void SyncCursorSettings()
        {
            Section cursorSettings = this.root.Sections["cursor"];

            cursorSettings.Options["color"].SetValue(new Color(this.cursorSettings.Color, 255));
            cursorSettings.Options["background_color"].SetValue(new Color(this.cursorSettings.BackgroundColor, 255));
            cursorSettings.Options["opacity"].SetValue(this.cursorSettings.Alpha);
            cursorSettings.Options["scale"].SetValue(this.cursorSettings.Scale);
        }

        #endregion

        #endregion

        #region Apply Settings
        private void ApplySettings()
        {
            ApplyVolumeSettings();
            ApplyVideoSettings();
            ApplyCursorSettings();
        }

        private void ApplyVolumeSettings()
        {
            SongEngine.Volume = this.volumeSettings.MusicVolume * this.volumeSettings.MasterVolume;
            // SoundEngine.Volume = this.volumeSettings.SFXVolume * this.volumeSettings.MasterVolume;
        }

        private void ApplyVideoSettings()
        {
            this.videoManager.ApplySettings();
        }

        private void ApplyCursorSettings()
        {
            this.cursorManager.ApplySettings();
        }

        #endregion

        private void ReturnButtonAction()
        {
            this.uiManager.CloseGUI();
        }

        #endregion

        #region BUILDER

        protected override void OnBuild(Layout layout)
        {
            // Decorations
            BuildPanelBackground(layout);
            BuildTitle(layout);

            // Buttons
            BuildSectionButtons(layout);
            BuildSystemButtons(layout);

            // Sections
            BuildSections(layout);

            // Final
            layout.AddElement(this.tooltipBoxElement);
            SelectSection("general");
        }

        private void BuildPanelBackground(Layout layout)
        {
            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiBackgroundOptions),
                Size = new(1084, 540),
                Margin = new(98, 90),
            };

            this.panelBackgroundElement.RepositionRelativeToScreen();

            layout.AddElement(this.panelBackgroundElement);
        }

        private void BuildTitle(Layout layout)
        {
            this.titleLabelElement = new()
            {
                Scale = new(0.15f),
                Margin = new(0f, 52.5f),
                Color = AAP64ColorPalette.White,
                Alignment = CardinalDirection.North,
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
            };

            this.titleLabelElement.SetTextualContent(this.titleName);
            this.titleLabelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(4.4f));
            this.titleLabelElement.RepositionRelativeToScreen();

            layout.AddElement(this.titleLabelElement);
        }

        private void BuildSectionButtons(Layout layout)
        {
            // BUTTONS
            Vector2 margin = new(-335f, 64f);

            // Labels
            foreach (KeyValuePair<string, Section> item in this.root.Sections)
            {
                LabelUIElement labelElement = CreateButtonLabelElement();

                labelElement.Alignment = CardinalDirection.North;
                labelElement.Margin = margin;
                labelElement.SetTextualContent(item.Value.Name);
                labelElement.RepositionRelativeToElement(this.panelBackgroundElement);

                this.sectionButtonElements.Add(item.Key, labelElement);
                margin.Y += leftPanelMarginVerticalSpacing;

                layout.AddElement(labelElement);
            }
        }

        private void BuildSystemButtons(Layout layout)
        {
            Vector2 margin = new(-335f, -64f);

            for (int i = 0; i < this.systemButtons.Length; i++)
            {
                LabelUIElement labelElement = CreateButtonLabelElement();

                labelElement.Alignment = CardinalDirection.South;
                labelElement.Margin = margin;
                labelElement.SetTextualContent(this.systemButtons[i].Name);
                labelElement.RepositionRelativeToElement(this.panelBackgroundElement);

                this.systemButtonElements[i] = labelElement;
                margin.Y -= leftPanelMarginVerticalSpacing;

                layout.AddElement(labelElement);
            }
        }

        // ============================================================================ //

        private void BuildSections(Layout layout)
        {
            foreach (KeyValuePair<string, Section> item in this.root.Sections)
            {
                List<LabelUIElement> contentBuffer = [];
                ContainerUIElement containerElement = new();

                Vector2 margin = defaultRightPanelMargin;

                foreach (Option option in item.Value.Options.Values)
                {
                    LabelUIElement labelElement = CreateOptionElement(option);

                    labelElement.Margin = margin;
                    labelElement.RepositionRelativeToElement(this.panelBackgroundElement);

                    switch (option)
                    {
                        case ColorOption:
                            BuildColorPreview(containerElement, labelElement);
                            break;

                        case ValueOption:
                            BuildValueControls(option, containerElement, labelElement);
                            break;

                        case ToggleOption:
                            BuildTogglePreview(containerElement, labelElement);
                            break;

                        default:
                            break;
                    }

                    containerElement.AddElement(labelElement);
                    margin.Y += rightPanelMarginVerticalSpacing;

                    contentBuffer.Add(labelElement);
                }

                this.sectionContainerElements.Add(item.Key, containerElement);
                layout.AddElement(containerElement);

                this.sectionContents.Add(item.Key, contentBuffer);
            }
        }

        private static void BuildColorPreview(ContainerUIElement containerElement, LabelUIElement labelElement)
        {
            Vector2 labelElementSize = labelElement.GetStringSize();

            UIColorSlot colorSlot = new(
                new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                    TextureRectangle = new(386, 0, 40, 22),
                    Scale = new(1.5f),
                    Size = new(40.0f, 22.0f),
                    Margin = new(labelElementSize.X + 6f, labelElementSize.Y / 2 * -1),
                },

                new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                    TextureRectangle = new(386, 22, 40, 22),
                    Scale = new(1.5f),
                    Size = new(40.0f, 22.0f),
                }
            );

            colorSlot.BackgroundElement.RepositionRelativeToElement(labelElement);
            colorSlot.BorderElement.RepositionRelativeToElement(colorSlot.BackgroundElement);

            containerElement.AddElement(colorSlot.BackgroundElement);
            containerElement.AddElement(colorSlot.BorderElement);

            labelElement.AddData("color_slot", colorSlot);
        }

        private void BuildValueControls(Option option, ContainerUIElement containerElement, LabelUIElement labelElement)
        {
            Vector2 labelElementSize = labelElement.GetStringSize();

            ImageUIElement minusElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUi),
                TextureRectangle = new(192, 160, 32, 32),
                Size = new(32.0f),
                Margin = new(0, labelElementSize.Y / 2 * -1)
            };

            ImageUIElement plusElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUi),
                TextureRectangle = new(160, 160, 32, 32),
                Size = new(32.0f),
                Margin = new(48.0f, 0.0f),
            };

            plusElement.AddData("option", option);
            minusElement.AddData("option", option);

            labelElement.AddData("plus_element", plusElement);
            labelElement.AddData("minus_element", minusElement);

            minusElement.RepositionRelativeToElement(labelElement);
            plusElement.RepositionRelativeToElement(minusElement);

            containerElement.AddElement(plusElement);
            containerElement.AddElement(minusElement);

            this.plusAndMinusButtons.Add((plusElement, minusElement));
        }

        private static void BuildTogglePreview(ContainerUIElement containerElement, LabelUIElement labelElement)
        {
            Vector2 labelElementSize = labelElement.GetStringSize();

            ImageUIElement togglePreviewImageElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GuiButtons),
                TextureRectangle = new(352, 140, 32, 32),
                Scale = new(1.25f),
                Size = new(32.0f),
                Margin = new(labelElementSize.X + 6.0f, labelElementSize.Y / 2.0f * -1.0f),
            };

            togglePreviewImageElement.RepositionRelativeToElement(labelElement);

            containerElement.AddElement(togglePreviewImageElement);

            labelElement.AddData("toogle_preview", togglePreviewImageElement);
        }
        // ============================================================================ //

        private static LabelUIElement CreateButtonLabelElement()
        {
            LabelUIElement labelElement = new()
            {
                Scale = defaultButtonScale,
                Color = AAP64ColorPalette.White,
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
            };

            labelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, defaultButtonBorderOffset);

            return labelElement;
        }

        private static LabelUIElement CreateOptionButtonLabelElement()
        {
            LabelUIElement labelElement = new()
            {
                Scale = new(0.12f),
                Color = AAP64ColorPalette.White,
                SpriteFont = AssetDatabase.GetSpriteFont(SpriteFontIndex.DigitalDisco),
                Alignment = CardinalDirection.North,
            };

            labelElement.SetAllBorders(true, AAP64ColorPalette.DarkGray, new(2f));

            return labelElement;
        }

        // ============================================================================ //

        private static LabelUIElement CreateOptionElement(Option option)
        {
            LabelUIElement labelElement = option switch
            {
                ButtonOption => CreateButtonOptionElement(option),
                SelectorOption => CreateSelectorOptionElement(option),
                ValueOption => CreateValueOptionElement(option),
                ColorOption => CreateColorOptionElement(option),
                ToggleOption => CreateToggleOptionElement(option),
                _ => null,
            };

            labelElement.AddData("option", option);

            return labelElement;
        }

        private static LabelUIElement CreateButtonOptionElement(Option option)
        {
            LabelUIElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(option.Name);
            return labelElement;
        }

        private static LabelUIElement CreateSelectorOptionElement(Option option)
        {
            LabelUIElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(string.Concat(option.Name, ": ", option.GetValue()));
            return labelElement;
        }

        private static LabelUIElement CreateValueOptionElement(Option option)
        {
            LabelUIElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(string.Concat(option.Name, ": ", option.GetValue()));
            return labelElement;
        }

        private static LabelUIElement CreateColorOptionElement(Option option)
        {
            LabelUIElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(string.Concat(option.Name, ": "));
            return labelElement;
        }

        private static LabelUIElement CreateToggleOptionElement(Option option)
        {
            LabelUIElement labelElement = CreateOptionButtonLabelElement();
            labelElement.SetTextualContent(string.Concat(option.Name, ": "));
            return labelElement;
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBoxElement.CanDraw = false;

            UpdateSectionButtons();
            UpdateSystemButtons();
            UpdateSectionOptions();

            this.tooltipBoxElement.RefreshDisplay(TooltipContent.Title, TooltipContent.Description);
        }

        private void UpdateSectionButtons()
        {
            foreach (KeyValuePair<string, LabelUIElement> item in this.sectionButtonElements)
            {
                LabelUIElement labelElement = item.Value;

                Vector2 size = labelElement.GetStringSize() / 2.0f;
                Vector2 position = labelElement.Position;

                bool onMouseOver = Interaction.OnMouseOver(position, size);

                if (Interaction.OnMouseClick(position, size))
                {
                    SelectSection(item.Key);
                }

                if (onMouseOver)
                {
                    this.tooltipBoxElement.CanDraw = true;

                    Section section = this.root.Sections[item.Key];

                    TooltipContent.Title = section.Name;
                    TooltipContent.Description = section.Description;
                }

                labelElement.Color = this.selectedSectionIdentififer.Equals(item.Key)
                    ? AAP64ColorPalette.LemonYellow
                    : onMouseOver ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        private void UpdateSystemButtons()
        {
            for (byte i = 0; i < this.systemButtons.Length; i++)
            {
                LabelUIElement labelElement = this.systemButtonElements[i];
                UIButton button = this.systemButtons[i];

                Vector2 size = labelElement.GetStringSize() / 2.0f;
                Vector2 position = labelElement.Position;

                if (Interaction.OnMouseClick(position, size))
                {
                    button.ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(position, size))
                {
                    this.tooltipBoxElement.CanDraw = true;

                    TooltipContent.Title = button.Name;
                    TooltipContent.Description = button.Description;

                    labelElement.Color = AAP64ColorPalette.LemonYellow;
                }
                else
                {
                    labelElement.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateSectionOptions()
        {
            Vector2 interactiveAreaSize = new(295.0f, 18.0f);
            Vector2 plusAndMinusButtonAreaSize = new(32.0f);

            foreach (UIElement element in this.sectionContents[this.selectedSectionIdentififer])
            {
                Vector2 position = new(element.Position.X + interactiveAreaSize.X, element.Position.Y - 6);
                Option option = (Option)element.GetData("option");

                if (Interaction.OnMouseClick(position, interactiveAreaSize))
                {
                    HandleOptionInteractivity(option);
                }

                UpdateOptionSync(option, element);

                if (Interaction.OnMouseOver(position, interactiveAreaSize))
                {
                    element.Color = AAP64ColorPalette.LemonYellow;

                    this.tooltipBoxElement.CanDraw = true;

                    TooltipContent.Title = option.Name;
                    TooltipContent.Description = option.Description;
                }
                else
                {
                    element.Color = AAP64ColorPalette.White;
                }
            }

            foreach ((UIElement plusElement, UIElement minusElement) in this.plusAndMinusButtons)
            {
                if (Interaction.OnMouseDown(plusElement.Position, plusAndMinusButtonAreaSize))
                {
                    ((ValueOption)plusElement.GetData("option")).Increment();
                }
                else if (Interaction.OnMouseDown(minusElement.Position, plusAndMinusButtonAreaSize))
                {
                    ((ValueOption)minusElement.GetData("option")).Decrement();
                }
            }
        }

        #region Sync
        private static void UpdateOptionSync(Option option, UIElement element)
        {
            switch (option)
            {
                case ColorOption colorOption:
                    UpdateColorOption(colorOption, element.GetData("color_slot") as UIColorSlot);
                    break;

                case SelectorOption selectorOption:
                    UpdateSelectorOption(selectorOption, element as LabelUIElement);
                    break;

                case ValueOption valueOption:
                    UpdateValueOption(valueOption, element as LabelUIElement);
                    break;

                case ToggleOption toggleOption:
                    UpdateToggleOption(toggleOption, element.GetData("toogle_preview") as ImageUIElement);
                    break;

                default:
                    break;
            }
        }

        private static void UpdateColorOption(ColorOption colorOption, UIColorSlot colorSlot)
        {
            colorSlot.BackgroundElement.Color = colorOption.CurrentColor;
        }

        private static void UpdateSelectorOption(SelectorOption selectorOption, LabelUIElement labelElement)
        {
            labelElement.SetTextualContent(string.Concat(selectorOption.Name, ": ", selectorOption.GetValue()));
        }

        private static void UpdateValueOption(ValueOption valueOption, LabelUIElement labelElement)
        {
            labelElement.SetTextualContent(string.Concat(valueOption.Name, ": ", valueOption.CurrentValue.ToString($"D{valueOption.MaximumValue.ToString().Length}")));
            Vector2 labelElementSize = labelElement.GetStringSize();

            UIElement plusElement = (UIElement)labelElement.GetData("plus_element");
            UIElement minusElement = (UIElement)labelElement.GetData("minus_element");

            minusElement.Margin = new(labelElementSize.X + 8.0f, labelElementSize.Y / 2.0f * -1.0f);

            minusElement.RepositionRelativeToElement(labelElement);
            plusElement.RepositionRelativeToElement(minusElement);
        }

        private static void UpdateToggleOption(ToggleOption toggleOption, ImageUIElement toggleStateElement)
        {
            toggleStateElement.TextureRectangle = toggleOption.State ? new(new(0, 32), new(32)) : new(new(0), new(32));
        }
        #endregion

        #region Handlers
        private void HandleOptionInteractivity(Option option)
        {
            switch (option)
            {
                case ButtonOption buttonOption:
                    HandleButtonOption(buttonOption);
                    break;

                case ColorOption colorOption:
                    HandleColorOption(colorOption);
                    break;

                case SelectorOption selectorOption:
                    HandleSelectorOption(selectorOption);
                    break;

                case ToggleOption toggleOption:
                    HandleToggleOption(toggleOption);
                    break;

                default:
                    break;
            }
        }

        private static void HandleButtonOption(ButtonOption buttonOption)
        {
            buttonOption.OnClickCallback?.Invoke();
        }

        private void HandleColorOption(ColorOption colorOption)
        {
            this.colorPickerSettings.OnSelectCallback = result =>
            {
                colorOption.SetValue(result.SelectedColor);
            };

            this.colorPickerUI.Configure(this.colorPickerSettings);
            this.uiManager.OpenGUI(UIIndex.ColorPicker);
        }

        private static void HandleSelectorOption(SelectorOption selectorOption)
        {
            selectorOption.Next();
        }

        private static void HandleToggleOption(ToggleOption toggleOption)
        {
            toggleOption.Toggle();
        }
        #endregion

        #endregion

        #region UTILITIES

        private void SelectSection(string identififer)
        {
            this.selectedSectionIdentififer = identififer;

            foreach (KeyValuePair<string, ContainerUIElement> item in this.sectionContainerElements)
            {
                if (item.Key.Equals(identififer))
                {
                    item.Value.Active();
                    continue;
                }

                item.Value.Disable();
            }
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            SyncSettingElements();
        }

        protected override void OnBuild(ContainerUIElement root)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
