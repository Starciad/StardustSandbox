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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class InformationUI : UIBase
    {
        private Image panelBackground, shadowBackground;
        private Label menuTitle;

        private readonly Label[] infoLabels;
        private readonly SlotInfo[] buttonSlotInfos;
        private readonly ButtonInfo[] buttonInfos;

        private readonly ActorManager actorManager;
        private readonly AssetDatabase assetDatabase;
        private readonly GameHandler gameHandler;
        private readonly SoundEffectManager soundEffectManager;
        private readonly TooltipBox tooltipBox;
        private readonly UIManager uiManager;
        private readonly World world;

        internal InformationUI(
            ActorManager actorManager,
            AssetDatabase assetDatabase,
            GameHandler gameHandler,
            GameScreen gameScreen,
            SoundEffectManager soundEffectManager,
            TooltipBox tooltipBox,
            UIManager uiManager,
            World world
        ) : base(gameScreen)
        {
            this.actorManager = actorManager;
            this.assetDatabase = assetDatabase;
            this.gameHandler = gameHandler;
            this.soundEffectManager = soundEffectManager;
            this.tooltipBox = tooltipBox;
            this.uiManager = uiManager;
            this.world = world;

            this.buttonInfos = [
                new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, this.uiManager.CloseUI),
            ];

            this.buttonSlotInfos = new SlotInfo[this.buttonInfos.Length];
            this.infoLabels = new Label[7];
        }

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
            this.shadowBackground = new()
            {
                Texture = this.assetDatabase.GetTexture(TextureIndex.Pixel),
                Scale = this.GameScreen.GetViewport(),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.panelBackground = new()
            {
                Alignment = UIDirection.Center,
                Texture = this.assetDatabase.GetTexture(TextureIndex.UIBackgroundInformation),
                Size = new(1084.0f, 540.0f),
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.panelBackground);
        }

        private void BuildTitle()
        {
            this.menuTitle = new()
            {
                SpriteFont = this.assetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                Scale = new(0.12f),
                Margin = new(24.0f, 10.0f),
                Color = AAP64ColorPalette.White,
                TextContent = Localization_GUIs.Information_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            this.panelBackground.AddChild(this.menuTitle);
        }

        private void BuildMenuButtons()
        {
            for (int i = 0; i < this.buttonInfos.Length; i++)
            {
                ButtonInfo button = this.buttonInfos[i];
                SlotInfo slot = CreateButtonSlot(new(-32.0f - (i * 80.0f), -72.0f), button);

                slot.Background.Alignment = UIDirection.Northeast;
                slot.Icon.Alignment = UIDirection.Center;

                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);

                this.buttonSlotInfos[i] = slot;
            }
        }

        private void BuildInfoFields()
        {
            for (int i = 0; i < this.infoLabels.Length; i++)
            {
                Label label = new()
                {
                    SpriteFont = this.assetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm),
                    Scale = new(0.1f),
                    Alignment = UIDirection.Northwest,
                    Margin = new(32.0f, 128.0f + (i * 56.0f)),
                    Color = AAP64ColorPalette.White,
                    TextContent = string.Concat("Info ", i),

                    BorderDirections = LabelBorderDirection.All,
                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                this.panelBackground.AddChild(label);
                this.infoLabels[i] = label;
            }
        }

        private SlotInfo CreateButtonSlot(Vector2 margin, ButtonInfo button)
        {
            Image background = new()
            {
                Texture = this.assetDatabase.GetTexture(TextureIndex.UIButtons),
                SourceRectangle = new(320, 140, 32, 32),
                Scale = new(2.0f),
                Size = new(32.0f),
                Margin = margin
            };

            Image icon = new()
            {
                Texture = this.assetDatabase.GetTexture(button.TextureIndex),
                SourceRectangle = button.TextureSourceRectangle,
                Scale = new(1.5f),
                Size = new(32.0f)
            };

            return new(background, icon);
        }

        protected override void OnScreenResize()
        {
            this.shadowBackground.Scale = this.gameScreen.GetViewport();
        }

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
                    this.soundEffectManager.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    this.tooltipBox.SetTitle(this.buttonInfos[i].Name);
                    this.tooltipBox.SetDescription(this.buttonInfos[i].Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.soundEffectManager.Play(SoundEffectIndex.GUI_Click);
                    this.buttonInfos[i].ClickAction?.Invoke();
                    break;
                }
            }
        }

        protected override void OnOpened()
        {
            this.gameHandler.SetState(GameStates.IsCriticalMenuOpen);

            Point worldSize = this.world.Size;

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
            this.gameHandler.RemoveState(GameStates.IsCriticalMenuOpen);
        }
    }
}

