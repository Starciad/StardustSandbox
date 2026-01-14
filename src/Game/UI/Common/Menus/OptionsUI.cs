/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.InputSystem;
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
using System.Collections.Generic;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class OptionsUI : UIBase
    {
        private enum SectionIndex : byte { General, Gameplay, Volume, Video, Control, Cursor }
        private enum GeneralSectionOptionIndex : byte { Language }
        private enum GameplaySectionOptionIndex : byte { ShowPreviewArea, PreviewAreaColor, PreviewAreaOpacity, ShowGrid, GridOpacity, ShowTemperatureColorVariations }
        private enum VolumeSectionOptionIndex : byte { MasterVolume, MusicVolume, SFXVolume }
        private enum VideoSectionOptionIndex : byte { Framerate, Resolution, Fullscreen, VSync, Borderless }
        private enum ControlSectionOptionIndex : byte { MoveCameraUp, MoveCameraRight, MoveCameraDown, MoveCameraLeft, MoveCameraFast, TogglePause, ClearWorld, NextShape, Screenshot }
        private enum CursorSectionOptionIndex : byte { Color, BackgroundColor, Scale, Opacity }

        private sealed class Origin
        {
            internal Section[] Sections { get; }
            internal Origin(Section[] sections) { this.Sections = sections; }
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

        private sealed class SectionUI(Label title, Label[] options)
        {
            internal Label Title => title;
            internal Label[] Options => options;
        }

        private const float SCROLL_STEP = 52.0f;
        private const float ITEM_SPACING = 58.0f;

        private Label titleLabel;
        private Image background;

        private Image scrollbarUpButton, scrollbarDownButton, scrollbarSliderButton;

        private Container scrollableContainer;

        private bool isDraggingScrollbar = false;
        private float scrollbarDragOffsetY = 0f;

        private readonly Origin root;

        private readonly ColorPickerUI colorPicker;
        private readonly SliderUI sliderUI;

        private readonly string titleName = Localization_GUIs.Options_Title;

        private readonly TooltipBox tooltipBox;

        private readonly CursorManager cursorManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;
        private readonly KeySelectorUI keySelector;

        private readonly SectionUI[] sectionUIs;

        private readonly Dictionary<Type, Func<Option, Label>> optionBuilders;
        private readonly Dictionary<Type, Action<Option>> optionHandlers;
        private readonly Dictionary<Type, Action<Option, UIElement>> optionSyncHandlers;

        internal OptionsUI(
            ColorPickerUI colorPickerUI,
            CursorManager cursorManager,
            KeySelectorUI keySelectorUI,
            MessageUI messageUI,
            SliderUI sliderUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            VideoManager videoManager
        ) : base()
        {
            this.colorPicker = colorPickerUI;
            this.cursorManager = cursorManager;
            this.keySelector = keySelectorUI;
            this.sliderUI = sliderUI;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.videoManager = videoManager;

            this.optionBuilders = new()
            {
                [typeof(ButtonOption)] = option =>
                {
                    return CreateOptionButtonLabelElement(option.Name);
                },

                [typeof(ColorOption)] = option =>
                {
                    Label label = CreateOptionButtonLabelElement(option.Name + ": ");
                    BuildColorPreview(label);

                    return label;
                },

                [typeof(KeyOption)] = option =>
                {
                    return CreateOptionButtonLabelElement(option.Name + ": " + option.GetValue());
                },

                [typeof(SelectorOption)] = option =>
                {
                    return CreateOptionButtonLabelElement(option.Name + ": " + option.GetValue());
                },

                [typeof(SliderOption)] = option =>
                {
                    return CreateOptionButtonLabelElement(option.Name + ": " + option.GetValue());
                },

                [typeof(ToggleOption)] = option =>
                {
                    Label label = CreateOptionButtonLabelElement(option.Name + ": ");
                    BuildTogglePreview(label);

                    return label;
                },
            };

            this.optionHandlers = new()
            {
                [typeof(ButtonOption)] = option =>
                {
                    ((ButtonOption)option).Click();
                },

                [typeof(ColorOption)] = option =>
                {
                    this.colorPicker.Configure(new()
                    {
                        OnSelectCallback = result => option.SetValue(result),
                    });

                    this.uiManager.OpenUI(UIIndex.ColorPicker);
                },

                [typeof(KeyOption)] = option =>
                {
                    this.keySelector.Configure(new()
                    {
                        Synopsis = option.Description,
                        OnSelectedKey = result => option.SetValue(result),
                    });

                    this.uiManager.OpenUI(UIIndex.KeySelector);
                },

                [typeof(SelectorOption)] = option =>
                {
                    ((SelectorOption)option).Next();
                },

                [typeof(SliderOption)] = option =>
                {
                    this.sliderUI.Configure(new()
                    {
                        MinimumValue = ((SliderOption)option).MinimumValue,
                        MaximumValue = ((SliderOption)option).MaximumValue,
                        CurrentValue = Convert.ToInt32(option.GetValue()),
                        Synopsis = option.Description,
                        OnSendCallback = result => option.SetValue(result),
                    });

                    this.uiManager.OpenUI(UIIndex.Slider);
                },

                [typeof(ToggleOption)] = option =>
                {
                    ((ToggleOption)option).Toggle();
                },
            };

            this.optionSyncHandlers = new()
            {
                [typeof(ButtonOption)] = (option, element) =>
                {
                    // No sync needed
                },

                [typeof(ColorOption)] = (option, element) =>
                {
                    ((ColorSlotInfo)element.GetData("color_slot")).Background.Color = (Color)option.GetValue();
                },

                [typeof(KeyOption)] = (option, element) =>
                {
                    ((Label)element).TextContent = string.Concat(option.Name, ": ", option.GetValue());
                },

                [typeof(SelectorOption)] = (option, element) =>
                {
                    ((Label)element).TextContent = string.Concat(option.Name, ": ", option.GetValue());
                },

                [typeof(SliderOption)] = (option, element) =>
                {
                    ((Label)element).TextContent = string.Concat(option.Name, ": ", option.GetValue());
                },

                [typeof(ToggleOption)] = (option, element) =>
                {
                    ((Image)element.GetData("toogle_preview")).SourceRectangle = (bool)option.GetValue() ? new(352, 171, 32, 32) : new(352, 140, 32, 32);
                },
            };

            this.root = new([
                new(Localization_GUIs.Options_General_Name, Localization_GUIs.Options_General_Description,
                [
                    new SelectorOption(Localization_GUIs.Options_General_Language_Name, Localization_GUIs.Options_General_Language_Description, Array.ConvertAll<GameCulture, object>(LocalizationConstants.AVAILABLE_GAME_CULTURES, x => x)),
                ]),
                new(Localization_GUIs.Options_Gameplay_Name, Localization_GUIs.Options_Gameplay_Description,
                [
                    new ToggleOption(Localization_GUIs.Options_Gameplay_ShowPreviewArea_Name, Localization_GUIs.Options_Gameplay_ShowPreviewArea_Description),
                    new ColorOption(Localization_GUIs.Options_Gameplay_PreviewAreaColor_Name, Localization_GUIs.Options_Gameplay_PreviewAreaColor_Description),
                    new SliderOption(Localization_GUIs.Options_Gameplay_PreviewAreaOpacity_Name, Localization_GUIs.Options_Gameplay_PreviewAreaOpacity_Description, byte.MinValue, byte.MaxValue),
                    new ToggleOption(Localization_GUIs.Options_Gameplay_ShowGrid_Name, Localization_GUIs.Options_Gameplay_ShowGrid_Description),
                    new SliderOption(Localization_GUIs.Options_Gameplay_GridOpacity_Name, Localization_GUIs.Options_Gameplay_GridOpacity_Description, byte.MinValue, byte.MaxValue),
                    new ToggleOption(Localization_GUIs.Options_Gameplay_ShowTemperatureColorVariations_Name, Localization_GUIs.Options_Gameplay_ShowTemperatureColorVariations_Description),
                ]),
                new(Localization_GUIs.Options_Volume_Name, Localization_GUIs.Options_Volume_Description,
                [
                    new SliderOption(Localization_GUIs.Options_Volume_MasterVolume_Name, Localization_GUIs.Options_Volume_MasterVolume_Description, 0, 100),
                    new SliderOption(Localization_GUIs.Options_Volume_MusicVolume_Name, Localization_GUIs.Options_Volume_MusicVolume_Description, 0, 100),
                    new SliderOption(Localization_GUIs.Options_Volume_SFXVolume_Name, Localization_GUIs.Options_Volume_SFXVolume_Description, 0, 100),
                ]),
                new(Localization_GUIs.Options_Video_Name, Localization_GUIs.Options_Video_Description,
                [
                    new SelectorOption(Localization_GUIs.Options_Video_Framerate_Name, Localization_GUIs.Options_Video_Framerate_Description, Array.ConvertAll<float, object>(ScreenConstants.FRAMERATES, x => x)),
                    new SelectorOption(Localization_GUIs.Options_Video_Resolution_Name, Localization_GUIs.Options_Video_Resolution_Description, Array.ConvertAll<Resolution, object>(ScreenConstants.RESOLUTIONS, x => x)),
                    new ToggleOption(Localization_GUIs.Options_Video_Fullscreen_Name, Localization_GUIs.Options_Video_Fullscreen_Description),
                    new ToggleOption(Localization_GUIs.Options_Video_VSync_Name, Localization_GUIs.Options_Video_VSync_Description),
                    new ToggleOption(Localization_GUIs.Options_Video_Borderless_Name, Localization_GUIs.Options_Video_Borderless_Description),
                ]),
                new(Localization_GUIs.Options_Controls_Name, Localization_GUIs.Options_Controls_Description,
                [
                    new KeyOption(Localization_GUIs.Options_Controls_MoveCameraUp_Name, Localization_GUIs.Options_Controls_MoveCameraUp_Description),
                    new KeyOption(Localization_GUIs.Options_Controls_MoveCameraRight_Name, Localization_GUIs.Options_Controls_MoveCameraRight_Description),
                    new KeyOption(Localization_GUIs.Options_Controls_MoveCameraDown_Name, Localization_GUIs.Options_Controls_MoveCameraDown_Description),
                    new KeyOption(Localization_GUIs.Options_Controls_MoveCameraLeft_Name, Localization_GUIs.Options_Controls_MoveCameraLeft_Description),
                    new KeyOption(Localization_GUIs.Options_Controls_MoveCameraFast_Name, Localization_GUIs.Options_Controls_MoveCameraFast_Description),
                    new KeyOption(Localization_GUIs.Options_Controls_TogglePause_Name, Localization_GUIs.Options_Controls_TogglePause_Description),
                    new KeyOption(Localization_GUIs.Options_Controls_ClearWorld_Name, Localization_GUIs.Options_Controls_ClearWorld_Description),
                    new KeyOption(Localization_GUIs.Options_Controls_NextShape_Name, Localization_GUIs.Options_Controls_NextShape_Description),
                    new KeyOption(Localization_GUIs.Options_Controls_Screenshot_Name, Localization_GUIs.Options_Controls_Screenshot_Description),
                ]),
                new(Localization_GUIs.Options_Cursor_Name, Localization_GUIs.Options_Cursor_Description,
                [
                    new ColorOption(Localization_GUIs.Options_Cursor_Color_Name, Localization_GUIs.Options_Cursor_Color_Description),
                    new ColorOption(Localization_GUIs.Options_Cursor_BackgroundColor_Name, Localization_GUIs.Options_Cursor_BackgroundColor_Description),
                    new SelectorOption(Localization_GUIs.Options_Cursor_Scale_Name, Localization_GUIs.Options_Cursor_Scale_Description, [0.5f, 1f, 1.5f, 2f, 2.5f, 3f]),
                    new SliderOption(Localization_GUIs.Options_Cursor_Opacity_Name, Localization_GUIs.Options_Cursor_Opacity_Description, byte.MinValue, byte.MaxValue)
                ]),
                new(string.Empty, string.Empty,
                [
                    new ButtonOption(Localization_Statements.Save, Localization_GUIs.Options_Save_Description, () =>
                    {
                        Save();
                        ApplySettings();

                        StatusSettings statusSettings = SettingsSerializer.Load<StatusSettings>();

                        if (!statusSettings.TheRestartAfterSavingSettingsWarningWasDisplayed)
                        {
                            SoundEngine.Play(SoundEffectIndex.GUI_Message);

                            messageUI.SetContent(Localization_Messages.Settings_RestartRequired);
                            uiManager.OpenUI(UIIndex.Message);

                            SettingsSerializer.Save<StatusSettings>(new(statusSettings)
                            {
                                TheRestartAfterSavingSettingsWarningWasDisplayed = true,
                            });
                        }
                        else
                        {
                            SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                        }
                    }),

                    new ButtonOption(Localization_Statements.Return, Localization_GUIs.Button_Exit_Description, () =>
                    {
                        SoundEngine.Play(SoundEffectIndex.GUI_Click);
                        uiManager.CloseUI();
                    }),
                ]),
            ]);

            this.sectionUIs = new SectionUI[this.root.Sections.Length];
        }

        private void Save()
        {
            Section generalSection = this.root.Sections[(byte)SectionIndex.General];
            Section gameplaySection = this.root.Sections[(byte)SectionIndex.Gameplay];
            Section volumeSection = this.root.Sections[(byte)SectionIndex.Volume];
            Section videoSection = this.root.Sections[(byte)SectionIndex.Video];
            Section controlSection = this.root.Sections[(byte)SectionIndex.Control];
            Section cursorSection = this.root.Sections[(byte)SectionIndex.Cursor];

            GameCulture gameCulture = LocalizationConstants.GetGameCultureFromNativeName(Convert.ToString(generalSection.Options[(byte)GeneralSectionOptionIndex.Language].GetValue()));

            SettingsSerializer.Save<GeneralSettings>(new()
            {
                Language = gameCulture.Language,
                Region = gameCulture.Region,
            });

            SettingsSerializer.Save<GameplaySettings>(new()
            {
                ShowPreviewArea = Convert.ToBoolean(gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowPreviewArea].GetValue()),
                PreviewAreaColor = (Color)gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaColor].GetValue(),
                PreviewAreaColorA = Convert.ToByte(gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaOpacity].GetValue()),
                ShowGrid = Convert.ToBoolean(gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowGrid].GetValue()),
                GridOpacity = Convert.ToByte(gameplaySection.Options[(byte)GameplaySectionOptionIndex.GridOpacity].GetValue()),
                ShowTemperatureColorVariations = Convert.ToBoolean(gameplaySection.Options[(byte)GameplaySectionOptionIndex.ShowTemperatureColorVariations].GetValue()),
            });

            SettingsSerializer.Save<VolumeSettings>(new()
            {
                MasterVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.MasterVolume].GetValue()) / 100.0f,
                MusicVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.MusicVolume].GetValue()) / 100.0f,
                SFXVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.SFXVolume].GetValue()) / 100.0f,
            });

            SettingsSerializer.Save<VideoSettings>(new()
            {
                Framerate = Convert.ToSingle(videoSection.Options[(byte)VideoSectionOptionIndex.Framerate].GetValue()),
                Resolution = (Resolution)videoSection.Options[(byte)VideoSectionOptionIndex.Resolution].GetValue(),
                FullScreen = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.Fullscreen].GetValue()),
                VSync = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.VSync].GetValue()),
                Borderless = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.Borderless].GetValue()),
            });

            SettingsSerializer.Save<ControlSettings>(new()
            {
                MoveCameraUp = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraUp].GetValue(),
                MoveCameraRight = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraRight].GetValue(),
                MoveCameraDown = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraDown].GetValue(),
                MoveCameraLeft = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraLeft].GetValue(),
                MoveCameraFast = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraFast].GetValue(),
                TogglePause = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.TogglePause].GetValue(),
                ClearWorld = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.ClearWorld].GetValue(),
                NextShape = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.NextShape].GetValue(),
                Screenshot = (Keys)controlSection.Options[(byte)ControlSectionOptionIndex.Screenshot].GetValue(),
            });

            SettingsSerializer.Save<CursorSettings>(new()
            {
                Color = (Color)cursorSection.Options[(byte)CursorSectionOptionIndex.Color].GetValue(),
                BackgroundColor = (Color)cursorSection.Options[(byte)CursorSectionOptionIndex.BackgroundColor].GetValue(),
                Alpha = Convert.ToByte(cursorSection.Options[(byte)CursorSectionOptionIndex.Opacity].GetValue()),
                Scale = Convert.ToSingle(cursorSection.Options[(byte)CursorSectionOptionIndex.Scale].GetValue()),
            });
        }

        private void SyncSettingElements()
        {
            ControlSettings controlSettings = SettingsSerializer.Load<ControlSettings>();
            CursorSettings cursorSettings = SettingsSerializer.Load<CursorSettings>();
            GameplaySettings gameplaySettings = SettingsSerializer.Load<GameplaySettings>();
            GeneralSettings generalSettings = SettingsSerializer.Load<GeneralSettings>();
            VideoSettings videoSettings = SettingsSerializer.Load<VideoSettings>();
            VolumeSettings volumeSettings = SettingsSerializer.Load<VolumeSettings>();

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
            controlSection.Options[(byte)ControlSectionOptionIndex.MoveCameraFast].SetValue(controlSettings.MoveCameraFast);
            controlSection.Options[(byte)ControlSectionOptionIndex.TogglePause].SetValue(controlSettings.TogglePause);
            controlSection.Options[(byte)ControlSectionOptionIndex.ClearWorld].SetValue(controlSettings.ClearWorld);
            controlSection.Options[(byte)ControlSectionOptionIndex.NextShape].SetValue(controlSettings.NextShape);
            controlSection.Options[(byte)ControlSectionOptionIndex.Screenshot].SetValue(controlSettings.Screenshot);

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
            CursorSettings cursorSettings = SettingsSerializer.Load<CursorSettings>();
            VideoSettings videoSettings = SettingsSerializer.Load<VideoSettings>();
            VolumeSettings volumeSettings = SettingsSerializer.Load<VolumeSettings>();

            SongEngine.ApplyVolumeSettings(volumeSettings);
            SoundEngine.ApplyVolumeSettings(volumeSettings);

            this.videoManager.ApplySettings(videoSettings);
            this.cursorManager.ApplySettings(cursorSettings);
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
                    Label label = this.optionBuilders[option.GetType()]?.Invoke(option);

                    label.SetData("option", option);
                    label.Margin = new(32.0f, scrollableContainerMarginY);

                    this.scrollableContainer.AddChild(label);
                    contentBuffer[j] = label;
                }

                this.sectionUIs[i] = new(sectionLabel, contentBuffer);
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
            label.SetData("color_slot", colorSlot);
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
            label.SetData("toogle_preview", togglePreviewImageElement);
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

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;
            UpdateScrollableContainer();
            UpdateScrollbar();
            UpdateSectionLabels();
            UpdateSectionOptions();
        }

        private void UpdateScrollableContainer()
        {
            float marginY = this.scrollableContainer.Margin.Y;

            if (Interaction.OnMouseScrollUp())
            {
                marginY -= SCROLL_STEP;
            }
            else if (Interaction.OnMouseScrollDown())
            {
                marginY += SCROLL_STEP;
            }

            float topLimit = 0.0f;
            float bottomLimit = this.scrollableContainer.ChildCount * ITEM_SPACING * -1f;

            this.scrollableContainer.Margin = new(this.scrollableContainer.Margin.X, float.Clamp(marginY, bottomLimit, topLimit));
        }

        private void UpdateScrollbar()
        {
            if (Interaction.OnMouseLeftClick(this.scrollbarUpButton))
            {
                float marginY = this.scrollableContainer.Margin.Y + SCROLL_STEP;
                float bottomLimit = this.scrollableContainer.ChildCount * ITEM_SPACING * -1f;
                this.scrollableContainer.Margin = new(this.scrollableContainer.Margin.X, float.Clamp(marginY, bottomLimit, 0.0f));
            }
            else if (Interaction.OnMouseLeftClick(this.scrollbarDownButton))
            {
                float marginY = this.scrollableContainer.Margin.Y - SCROLL_STEP;
                float bottomLimit = this.scrollableContainer.ChildCount * ITEM_SPACING * -1f;
                this.scrollableContainer.Margin = new(this.scrollableContainer.Margin.X, float.Clamp(marginY, bottomLimit, 0.0f));
            }

            int mouseY = Input.MouseState.Y;

            float scrollableHeight = this.scrollableContainer.ChildCount * ITEM_SPACING;
            float backgroundHeight = this.background.Size.Y;
            float scrollableMarginY = this.scrollableContainer.Margin.Y;

            float sliderMinY = this.scrollbarUpButton.Size.Y;
            float sliderMaxY = backgroundHeight - this.scrollbarSliderButton.Size.Y - this.scrollbarDownButton.Size.Y;

            // Start dragging by pressing the left mouse button over the slider
            if (Input.MouseState.LeftButton == ButtonState.Pressed)
            {
                if (!this.isDraggingScrollbar && Interaction.OnMouseOver(this.scrollbarSliderButton))
                {
                    this.isDraggingScrollbar = true;
                    // Distance between the click point and the current slider margin
                    this.scrollbarDragOffsetY = mouseY - this.scrollbarSliderButton.Margin.Y;
                }
            }
            else
            {
                // Release
                this.isDraggingScrollbar = false;
            }

            if (this.isDraggingScrollbar)
            {
                // Calculate the new slider position based on the mouse, applying an offset.
                float desiredSliderY = mouseY - this.scrollbarDragOffsetY;
                desiredSliderY = float.Clamp(desiredSliderY, sliderMinY, sliderMaxY);

                this.scrollbarSliderButton.Margin = new(this.scrollbarSliderButton.Margin.X, desiredSliderY);

                // Convert slider position to scrollable content margin (inverse of calculation below)
                float newScrollableMarginY = -desiredSliderY / sliderMaxY * scrollableHeight;
                float bottomLimit = this.scrollableContainer.ChildCount * ITEM_SPACING * -1f;
                this.scrollableContainer.Margin = new(this.scrollableContainer.Margin.X, float.Clamp(newScrollableMarginY, bottomLimit, 0.0f));

                // While dragging, do not recalculate the slider based on the current margin to avoid value "conflicts".
                return;
            }

            // Updates the slider's visual position based on the current content margin.
            float sliderY = -scrollableMarginY / scrollableHeight * sliderMaxY;
            sliderY = float.Clamp(sliderY, sliderMinY, sliderMaxY);

            this.scrollbarSliderButton.Margin = new(this.scrollbarSliderButton.Margin.X, sliderY);
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

                    if (Interaction.OnMouseEnter(label))
                    {
                        SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                    }

                    if (Interaction.OnMouseLeftClick(label))
                    {
                        SoundEngine.Play(SoundEffectIndex.GUI_Click);
                        this.optionHandlers[option.GetType()]?.Invoke(option);
                        break;
                    }

                    this.optionSyncHandlers[option.GetType()]?.Invoke(option, label);

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

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
            SyncSettingElements();
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}

