/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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
using StardustSandbox.Enums.States;
using StardustSandbox.Enums.UI;
using StardustSandbox.Localization;
using StardustSandbox.Managers;
using StardustSandbox.UI.Elements;
using StardustSandbox.UI.Information;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.UI.Common.HUD
{
    internal sealed class InformationUI : UIBase
    {
        private Image background;
        private Label menuTitle;

        private readonly Label[] infoLabels;
        private readonly SlotInfo[] buttonSlotInfos;
        private readonly ButtonInfo[] buttonInfos;

        private readonly ActorManager actorManager;
        private readonly TooltipBox tooltipBox;
        private readonly UIManager uiManager;
        private readonly World world;

        internal InformationUI(
            ActorManager actorManager,
            UIIndex index,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(index)
        {
            this.actorManager = actorManager;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.buttonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseUI),
            ];

            this.buttonSlotInfos = new SlotInfo[this.buttonInfos.Length];
            this.infoLabels = new Label[7];
        }

        #region BUILDER

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildMenuButtons();
            BuildInfoFields();

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
                Texture = AssetDatabase.GetTexture(TextureIndex.GUIBackgroundInformation),
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
                TextContent = Localization_GUIs.Information_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.background.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            float marginX = -32.0f;

            for (int i = 0; i < this.buttonInfos.Length; i++)
            {
                ButtonInfo button = this.buttonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(marginX, -72.0f), button);

                slot.Background.Alignment = UIDirection.Northeast;
                slot.Icon.Alignment = UIDirection.Center;

                // Update
                this.background.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                // Save
                this.buttonSlotInfos[i] = slot;

                // Spacing
                marginX -= 80.0f;
            }
        }

        private void BuildInfoFields()
        {
            float marginY = 128.0f;

            for (int i = 0; i < this.infoLabels.Length; i++)
            {
                Label label = new()
                {
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Scale = new(0.1f),
                    Alignment = UIDirection.Northwest,
                    Margin = new(32.0f, marginY),
                    Color = AAP64ColorPalette.White,
                    TextContent = string.Concat("Info ", i),

                    BorderDirections = LabelBorderDirection.All,
                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                this.background.AddChild(label);

                // Save
                this.infoLabels[i] = label;

                // Spacing
                marginY += label.Size.Y + 8.0f;
            }
        }

        private static SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            Image background = new()
            {
                Texture = AssetDatabase.GetTexture(TextureIndex.GUIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(2.0f),
                Size = new(32.0f),
                Margin = margin,
            };

            Image icon = new()
            {
                Texture = button.Texture,
                SourceRectangle = button.TextureSourceRectangle,
                Scale = new(1.5f),
                Size = new(32.0f)
            };

            return new(background, icon);
        }

        #endregion

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;
            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.buttonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.buttonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(this.buttonInfos[i].Name);
                    TooltipBoxContent.SetDescription(this.buttonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.buttonInfos[i].ClickAction?.Invoke();
                    break;
                }
            }
        }

        #region EVENTS

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);

            Point worldSize = this.world.Information.Size;

            uint limitOfElementsOnTheMap = (uint)(worldSize.X * worldSize.Y * 2);
            uint limitOfElementsPerLayer = (uint)(worldSize.X * worldSize.Y);

            this.infoLabels[0].TextContent = string.Concat(Localization_Statements.Size, ": ", worldSize.X, 'x', worldSize.Y);
            this.infoLabels[1].TextContent = string.Concat(Localization_Statements.Time, ": ", this.world.Time.CurrentTime.ToString(@"hh\:mm\:ss"));
            this.infoLabels[2].TextContent = string.Concat(Localization_Statements.Elements, ": ", this.world.GetTotalElementCount(), '/', limitOfElementsOnTheMap);
            this.infoLabels[3].TextContent = string.Concat(Localization_GUIs.Information_Field_ForegroundElements, ": ", this.world.GetTotalForegroundElementCount(), '/', limitOfElementsPerLayer);
            this.infoLabels[4].TextContent = string.Concat(Localization_GUIs.Information_Field_BackgroundElements, ": ", this.world.GetTotalBackgroundElementCount(), '/', limitOfElementsPerLayer);

            this.infoLabels[5].TextContent = this.world.Temperature.CanApplyTemperature
                ? string.Concat(Localization_Statements.Temperature, ": ", this.world.Temperature.CurrentTemperature.ToString("0.00"), " °C")
                : string.Concat(Localization_Statements.Temperature, ": ", Localization_Messages.Information_NoTemperature);

            this.infoLabels[6].TextContent = string.Concat(Localization_Statements.Entities, ": ", this.actorManager.TotalActorCount, '/', ActorConstants.MAX_SIMULTANEOUS_ACTORS);
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }

        #endregion
    }
}

