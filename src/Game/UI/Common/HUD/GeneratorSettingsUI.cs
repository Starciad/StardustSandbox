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
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Generators;
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.Generators;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Common.Tools;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class GeneratorSettingsUI : UIBase
    {
        private WorldGenerationTheme selectedTheme = WorldGenerationTheme.Plain;
        private WorldGenerationSettings selectedSettings = WorldGenerationSettings.GenerateForeground;
        private WorldGenerationContents selectedContents = WorldGenerationContents.HasOceans | WorldGenerationContents.HasVegetation;

        private Image background;
        private Label menuTitle, themeSectionTitle, settingsSectionTitle, contentsSectionTitle;

        private SlotInfo exitButtonSlotInfo, generateButtonSlotInfo;
        private readonly SlotInfo[] themeButtonSlotInfos, settingsButtonSlotInfos, contentsButtonSlotInfos;

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

            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI);
            this.generateButtonInfo = new(TextureIndex.IconUI, new(192, 0, 32, 32), Localization_GUIs.GeneratorSettings_Generate_Name, Localization_GUIs.GeneratorSettings_Generate_Description, () =>
            {
                confirmUI.Configure(new()
                {
                    Caption = "Confirm",
                    Message = "Confirm",
                    OnConfirmCallback = status =>
                    {
                        if (status == ConfirmStatus.Confirmed)
                        {
                            WorldGenerator.Start(actorManager, world, this.selectedTheme, this.selectedSettings, this.selectedContents);
                        }

                        GameHandler.SetState(GameStates.IsCriticalMenuOpen);
                    },
                });

                this.uiManager.OpenUI(UIIndex.Confirm);
            });

            this.themeButtonInfos =
            [
                new(TextureIndex.IconUI, new(0, 0, 32, 32), "Plain", "A plain world theme.", () => { this.selectedTheme = WorldGenerationTheme.Plain; }),
                new(TextureIndex.IconUI, new(32, 32, 32, 32), "Desert", "A desert world theme.", () => { this.selectedTheme = WorldGenerationTheme.Desert; }),
                new(TextureIndex.IconUI, new(64, 32, 32, 32), "Snow", "A snowy world theme.", () => { this.selectedTheme = WorldGenerationTheme.Snow; }),
                new(TextureIndex.IconUI, new(96, 32, 32, 32), "Volcanic", "A volcanic world theme.", () => { this.selectedTheme = WorldGenerationTheme.Volcanic; }),
            ];

            this.settingsButtonInfos =
            [
                new(TextureIndex.IconUI, new(32, 0, 32, 32), "Foreground", "Generate foreground elements.", () => { this.selectedSettings ^= WorldGenerationSettings.GenerateForeground; }),
                new(TextureIndex.IconUI, new(64, 0, 32, 32), "Background", "Generate background elements.", () => { this.selectedSettings ^= WorldGenerationSettings.GenerateBackground; }),
            ];

            this.contentsButtonInfos =
            [
                new(TextureIndex.IconUI, new(0, 0, 32, 32), "Oceans", "Include oceans in the world.", () => { this.selectedContents ^= WorldGenerationContents.HasOceans; }),
                new(TextureIndex.IconUI, new(128, 0, 32, 32), "Vegetation", "Include vegetation in the world.", () => { this.selectedContents ^= WorldGenerationContents.HasVegetation; }),
                new(TextureIndex.IconUI, new(160, 0, 32, 32), "Clouds", "Include clouds in the world.", () => { this.selectedContents ^= WorldGenerationContents.HasClouds; }),
            ];

            this.themeButtonSlotInfos = new SlotInfo[this.themeButtonInfos.Length];
            this.settingsButtonSlotInfos = new SlotInfo[this.settingsButtonInfos.Length];
            this.contentsButtonSlotInfos = new SlotInfo[this.contentsButtonInfos.Length];
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
                Texture = AssetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = new(ScreenConstants.SCREEN_WIDTH, ScreenConstants.SCREEN_HEIGHT),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.background = new()
            {
                Alignment = UIDirection.Center,
                Texture = AssetDatabase.GetTexture(TextureIndex.UIBackgroundGeneratorSettings),
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
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.GeneratorSettings_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildExitButton()
        {
            SlotInfo slot = CreateButtonSlot(new(-32.0f, -72.0f), this.exitButtonInfo);

            slot.Background.Alignment = UIDirection.Northeast;
            slot.Icon.Alignment = UIDirection.Center;

            this.background.AddChild(slot.Background);
            slot.Background.AddChild(slot.Icon);

            this.exitButtonSlotInfo = slot;
        }

        private void BuildGenerateButton()
        {
            this.generateButtonSlotInfo = new(
                new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
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

            this.background.AddChild(this.generateButtonSlotInfo.Background);
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
                TextContent = "Theme"
            };

            this.background.AddChild(this.themeSectionTitle);

            BuildSectionButtons(this.themeSectionTitle, this.themeButtonSlotInfos, this.themeButtonInfos, 3, new(0.0f, 52.0f), new(80.0f, 80.0f));
        }

        private void BuildSettingsSection()
        {
            this.settingsSectionTitle = new()
            {
                Alignment = UIDirection.North,
                Scale = new(0.1f),
                Margin = new(-48.0f, 128.0f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = "Settings"
            };

            this.background.AddChild(this.settingsSectionTitle);

            BuildSectionButtons(this.settingsSectionTitle, this.settingsButtonSlotInfos, this.settingsButtonInfos, 3, new(0.0f, 52.0f), new(80.0f, 80.0f));
        }

        private void BuildContentsSection()
        {
            this.contentsSectionTitle = new()
            {
                Alignment = UIDirection.Northeast,
                Scale = new(0.1f),
                Margin = new(-112.0f, 128.0f),
                Color = AAP64ColorPalette.White,
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                TextContent = "Contents"
            };

            this.background.AddChild(this.contentsSectionTitle);

            BuildSectionButtons(this.contentsSectionTitle, this.contentsButtonSlotInfos, this.contentsButtonInfos, 3, new(0.0f, 52.0f), new(80.0f, 80.0f));
        }

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            return new(
                background: new()
                {
                    Texture = AssetDatabase.GetTexture(TextureIndex.UIButtons),
                    SourceRectangle = new(320, 140, 32, 32),
                    Scale = new(2.0f),
                    Size = new(32.0f),
                    Margin = margin,
                },

                icon: new()
                {
                    Texture = button.Texture,
                    SourceRectangle = button.TextureSourceRectangle,
                    Scale = new(1.5f),
                    Size = new(32.0f)
                }
            );
        }

        private static void BuildSectionButtons(UIElement parent, SlotInfo[] slotInfos, ButtonInfo[] buttonInfo, int itemsPerRow, Vector2 start, Vector2 spacing)
        {
            if (slotInfos.Length != buttonInfo.Length)
            {
                throw new ArgumentException($"{nameof(slotInfos)} and {nameof(buttonInfo)} arrays must have the same length.");
            }

            for (int i = 0; i < slotInfos.Length; i++)
            {
                int col = i % itemsPerRow;
                int row = i / itemsPerRow;

                Vector2 position = new(start.X + (col * spacing.X), start.Y + (row * spacing.Y));
                SlotInfo slot = CreateButtonSlot(position, buttonInfo[i]);

                slot.Background.Alignment = UIDirection.Southwest;
                slot.Icon.Alignment = UIDirection.Center;

                parent.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                slotInfos[i] = slot;
            }
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
