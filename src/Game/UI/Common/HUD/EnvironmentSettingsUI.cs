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

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class EnvironmentSettingsUI : UIBase
    {
        private Image background;
        private Label menuTitle, timeStateSectionTitle, timeSectionTitle;
        private SlotInfo[] menuButtonSlotInfos, timeStateButtonSlotInfos, timeButtonSlotInfos;

        private readonly TooltipBox tooltipBox;
        private readonly ButtonInfo[] menuButtonInfos, timeStateButtonInfos, timeButtonInfos;

        private readonly UIManager uiManager;
        private readonly World world;

        internal EnvironmentSettingsUI(
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.menuButtonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseUI),
                new(TextureIndex.IconTools, new(32, 0, 32, 32), Localization_GUIs.EnvironmentSettings_TemperatureEditor_Name, Localization_GUIs.EnvironmentSettings_TemperatureEditor_Description, () => this.uiManager.OpenUI(UIIndex.TemperatureSettings)),
            ];

            this.timeStateButtonInfos = [
                new ButtonInfo(TextureIndex.IconUI, new(160, 64, 32, 32), Localization_Statements.Disable, Localization_GUIs.EnvironmentSettings_TimeState_Disable_Description, () => { world.Time.IsFrozen = true; }),
                new ButtonInfo(TextureIndex.IconUI, new(192, 64, 32, 32), Localization_Statements.Enable, Localization_GUIs.EnvironmentSettings_TimeState_Enable_Description, () => { world.Time.IsFrozen = false; }),
            ];

            this.timeButtonInfos = [
                new ButtonInfo(TextureIndex.IconUI, new(0, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_LateNight_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_LateNight_Description, () => world.Time.SetTime(new(0, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(192, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_EarlyMorning_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_EarlyMorning_Description, () => world.Time.SetTime(new(3, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(32, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Dawn_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Dawn_Description, () => world.Time.SetTime(new(6, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(64, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Morning_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Morning_Description, () => world.Time.SetTime(new(8, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(96, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_EarlyAfternoon_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_EarlyAfternoon_Description, () => world.Time.SetTime(new(12, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(128, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Afternoon_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Afternoon_Description, () => world.Time.SetTime(new(15, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(160, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Evening_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Evening_Description, () => world.Time.SetTime(new(18, 0, 0))),
                new ButtonInfo(TextureIndex.IconUI, new(192, 96, 32, 32), Localization_GUIs.EnvironmentSettings_TimeOfDay_Night_Name, Localization_GUIs.EnvironmentSettings_TimeOfDay_Night_Description, () => world.Time.SetTime(new(20, 0, 0))),
            ];
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildTimeStateSection();
            BuildTimeSection();

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
                TextureIndex = TextureIndex.UIBackgroundEnvironmentSettings,
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
                TextContent = Localization_GUIs.EnvironmentSettings_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            this.menuButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(this.background, this.menuButtonInfos, new(-32.0f, -72.0f), -80.0f, UIDirection.Northeast);
        }

        private void BuildTimeStateSection()
        {
            this.timeStateSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(32.0f, 128.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.EnvironmentSettings_TimeState_Title
            };

            this.background.AddChild(this.timeStateSectionTitle);

            this.timeStateButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(this.timeStateSectionTitle, this.timeStateButtonInfos, new(0.0f, 52.0f), 80.0f, UIDirection.Southwest);
        }

        private void BuildTimeSection()
        {
            this.timeSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(this.timeStateSectionTitle.GetLayoutBounds().Size.X + 32.0f, 0.0f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.EnvironmentSettings_TimeOfDay_Title
            };

            this.timeStateSectionTitle.AddChild(this.timeSectionTitle);

            this.timeButtonSlotInfos = UIBuilderUtility.BuildHorizontalButtonLine(this.timeSectionTitle, this.timeButtonInfos, new(0.0f, 52.0f), 80.0f, UIDirection.Southwest);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateMenuButtons();
            UpdateTimeStateButtons();
            UpdateTimeButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonInfos.Length; i++)
            {
                SlotInfo slot = this.menuButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.menuButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.menuButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.menuButtonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateTimeStateButtons()
        {
            for (int i = 0; i < this.timeStateButtonInfos.Length; i++)
            {
                SlotInfo slot = this.timeStateButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.timeStateButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.timeStateButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.timeStateButtonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }

            if (this.world.Time.IsFrozen)
            {
                this.timeStateButtonSlotInfos[0].Background.Color = AAP64ColorPalette.SelectedColor;
            }
            else
            {
                this.timeStateButtonSlotInfos[1].Background.Color = AAP64ColorPalette.SelectedColor;
            }
        }

        private void UpdateTimeButtons()
        {
            for (int i = 0; i < this.timeButtonInfos.Length; i++)
            {
                SlotInfo slot = this.timeButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.timeButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.timeButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.timeButtonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
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

