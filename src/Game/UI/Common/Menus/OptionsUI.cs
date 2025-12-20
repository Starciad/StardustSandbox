using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics.Primitives;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Options;

using System;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class OptionsUI : UIBase
    {
        private enum SectionIndex : byte { General, Gameplay, Volume, Video, Control, Cursor }
        private enum GeneralSectionOptionIndex : byte { Language }
        private enum GameplaySectionOptionIndex : byte { ShowPreviewArea, PreviewAreaColor, PreviewAreaOpacity, ShowGrid, GridOpacity, ShowTemperatureColorVariations }
        private enum VolumeSectionOptionIndex : byte { MasterVolume, MusicVolume, SFXVolume }
        private enum VideoSectionOptionIndex : byte { Framerate, Resolution, Fullscreen, VSync, Borderless }
        private enum ControlSectionOptionIndex : byte { MoveCameraUp, MoveCameraRight, MoveCameraDown, MoveCameraLeft, TogglePause, ClearWorld }
        private enum CursorSectionOptionIndex : byte { Color, BackgroundColor, Scale, Opacity }

        private sealed class Root
        {
            internal Section[] Sections { get; }
            internal Root(Section[] sections) { this.Sections = sections; }
        }

        private sealed class Section
        {
            internal string Name { get; }
            internal string Description { get; }
            internal Option[] Options { get; }
            internal Section(string name, string description, Option[] options)
            {
                this.Name = name;
                this.Description = description;
                this.Options = options;
            }
        }

        private struct SectionUI
        {
            internal Label Title;
            internal Label[] Options;
        }

        private Label titleLabel;
        private Image background;

        private Image scrollbarUpButton, scrollbarDownButton, scrollbarSliderButton;

        private Container scrollableContainer;

        private readonly Root root;

        private readonly ColorPickerUI colorPickerUI;
        private readonly SliderUI sliderUI;

        private readonly string titleName = Localization_GUIs.Menu_Options_Title;

        private readonly ButtonInfo[] systemButtonInfos;
        private readonly Label[] systemButtonLabels;
        private readonly TooltipBox tooltipBox;

        private readonly CursorManager cursorManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;
        private readonly GameManager gameManager;

        private readonly SectionUI[] sectionUIs;

        internal OptionsUI(
            ColorPickerUI colorPickerUI,
            CursorManager cursorManager,
            GameManager gameManager,
            UIIndex index,
            MessageUI messageUI,
            SliderUI sliderUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            VideoManager videoManager
        ) : base(index)
        {
            this.colorPickerUI = colorPickerUI;
            this.cursorManager = cursorManager;
            this.gameManager = gameManager;
            this.sliderUI = sliderUI;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.videoManager = videoManager;

            this.systemButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Save, Localization_GUIs.Menu_Options_Button_Save_Description, () =>
                {
                    SaveSettings();
                    ApplySettings();

                    StatusSettings statusSettings = SettingsSerializer.LoadSettings<StatusSettings>();

                    if (!statusSettings.TheRestartAfterSavingSettingsWarningWasDisplayed)
                    {
                        messageUI.SetContent(Localization_Messages.Settings_RestartRequired);
                        uiManager.OpenGUI(UIIndex.Message);

                        SettingsSerializer.SaveSettings<StatusSettings>(new(statusSettings)
                        {
                            TheRestartAfterSavingSettingsWarningWasDisplayed = true,
                        });
                    }
                }),
                new(TextureIndex.None, null, Localization_Statements.Return, Localization_GUIs.Button_Exit_Description, uiManager.CloseGUI),
            ];

            this.root = new([
                new(Localization_GUIs.Menu_Options_Section_General_Name, Localization_GUIs.Menu_Options_Section_General_Description,
                [
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_General_Option_Language_Name, Localization_GUIs.Menu_Options_Section_General_Option_Language_Description, Array.ConvertAll<GameCulture, object>(LocalizationConstants.AVAILABLE_GAME_CULTURES, x => x)),
                ]),
                new(Localization_GUIs.Menu_Options_Section_Gameplay_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Description,
                [
                    new ToggleOption("Show Preview Area", "Description"),
                    new ColorOption(Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaColor_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaColor_Description),
                    new SliderOption(Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaOpacity_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaOpacity_Description, byte.MinValue, byte.MaxValue),
                    new ToggleOption("Show Grid", "Description"),
                    new SliderOption("Grid Opacity", "Description", byte.MinValue, byte.MaxValue),
                    new ToggleOption("Temperature Color Variations", "Description"),
                ]),
                new(Localization_GUIs.Menu_Options_Section_Volume_Name, Localization_GUIs.Menu_Options_Section_Volume_Description,
                [
                    new SliderOption(Localization_GUIs.Menu_Options_Section_Volume_Option_MasterVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_MasterVolume_Description, 0, 100),
                    new SliderOption(Localization_GUIs.Menu_Options_Section_Volume_Option_MusicVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_MusicVolume_Description, 0, 100),
                    new SliderOption(Localization_GUIs.Menu_Options_Section_Volume_Option_SFXVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_SFXVolume_Description, 0, 100),
                ]),
                new(Localization_GUIs.Menu_Options_Section_Video_Name, Localization_GUIs.Menu_Options_Section_Video_Description,
                [
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_Video_Option_Framerate_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Framerate_Description, Array.ConvertAll<float, object>(ScreenConstants.FRAMERATES, x => x)),
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_Video_Option_Resolution_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Resolution_Description, Array.ConvertAll<Resolution, object>(ScreenConstants.RESOLUTIONS, x => x)),
                    new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_Fullscreen_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Fullscreen_Description),
                    new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_VSync_Name, Localization_GUIs.Menu_Options_Section_Video_Option_VSync_Description),
                    new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_Borderless_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Borderless_Description),
                ]),
                new("Controls", "Description",
                [
                    new KeyOption("Move Camera Up", "Description"),
                    new KeyOption("Move Camera Right", "Description"),
                    new KeyOption("Move Camera Down", "Description"),
                    new KeyOption("Move Camera Left", "Description"),
                    new KeyOption("Toggle Pause", "Description"),
                    new KeyOption("Clear World", "Description"),
                ]),
                new(Localization_GUIs.Menu_Options_Section_Cursor_Name, Localization_GUIs.Menu_Options_Section_Cursor_Description,
                [
                    new ColorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Color_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Color_Description),
                    new ColorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_BackgroundColor_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_BackgroundColor_Description),
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Scale_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Scale_Description, [0.5f, 1f, 1.5f, 2f, 2.5f, 3f]),
                    new SliderOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Opacity_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Opacity_Description, byte.MinValue, byte.MaxValue)
                ]),
            ]);

            this.sectionUIs = new SectionUI[this.root.Sections.Length];
            this.systemButtonLabels = new Label[this.systemButtonInfos.Length];
        }

        private void SaveSettings()
        {
            Section generalSection = this.root.Sections[(byte)SectionIndex.General];
            Section gameplaySection = this.root.Sections[(byte)SectionIndex.Gameplay];
            Section volumeSection = this.root.Sections[(byte)SectionIndex.Volume];
            Section videoSection = this.root.Sections[(byte)SectionIndex.Video];
            Section controlSection = this.root.Sections[(byte)SectionIndex.Control];
            Section cursorSection = this.root.Sections[(byte)SectionIndex.Cursor];

            GameCulture gameCulture = LocalizationConstants.GetGameCultureFromNativeName(Convert.ToString(generalSection.Options[(byte)GeneralSectionOptionIndex.Language].GetValue()));

            SettingsSerializer.SaveSettings<GeneralSettings>(new()
            {
                Language = gameCulture.Language,
                Region = gameCulture.Region,
            });

            SettingsSerializer.SaveSettings<GameplaySettings>(new()
            {
                ShowPreviewArea = Convert.ToBoolean(gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowPreviewArea].GetValue()),
                PreviewAreaColor = (Color)gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaColor].GetValue(),
                PreviewAreaColorA = Convert.ToByte(gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaOpacity].GetValue()),
                ShowGrid = Convert.ToBoolean(gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowGrid].GetValue()),
                GridOpacity = Convert.ToByte(gameplaySection.Options[(byte)GameplaySectionOptionIndex.GridOpacity].GetValue()),
                ShowTemperatureColorVariations = Convert.ToBoolean(gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowTemperatureColorVariations].GetValue()),
            });

            SettingsSerializer.SaveSettings<VolumeSettings>(new()
            {
                MasterVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.MasterVolume].GetValue()) / 100.0f,
                MusicVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.MusicVolume].GetValue()) / 100.0f,
                SFXVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.SFXVolume].GetValue()) / 100.0f,
            });

            SettingsSerializer.SaveSettings<VideoSettings>(new()
            {
                Framerate = Convert.ToSingle(videoSection.Options[(byte)VideoSectionOptionIndex.Framerate].GetValue()),
                Resolution = (Resolution)videoSection.Options[(byte)VideoSectionOptionIndex.Resolution].GetValue(),
                FullScreen = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.Fullscreen].GetValue()),
                VSync = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.VSync].GetValue()),
                Borderless = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.Borderless].GetValue()),
            });

            SettingsSerializer.SaveSettings<ControlSettings>(new()
            {
                MoveCameraUp = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraUp].GetValue(),
                MoveCameraRight = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraRight].GetValue(),
                MoveCameraDown = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraDown].GetValue(),
                MoveCameraLeft = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraLeft].GetValue(),
                TogglePause = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.TogglePause].GetValue(),
                ClearWorld = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.ClearWorld].GetValue(),
            });

            SettingsSerializer.SaveSettings<CursorSettings>(new()
            {
                Color = (Color)cursorSection.Options[(byte)CursorSectionOptionIndex.Color].GetValue(),
                BackgroundColor = (Color)cursorSection.Options[(byte)CursorSectionOptionIndex.BackgroundColor].GetValue(),
                Alpha = Convert.ToByte(cursorSection.Options[(byte)CursorSectionOptionIndex.Opacity].GetValue()),
                Scale = Convert.ToSingle(cursorSection.Options[(byte)CursorSectionOptionIndex.Scale].GetValue()),
            });
        }

        private void SyncSettingElements()
        {
            ControlSettings controlSettings = SettingsSerializer.LoadSettings<ControlSettings>();
            CursorSettings cursorSettings = SettingsSerializer.LoadSettings<CursorSettings>();
            GameplaySettings gameplaySettings = SettingsSerializer.LoadSettings<GameplaySettings>();
            GeneralSettings generalSettings = SettingsSerializer.LoadSettings<GeneralSettings>();
            VideoSettings videoSettings = SettingsSerializer.LoadSettings<VideoSettings>();
            VolumeSettings volumeSettings = SettingsSerializer.LoadSettings<VolumeSettings>();

            Section controlSection = this.root.Sections[(byte)SectionIndex.Control];
            Section cursorSection = this.root.Sections[(byte)SectionIndex.Cursor];
            Section gameplaySection = this.root.Sections[(byte)SectionIndex.Gameplay];
            Section generalSection = this.root.Sections[(byte)SectionIndex.General];
            Section videoSection = this.root.Sections[(byte)SectionIndex.Video];
            Section volumeSection = this.root.Sections[(byte)SectionIndex.Volume];

            controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraUp].SetValue(controlSettings.MoveCameraUp);
            controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraRight].SetValue(controlSettings.MoveCameraRight);
            controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraDown].SetValue(controlSettings.MoveCameraDown);
            controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraLeft].SetValue(controlSettings.MoveCameraLeft);
            controlSection.Options[(byte)ControlSectionOptionIndex.TogglePause].SetValue(controlSettings.TogglePause);
            controlSection.Options[(byte)ControlSectionOptionIndex.ClearWorld].SetValue(controlSettings.ClearWorld);

            cursorSection.Options[(byte)CursorSectionOptionIndex.Color].SetValue(new Color(cursorSettings.Color, 255));
            cursorSection.Options[(byte)CursorSectionOptionIndex.BackgroundColor].SetValue(new Color(cursorSettings.BackgroundColor, 255));
            cursorSection.Options[(byte)CursorSectionOptionIndex.Opacity].SetValue(cursorSettings.Alpha);
            cursorSection.Options[(byte)CursorSectionOptionIndex.Scale].SetValue(cursorSettings.Scale);

            gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowPreviewArea].SetValue(gameplaySettings.ShowPreviewArea);
            gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaColor].SetValue(new Color(gameplaySettings.PreviewAreaColor, 255));
            gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaOpacity].SetValue(gameplaySettings.PreviewAreaColorA);
            gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowGrid].SetValue(gameplaySettings.ShowGrid);
            gameplaySection.Options[(byte)GameplaySectionOptionIndex.GridOpacity].SetValue(gameplaySettings.GridOpacity);
            gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowTemperatureColorVariations].SetValue(gameplaySettings.ShowTemperatureColorVariations);

            generalSection.Options[(byte)GeneralSectionOptionIndex.Language].SetValue(generalSettings.GetGameCulture());

            videoSection.Options[(byte)VideoSectionOptionIndex.Framerate].SetValue(videoSettings.Framerate);
            videoSection.Options[(byte)VideoSectionOptionIndex.Resolution].SetValue(videoSettings.Resolution);
            videoSection.Options[(byte)VideoSectionOptionIndex.Fullscreen].SetValue(videoSettings.FullScreen);
            videoSection.Options[(byte)VideoSectionOptionIndex.VSync].SetValue(videoSettings.VSync);
            videoSection.Options[(byte)VideoSectionOptionIndex.Borderless].SetValue(videoSettings.Borderless);

            volumeSection.Options[(byte)VolumeSectionOptionIndex.MasterVolume].SetValue(volumeSettings.MasterVolume * 100.0f);
            volumeSection.Options[(byte)VolumeSectionOptionIndex.MusicVolume].SetValue(volumeSettings.MusicVolume * 100.0f);
            volumeSection.Options[(byte)VolumeSectionOptionIndex.SFXVolume].SetValue(volumeSettings.SFXVolume * 100.0f);
        }

        private void ApplySettings()
        {
            VolumeSettings volumeSettings = SettingsSerializer.LoadSettings<VolumeSettings>();

            MediaPlayer.Volume = volumeSettings.MusicVolume * volumeSettings.MasterVolume;
            SoundEffect.MasterVolume = volumeSettings.SFXVolume * volumeSettings.MasterVolume;

            this.videoManager.ApplySettings();
            this.cursorManager.ApplySettings();
        }

        protected override void OnBuild(Container root)
        {
            this.background = new()
            {
                Alignment = UIDirection.Center,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundOptions),
                Size = new(730.0f, 720.0f),
            };

            root.AddChild(this.background);

            this.scrollableContainer = new()
            {
                Alignment = UIDirection.Center,
                Size = this.background.Size,
            };

            float scrollableContainerMarginY = 0.0f;

            BuildTitle(ref scrollableContainerMarginY);
            BuildSections(ref scrollableContainerMarginY);
            BuildSystemButtons(ref scrollableContainerMarginY);

            BuildScrollBar();

            root.AddChild(this.scrollableContainer);
            root.AddChild(this.tooltipBox);
        }

        private void BuildTitle(ref float scrollableContainerMarginY)
        {
            scrollableContainerMarginY += 32.0f;

            this.titleLabel = new()
            {
                Margin = new(0.0f, scrollableContainerMarginY),
                Alignment = UIDirection.North,
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = this.titleName,
            };

            this.scrollableContainer.AddChild(this.titleLabel);
        }

        private void BuildSections(ref float scrollableContainerMarginY)
        {
            for (int i = 0; i < this.root.Sections.Length; i++)
            {
                Section section = this.root.Sections[i];
                Label[] contentBuffer = new Label[section.Options.Length];

                scrollableContainerMarginY += 80.0f;

                Label sectionLabel = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = UIDirection.North,
                    Scale = new(0.11f),
                    Margin = new(0.0f, scrollableContainerMarginY),
                    TextContent = section.Name,
                };

                this.scrollableContainer.AddChild(sectionLabel);

                scrollableContainerMarginY += 32.0f;

                for (int j = 0; j < section.Options.Length; j++)
                {
                    scrollableContainerMarginY += 64.0f;

                    Option option = section.Options[j];

                    Label label = option switch
                    {
                        KeyOption => CreateOptionButtonLabelElement(option.Name + ": "),
                        SelectorOption => CreateOptionButtonLabelElement(option.Name + ": " + option.GetValue()),
                        SliderOption => CreateOptionButtonLabelElement(option.Name + ": " + option.GetValue()),
                        ColorOption => CreateOptionButtonLabelElement(option.Name + ": "),
                        ToggleOption => CreateOptionButtonLabelElement(option.Name + ": "),
                        _ => null,
                    };

                    label.AddData("option", option);
                    label.Margin = new(32.0f, scrollableContainerMarginY);

                    if (option is ColorOption)
                    {
                        BuildColorPreview(label);
                    }
                    else if (option is ToggleOption)
                    {
                        BuildTogglePreview(label);
                    }

                    this.scrollableContainer.AddChild(label);
                    contentBuffer[j] = label;
                }

                this.sectionUIs[i] = new SectionUI { Title = sectionLabel, Options = contentBuffer };
            }
        }

        private static void BuildColorPreview(Label label)
        {
            ColorSlotInfo colorSlot = new(
                new()
                {
                    Alignment = UIDirection.East,
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(386, 0, 40, 22),
                    Scale = new(1.5f),
                    Size = new(40.0f, 22.0f),
                    Margin = new(58.0f, 0.0f),
                },

                new()
                {
                    Alignment = UIDirection.Center,
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(386, 22, 40, 22),
                    Scale = new(1.5f),
                    Size = new(40.0f, 22.0f),
                }
            );

            colorSlot.Background.AddChild(colorSlot.Border);

            label.AddChild(colorSlot.Background);
            label.AddData("color_slot", colorSlot);
        }

        private static void BuildTogglePreview(Label label)
        {
            Image togglePreviewImageElement = new()
            {
                Alignment = UIDirection.East,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(352, 140, 32, 32),
                Scale = new(1.25f),
                Size = new(32.0f),
                Margin = new(48.0f, 0.0f),
            };

            label.AddChild(togglePreviewImageElement);
            label.AddData("toogle_preview", togglePreviewImageElement);
        }

        private void BuildSystemButtons(ref float scrollableContainerMarginY)
        {
            scrollableContainerMarginY += 32.0f;

            for (int i = 0; i < this.systemButtonInfos.Length; i++)
            {
                scrollableContainerMarginY += 64.0f;

                Label label = new()
                {
                    Margin = new(32.0f, scrollableContainerMarginY),
                    Scale = new(0.11f),
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    TextContent = this.systemButtonInfos[i].Name
                };

                this.systemButtonLabels[i] = label;
                this.scrollableContainer.AddChild(label);
            }
        }

        private void BuildScrollBar()
        {
            this.scrollbarUpButton = new()
            {
                Alignment = UIDirection.Northeast,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(388, 50, 34, 32),
                Size = new(34.0f, 32.0f),
            };

            this.scrollbarDownButton = new()
            {
                Alignment = UIDirection.Southeast,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(388, 206, 34, 32),
                Size = new(34.0f, 32.0f),
            };

            this.scrollbarSliderButton = new()
            {
                Alignment = UIDirection.Northeast,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(388, 80, 34, 128),
                Size = new(34.0f, 128.0f),
            };

            this.background.AddChild(this.scrollbarUpButton);
            this.background.AddChild(this.scrollbarDownButton);
            this.background.AddChild(this.scrollbarSliderButton);
        }

        private static Label CreateOptionButtonLabelElement(string text)
        {
            return new Label
            {
                Scale = new(0.12f),
                SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                TextContent = text
            };
        }

        internal override void Update(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;
            UpdateScrollableContainer();
            UpdateScrollbar();
            UpdateSystemButtons();
            UpdateSectionLabels();
            UpdateSectionOptions();
            base.Update(gameTime);
        }

        private void UpdateScrollableContainer()
        {
            float marginY = this.scrollableContainer.Margin.Y;

            if (Interaction.OnMouseScrollUp())
            {
                marginY -= 52.0f;
            }
            else if (Interaction.OnMouseScrollDown())
            {
                marginY += 52.0f;
            }

            float topLimit = 0.0f;
            float bottomLimit = this.scrollableContainer.Children.Count * 58.0f * -1;

            this.scrollableContainer.Margin = new(this.scrollableContainer.Margin.X, float.Clamp(marginY, bottomLimit, topLimit));
        }

        private void UpdateScrollbar()
        {
            if (Interaction.OnMouseLeftClick(this.scrollbarUpButton))
            {
                float marginY = this.scrollableContainer.Margin.Y + 52.0f;
                float bottomLimit = this.scrollableContainer.Children.Count * 58.0f * -1;
                this.scrollableContainer.Margin = new(this.scrollableContainer.Margin.X, float.Clamp(marginY, bottomLimit, 0.0f));
            }
            else if (Interaction.OnMouseLeftClick(this.scrollbarDownButton))
            {
                float marginY = this.scrollableContainer.Margin.Y - 52.0f;
                float bottomLimit = this.scrollableContainer.Children.Count * 58.0f * -1;
                this.scrollableContainer.Margin = new(this.scrollableContainer.Margin.X, float.Clamp(marginY, bottomLimit, 0.0f));
            }

            float scrollableHeight = this.scrollableContainer.Children.Count * 58.0f;
            float backgroundHeight = this.background.Size.Y;
            float scrollableMarginY = this.scrollableContainer.Margin.Y;

            float sliderMinY = this.scrollbarUpButton.Size.Y;
            float sliderMaxY = backgroundHeight - this.scrollbarSliderButton.Size.Y - this.scrollbarDownButton.Size.Y;
            float sliderY = -scrollableMarginY / scrollableHeight * sliderMaxY;

            sliderY = float.Clamp(sliderY, sliderMinY, sliderMaxY);

            this.scrollbarSliderButton.Margin = new(this.scrollbarSliderButton.Margin.X, sliderY);
        }

        private void UpdateSystemButtons()
        {
            for (int i = 0; i < this.systemButtonInfos.Length; i++)
            {
                Label label = this.systemButtonLabels[i];
                ButtonInfo button = this.systemButtonInfos[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    button.ClickAction?.Invoke();
                }

                if (Interaction.OnMouseOver(label))
                {
                    this.tooltipBox.CanDraw = true;
                    TooltipBoxContent.SetTitle(button.Name);
                    TooltipBoxContent.SetDescription(button.Description);
                    label.Color = AAP64ColorPalette.LemonYellow;
                }
                else
                {
                    label.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateSectionLabels()
        {
            for (int i = 0; i < this.sectionUIs.Length; i++)
            {
                Label sectionLabel = this.sectionUIs[i].Title;

                if (Interaction.OnMouseOver(sectionLabel))
                {
                    this.tooltipBox.CanDraw = true;
                    TooltipBoxContent.SetTitle(this.root.Sections[i].Name);
                    TooltipBoxContent.SetDescription(this.root.Sections[i].Description);
                    sectionLabel.Color = AAP64ColorPalette.LemonYellow;
                }
                else
                {
                    sectionLabel.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateSectionOptions()
        {
            for (int i = 0; i < this.sectionUIs.Length; i++)
            {
                Label[] contentLabels = this.sectionUIs[i].Options;

                for (int j = 0; j < contentLabels.Length; j++)
                {
                    Label label = contentLabels[j];
                    Option option = (Option)label.GetData("option");

                    if (Interaction.OnMouseLeftClick(label))
                    {
                        if (option is SliderOption slider)
                        {
                            HandleSliderOption(slider);
                        }
                        else if (option is ColorOption color)
                        {
                            HandleColorOption(color);
                        }
                        else if (option is SelectorOption selector)
                        {
                            selector.Next();
                        }
                        else if (option is ToggleOption toggle)
                        {
                            toggle.Toggle();
                        }
                    }
                    else if (Interaction.OnMouseRightClick(label))
                    {
                        if (option is SelectorOption selector)
                        {
                            selector.Previous();
                        }
                    }

                    UpdateOptionSync(option, label);

                    if (Interaction.OnMouseOver(label))
                    {
                        label.Color = AAP64ColorPalette.LemonYellow;
                        this.tooltipBox.CanDraw = true;
                        TooltipBoxContent.SetTitle(option.Name);
                        TooltipBoxContent.SetDescription(option.Description);
                    }
                    else
                    {
                        label.Color = AAP64ColorPalette.White;
                    }
                }
            }
        }

        private static void UpdateOptionSync(Option option, UIElement element)
        {
            if (option is ColorOption colorOption)
            {
                ((ColorSlotInfo)element.GetData("color_slot")).Background.Color = (Color)Convert.ChangeType(colorOption.GetValue(), typeof(Color));
            }
            else if (option is SelectorOption selectorOption)
            {
                ((Label)element).TextContent = string.Concat(selectorOption.Name, ": ", selectorOption.GetValue());
            }
            else if (option is SliderOption sliderOption)
            {
                ((Label)element).TextContent = string.Concat(sliderOption.Name, ": ", sliderOption.GetValue());
            }
            else if (option is ToggleOption toggleOption)
            {
                ((Image)element.GetData("toogle_preview")).SourceRectangle = Convert.ToBoolean(toggleOption.GetValue()) ? new(352, 171, 32, 32) : new(352, 140, 32, 32);
            }
        }

        private void HandleSliderOption(SliderOption sliderOption)
        {
            this.sliderUI.Configure(new()
            {
                MinimumValue = sliderOption.MinimumValue,
                MaximumValue = sliderOption.MaximumValue,
                CurrentValue = Convert.ToInt32(sliderOption.GetValue()),
                Synopsis = sliderOption.Description,
                OnSendCallback = result => sliderOption.SetValue(result.Value),
            });

            this.uiManager.OpenGUI(UIIndex.Slider);
        }

        private void HandleColorOption(ColorOption colorOption)
        {
            this.colorPickerUI.Configure(new()
            {
                OnSelectCallback = result => colorOption.SetValue(result.SelectedColor),
            });

            this.uiManager.OpenGUI(UIIndex.ColorPicker);
        }

        protected override void OnOpened()
        {
            this.gameManager.SetState(GameStates.IsCriticalMenuOpen);
            SyncSettingElements();
        }

        protected override void OnClosed()
        {
            this.gameManager.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}
