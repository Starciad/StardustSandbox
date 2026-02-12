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

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Generators;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Enums.UI.Tools;
using StardustSandbox.Core.Generators;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Common.Tools;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.UI.Common.HUD
{
    internal sealed class GeneratorSettingsUI : UIBase
    {
        private WorldGenerationTheme selectedTheme = WorldGenerationTheme.Plain;
        private WorldGenerationSettings selectedSettings = WorldGenerationSettings.GenerateForeground;
        private WorldGenerationContents selectedContents = WorldGenerationContents.None;

        private Image panelBackground;
        private Label menuTitle, themeSectionTitle, settingsSectionTitle, contentsSectionTitle;

        private SlotInfo exitButtonSlotInfo, generateButtonSlotInfo;
        private SlotInfo[] themeButtonSlotInfos, settingsButtonSlotInfos, contentsButtonSlotInfos;

        private readonly ButtonInfo exitButtonInfo, generateButtonInfo;
        private readonly ButtonInfo[] themeButtonInfos, settingsButtonInfos, contentsButtonInfos;

        private readonly TooltipBox tooltipBox;
        private readonly UIManager uiManager;

        internal GeneratorSettingsUI(
            ActorManager actorManager,
            ConfirmUI confirmUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base()
        {
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;

            this.exitButtonInfo = new(
                TextureIndex.IconUI,
                new(224, 0, 32, 32),
                Localization_Statements.Exit,
                Localization_GUIs.Button_Exit_Description,
                uiManager.CloseUI
            );

            this.generateButtonInfo = new(
                TextureIndex.IconUI,
                new(192, 0, 32, 32),
                Localization_GUIs.GeneratorSettings_Generate_Name,
                Localization_GUIs.GeneratorSettings_Generate_Description,
                () =>
                {
                    confirmUI.Configure(new()
                    {
                        Caption = Localization_Messages.GeneratorSettings_Confirm_Title,
                        Message = Localization_Messages.GeneratorSettings_Confirm_Message,

                        OnConfirmCallback = status =>
                        {
                            if (status == ConfirmStatus.Confirmed)
                            {
                                WorldGenerator.Start(
                                    actorManager,
                                    world,
                                    this.selectedTheme,
                                    this.selectedSettings,
                                    this.selectedContents
                                );
                            }

                            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
                        },
                    });

                    this.uiManager.OpenUI(UIIndex.Confirm);
                }
            );

            this.themeButtonInfos =
            [
                new(TextureIndex.IconUI, new(320, 0, 32, 32),
                    Localization_GUIs.GeneratorSettings_Theme_Plain_Name,
                    Localization_GUIs.GeneratorSettings_Theme_Plain_Description,
                    () => { this.selectedTheme = WorldGenerationTheme.Plain; }
                ),

                new(TextureIndex.IconUI, new(320, 32, 32, 32),
                    Localization_GUIs.GeneratorSettings_Theme_Desert_Name,
                    Localization_GUIs.GeneratorSettings_Theme_Desert_Description,
                    () => { this.selectedTheme = WorldGenerationTheme.Desert; }
                ),

                new(TextureIndex.IconUI, new(320, 64, 32, 32),
                    Localization_GUIs.GeneratorSettings_Theme_Snow_Name,
                    Localization_GUIs.GeneratorSettings_Theme_Snow_Description,
                    () => { this.selectedTheme = WorldGenerationTheme.Snow; }
                ),

                new(TextureIndex.IconUI, new(320, 96, 32, 32),
                    Localization_GUIs.GeneratorSettings_Theme_Volcanic_Name,
                    Localization_GUIs.GeneratorSettings_Theme_Volcanic_Description,
                    () => { this.selectedTheme = WorldGenerationTheme.Volcanic; }
                ),

                new(TextureIndex.IconUI, new(320, 128, 32, 32),
                    Localization_GUIs.GeneratorSettings_Theme_Ocean_Name,
                    Localization_GUIs.GeneratorSettings_Theme_Ocean_Description,
                    () => { this.selectedTheme = WorldGenerationTheme.Ocean; }
                ),
            ];

            this.settingsButtonInfos =
            [
                new(TextureIndex.IconUI, new(192, 32, 32, 32),
                    Localization_GUIs.GeneratorSettings_Settings_Foreground_Name,
                    Localization_GUIs.GeneratorSettings_Settings_Foreground_Description,
                    () => { this.selectedSettings ^= WorldGenerationSettings.GenerateForeground; }
                ),

                new(TextureIndex.IconUI, new(224, 32, 32, 32),
                    Localization_GUIs.GeneratorSettings_Settings_Background_Name,
                    Localization_GUIs.GeneratorSettings_Settings_Background_Description,
                    () => { this.selectedSettings ^= WorldGenerationSettings.GenerateBackground; }
                ),
            ];

            this.contentsButtonInfos =
            [
                new(TextureIndex.IconElements, new(96, 0, 32, 32),
                    Localization_GUIs.GeneratorSettings_Contents_Oceans_Name,
                    Localization_GUIs.GeneratorSettings_Contents_Oceans_Description,
                    () => { this.selectedContents ^= WorldGenerationContents.HasOceans; }
                ),

                new(TextureIndex.IconElements, new(160, 128, 32, 32),
                    Localization_GUIs.GeneratorSettings_Contents_Vegetation_Name,
                    Localization_GUIs.GeneratorSettings_Contents_Vegetation_Description,
                    () => { this.selectedContents ^= WorldGenerationContents.HasVegetation; }
                ),

                new(TextureIndex.IconElements, new(0, 128, 32, 32),
                    Localization_GUIs.GeneratorSettings_Contents_Clouds_Name,
                    Localization_GUIs.GeneratorSettings_Contents_Clouds_Description,
                    () => { this.selectedContents ^= WorldGenerationContents.HasClouds; }
                ),
            ];
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildExitButton();
            BuildGenerateButton();
            BuildSections();

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            Image shadow = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.panelBackground = new()
            {
                Alignment = UIDirection.Center,
                TextureIndex = TextureIndex.UIBackgroundGeneratorSettings,
                Size = new(1084.0f, 540.0f),
            };

            root.AddChild(shadow);
            root.AddChild(this.panelBackground);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.12f),
                Margin = new(24.0f, 10.0f),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.GeneratorSettings_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.panelBackground.AddChild(this.menuTitle);
        }

        private void BuildExitButton()
        {
            SlotInfo slot = UIBuilderUtility.BuildButtonSlot(new(-32.0f, -72.0f), this.exitButtonInfo);

            slot.Background.Alignment = UIDirection.Northeast;
            slot.Icon.Alignment = UIDirection.Center;

            this.panelBackground.AddChild(slot.Background);
            slot.Background.AddChild(slot.Icon);

            this.exitButtonSlotInfo = slot;
        }

        private void BuildGenerateButton()
        {
            this.generateButtonSlotInfo = new(
                new()
                {
                    TextureIndex = TextureIndex.UIButtons,
                    SourceRectangle = new(0, 140, 320, 80),
                    Size = new(320.0f, 80.0f),
                    Margin = new(0.0f, -32.0f),
                    Alignment = UIDirection.South,
                },

                null,

                new()
                {
                    Scale = new(0.1f),
                    Color = AAP64ColorPalette.White,
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = UIDirection.Center,
                    TextContent = this.generateButtonInfo.Name,

                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderDirections = LabelBorderDirection.All,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                }
            );

            this.panelBackground.AddChild(this.generateButtonSlotInfo.Background);
            this.generateButtonSlotInfo.Background.AddChild(this.generateButtonSlotInfo.Label);
        }

        private void BuildSections()
        {
            BuildThemeSection();
            BuildSettingsSection();
            BuildContentsSection();
        }

        private void BuildThemeSection()
        {
            this.themeSectionTitle = new()
            {
                Scale = new(0.1f),
                Margin = new(32.0f, 128.0f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.GeneratorSettings_Theme_Title
            };

            this.panelBackground.AddChild(this.themeSectionTitle);

            this.themeButtonSlotInfos = UIBuilderUtility.BuildGridButtons(
                this.themeSectionTitle,
                this.themeButtonInfos,
                3,
                new(0.0f, 52.0f),
                new(80.0f, 80.0f),
                UIDirection.Northwest
            );
        }

        private void BuildSettingsSection()
        {
            this.settingsSectionTitle = new()
            {
                Alignment = UIDirection.North,
                Scale = new(0.1f),
                Margin = new(-56.0f, 128.0f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.GeneratorSettings_Settings_Title
            };

            this.panelBackground.AddChild(this.settingsSectionTitle);

            this.settingsButtonSlotInfos = UIBuilderUtility.BuildGridButtons(
                this.settingsSectionTitle,
                this.settingsButtonInfos,
                3,
                new(0.0f, 52.0f),
                new(80.0f, 80.0f),
                UIDirection.Northwest
            );
        }

        private void BuildContentsSection()
        {
            this.contentsSectionTitle = new()
            {
                Alignment = UIDirection.Northeast,
                Scale = new(0.1f),
                Margin = new(-32.0f, 128.0f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = Localization_GUIs.GeneratorSettings_Contents_Title
            };

            this.panelBackground.AddChild(this.contentsSectionTitle);

            this.contentsButtonSlotInfos = UIBuilderUtility.BuildGridButtons(
                this.contentsSectionTitle,
                this.contentsButtonInfos,
                3,
                new(0.0f, 52.0f),
                new(-80.0f, 80.0f),
                UIDirection.Northeast
            );
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
            UpdateGenerateButton();
            UpdateSectionButtons();
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

        private void UpdateGenerateButton()
        {
            if (Interaction.OnMouseEnter(this.generateButtonSlotInfo.Background))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Hover);
            }

            if (Interaction.OnMouseLeftClick(this.generateButtonSlotInfo.Background))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Click);
                this.generateButtonInfo.ClickAction?.Invoke();
            }

            if (Interaction.OnMouseOver(this.generateButtonSlotInfo.Background))
            {
                this.tooltipBox.CanDraw = true;
                TooltipBoxContent.SetTitle(this.generateButtonInfo.Name);
                TooltipBoxContent.SetDescription(this.generateButtonInfo.Description);

                this.generateButtonSlotInfo.Background.Color = AAP64ColorPalette.HoverColor;
            }
            else
            {
                this.generateButtonSlotInfo.Background.Color = AAP64ColorPalette.White;
            }
        }

        private void UpdateSectionButtons()
        {
            UpdateThemeButtons();
            UpdateSettingsButtons(this.settingsButtonSlotInfos, this.settingsButtonInfos, this.tooltipBox, (int)this.selectedSettings);
            UpdateSettingsButtons(this.contentsButtonSlotInfos, this.contentsButtonInfos, this.tooltipBox, (int)this.selectedContents);
        }

        private void UpdateThemeButtons()
        {
            for (int i = 0; i < this.themeButtonInfos.Length; i++)
            {
                SlotInfo slot = this.themeButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    this.themeButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                bool isOver = Interaction.OnMouseOver(slot.Background);

                if (isOver)
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.themeButtonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.themeButtonInfos[i].Description);
                }

                slot.Background.Color = i == (int)this.selectedTheme ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        private static void UpdateSettingsButtons(SlotInfo[] slotInfos, ButtonInfo[] buttonInfo, TooltipBox tooltipBox, int flags)
        {
            for (int i = 0; i < buttonInfo.Length; i++)
            {
                SlotInfo slot = slotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                    buttonInfo[i].ClickAction?.Invoke();
                    break;
                }

                bool isOver = Interaction.OnMouseOver(slot.Background);

                if (isOver)
                {
                    tooltipBox.CanDraw = true;
                    TooltipBoxContent.SetTitle(buttonInfo[i].Name);
                    TooltipBoxContent.SetDescription(buttonInfo[i].Description);
                }

                int settingFlag = 1 << i;
                bool isSelected = (flags & settingFlag) == settingFlag;

                slot.Background.Color = isSelected ? AAP64ColorPalette.SelectedColor : isOver ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
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
