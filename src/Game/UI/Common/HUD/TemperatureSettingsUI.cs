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

using StardustSandbox.Audio;
using StardustSandbox.Colors.Palettes;
using StardustSandbox.Constants;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class TemperatureSettingsUI : UIBase
    {
        private enum TemperatureIndex : byte
        {
            None = 0,
            VeryCold = 1,
            Cold = 2,
            Normal = 3,
            Hot = 4,
            VeryHot = 5,
        }

        private sealed class Section
        {
            internal string Title => this.title;
            internal TimeSpan StartTime => this.startTime;
            internal TimeSpan EndTime => this.endTime;
            internal TemperatureIndex Index { get => this.index; set => this.index = value; }

            internal SlotInfo[] ButtonSlotInfos => this.buttonSlotInfos;
            internal ButtonInfo[] ButtonInfos => this.buttonInfos;

            private TemperatureIndex index;

            private readonly SlotInfo[] buttonSlotInfos;
            private readonly ButtonInfo[] buttonInfos;

            private readonly string title;
            private readonly TimeSpan startTime;
            private readonly TimeSpan endTime;

            internal Section(string title, TimeSpan startTime, TimeSpan endTime, World world)
            {
                this.title = title;
                this.startTime = startTime;
                this.endTime = endTime;

                this.buttonInfos =
                [
                    new(TextureIndex.IconUI, new(224, 192, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_None_Name, Localization_GUIs.TemperatureSettings_Temperature_None_Description, () =>
                    {
                        this.Index = TemperatureIndex.None;
                        world.Temperature.SetTemperatureValue(this.StartTime, 0.0f, false);
                    }),

                    new(TextureIndex.IconUI, new(0, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_VeryCold_Name, Localization_GUIs.TemperatureSettings_Temperature_VeryCold_Description, () =>
                    {
                        this.Index = TemperatureIndex.VeryCold;
                        world.Temperature.SetTemperatureValue(this.StartTime, -60.0f, true);
                    }),

                    new(TextureIndex.IconUI, new(32, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_Cold_Name, Localization_GUIs.TemperatureSettings_Temperature_Cold_Description, () =>
                    {
                        this.Index = TemperatureIndex.Cold;
                        world.Temperature.SetTemperatureValue(this.StartTime, -20.0f, true);
                    }),

                    new(TextureIndex.IconUI, new(64, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_Normal_Name, Localization_GUIs.TemperatureSettings_Temperature_Normal_Description, () =>
                    {
                        this.Index = TemperatureIndex.Normal;
                        world.Temperature.SetTemperatureValue(this.StartTime, 25.0f, true);
                    }),

                    new(TextureIndex.IconUI, new(96, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_Hot_Name, Localization_GUIs.TemperatureSettings_Temperature_Hot_Description, () =>
                    {
                        this.Index = TemperatureIndex.Hot;
                        world.Temperature.SetTemperatureValue(this.StartTime, 60.0f, true);
                    }),

                    new(TextureIndex.IconUI, new(128, 224, 32, 32), Localization_GUIs.TemperatureSettings_Temperature_VeryHot_Name, Localization_GUIs.TemperatureSettings_Temperature_VeryHot_Description, () =>
                    {
                        this.Index = TemperatureIndex.VeryHot;
                        world.Temperature.SetTemperatureValue(this.StartTime, 90.0f, true);
                    }),
                ];

                this.buttonSlotInfos = new SlotInfo[this.buttonInfos.Length];

                for (int i = 0; i < this.buttonInfos.Length; i++)
                {
                    SlotInfo buttonSlotInfo = UIBuilderUtility.BuildButtonSlot(new(0, 0), this.buttonInfos[i]);

                    buttonSlotInfo.Background.Alignment = UIDirection.Southwest;
                    buttonSlotInfo.Icon.Alignment = UIDirection.Center;

                    buttonSlotInfo.Icon.TextureIndex = this.buttonInfos[i].TextureIndex;
                    buttonSlotInfo.Icon.SourceRectangle = this.buttonInfos[i].TextureSourceRectangle;

                    buttonSlotInfo.Background.AddChild(buttonSlotInfo.Icon);

                    this.buttonSlotInfos[i] = buttonSlotInfo;
                }
            }
        }

        private Image background;
        private Label menuTitle;

        private SlotInfo exitButtonSlotInfo;

        private readonly TooltipBox tooltipBox;

        private readonly ButtonInfo exitButtonInfo;
        private readonly Section[] sections;

        internal TemperatureSettingsUI(
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.tooltipBox = tooltipBox;

            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI);

            this.sections =
            [
                new(Localization_GUIs.TemperatureSettings_TimeOfDay_LateNight, new(0, 0, 0), new(3, 0, 0), world),
                new(Localization_GUIs.TemperatureSettings_TimeOfDay_EarlyMorning, new(3, 0, 0), new(6, 0, 0), world),
                new(Localization_GUIs.TemperatureSettings_TimeOfDay_Dawn, new(6, 0, 0), new(8, 0, 0), world),
                new(Localization_GUIs.TemperatureSettings_TimeOfDay_Morning, new(8, 0, 0), new(12, 0, 0), world),
                new(Localization_GUIs.TemperatureSettings_TimeOfDay_EarlyAfternoon, new(12, 0, 0), new(15, 0, 0), world),
                new(Localization_GUIs.TemperatureSettings_TimeOfDay_Afternoon, new(15, 0, 0), new(18, 0, 0), world),
                new(Localization_GUIs.TemperatureSettings_TimeOfDay_Evening, new(18, 0, 0), new(20, 0, 0), world),
                new(Localization_GUIs.TemperatureSettings_TimeOfDay_Night, new(20, 0, 0), new(24, 0, 0), world),
            ];
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildExitButton();
            BuildSections();

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            Image shadow = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.background = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundTemperatureSettings,
                Size = new(1084.0f, 540.0f),
            };

            root.AddChild(shadow);
            root.AddChild(this.background);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Margin = new(24.0f, 10.0f),
                TextContent = Localization_GUIs.TemperatureSettings_Name,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildExitButton()
        {
            SlotInfo slot = UIBuilderUtility.BuildButtonSlot(new(-32.0f, -72.0f), this.exitButtonInfo);

            slot.Background.Alignment = UIDirection.Northeast;
            slot.Icon.Alignment = UIDirection.Center;

            this.background.AddChild(slot.Background);
            slot.Background.AddChild(slot.Icon);

            this.exitButtonSlotInfo = slot;
        }

        private void BuildSections()
        {
            for (int i = 0; i < this.sections.Length; i++)
            {
                Section section = this.sections[i];

                Label sectionTitle = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.1f),
                    Margin = new(32.0f, 80.0f + (i * 112.0f)),
                    TextContent = string.Format("{0} ({1:00}:{2:00} - {3:00}:{4:00})",
                        section.Title,
                        section.StartTime.Hours, section.StartTime.Minutes,
                        section.EndTime.Hours, section.EndTime.Minutes
                    ),

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                if (i > (this.sections.Length / 2) - 1)
                {
                    sectionTitle.Margin = new(556.0f + 32.0f, 80.0f + ((i - (this.sections.Length / 2)) * 112.0f));
                }

                for (int j = 0; j < section.ButtonSlotInfos.Length; j++)
                {
                    SlotInfo buttonSlot = section.ButtonSlotInfos[j];

                    buttonSlot.Background.Margin = new(j * 80.0f, 48.0f);
                    buttonSlot.Icon.Alignment = UIDirection.Center;

                    sectionTitle.AddChild(buttonSlot.Background);
                }

                this.background.AddChild(sectionTitle);
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
            UpdateSections();
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

        private void UpdateSections()
        {
            for (int i = 0; i < this.sections.Length; i++)
            {
                Section section = this.sections[i];

                for (int j = 0; j < section.ButtonSlotInfos.Length; j++)
                {
                    SlotInfo buttonSlot = section.ButtonSlotInfos[j];
                    ButtonInfo buttonInfo = section.ButtonInfos[j];

                    if (Interaction.OnMouseEnter(buttonSlot.Background))
                    {
                        SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                    }

                    if (Interaction.OnMouseLeftClick(buttonSlot.Background))
                    {
                        SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                        buttonInfo.ClickAction?.Invoke();
                        break;
                    }

                    if (Interaction.OnMouseOver(buttonSlot.Background))
                    {
                        this.tooltipBox.CanDraw = true;

                        TooltipBoxContent.SetTitle(buttonInfo.Name);
                        TooltipBoxContent.SetDescription(buttonInfo.Description);

                        buttonSlot.Background.Color = AAP64ColorPalette.HoverColor;
                    }
                    else
                    {
                        buttonSlot.Background.Color = (int)section.Index == j ? AAP64ColorPalette.Graphite : AAP64ColorPalette.White;
                    }
                }
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

