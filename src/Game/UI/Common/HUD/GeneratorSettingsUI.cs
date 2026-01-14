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

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class GeneratorSettingsUI : UIBase
    {
        private WorldGenerationPreset selectedPreset;
        private WorldGenerationSettings selectedSettings;
        private WorldGenerationContents selectedContents;

        private Image background;
        private Label menuTitle;

        private SlotInfo exitButtonSlotInfo, generateButtonSlotInfo;

        private readonly ButtonInfo exitButtonInfo, generateButtonInfo;

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
                            WorldGenerator.Start(actorManager, world, this.selectedPreset, this.selectedSettings, this.selectedContents);
                        }

                        GameHandler.SetState(GameStates.IsCriticalMenuOpen);
                    },
                });

                this.uiManager.OpenUI(UIIndex.Confirm);
            });
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildExitButton();
            BuildGenerateButton();

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
                    Margin = new(-32.0f),
                    Alignment = UIDirection.Southeast,
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

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
            UpdateGenerateButton();
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

        protected override void OnOpened()
        {
            this.selectedPreset = WorldGenerationPreset.Plain;
            this.selectedSettings = WorldGenerationSettings.GenerateForeground |
                                    WorldGenerationSettings.GenerateBackground;
            this.selectedContents = WorldGenerationContents.HasTrees;

            GameHandler.SetState(GameStates.IsCriticalMenuOpen);
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}
