using Microsoft.Xna.Framework;

using StardustSandbox.AudioSystem;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.IO.Handlers;
using StardustSandbox.IO.Settings;
using StardustSandbox.LocalizationSystem;
using StardustSandbox.Managers;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Options;
using StardustSandbox.UI.Settings;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class OptionsMenuUI : UIBase
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

        private Label titleLabelElement;
        private Image panelBackgroundElement;

        private readonly Root root;
        private readonly ColorPickerSettings colorPickerSettings;

        private readonly ColorPickerUI colorPickerUI;
        private readonly MessageUI messageUI;

        private readonly string titleName = Localization_GUIs.Menu_Options_Title;
        private readonly List<(UIElement, UIElement)> plusAndMinusButtons = [];

        private readonly TooltipBox tooltipBox;

        private readonly Label[] systemButtonElements;
        private readonly Dictionary<string, IEnumerable<Label>> sectionContents = [];
        private readonly Dictionary<string, Container> sectionContainerElements = [];
        private readonly Dictionary<string, Label> sectionButtonElements = [];

        private readonly ButtonInfo[] systemButtons;

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
        private static readonly float defaultButtonBorderOffset = 2.0f;
        private static readonly float leftPanelMarginVerticalSpacing = 48.0f;
        private static readonly float rightPanelMarginVerticalSpacing = 48.0f;

        internal OptionsMenuUI(
            ColorPickerUI colorPickerUI,
            CursorManager cursorManager,
            UIIndex index,
            MessageUI messageUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            VideoManager videoManager
        ) : base(index)
        {
            this.colorPickerUI = colorPickerUI;
            this.cursorManager = cursorManager;
            this.messageUI = messageUI;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.videoManager = videoManager;

            this.colorPickerSettings = new();

            this.generalSettings = SettingsHandler.LoadSettings<GeneralSettings>();
            this.gameplaySettings = SettingsHandler.LoadSettings<GameplaySettings>();
            this.volumeSettings = SettingsHandler.LoadSettings<VolumeSettings>();
            this.videoSettings = SettingsHandler.LoadSettings<VideoSettings>();
            this.cursorSettings = SettingsHandler.LoadSettings<CursorSettings>();

            this.systemButtons = [
                new(TextureIndex.None, null, Localization_Statements.Return, Localization_GUIs.Button_Exit_Description, ReturnButtonAction),
                new(TextureIndex.None, null, Localization_Statements.Save, Localization_GUIs.Menu_Options_Button_Save_Description, SaveButtonAction),
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

            this.systemButtonElements = new Label[this.systemButtons.Length];
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

        protected override void OnBuild(Container root)
        {
            // Decorations
            BuildPanelBackground(root);
            BuildTitle(root);

            // Buttons
            BuildSectionButtons();
            BuildSystemButtons();

            // Sections
            BuildSections(root);

            // Final
            root.AddChild(this.tooltipBox);
            SelectSection("general");
        }

        private void BuildPanelBackground(Container root)
        {
            this.panelBackgroundElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundOptions),
                Size = new(1084, 540),
                Margin = new(98, 90),
            };

            root.AddChild(this.panelBackgroundElement);
        }

        private void BuildTitle(Container root)
        {
            this.titleLabelElement = new()
            {
                Scale = new(0.15f),
                Margin = new(0f, 52.5f),
                Color = AAP64ColorPalette.White,
                Alignment = CardinalDirection.North,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = this.titleName,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 4.4f,
                BorderThickness = 4.4f,
            };

            root.AddChild(this.titleLabelElement);
        }

        private void BuildSectionButtons()
        {
            // BUTTONS
            Vector2 margin = new(-335f, 64f);

            // Labels
            foreach (KeyValuePair<string, Section> item in this.root.Sections)
            {
                Label label = CreateButtonLabelElement(item.Value.Name);

                label.Alignment = CardinalDirection.North;
                label.Margin = margin;
                this.panelBackgroundElement.AddChild(label);

                this.sectionButtonElements.Add(item.Key, label);
                margin.Y += leftPanelMarginVerticalSpacing;
            }
        }

        private void BuildSystemButtons()
        {
            Vector2 margin = new(-335f, -64f);

            for (int i = 0; i < this.systemButtons.Length; i++)
            {
                Label label = CreateButtonLabelElement(this.systemButtons[i].Name);

                label.Alignment = CardinalDirection.South;
                label.Margin = margin;
                this.panelBackgroundElement.AddChild(label);

                this.systemButtonElements[i] = label;
                margin.Y -= leftPanelMarginVerticalSpacing;
            }
        }

        // ============================================================================ //

        private void BuildSections(Container root)
        {
            foreach (KeyValuePair<string, Section> item in this.root.Sections)
            {
                List<Label> contentBuffer = [];
                Container container = new();

                Vector2 margin = defaultRightPanelMargin;

                foreach (Option option in item.Value.Options.Values)
                {
                    Label label = CreateOptionElement(option);

                    label.Margin = margin;
                    this.panelBackgroundElement.AddChild(label);

                    switch (option)
                    {
                        case ColorOption:
                            BuildColorPreview(container, label);
                            break;

                        case ValueOption:
                            BuildValueControls(option, container, label);
                            break;

                        case ToggleOption:
                            BuildTogglePreview(container, label);
                            break;

                        default:
                            break;
                    }

                    container.AddChild(label);
                    margin.Y += rightPanelMarginVerticalSpacing;

                    contentBuffer.Add(label);
                }

                this.sectionContainerElements.Add(item.Key, container);
                root.AddChild(container);

                this.sectionContents.Add(item.Key, contentBuffer);
            }
        }

        private static void BuildColorPreview(Container container, Label label)
        {
            ColorSlotInfo colorSlot = new(
                new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(386, 0, 40, 22),
                    Scale = new(1.5f),
                    Size = new(40.0f, 22.0f),
                    Margin = new(label.MeasuredText.X + 6f, label.MeasuredText.Y / 2f * -1f),
                },

                new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(386, 22, 40, 22),
                    Scale = new(1.5f),
                    Size = new(40.0f, 22.0f),
                }
            );

            label.AddChild(colorSlot.Background);
            colorSlot.Background.AddChild(colorSlot.Border);

            container.AddChild(colorSlot.Background);
            container.AddChild(colorSlot.Border);

            label.AddData("color_slot", colorSlot);
        }

        private void BuildValueControls(Option option, Container container, Label label)
        {
            Image minus = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUI),
                SourceRectangle = new(192, 160, 32, 32),
                Size = new(32.0f),
                Margin = new(0, label.MeasuredText.Y / 2f * -1f)
            };

            Image plus = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUI),
                SourceRectangle = new(160, 160, 32, 32),
                Size = new(32.0f),
                Margin = new(48.0f, 0.0f),
            };

            plus.AddData("option", option);
            minus.AddData("option", option);

            label.AddData("plus_element", plus);
            label.AddData("minus_element", minus);

            label.AddChild(minus);
            minus.AddChild(plus);

            container.AddChild(plus);
            container.AddChild(minus);

            this.plusAndMinusButtons.Add((plus, minus));
        }

        private static void BuildTogglePreview(Container container, Label label)
        {
            Image togglePreviewImageElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(352, 140, 32, 32),
                Scale = new(1.25f),
                Size = new(32.0f),
                Margin = new(label.MeasuredText.X + 6.0f, label.MeasuredText.Y / 2.0f * -1.0f),
            };

            label.AddChild(togglePreviewImageElement);

            container.AddChild(togglePreviewImageElement);

            label.AddData("toogle_preview", togglePreviewImageElement);
        }
        // ============================================================================ //

        private static Label CreateButtonLabelElement(string text)
        {
            Label label = new()
            {
                Scale = defaultButtonScale,
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = defaultButtonBorderOffset,
                BorderThickness = defaultButtonBorderOffset,
                TextContent = text
            };

            return label;
        }

        private static Label CreateOptionButtonLabelElement(string text)
        {
            Label label = new()
            {
                Scale = new(0.12f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                Alignment = CardinalDirection.North,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2f,
                BorderThickness = 2f,
                TextContent = text
            };

            return label;
        }

        // ============================================================================ //

        private static Label CreateOptionElement(Option option)
        {
            Label label = option switch
            {
                ButtonOption => CreateButtonOptionElement(option),
                SelectorOption => CreateSelectorOptionElement(option),
                ValueOption => CreateValueOptionElement(option),
                ColorOption => CreateColorOptionElement(option),
                ToggleOption => CreateToggleOptionElement(option),
                _ => null,
            };

            label.AddData("option", option);

            return label;
        }

        private static Label CreateButtonOptionElement(Option option)
        {
            return CreateOptionButtonLabelElement(option.Name);
        }

        private static Label CreateSelectorOptionElement(Option option)
        {
            return CreateOptionButtonLabelElement(string.Concat(option.Name, ": ", option.GetValue()));
        }

        private static Label CreateValueOptionElement(Option option)
        {
            return CreateOptionButtonLabelElement(string.Concat(option.Name, ": ", option.GetValue()));
        }

        private static Label CreateColorOptionElement(Option option)
        {
            return CreateOptionButtonLabelElement(string.Concat(option.Name, ": "));
        }

        private static Label CreateToggleOptionElement(Option option)
        {
            return CreateOptionButtonLabelElement(string.Concat(option.Name, ": "));
        }

        #endregion

        #region UPDATING

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.tooltipBox.CanDraw = false;

            UpdateSectionButtons();
            UpdateSystemButtons();
            UpdateSectionOptions();

            this.tooltipBox.RefreshDisplay();
        }

        private void UpdateSectionButtons()
        {
            foreach (KeyValuePair<string, Label> item in this.sectionButtonElements)
            {
                Label label = item.Value;

                Vector2 size = label.MeasuredText / 2.0f;
                Vector2 position = label.Position;

                bool onMouseOver = Interaction.OnMouseOver(position, size);

                if (Interaction.OnMouseClick(position, size))
                {
                    SelectSection(item.Key);
                }

                if (onMouseOver)
                {
                    this.tooltipBox.CanDraw = true;

                    Section section = this.root.Sections[item.Key];

                    TooltipBoxContent.Title = section.Name;
                    TooltipBoxContent.Description = section.Description;
                }

                label.Color = this.selectedSectionIdentififer.Equals(item.Key)
                    ? AAP64ColorPalette.LemonYellow
                    : onMouseOver ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        private void UpdateSystemButtons()
        {
            for (byte i = 0; i < this.systemButtons.Length; i++)
            {
                Label label = this.systemButtonElements[i];
                ButtonInfo button = this.systemButtons[i];

                Vector2 size = label.MeasuredText / 2.0f;
                Vector2 position = label.Position;

                if (Interaction.OnMouseClick(position, size))
                {
                    button.ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(position, size))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = button.Name;
                    TooltipBoxContent.Description = button.Description;

                    label.Color = AAP64ColorPalette.LemonYellow;
                }
                else
                {
                    label.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateSectionOptions()
        {
            Vector2 interactiveAreaSize = new(295.0f, 18.0f);
            Vector2 plusAndMinusButtonAreaSize = new(32.0f);

            foreach (Label label in this.sectionContents[this.selectedSectionIdentififer])
            {
                Vector2 position = new(label.Position.X + interactiveAreaSize.X, label.Position.Y - 6);
                Option option = (Option)label.GetData("option");

                if (Interaction.OnMouseClick(position, interactiveAreaSize))
                {
                    HandleOptionInteractivity(option);
                }

                UpdateOptionSync(option, label);

                if (Interaction.OnMouseOver(position, interactiveAreaSize))
                {
                    label.Color = AAP64ColorPalette.LemonYellow;

                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.Title = option.Name;
                    TooltipBoxContent.Description = option.Description;
                }
                else
                {
                    label.Color = AAP64ColorPalette.White;
                }
            }

            foreach ((UIElement plus, UIElement minus) in this.plusAndMinusButtons)
            {
                if (Interaction.OnMouseDown(plus.Position, plusAndMinusButtonAreaSize))
                {
                    ((ValueOption)plus.GetData("option")).Increment();
                }
                else if (Interaction.OnMouseDown(minus.Position, plusAndMinusButtonAreaSize))
                {
                    ((ValueOption)minus.GetData("option")).Decrement();
                }
            }
        }

        #region Sync
        private static void UpdateOptionSync(Option option, UIElement element)
        {
            switch (option)
            {
                case ColorOption colorOption:
                    UpdateColorOption(colorOption, element.GetData("color_slot") as ColorSlotInfo);
                    break;

                case SelectorOption selectorOption:
                    UpdateSelectorOption(selectorOption, element as Label);
                    break;

                case ValueOption valueOption:
                    UpdateValueOption(valueOption, element as Label);
                    break;

                case ToggleOption toggleOption:
                    UpdateToggleOption(toggleOption, element.GetData("toogle_preview") as Image);
                    break;

                default:
                    break;
            }
        }

        private static void UpdateColorOption(ColorOption colorOption, ColorSlotInfo colorSlot)
        {
            colorSlot.Background.Color = colorOption.CurrentColor;
        }

        private static void UpdateSelectorOption(SelectorOption selectorOption, Label label)
        {
            label.TextContent = string.Concat(selectorOption.Name, ": ", selectorOption.GetValue());
        }

        private static void UpdateValueOption(ValueOption valueOption, Label label)
        {
            label.TextContent = string.Concat(valueOption.Name, ": ", valueOption.CurrentValue.ToString($"D{valueOption.MaximumValue.ToString().Length}"));
            Vector2 labelElementSize = label.MeasuredText;

            UIElement plus = (UIElement)label.GetData("plus_element");
            UIElement minus = (UIElement)label.GetData("minus_element");

            minus.Margin = new(labelElementSize.X + 8.0f, labelElementSize.Y / 2.0f * -1.0f);

            label.AddChild(minus);
            minus.AddChild(plus);
        }

        private static void UpdateToggleOption(ToggleOption toggleOption, Image toggleStateElement)
        {
            toggleStateElement.SourceRectangle = toggleOption.State ? new(new(0, 32), new(32)) : new(new(0), new(32));
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

            foreach (KeyValuePair<string, Container> item in this.sectionContainerElements)
            {
                if (item.Key.Equals(identififer))
                {
                    item.Value.CanDraw = true;
                    item.Value.CanUpdate = true;
                    continue;
                }

                item.Value.CanDraw = false;
                item.Value.CanUpdate = false;
            }
        }

        #endregion

        #region EVENTS

        protected override void OnOpened()
        {
            SyncSettingElements();
        }

        #endregion
    }
}
