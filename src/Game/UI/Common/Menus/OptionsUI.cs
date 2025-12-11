using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.Mathematics.Primitives;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;
using StardustSandbox.UI.Common.Menus.Options;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.UI.Settings;

using System;
using System.Collections.Generic;

namespace StardustSandbox.UI.Common.Menus
{
    internal sealed class OptionsUI : UIBase
    {
        private enum SectionIndex : byte { General, Gameplay, Volume, Video, Cursor }
        private enum GeneralSectionOptionIndex : byte { Language }
        private enum GameplaySectionOptionIndex : byte { PreviewAreaColor, PreviewAreaOpacity }
        private enum VolumeSectionOptionIndex : byte { MasterVolume, MusicVolume, SFXVolume }
        private enum VideoSectionOptionIndex : byte { Framerate, Resolution, Fullscreen, VSync, Borderless }
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

        private bool restartMessageAppeared;

        private Label titleLabel;
        private Image background;

        private Container scrollableContainer;

        private readonly Root root;
        private readonly ColorPickerSettings colorPickerSettings;
        private readonly ColorPickerUI colorPickerUI;
        private readonly MessageUI messageUI;

        private readonly string titleName = Localization_GUIs.Menu_Options_Title;

        private readonly List<PlusMinusButtonInfo> plusMinusButtons = [];
        private readonly ButtonInfo[] systemButtonInfos;
        private readonly Label[] systemButtonLabels;
        private readonly TooltipBox tooltipBox;

        private readonly CursorManager cursorManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;

        private readonly SectionUI[] sectionUIs;

        private readonly GeneralSettings generalSettings;
        private readonly GameplaySettings gameplaySettings;
        private readonly VolumeSettings volumeSettings;
        private readonly VideoSettings videoSettings;
        private readonly CursorSettings cursorSettings;

        internal OptionsUI(
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
            this.generalSettings = SettingsSerializer.LoadSettings<GeneralSettings>();
            this.gameplaySettings = SettingsSerializer.LoadSettings<GameplaySettings>();
            this.volumeSettings = SettingsSerializer.LoadSettings<VolumeSettings>();
            this.videoSettings = SettingsSerializer.LoadSettings<VideoSettings>();
            this.cursorSettings = SettingsSerializer.LoadSettings<CursorSettings>();

            this.systemButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Save, Localization_GUIs.Menu_Options_Button_Save_Description, () =>
                {
                    SaveSettings();
                    ApplySettings();
                    if (!this.restartMessageAppeared)
                    {
                        messageUI.SetContent(Localization_Messages.Settings_RestartRequired);
                        uiManager.OpenGUI(UIIndex.Message);
                        this.restartMessageAppeared = true;
                    }
                }),
                new(TextureIndex.None, null, Localization_Statements.Return, Localization_GUIs.Button_Exit_Description, uiManager.CloseGUI),
            ];

            this.root = new([
                new(Localization_GUIs.Menu_Options_Section_General_Name, Localization_GUIs.Menu_Options_Section_General_Description,
                [
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_General_Option_Language_Name, Localization_GUIs.Menu_Options_Section_General_Option_Language_Description, Array.ConvertAll<GameCulture, object>(LocalizationConstants.AVAILABLE_GAME_CULTURES, x => x))
                ]),
                new(Localization_GUIs.Menu_Options_Section_Gameplay_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Description,
                [
                    new ColorOption(Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaColor_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaColor_Description),
                    new ValueOption(Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaOpacity_Name, Localization_GUIs.Menu_Options_Section_Gameplay_Option_PreviewAreaOpacity_Description, byte.MinValue, byte.MaxValue)
                ]),
                new(Localization_GUIs.Menu_Options_Section_Volume_Name, Localization_GUIs.Menu_Options_Section_Volume_Description,
                [
                    new ValueOption(Localization_GUIs.Menu_Options_Section_Volume_Option_MasterVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_MasterVolume_Description, 0, 100),
                    new ValueOption(Localization_GUIs.Menu_Options_Section_Volume_Option_MusicVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_MusicVolume_Description, 0, 100),
                    new ValueOption(Localization_GUIs.Menu_Options_Section_Volume_Option_SFXVolume_Name, Localization_GUIs.Menu_Options_Section_Volume_Option_SFXVolume_Description, 0, 100)
                ]),
                new(Localization_GUIs.Menu_Options_Section_Video_Name, Localization_GUIs.Menu_Options_Section_Video_Description,
                [
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_Video_Option_Framerate_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Framerate_Description, Array.ConvertAll<float, object>(ScreenConstants.FRAMERATES, x => x)),
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_Video_Option_Resolution_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Resolution_Description, Array.ConvertAll<Resolution, object>(ScreenConstants.RESOLUTIONS, x => x)),
                    new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_Fullscreen_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Fullscreen_Description),
                    new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_VSync_Name, Localization_GUIs.Menu_Options_Section_Video_Option_VSync_Description),
                    new ToggleOption(Localization_GUIs.Menu_Options_Section_Video_Option_Borderless_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Borderless_Description)
                ]),
                new(Localization_GUIs.Menu_Options_Section_Cursor_Name, Localization_GUIs.Menu_Options_Section_Cursor_Description,
                [
                    new ColorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Color_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Color_Description),
                    new ColorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_BackgroundColor_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_BackgroundColor_Description),
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Scale_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Scale_Description, [0.5f, 1f, 1.5f, 2f, 2.5f, 3f]),
                    new ValueOption(Localization_GUIs.Menu_Options_Section_Cursor_Option_Opacity_Name, Localization_GUIs.Menu_Options_Section_Cursor_Option_Opacity_Description, byte.MinValue, byte.MaxValue)
                ]),
            ]);

            this.sectionUIs = new SectionUI[this.root.Sections.Length];
            this.systemButtonLabels = new Label[this.systemButtonInfos.Length];
        }

        private void SaveSettings()
        {
            Section generalSection = this.root.Sections[(byte)SectionIndex.General];
            GameCulture gameCulture = LocalizationConstants.GetGameCultureFromNativeName(Convert.ToString(generalSection.Options[(byte)GeneralSectionOptionIndex.Language].GetValue()));
            this.generalSettings.Language = gameCulture.Language;
            this.generalSettings.Region = gameCulture.Region;
            SettingsSerializer.SaveSettings(this.generalSettings);

            Section gameplaySection = this.root.Sections[(byte)SectionIndex.Gameplay];
            this.gameplaySettings.PreviewAreaColor = (Color)gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaColor].GetValue();
            this.gameplaySettings.PreviewAreaColorA = Convert.ToByte(gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaOpacity].GetValue());
            SettingsSerializer.SaveSettings(this.gameplaySettings);

            Section volumeSection = this.root.Sections[(byte)SectionIndex.Volume];
            this.volumeSettings.MasterVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.MasterVolume].GetValue()) / 100.0f;
            this.volumeSettings.MusicVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.MusicVolume].GetValue()) / 100.0f;
            this.volumeSettings.SFXVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.SFXVolume].GetValue()) / 100.0f;
            SettingsSerializer.SaveSettings(this.volumeSettings);

            Section videoSection = this.root.Sections[(byte)SectionIndex.Video];
            this.videoSettings.Framerate = Convert.ToSingle(videoSection.Options[(byte)VideoSectionOptionIndex.Framerate].GetValue());
            this.videoSettings.Resolution = (Resolution)videoSection.Options[(byte)VideoSectionOptionIndex.Resolution].GetValue();
            this.videoSettings.FullScreen = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.Fullscreen].GetValue());
            this.videoSettings.VSync = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.VSync].GetValue());
            this.videoSettings.Borderless = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.Borderless].GetValue());
            SettingsSerializer.SaveSettings(this.videoSettings);

            Section cursorSection = this.root.Sections[(byte)SectionIndex.Cursor];
            this.cursorSettings.Color = (Color)cursorSection.Options[(byte)CursorSectionOptionIndex.Color].GetValue();
            this.cursorSettings.BackgroundColor = (Color)cursorSection.Options[(byte)CursorSectionOptionIndex.BackgroundColor].GetValue();
            this.cursorSettings.Alpha = Convert.ToByte(cursorSection.Options[(byte)CursorSectionOptionIndex.Opacity].GetValue());
            this.cursorSettings.Scale = Convert.ToSingle(cursorSection.Options[(byte)CursorSectionOptionIndex.Scale].GetValue());
            SettingsSerializer.SaveSettings(this.cursorSettings);
        }

        private void SyncSettingElements()
        {
            Section generalSection = this.root.Sections[(byte)SectionIndex.General];
            generalSection.Options[(byte)GeneralSectionOptionIndex.Language].SetValue(this.generalSettings.GameCulture);

            Section gameplaySection = this.root.Sections[(byte)SectionIndex.Gameplay];
            gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaColor].SetValue(new Color(this.gameplaySettings.PreviewAreaColor, 255));
            gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaOpacity].SetValue(this.gameplaySettings.PreviewAreaColorA);

            Section volumeSection = this.root.Sections[(byte)SectionIndex.Volume];
            volumeSection.Options[(byte)VolumeSectionOptionIndex.MasterVolume].SetValue(this.volumeSettings.MasterVolume * 100.0f);
            volumeSection.Options[(byte)VolumeSectionOptionIndex.MusicVolume].SetValue(this.volumeSettings.MusicVolume * 100.0f);
            volumeSection.Options[(byte)VolumeSectionOptionIndex.SFXVolume].SetValue(this.volumeSettings.SFXVolume * 100.0f);

            Section videoSection = this.root.Sections[(byte)SectionIndex.Video];
            videoSection.Options[(byte)VideoSectionOptionIndex.Framerate].SetValue(this.videoSettings.Framerate);
            videoSection.Options[(byte)VideoSectionOptionIndex.Resolution].SetValue(this.videoSettings.Resolution);
            videoSection.Options[(byte)VideoSectionOptionIndex.Fullscreen].SetValue(this.videoSettings.FullScreen);
            videoSection.Options[(byte)VideoSectionOptionIndex.VSync].SetValue(this.videoSettings.VSync);
            videoSection.Options[(byte)VideoSectionOptionIndex.Borderless].SetValue(this.videoSettings.Borderless);

            Section cursorSection = this.root.Sections[(byte)SectionIndex.Cursor];
            cursorSection.Options[(byte)CursorSectionOptionIndex.Color].SetValue(new Color(this.cursorSettings.Color, 255));
            cursorSection.Options[(byte)CursorSectionOptionIndex.BackgroundColor].SetValue(new Color(this.cursorSettings.BackgroundColor, 255));
            cursorSection.Options[(byte)CursorSectionOptionIndex.Opacity].SetValue(this.cursorSettings.Alpha);
            cursorSection.Options[(byte)CursorSectionOptionIndex.Scale].SetValue(this.cursorSettings.Scale);
        }

        private void ApplySettings()
        {
            MediaPlayer.Volume = this.volumeSettings.MusicVolume * this.volumeSettings.MasterVolume;
            SoundEffect.MasterVolume = this.volumeSettings.SFXVolume * this.volumeSettings.MasterVolume;
            this.videoManager.ApplySettings();
            this.cursorManager.ApplySettings();
        }

        protected override void OnBuild(Container rootContainer)
        {
            this.background = new()
            {
                Alignment = CardinalDirection.Center,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundOptions),
                Size = new(698.0f, ScreenConstants.SCREEN_HEIGHT),
            };

            rootContainer.AddChild(this.background);

            this.scrollableContainer = new()
            {
                Alignment = CardinalDirection.Center,
                Size = this.background.Size,
            };

            float scrollableContainerMarginY = 0.0f;

            BuildTitle(ref scrollableContainerMarginY);
            BuildSections(ref scrollableContainerMarginY);
            BuildSystemButtons(ref scrollableContainerMarginY);

            rootContainer.AddChild(this.scrollableContainer);
            rootContainer.AddChild(this.tooltipBox);
        }

        private void BuildTitle(ref float scrollableContainerMarginY)
        {
            scrollableContainerMarginY += 32.0f;

            this.titleLabel = new()
            {
                Margin = new(0.0f, scrollableContainerMarginY),
                Alignment = CardinalDirection.North,
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
                    Alignment = CardinalDirection.North,
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
                        ButtonOption => CreateOptionButtonLabelElement(option.Name),
                        SelectorOption => CreateOptionButtonLabelElement(option.Name + ": " + option.GetValue()),
                        ValueOption => CreateOptionButtonLabelElement(option.Name + ": " + option.GetValue()),
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
                    else if (option is ValueOption)
                    {
                        BuildValueControls(option, label);
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
                    Alignment = CardinalDirection.East,
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(386, 0, 40, 22),
                    Scale = new(1.5f),
                    Size = new(40.0f, 22.0f),
                    Margin = new(58.0f, 0.0f),
                },

                new()
                {
                    Alignment = CardinalDirection.Center,
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

        private void BuildValueControls(Option option, Label label)
        {
            Image minus = new()
            {
                Alignment = CardinalDirection.East,
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUI),
                SourceRectangle = new(192, 160, 32, 32),
                Size = new(32.0f),
                Margin = new(42.0f, 0.0f),
            };

            Image plus = new()
            {
                Alignment = CardinalDirection.East,
                Texture = AssetDatabase.GetTexture(TextureIndex.IconUI),
                SourceRectangle = new(160, 160, 32, 32),
                Size = new(32.0f),
                Margin = new(42.0f, 0.0f),
            };

            plus.AddData("option", option);
            minus.AddData("option", option);
            label.AddData("plus_element", plus);
            label.AddData("minus_element", minus);

            label.AddChild(minus);
            minus.AddChild(plus);

            this.plusMinusButtons.Add(new(plus, minus));
        }

        private static void BuildTogglePreview(Label label)
        {
            Image togglePreviewImageElement = new()
            {
                Alignment = CardinalDirection.East,
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

        private static Label CreateOptionButtonLabelElement(string text)
        {
            return new Label
            {
                Scale = new(0.12f),
                SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                TextContent = text
            };
        }

        internal override void Update(in GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;
            UpdateScrollableContainer();
            UpdateSystemButtons();
            UpdateSectionLabels();
            UpdateSectionOptions();
            base.Update(gameTime);
        }

        private void UpdateScrollableContainer()
        {
            if (!Interaction.OnMouseOver(this.background))
            {
                return;
            }

            float marginY = this.scrollableContainer.Margin.Y;

            if (Interaction.OnMouseScrollUp())
            {
                marginY -= 32.0f;
            }
            else if (Interaction.OnMouseScrollDown())
            {
                marginY += 32.0f;
            }

            float topLimit = 0.0f;
            float bottomLimit = this.scrollableContainer.Children.Count * 48.0f * -1;

            this.scrollableContainer.Margin = new(this.scrollableContainer.Margin.X, float.Clamp(marginY, bottomLimit, topLimit));
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
                        if (option is ButtonOption buttonOption)
                        {
                            buttonOption.OnClickCallback?.Invoke();
                        }
                        else if (option is ColorOption colorOption)
                        {
                            HandleColorOption(colorOption);
                        }
                        else if (option is SelectorOption selectorOption)
                        {
                            selectorOption.Next();
                        }
                        else if (option is ToggleOption toggleOption)
                        {
                            toggleOption.Toggle();
                        }
                    }
                    else if (Interaction.OnMouseRightClick(label))
                    {
                        if (option is SelectorOption selectorOption)
                        {
                            selectorOption.Previous();
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

            foreach (PlusMinusButtonInfo buttonInfo in this.plusMinusButtons)
            {
                if (Interaction.OnMouseLeftDown(buttonInfo.PlusElement.Position, new(32.0f)))
                {
                    ((ValueOption)buttonInfo.PlusElement.GetData("option")).Increment();
                }
                else if (Interaction.OnMouseLeftDown(buttonInfo.MinusElement.Position, new(32.0f)))
                {
                    ((ValueOption)buttonInfo.MinusElement.GetData("option")).Decrement();
                }
            }
        }

        private static void UpdateOptionSync(Option option, UIElement element)
        {
            if (option is ColorOption colorOption)
            {
                UpdateColorOption(colorOption, element.GetData("color_slot") as ColorSlotInfo);
            }
            else if (option is SelectorOption selectorOption)
            {
                UpdateSelectorOption(selectorOption, element as Label);
            }
            else if (option is ValueOption valueOption)
            {
                UpdateValueOption(valueOption, element as Label);
            }
            else if (option is ToggleOption toggleOption)
            {
                UpdateToggleOption(toggleOption, element.GetData("toogle_preview") as Image);
            }
        }

        private static void UpdateColorOption(ColorOption colorOption, ColorSlotInfo colorSlot)
        {
            colorSlot.Background.Color = colorOption.CurrentColor;
        }

        private static void UpdateSelectorOption(SelectorOption selectorOption, Label label)
        {
            label.TextContent = selectorOption.Name + ": " + selectorOption.GetValue();
        }

        private static void UpdateValueOption(ValueOption valueOption, Label label)
        {
            label.TextContent = valueOption.Name + ": " + valueOption.CurrentValue.ToString("D" + valueOption.MaximumValue.ToString().Length);
        }

        private static void UpdateToggleOption(ToggleOption toggleOption, Image toggleStateElement)
        {
            toggleStateElement.SourceRectangle = toggleOption.State ? new(352, 171, 32, 32) : new(352, 140, 32, 32);
        }

        private void HandleColorOption(ColorOption colorOption)
        {
            this.colorPickerSettings.OnSelectCallback = result => colorOption.SetValue(result.SelectedColor);
            this.colorPickerUI.Configure(this.colorPickerSettings);
            this.uiManager.OpenGUI(UIIndex.ColorPicker);
        }

        protected override void OnOpened()
        {
            SyncSettingElements();
        }
    }
}
