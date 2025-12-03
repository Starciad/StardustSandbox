using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
            internal Container Container;
            internal Label[] ContentLabels;
        }

        private byte selectedSectionIndex;
        private bool restartMessageAppeared;
        private Label titleLabel;
        private Image background;
        private readonly Root root;
        private readonly ColorPickerSettings colorPickerSettings;
        private readonly ColorPickerUI colorPickerUI;
        private readonly MessageUI messageUI;
        private readonly string titleName = Localization_GUIs.Menu_Options_Title;
        private readonly List<PlusMinusButtonInfo> plusMinusButtons = [];
        private readonly TooltipBox tooltipBox;
        private readonly Label[] systemButtonLabels;
        private readonly ButtonInfo[] systemButtonInfos;
        private readonly SectionUI[] sectionUIs;
        private readonly Label[] sectionButtonLabels;
        private readonly CursorManager cursorManager;
        private readonly UIManager uiManager;
        private readonly VideoManager videoManager;
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
            this.generalSettings = SettingsHandler.LoadSettings<GeneralSettings>();
            this.gameplaySettings = SettingsHandler.LoadSettings<GameplaySettings>();
            this.volumeSettings = SettingsHandler.LoadSettings<VolumeSettings>();
            this.videoSettings = SettingsHandler.LoadSettings<VideoSettings>();
            this.cursorSettings = SettingsHandler.LoadSettings<CursorSettings>();

            this.systemButtonInfos = [
                new(TextureIndex.None, null, Localization_Statements.Return, Localization_GUIs.Button_Exit_Description, uiManager.CloseGUI),
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
            ];

            this.root = new([
                new(Localization_GUIs.Menu_Options_Section_General_Name, Localization_GUIs.Menu_Options_Section_General_Description,
                [
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_General_Option_Language_Name, Localization_GUIs.Menu_Options_Section_General_Option_Language_Description, Array.ConvertAll(LocalizationConstants.AVAILABLE_GAME_CULTURES, x => x.Name))
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
                    new SelectorOption("Framerate", "Description", Array.ConvertAll<double, object>(ScreenConstants.FRAMERATES, x => x)),
                    new SelectorOption(Localization_GUIs.Menu_Options_Section_Video_Option_Resolution_Name, Localization_GUIs.Menu_Options_Section_Video_Option_Resolution_Description, Array.ConvertAll<Point, object>(ScreenConstants.RESOLUTIONS, x => x)),
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

            this.systemButtonLabels = new Label[this.systemButtonInfos.Length];
            this.sectionUIs = new SectionUI[this.root.Sections.Length];
            this.sectionButtonLabels = new Label[this.root.Sections.Length];
        }

        private void SaveSettings()
        {
            Section generalSection = this.root.Sections[(byte)SectionIndex.General];
            GameCulture gameCulture = LocalizationConstants.GetGameCulture(Convert.ToString(generalSection.Options[(byte)GeneralSectionOptionIndex.Language].GetValue()));
            this.generalSettings.Language = gameCulture.Language;
            this.generalSettings.Region = gameCulture.Region;
            SettingsHandler.SaveSettings(this.generalSettings);

            Section gameplaySection = this.root.Sections[(byte)SectionIndex.Gameplay];
            this.gameplaySettings.PreviewAreaColor = (Color)gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaColor].GetValue();
            this.gameplaySettings.PreviewAreaColorA = Convert.ToByte(gameplaySection.Options[(byte)GameplaySectionOptionIndex.PreviewAreaOpacity].GetValue());
            SettingsHandler.SaveSettings(this.gameplaySettings);

            Section volumeSection = this.root.Sections[(byte)SectionIndex.Volume];
            this.volumeSettings.MasterVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.MasterVolume].GetValue()) / 100.0f;
            this.volumeSettings.MusicVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.MusicVolume].GetValue()) / 100.0f;
            this.volumeSettings.SFXVolume = Convert.ToSingle(volumeSection.Options[(byte)VolumeSectionOptionIndex.SFXVolume].GetValue()) / 100.0f;
            SettingsHandler.SaveSettings(this.volumeSettings);

            Section videoSection = this.root.Sections[(byte)SectionIndex.Video];
            this.videoSettings.Framerate = (double)videoSection.Options[(byte)VideoSectionOptionIndex.Framerate].GetValue();
            this.videoSettings.Resolution = (Point)videoSection.Options[(byte)VideoSectionOptionIndex.Resolution].GetValue();
            this.videoSettings.FullScreen = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.Fullscreen].GetValue());
            this.videoSettings.VSync = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.VSync].GetValue());
            this.videoSettings.Borderless = Convert.ToBoolean(videoSection.Options[(byte)VideoSectionOptionIndex.Borderless].GetValue());
            SettingsHandler.SaveSettings(this.videoSettings);

            Section cursorSection = this.root.Sections[(byte)SectionIndex.Cursor];
            this.cursorSettings.Color = (Color)cursorSection.Options[(byte)CursorSectionOptionIndex.Color].GetValue();
            this.cursorSettings.BackgroundColor = (Color)cursorSection.Options[(byte)CursorSectionOptionIndex.BackgroundColor].GetValue();
            this.cursorSettings.Alpha = Convert.ToByte(cursorSection.Options[(byte)CursorSectionOptionIndex.Opacity].GetValue());
            this.cursorSettings.Scale = Convert.ToSingle(cursorSection.Options[(byte)CursorSectionOptionIndex.Scale].GetValue());
            SettingsHandler.SaveSettings(this.cursorSettings);
        }

        private void SyncSettingElements()
        {
            Section generalSection = this.root.Sections[(byte)SectionIndex.General];
            generalSection.Options[(byte)GeneralSectionOptionIndex.Language].SetValue(this.generalSettings.GameCulture.Name);

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
            BuildPanelBackground(rootContainer);
            BuildTitle();
            BuildSectionButtons();
            BuildSystemButtons();
            BuildSections(rootContainer);

            rootContainer.AddChild(this.tooltipBox);
        }

        private void BuildPanelBackground(Container rootContainer)
        {
            this.background = new()
            {
                Alignment = CardinalDirection.Center,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundOptions),
                Size = new(1084.0f, 540.0f),
            };

            rootContainer.AddChild(this.background);
        }

        private void BuildTitle()
        {
            this.titleLabel = new()
            {
                Scale = new(0.15f),
                Margin = new(0.0f, -38.0f),
                Alignment = CardinalDirection.North,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = this.titleName,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 4.4f,
                BorderThickness = 4.4f,
            };

            this.background.AddChild(this.titleLabel);
        }

        private void BuildSectionButtons()
        {
            float marginY = 48.0f;

            for (byte i = 0; i < this.root.Sections.Length; i++)
            {
                Section section = this.root.Sections[i];

                Label label = CreateButtonLabelElement(section.Name);
                label.Alignment = CardinalDirection.North;
                label.Margin = new(-335.0f, marginY);

                this.background.AddChild(label);
                this.sectionButtonLabels[i] = label;
                marginY += 48.0f;
            }
        }

        private void BuildSystemButtons()
        {
            float marginY = -38.0f;

            for (byte i = 0; i < this.systemButtonInfos.Length; i++)
            {
                Label label = CreateButtonLabelElement(this.systemButtonInfos[i].Name);

                label.Alignment = CardinalDirection.South;
                label.Margin = new(-335.0f, marginY);

                this.background.AddChild(label);
                this.systemButtonLabels[i] = label;

                marginY -= 48.0f;
            }
        }

        private void BuildSections(Container rootContainer)
        {
            for (byte i = 0; i < this.root.Sections.Length; i++)
            {
                Section section = this.root.Sections[i];
                Label[] contentBuffer = new Label[section.Options.Length];

                Container container = new()
                {
                    CanDraw = false,
                    CanUpdate = false,
                    Alignment = CardinalDirection.Northeast,
                    Margin = new(-96.0f, 80.0f),
                    Size = new(642.0f, 476.0f),
                };

                Vector2 margin = new(0.0f, 64.0f);

                for (byte j = 0; j < section.Options.Length; j++)
                {
                    Option option = section.Options[j];
                    Label label = CreateOptionElement(option);

                    label.Margin = margin;

                    if (option is ColorOption)
                    {
                        BuildColorPreview(container, label);
                    }
                    else if (option is ValueOption)
                    {
                        BuildValueControls(option, container, label);
                    }
                    else if (option is ToggleOption)
                    {
                        BuildTogglePreview(container, label);
                    }

                    container.AddChild(label);
                    margin.Y += 48.0f;
                    contentBuffer[j] = label;
                }

                this.sectionUIs[i] = new SectionUI { Container = container, ContentLabels = contentBuffer };
                rootContainer.AddChild(container);
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
                    Margin = new(label.Size.X + 6.0f, label.Size.Y / 2.0f * -1.0f),
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
                Margin = new(0.0f, label.Size.Y / 2.0f * -1.0f)
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

            this.plusMinusButtons.Add(new(plus, minus));
        }

        private static void BuildTogglePreview(Container container, Label label)
        {
            Image togglePreviewImageElement = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(352, 140, 32, 32),
                Scale = new(1.25f),
                Size = new(32.0f),
                Margin = new(label.Size.X + 6.0f, label.Size.Y / 2.0f * -1.0f),
            };

            label.AddChild(togglePreviewImageElement);
            container.AddChild(togglePreviewImageElement);
            label.AddData("toogle_preview", togglePreviewImageElement);
        }

        private static Label CreateButtonLabelElement(string text)
        {
            return new Label
            {
                Scale = new(0.11f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
                TextContent = text
            };
        }

        private static Label CreateOptionButtonLabelElement(string text)
        {
            return new Label
            {
                Scale = new(0.12f),
                SpriteFontIndex = SpriteFontIndex.DigitalDisco,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
                TextContent = text
            };
        }

        private static Label CreateOptionElement(Option option)
        {
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
            return label;
        }

        internal override void Update(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;
            UpdateSectionButtons();
            UpdateSystemButtons();
            UpdateSectionOptions();
            base.Update(gameTime);
        }

        private void UpdateSectionButtons()
        {
            for (byte i = 0; i < this.sectionButtonLabels.Length; i++)
            {
                Label label = this.sectionButtonLabels[i];

                if (Interaction.OnMouseLeftClick(label))
                {
                    SelectSection(i);
                }

                bool onMouseOver = Interaction.OnMouseOver(label);
                if (onMouseOver)
                {
                    this.tooltipBox.CanDraw = true;
                    Section section = this.root.Sections[i];
                    TooltipBoxContent.SetTitle(section.Name);
                    TooltipBoxContent.SetDescription(section.Description);
                }

                label.Color = this.selectedSectionIndex.Equals(i)
                    ? AAP64ColorPalette.LemonYellow
                    : onMouseOver ? AAP64ColorPalette.LemonYellow : AAP64ColorPalette.White;
            }
        }

        private void UpdateSystemButtons()
        {
            for (byte i = 0; i < this.systemButtonInfos.Length; i++)
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

        private void UpdateSectionOptions()
        {
            Label[] contentLabels = this.sectionUIs[this.selectedSectionIndex].ContentLabels;
            for (byte i = 0; i < contentLabels.Length; i++)
            {
                Label label = contentLabels[i];
                Option option = (Option)label.GetData("option");

                if (Interaction.OnMouseLeftClick(label))
                {
                    HandleOptionInteractivity(option);
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
            toggleStateElement.SourceRectangle = toggleOption.State ? new(0, 32, 32, 32) : new(0, 0, 32, 32);
        }

        private void HandleOptionInteractivity(Option option)
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

        private void HandleColorOption(ColorOption colorOption)
        {
            this.colorPickerSettings.OnSelectCallback = result => colorOption.SetValue(result.SelectedColor);
            this.colorPickerUI.Configure(this.colorPickerSettings);
            this.uiManager.OpenGUI(UIIndex.ColorPicker);
        }

        private void SelectSection(byte sectionIndex)
        {
            this.selectedSectionIndex = sectionIndex;
            for (byte i = 0; i < this.sectionUIs.Length; i++)
            {
                SectionUI sectionUI = this.sectionUIs[i];
                bool isSelected = sectionIndex == i;
                sectionUI.Container.CanDraw = isSelected;
                sectionUI.Container.CanUpdate = isSelected;
            }
        }

        protected override void OnOpened()
        {
            SelectSection(0);
            SyncSettingElements();
        }
    }
}
