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

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class OptionsUI : UIBase
    {
        private Category selectedCategory;
        private int currentPageIndex = 0, totalPages = 0;

        private Range selectedOptionsRange;

        private Image panelBackground, shadowBackground;
        private Label title, pageIndexLabel;

        private SlotInfo exitButtonSlotInfo;
        private SlotInfo[] categoryButtonSlotInfos;

        private readonly SlotInfo[] paginationButtonSlotInfos;
        private readonly OptionSlotInfo[] optionButtonSlotInfos;

        private readonly ButtonInfo exitButtonInfo;
        private readonly ButtonInfo[] categoryButtonInfos, paginationButtonInfos;

        private readonly TooltipBox tooltipBox;

        private readonly Category[] categories;

        private readonly SelectorUI.IChoice[] availableGameCulturesChoices;
        private readonly SelectorUI.IChoice[] resolutionChoices;

        internal OptionsUI(
            ColorPickerUI colorPickerUI,
            CursorManager cursorManager,
            KeySelectorUI keySelectorUI,
            PlayerInputController playerInputController,
            SelectorUI selectorUI,
            SliderUI sliderUI,
            StardustSandboxGame stardustSandboxGame,
            TooltipBox tooltipBox,
            UIManager uiManager,
            VideoManager videoManager
        ) : base()
        {
            this.tooltipBox = tooltipBox;

            ControlSettings controlSettings = SettingsSerializer.Load<ControlSettings>();
            CursorSettings cursorSettings = SettingsSerializer.Load<CursorSettings>();
            GameplaySettings gameplaySettings = SettingsSerializer.Load<GameplaySettings>();
            GeneralSettings generalSettings = SettingsSerializer.Load<GeneralSettings>();
            VideoSettings videoSettings = SettingsSerializer.Load<VideoSettings>();
            VolumeSettings volumeSettings = SettingsSerializer.Load<VolumeSettings>();

            this.availableGameCulturesChoices = new SelectorUI.IChoice[LocalizationConstants.AVAILABLE_GAME_CULTURES.Length];
            this.resolutionChoices = new SelectorUI.IChoice[ScreenConstants.RESOLUTIONS.Length];

            for (int i = 0; i < LocalizationConstants.AVAILABLE_GAME_CULTURES.Length; i++)
            {
                GameCulture gameCulture = LocalizationConstants.AVAILABLE_GAME_CULTURES[i];
                this.availableGameCulturesChoices[i] = new SelectorUI.Choice<GameCulture>(gameCulture.CultureInfo.NativeName, gameCulture);
            }

            for (int i = 0; i < ScreenConstants.RESOLUTIONS.Length; i++)
            {
                Point resolution = ScreenConstants.RESOLUTIONS[i];
                this.resolutionChoices[i] = new SelectorUI.Choice<Point>(string.Concat(resolution.X, " x ", resolution.Y), resolution);
            }

            this.categories =
            [
                // [0] General
                new Category(
                    Localization_GUIs.Options_General_Name,
                    Localization_GUIs.Options_General_Description,
                    TextureIndex.IconUI,
                    new Rectangle(256, 160, 32, 32),
                    new Option<GameCulture>(
                        Localization_GUIs.Options_General_Language_Name,
                        Localization_GUIs.Options_General_Language_Description,
                        generalSettings.GetGameCulture,
                        (value) =>
                        {
                            return value.CultureInfo.NativeName;
                        },
                        (option, optionSlotInfo) =>
                        {
                            selectorUI.Setup(
                                Localization_GUIs.Options_General_Language_Name,
                                (choice) =>
                                {
                                    generalSettings.SetGameCulture((GameCulture)choice.Value);
                                    SettingsSerializer.Save(generalSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();
                                },
                                this.availableGameCulturesChoices
                            );

                            uiManager.OpenUI(UIIndex.Selector);
                        }
                    )
                    {
                        Options = LocalizationConstants.AVAILABLE_GAME_CULTURES,
                        RequiresApplicationRestart = true
                    }
                ),

                // [1] Gameplay
                new Category(
                    Localization_GUIs.Options_Gameplay_Name,
                    Localization_GUIs.Options_Gameplay_Description,
                    TextureIndex.IconUI,
                    new Rectangle(256, 128, 32, 32),
                    new Option<bool>(
                        Localization_GUIs.Options_Gameplay_ShowPreviewArea_Name,
                        Localization_GUIs.Options_Gameplay_ShowPreviewArea_Description,
                        () =>
                        {
                            return gameplaySettings.ShowPreviewArea;
                        },
                        (value) =>
                        {
                            return value ? Localization_Statements.Enabled : Localization_Statements.Disabled;
                        },
                        (option, optionSlotInfo) =>
                        {
                            gameplaySettings.ShowPreviewArea = !gameplaySettings.ShowPreviewArea;
                            SettingsSerializer.Save(gameplaySettings);

                            optionSlotInfo.Value.TextContent = option.GetValueString();
                        }
                    ),
                    new Option<Color>(
                        Localization_GUIs.Options_Gameplay_PreviewAreaColor_Name,
                        Localization_GUIs.Options_Gameplay_PreviewAreaColor_Description,
                        () =>
                        {
                            return gameplaySettings.PreviewAreaColor;
                        },
                        (value) =>
                        {
                            return value.ToHexString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            colorPickerUI.Setup((newColor) =>
                            {
                                gameplaySettings.PreviewAreaColor = newColor;
                                SettingsSerializer.Save(gameplaySettings);

                                optionSlotInfo.Value.TextContent = option.GetValueString();
                            });

                            uiManager.OpenUI(UIIndex.ColorPicker);
                        }
                    ),
                    new Option<float>(
                        Localization_GUIs.Options_Gameplay_PreviewAreaOpacity_Name,
                        Localization_GUIs.Options_Gameplay_PreviewAreaOpacity_Description,
                        () =>
                        {
                            return gameplaySettings.PreviewAreaColorOpacity * 100.0f;
                        },
                        (value) =>
                        {
                            return string.Concat((int)value, '%');
                        },
                        (option, optionSlotInfo) =>
                        {
                            sliderUI.Setup(
                                Localization_GUIs.Options_Gameplay_PreviewAreaOpacity_Description,
                                new(0, 100),
                                Convert.ToInt32(option.GetValue()),
                                (newValue) => {
                                    gameplaySettings.PreviewAreaColorOpacity = newValue / 100.0f;
                                    SettingsSerializer.Save(gameplaySettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();
                                }
                            );

                            uiManager.OpenUI(UIIndex.Slider);
                        }
                    ),
                    new Option<bool>(
                        Localization_GUIs.Options_Gameplay_ShowGrid_Name,
                        Localization_GUIs.Options_Gameplay_ShowGrid_Description,
                        () =>
                        {
                            return gameplaySettings.ShowGrid;
                        },
                        (value) =>
                        {
                            return value ? Localization_Statements.Enabled : Localization_Statements.Disabled;
                        },
                        (option, optionSlotInfo) =>
                        {
                            gameplaySettings.ShowGrid = !gameplaySettings.ShowGrid;
                            SettingsSerializer.Save(gameplaySettings);

                            optionSlotInfo.Value.TextContent = option.GetValueString();
                        }
                    ),
                    new Option<float>(
                        Localization_GUIs.Options_Gameplay_GridOpacity_Name,
                        Localization_GUIs.Options_Gameplay_GridOpacity_Description,
                        () =>
                        {
                            return gameplaySettings.GridOpacity * 100.0f;
                        },
                        (value) =>
                        {
                            return string.Concat((int)value, '%');
                        },
                        (option, optionSlotInfo) =>
                        {
                            sliderUI.Setup(
                                Localization_GUIs.Options_Gameplay_GridOpacity_Description,
                                new(0, 100),
                                Convert.ToInt32(option.GetValue()),
                                (newValue) => {
                                    gameplaySettings.GridOpacity = newValue / 100.0f;
                                    SettingsSerializer.Save(gameplaySettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();
                                }
                            );
                            uiManager.OpenUI(UIIndex.Slider);
                        }
                    ),
                    new Option<bool>(
                        Localization_GUIs.Options_Gameplay_ShowTemperatureColorVariations_Name,
                        Localization_GUIs.Options_Gameplay_ShowTemperatureColorVariations_Description,
                        () =>
                        {
                            return gameplaySettings.ShowTemperatureColorVariations;
                        },
                        (value) =>
                        {
                            return value ? Localization_Statements.Enabled : Localization_Statements.Disabled;
                        },
                        (option, optionSlotInfo) =>
                        {
                            gameplaySettings.ShowTemperatureColorVariations = !gameplaySettings.ShowTemperatureColorVariations;
                            SettingsSerializer.Save(gameplaySettings);

                            optionSlotInfo.Value.TextContent = option.GetValueString();
                        }
                    )
                ),

                // [2] Volume
                new Category(
                    Localization_GUIs.Options_Volume_Name,
                    Localization_GUIs.Options_Volume_Description,
                    TextureIndex.IconUI,
                    new Rectangle(320, 256, 32, 32),
                    new Option<float>(
                        Localization_GUIs.Options_Volume_MasterVolume_Name,
                        Localization_GUIs.Options_Volume_MasterVolume_Description,
                        () =>
                        {
                            return volumeSettings.MasterVolume * 100.0f;
                        },
                        (value) =>
                        {
                            return string.Concat((int)value, '%');
                        },
                        (option, optionSlotInfo) =>
                        {
                            sliderUI.Setup(
                                Localization_GUIs.Options_Volume_MasterVolume_Description,
                                new(0, 100),
                                Convert.ToInt32(option.GetValue()),
                                (newValue) => {
                                    volumeSettings.MasterVolume = newValue / 100.0f;
                                    SettingsSerializer.Save(volumeSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    SongEngine.ApplyVolumeSettings(volumeSettings);
                                    SoundEngine.ApplyVolumeSettings(volumeSettings);
                                }
                            );

                            uiManager.OpenUI(UIIndex.Slider);
                        }
                    ),
                    new Option<float>(
                        Localization_GUIs.Options_Volume_MusicVolume_Name,
                        Localization_GUIs.Options_Volume_MusicVolume_Description,
                        () =>
                        {
                            return volumeSettings.MusicVolume * 100.0f;
                        },
                        (value) =>
                        {
                            return string.Concat((int)value, '%');
                        },
                        (option, optionSlotInfo) =>
                        {
                            sliderUI.Setup(
                                Localization_GUIs.Options_Volume_MusicVolume_Description,
                                new(0, 100),
                                Convert.ToInt32(option.GetValue()),
                                (newValue) => {
                                    volumeSettings.MusicVolume = newValue / 100.0f;
                                    SettingsSerializer.Save(volumeSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    SongEngine.ApplyVolumeSettings(volumeSettings);
                                }
                            );

                            uiManager.OpenUI(UIIndex.Slider);
                        }
                    ),
                    new Option<float>(
                        Localization_GUIs.Options_Volume_SFXVolume_Name,
                        Localization_GUIs.Options_Volume_SFXVolume_Description,
                        () =>
                        {
                            return volumeSettings.SFXVolume * 100.0f;
                        },
                        (value) =>
                        {
                            return string.Concat((int)value, '%');
                        },
                        (option, optionSlotInfo) =>
                        {
                            sliderUI.Setup(
                                Localization_GUIs.Options_Volume_SFXVolume_Description,
                                new(0, 100),
                                Convert.ToInt32(option.GetValue()),
                                (newValue) => {
                                    volumeSettings.SFXVolume = newValue / 100.0f;
                                    SettingsSerializer.Save(volumeSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    SoundEngine.ApplyVolumeSettings(volumeSettings);
                                }
                            );

                            uiManager.OpenUI(UIIndex.Slider);
                        }
                    )
                ),

                // [3] Video
                new Category(
                    Localization_GUIs.Options_Video_Name,
                    Localization_GUIs.Options_Video_Description,
                    TextureIndex.IconUI,
                    new Rectangle(320, 192, 32, 32),
                    new Option<float>(
                        Localization_GUIs.Options_Video_Framerate_Name,
                        Localization_GUIs.Options_Video_Framerate_Description,
                        () =>
                        {
                            return videoSettings.Framerate;
                        },
                        (value) =>
                        {
                            return string.Concat((int)value, " FPS");
                        },
                        (option, optionSlotInfo) =>
                        {
                            sliderUI.Setup(
                                Localization_GUIs.Options_Video_Framerate_Description,
                                new(30, 240),
                                Convert.ToInt32(option.GetValue()),
                                (newValue) => {
                                    videoSettings.Framerate = newValue;
                                    SettingsSerializer.Save(videoSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    stardustSandboxGame.SetFrameRate(videoSettings.Framerate);
                                }
                            );
                            uiManager.OpenUI(UIIndex.Slider);
                        }
                    ),
                    new Option<Point>(
                        Localization_GUIs.Options_Video_Resolution_Name,
                        Localization_GUIs.Options_Video_Resolution_Description,
                        () =>
                        {
                            return videoSettings.Resolution;
                        },
                        (value) =>
                        {
                            return string.Concat(value.X, " x ", value.Y);
                        },
                        (option, optionSlotInfo) =>
                        {
                            selectorUI.Setup(
                                Localization_GUIs.Options_Video_Resolution_Name,
                                (choice) =>
                                {
                                    videoSettings.Resolution = (Point)choice.Value;
                                    SettingsSerializer.Save(videoSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    videoManager.SetResolution(videoSettings.Resolution);
                                },
                                this.resolutionChoices
                            );

                            uiManager.OpenUI(UIIndex.Selector);
                        }
                    ),
                    new Option<bool>(
                        Localization_GUIs.Options_Video_Fullscreen_Name,
                        Localization_GUIs.Options_Video_Fullscreen_Description,
                        () =>
                        {
                            return videoSettings.FullScreen;
                        },
                        (value) =>
                        {
                            return value ? Localization_Statements.Enabled : Localization_Statements.Disabled;
                        },
                        (option, optionSlotInfo) =>
                        {
                            videoSettings.FullScreen = !videoSettings.FullScreen;
                            SettingsSerializer.Save(videoSettings);

                            optionSlotInfo.Value.TextContent = option.GetValueString();

                            videoManager.SetFullScreen(videoSettings.FullScreen);
                        }
                    ),
                    new Option<bool>(
                        Localization_GUIs.Options_Video_VSync_Name,
                        Localization_GUIs.Options_Video_VSync_Description,
                        () =>
                        {
                            return videoSettings.VSync;
                        },
                        (value) =>
                        {
                            return value ? Localization_Statements.Enabled : Localization_Statements.Disabled;
                        },
                        (option, optionSlotInfo) =>
                        {
                            videoSettings.VSync = !videoSettings.VSync;
                            SettingsSerializer.Save(videoSettings);

                            optionSlotInfo.Value.TextContent = option.GetValueString();

                            videoManager.SetVSync(videoSettings.VSync);
                        }
                    ),
                    new Option<bool>(
                        Localization_GUIs.Options_Video_Borderless_Name,
                        Localization_GUIs.Options_Video_Borderless_Description,
                        () =>
                        {
                            return videoSettings.Borderless;
                        },
                        (value) =>
                        {
                            return value ? Localization_Statements.Enabled : Localization_Statements.Disabled;
                        },
                        (option, optionSlotInfo) =>
                        {
                            videoSettings.Borderless = !videoSettings.Borderless;
                            SettingsSerializer.Save(videoSettings);

                            optionSlotInfo.Value.TextContent = option.GetValueString();

                            videoManager.SetBorderless(videoSettings.Borderless);
                        }
                    )
                ),

                // [4] Controls
                new Category(
                    Localization_GUIs.Options_Controls_Name,
                    Localization_GUIs.Options_Controls_Description,
                    TextureIndex.IconUI,
                    new Rectangle(192, 256, 32, 32),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_MoveCameraUp_Name,
                        Localization_GUIs.Options_Controls_MoveCameraUp_Description,
                        () =>
                        {
                            return controlSettings.MoveCameraUpKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_MoveCameraUp_Description,
                                (newKey) =>
                                {
                                    controlSettings.MoveCameraUpKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.GameplayInputHandler.GetMap("Camera").GetAction("MoveUp").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    ),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_MoveCameraRight_Name,
                        Localization_GUIs.Options_Controls_MoveCameraRight_Description,
                        () =>
                        {
                            return controlSettings.MoveCameraRightKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_MoveCameraRight_Description,
                                (newKey) =>
                                {
                                    controlSettings.MoveCameraRightKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.GameplayInputHandler.GetMap("Camera").GetAction("MoveRight").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    ),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_MoveCameraDown_Name,
                        Localization_GUIs.Options_Controls_MoveCameraDown_Description,
                        () =>
                        {
                            return controlSettings.MoveCameraDownKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_MoveCameraDown_Description,
                                (newKey) =>
                                {
                                    controlSettings.MoveCameraDownKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.GameplayInputHandler.GetMap("Camera").GetAction("MoveDown").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    ),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_MoveCameraLeft_Name,
                        Localization_GUIs.Options_Controls_MoveCameraLeft_Description,
                        () =>
                        {
                            return controlSettings.MoveCameraLeftKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_MoveCameraLeft_Description,
                                (newKey) =>
                                {
                                    controlSettings.MoveCameraLeftKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.GameplayInputHandler.GetMap("Camera").GetAction("MoveLeft").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    ),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_MoveCameraFast_Name,
                        Localization_GUIs.Options_Controls_MoveCameraFast_Description,
                        () =>
                        {
                            return controlSettings.MoveCameraFastKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_MoveCameraFast_Description,
                                (newKey) =>
                                {
                                    controlSettings.MoveCameraFastKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.GameplayInputHandler.GetMap("Camera").GetAction("MoveFast").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    ),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_TogglePause_Name,
                        Localization_GUIs.Options_Controls_TogglePause_Description,
                        () =>
                        {
                            return controlSettings.TogglePauseKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_TogglePause_Description,
                                (newKey) =>
                                {
                                    controlSettings.TogglePauseKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.GameplayInputHandler.GetMap("Simulation").GetAction("TogglePause").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    ),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_ClearWorld_Name,
                        Localization_GUIs.Options_Controls_ClearWorld_Description,
                        () =>
                        {
                            return controlSettings.ClearWorldKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_ClearWorld_Description,
                                (newKey) =>
                                {
                                    controlSettings.ClearWorldKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.GameplayInputHandler.GetMap("Simulation").GetAction("ClearWorld").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    ),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_NextShape_Name,
                        Localization_GUIs.Options_Controls_NextShape_Description,
                        () =>
                        {
                            return controlSettings.NextShapeKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_NextShape_Description,
                                (newKey) =>
                                {
                                    controlSettings.NextShapeKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.GameplayInputHandler.GetMap("Simulation").GetAction("NextShape").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    ),
                    new Option<Keys>(
                        Localization_GUIs.Options_Controls_Screenshot_Name,
                        Localization_GUIs.Options_Controls_Screenshot_Description,
                        () =>
                        {
                            return controlSettings.ScreenshotKeyboardBinding;
                        },
                        (value) =>
                        {
                            return value.ToString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            keySelectorUI.Setup(Localization_GUIs.Options_Controls_Screenshot_Description,
                                (newKey) =>
                                {
                                    controlSettings.ScreenshotKeyboardBinding = newKey;
                                    SettingsSerializer.Save(controlSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    playerInputController.SystemInputHandler.GetMap("General").GetAction("Screenshot").KeyboardBinding = newKey;
                                }
                            );

                            uiManager.OpenUI(UIIndex.KeySelector);
                        }
                    )
                ),

                // [5] Cursor
                new Category(
                    Localization_GUIs.Options_Cursor_Name,
                    Localization_GUIs.Options_Cursor_Description,
                    TextureIndex.IconUI,
                    new Rectangle(320, 224, 32, 32),
                    new Option<Color>(
                        Localization_GUIs.Options_Cursor_Color_Name,
                        Localization_GUIs.Options_Cursor_Color_Description,
                        () =>
                        {
                            return cursorSettings.Color;
                        },
                        (value) =>
                        {
                            return value.ToHexString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            colorPickerUI.Setup((newColor) =>
                            {
                                cursorSettings.Color = newColor;
                                SettingsSerializer.Save(cursorSettings);

                                optionSlotInfo.Value.TextContent = option.GetValueString();

                                cursorManager.Color = newColor;
                            });

                            uiManager.OpenUI(UIIndex.ColorPicker);
                        }
                    ),
                    new Option<Color>(
                        Localization_GUIs.Options_Cursor_BackgroundColor_Name,
                        Localization_GUIs.Options_Cursor_BackgroundColor_Description,
                        () =>
                        {
                            return cursorSettings.BackgroundColor;
                        },
                        (value) =>
                        {
                            return value.ToHexString();
                        },
                        (option, optionSlotInfo) =>
                        {
                            colorPickerUI.Setup((newColor) =>
                            {
                                cursorSettings.BackgroundColor = newColor;
                                SettingsSerializer.Save(cursorSettings);

                                optionSlotInfo.Value.TextContent = option.GetValueString();

                                cursorManager.BackgroundColor = newColor;
                            });

                            uiManager.OpenUI(UIIndex.ColorPicker);
                        }
                    ),
                    new Option<float>(
                        Localization_GUIs.Options_Cursor_Scale_Name,
                        Localization_GUIs.Options_Cursor_Scale_Description,
                        () =>
                        {
                            return cursorSettings.Scale * 100.0f;
                        },
                        (value) =>
                        {
                            return string.Concat((int)value, '%');
                        },
                        (option, optionSlotInfo) =>
                        {
                            sliderUI.Setup(
                                Localization_GUIs.Options_Cursor_Scale_Description,
                                new(50, 500),
                                Convert.ToInt32(option.GetValue()),
                                (newValue) => {
                                    cursorSettings.Scale = newValue / 100.0f;
                                    SettingsSerializer.Save(cursorSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    cursorManager.Scale = cursorSettings.Scale;
                                }
                            );

                            uiManager.OpenUI(UIIndex.Slider);
                        }
                    ),
                    new Option<float>(
                        Localization_GUIs.Options_Cursor_Opacity_Name,
                        Localization_GUIs.Options_Cursor_Opacity_Description,
                        () =>
                        {
                            return cursorSettings.Opacity * 100.0f;
                        },
                        (value) =>
                        {
                            return string.Concat((int)value, '%');
                        },
                        (option, optionSlotInfo) =>
                        {
                            sliderUI.Setup(
                                Localization_GUIs.Options_Cursor_Opacity_Description,
                                new(0, 100),
                                Convert.ToInt32(option.GetValue()),
                                (newValue) => {
                                    cursorSettings.Opacity = newValue / 100.0f;
                                    SettingsSerializer.Save(cursorSettings);

                                    optionSlotInfo.Value.TextContent = option.GetValueString();

                                    cursorManager.Opacity = cursorSettings.Opacity;
                                }
                            );

                            uiManager.OpenUI(UIIndex.Slider);
                        }
                    )
                ),
            ];

            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI);
            this.categoryButtonInfos = new ButtonInfo[this.categories.Length];
            this.optionButtonSlotInfos = new OptionSlotInfo[UIConstants.OPTIONS_PER_PAGE];

            this.paginationButtonInfos =
            [
                new(TextureIndex.IconUI, new(128, 160, 32, 32), Localization_Statements.Previous, string.Empty, () =>
                {
                    if (this.currentPageIndex > 0)
                    {
                        this.currentPageIndex--;
                    }
                    else
                    {
                        this.currentPageIndex = this.totalPages - 1;
                    }

                    RefreshContent();
                }),
                new(TextureIndex.IconUI, new(64, 160, 32, 32), Localization_Statements.Next, string.Empty, () =>
                {
                    if (this.currentPageIndex < this.totalPages - 1)
                    {
                        this.currentPageIndex++;
                    }
                    else
                    {
                        this.currentPageIndex = 0;
                    }

                    RefreshContent();
                }),
            ];

            for (int i = 0; i < this.categories.Length; i++)
            {
                this.categoryButtonInfos[i] = new(this.categories[i].TextureIndex, this.categories[i].TextureSourceRectangle, this.categories[i].Name, this.categories[i].Description, null);
            }

            this.paginationButtonSlotInfos = new SlotInfo[this.paginationButtonInfos.Length];
        }

        private void RefreshContent()
        {
            this.pageIndexLabel.TextContent = string.Concat(this.currentPageIndex + 1, " / ", this.totalPages);

            this.selectedOptionsRange = new(
                this.currentPageIndex * UIConstants.OPTIONS_PER_PAGE,
                Math.Min(
                    this.currentPageIndex * UIConstants.OPTIONS_PER_PAGE + UIConstants.OPTIONS_PER_PAGE,
                    this.selectedCategory.Length
                )
            );

            int length = this.selectedOptionsRange.End.Value - this.selectedOptionsRange.Start.Value;

            for (int i = 0; i < this.optionButtonSlotInfos.Length; i++)
            {
                OptionSlotInfo slot = this.optionButtonSlotInfos[i];

                if (i < length)
                {
                    IOption option = this.selectedCategory[this.selectedOptionsRange.Start.Value + i];

                    slot.Background.CanDraw = true;
                    slot.Title.TextContent = option.Name.Truncate(32);
                    slot.Value.TextContent = option.GetValueString();
                }
                else
                {
                    slot.Background.CanDraw = false;
                }
            }
        }

        private void SelectCategory(int index)
        {
            this.selectedCategory = this.categories[index];
            this.title.TextContent = this.selectedCategory.Name;

            this.currentPageIndex = 0;
            this.totalPages = (int)MathF.Max(1.0f, MathF.Ceiling(this.selectedCategory.Length / (float)UIConstants.OPTIONS_PER_PAGE));

            RefreshContent();
        }

        internal void Setup()
        {
            SelectCategory(0);
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildExitButton();
            BuildCategoryButtons();
            BuildOptionButtons();
            BuildPagination();

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Size = Vector2.One,
                Color = new(AAP64ColorPalette.DarkGray, 160)
            };

            this.panelBackground = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundOptions,
                Size = new(1084.0f, 607.0f),
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.panelBackground);
        }

        private void BuildTitle()
        {
            this.title = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Margin = new(96.0f, 10.0f),
                TextContent = "Options",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.panelBackground.AddChild(this.title);
        }

        private void BuildExitButton()
        {
            SlotInfo slot = UIBuilderUtility.BuildButtonSlot(new(-4.0f, 6.5f), this.exitButtonInfo);

            slot.Background.Alignment = UIDirection.Northeast;
            slot.Icon.Alignment = UIDirection.Center;

            this.panelBackground.AddChild(slot.Background);
            slot.Background.AddChild(slot.Icon);

            this.exitButtonSlotInfo = slot;
        }

        private void BuildCategoryButtons()
        {
            this.categoryButtonSlotInfos = UIBuilderUtility.BuildVerticalButtonLine(
                this.panelBackground,
                this.categoryButtonInfos,
                new(8.0f, 8.0f),
                74.0f,
                UIDirection.Northwest
            );
        }

        private void BuildOptionButtons()
        {
            Vector2 firstColumnMargin = new(96.0f, 92.0f);
            Vector2 secondColumnMargin = new(589.0f, 92.0f);
            Vector2 spacing = new(12.0f);
            Vector2 size = new(477.0f, 61.0f);

            int index = 0;

            for (int row = 0; row < UIConstants.OPTIONS_PER_ROW; row++)
            {
                for (int column = 0; column < UIConstants.OPTIONS_PER_COLUMN; column++)
                {
                    Vector2 margin = column % 2 == 0 ? firstColumnMargin : secondColumnMargin;
                    margin.Y += row * (size.Y + spacing.Y);

                    Image background = new()
                    {
                        TextureIndex = TextureIndex.UIButtons,
                        SourceRectangle = new(0, 300, 477, 61),
                        Size = size,
                        Alignment = UIDirection.Northwest,
                        Margin = margin,
                    };

                    Label title = new()
                    {
                        SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                        Scale = new(0.065f),
                        Margin = new(16.0f, 0.0f),
                        TextContent = "Title",
                        Alignment = UIDirection.West,

                        BorderColor = AAP64ColorPalette.DarkGray,
                        BorderDirections = LabelBorderDirection.All,
                        BorderOffset = 2.0f,
                        BorderThickness = 2.0f,
                    };

                    Label value = new()
                    {
                        SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                        Scale = new(0.065f),
                        Margin = new(-16.0f, 0.0f),
                        TextContent = "Value",

                        Alignment = UIDirection.East,
                        BorderColor = AAP64ColorPalette.DarkGray,
                        BorderDirections = LabelBorderDirection.All,
                        BorderOffset = 2.0f,
                        BorderThickness = 2.0f,
                    };

                    this.panelBackground.AddChild(background);
                    background.AddChild(title);
                    background.AddChild(value);

                    this.optionButtonSlotInfos[index] = new(background, title, value);
                    index++;
                }
            }
        }

        private void BuildPagination()
        {
            this.pageIndexLabel = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = UIDirection.South,
                Margin = new(0.0f, -12.0f),
                TextContent = "1 / 1",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.panelBackground.AddChild(this.pageIndexLabel);

            for (int i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = new(
                    new()
                    {
                        TextureIndex = TextureIndex.UIButtons,
                        SourceRectangle = new(320, 140, 32, 32),
                        Scale = new(1.6f),
                        Size = new(32.0f),
                    },

                    new()
                    {
                        TextureIndex = this.paginationButtonInfos[i].TextureIndex,
                        SourceRectangle = this.paginationButtonInfos[i].TextureSourceRectangle,
                        Alignment = UIDirection.Center,
                        Size = new(32.0f)
                    }
                );

                // Spacing
                this.paginationButtonSlotInfos[i] = slot;

                // Adding
                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }

            SlotInfo left = this.paginationButtonSlotInfos[0];
            left.Background.Alignment = UIDirection.Southwest;
            left.Background.Margin = new(86.0f, -9.0f);

            SlotInfo right = this.paginationButtonSlotInfos[1];
            right.Background.Alignment = UIDirection.Southeast;
            right.Background.Margin = new(-9.0f);

            for (int i = 0; i < this.paginationButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }
        }

        protected override void OnResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
            UpdateCategoryButtons();
            UpdateOptionButtons();
            UpdatePagination();
        }

        private void UpdateExitButton()
        {
            if (Interaction.OnMouseEnter(this.exitButtonSlotInfo.Background))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Hover);
            }

            if (Interaction.OnMouseLeftClick(this.exitButtonSlotInfo.Background))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Click);
                this.exitButtonInfo.ClickAction?.Invoke();
            }

            if (Interaction.OnMouseOver(this.exitButtonSlotInfo.Background))
            {
                this.tooltipBox.CanDraw = true;

                TooltipBoxContent.SetTitle(this.exitButtonInfo.Name);
                TooltipBoxContent.SetDescription(this.exitButtonInfo.Description);

                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.HoverColor;
            }
            else
            {
                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.White;
            }
        }

        private void UpdateCategoryButtons()
        {
            for (int i = 0; i < this.categoryButtonSlotInfos.Length; i++)
            {
                SlotInfo slotInfo = this.categoryButtonSlotInfos[i];
                ButtonInfo buttonInfo = this.categoryButtonInfos[i];

                if (Interaction.OnMouseEnter(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    SelectCategory(i);
                    break;
                }

                if (Interaction.OnMouseOver(slotInfo.Background))
                {
                    this.tooltipBox.CanDraw = true;
                    TooltipBoxContent.SetTitle(buttonInfo.Name);
                    TooltipBoxContent.SetDescription(buttonInfo.Description);
                    slotInfo.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slotInfo.Background.Color = this.selectedCategory == this.categories[i] ? AAP64ColorPalette.TealGray : AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateOptionButtons()
        {
            for (int i = this.selectedOptionsRange.Start.Value; i < this.selectedOptionsRange.End.Value; i++)
            {
                OptionSlotInfo slot = this.optionButtonSlotInfos[i % UIConstants.OPTIONS_PER_PAGE];
                IOption option = this.selectedCategory[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    option.SetValue(slot);
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;
                    TooltipBoxContent.SetTitle(option.Name);
                    TooltipBoxContent.SetDescription(option.Description);
                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdatePagination()
        {
            for (int i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.paginationButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}
