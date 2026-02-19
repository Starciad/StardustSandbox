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
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class OptionsUI : UIBase
    {
        private Image panelBackground, shadowBackground;
        private Label title;

        private SlotInfo exitButtonSlotInfo;

        private readonly ButtonInfo exitButtonInfo;
        private readonly ButtonInfo[] categoryButtonInfos;
        // private readonly SlotInfo[] categoryButtonSlotInfos, optionButtonSlotInfos, paginationButtonSlotInfos;

        private readonly TooltipBox tooltipBox;

        private readonly Category[] categories =
        [
            new(
                Localization_GUIs.Options_General_Name,
                Localization_GUIs.Options_General_Description,
                TextureIndex.None,
                Rectangle.Empty,
                new Option(Localization_GUIs.Options_General_Language_Name, Localization_GUIs.Options_General_Language_Description, OptionType.Selector)
            ),

            new(
                Localization_GUIs.Options_Gameplay_Name,
                Localization_GUIs.Options_Gameplay_Description,
                TextureIndex.None,
                Rectangle.Empty,
                new Option(Localization_GUIs.Options_Gameplay_ShowPreviewArea_Name, Localization_GUIs.Options_Gameplay_ShowPreviewArea_Description, OptionType.Toggle),
                new Option(Localization_GUIs.Options_Gameplay_PreviewAreaColor_Name, Localization_GUIs.Options_Gameplay_PreviewAreaColor_Description, OptionType.ColorSelector),
                new Option(Localization_GUIs.Options_Gameplay_PreviewAreaOpacity_Name, Localization_GUIs.Options_Gameplay_PreviewAreaOpacity_Description, OptionType.Slider),
                new Option(Localization_GUIs.Options_Gameplay_ShowGrid_Name, Localization_GUIs.Options_Gameplay_ShowGrid_Description, OptionType.Toggle),
                new Option(Localization_GUIs.Options_Gameplay_GridOpacity_Name, Localization_GUIs.Options_Gameplay_GridOpacity_Description, OptionType.Slider),
                new Option(Localization_GUIs.Options_Gameplay_ShowTemperatureColorVariations_Name, Localization_GUIs.Options_Gameplay_ShowTemperatureColorVariations_Description, OptionType.Toggle)
            ),

            new(
                Localization_GUIs.Options_Volume_Name,
                Localization_GUIs.Options_Volume_Description,
                TextureIndex.None,
                Rectangle.Empty,
                new Option(Localization_GUIs.Options_Volume_MasterVolume_Name, Localization_GUIs.Options_Volume_MasterVolume_Description, OptionType.Slider),
                new Option(Localization_GUIs.Options_Volume_MusicVolume_Name, Localization_GUIs.Options_Volume_MusicVolume_Description, OptionType.Slider),
                new Option(Localization_GUIs.Options_Volume_SFXVolume_Name, Localization_GUIs.Options_Volume_SFXVolume_Description, OptionType.Slider)
            ),

            new(
                Localization_GUIs.Options_Video_Name,
                Localization_GUIs.Options_Video_Description,
                TextureIndex.None,
                Rectangle.Empty,
                new Option(Localization_GUIs.Options_Video_Framerate_Name, Localization_GUIs.Options_Video_Framerate_Description, OptionType.Selector),
                new Option(Localization_GUIs.Options_Video_Resolution_Name, Localization_GUIs.Options_Video_Resolution_Description, OptionType.Selector),
                new Option(Localization_GUIs.Options_Video_Fullscreen_Name, Localization_GUIs.Options_Video_Fullscreen_Description, OptionType.Toggle),
                new Option(Localization_GUIs.Options_Video_VSync_Name, Localization_GUIs.Options_Video_VSync_Description, OptionType.Toggle),
                new Option(Localization_GUIs.Options_Video_Borderless_Name, Localization_GUIs.Options_Video_Borderless_Description, OptionType.Toggle)
            ),

            new(
                Localization_GUIs.Options_Controls_Name,
                Localization_GUIs.Options_Controls_Description,
                TextureIndex.None,
                Rectangle.Empty,
                new Option(Localization_GUIs.Options_Controls_MoveCameraUp_Name, Localization_GUIs.Options_Controls_MoveCameraUp_Description, OptionType.KeySelector),
                new Option(Localization_GUIs.Options_Controls_MoveCameraRight_Name, Localization_GUIs.Options_Controls_MoveCameraRight_Description, OptionType.KeySelector),
                new Option(Localization_GUIs.Options_Controls_MoveCameraDown_Name, Localization_GUIs.Options_Controls_MoveCameraDown_Description, OptionType.KeySelector),
                new Option(Localization_GUIs.Options_Controls_MoveCameraLeft_Name, Localization_GUIs.Options_Controls_MoveCameraLeft_Description, OptionType.KeySelector),
                new Option(Localization_GUIs.Options_Controls_MoveCameraFast_Name, Localization_GUIs.Options_Controls_MoveCameraFast_Description, OptionType.KeySelector),
                new Option(Localization_GUIs.Options_Controls_TogglePause_Name, Localization_GUIs.Options_Controls_TogglePause_Description, OptionType.KeySelector),
                new Option(Localization_GUIs.Options_Controls_ClearWorld_Name, Localization_GUIs.Options_Controls_ClearWorld_Description, OptionType.KeySelector),
                new Option(Localization_GUIs.Options_Controls_NextShape_Name, Localization_GUIs.Options_Controls_NextShape_Description, OptionType.KeySelector),
                new Option(Localization_GUIs.Options_Controls_Screenshot_Name, Localization_GUIs.Options_Controls_Screenshot_Description, OptionType.KeySelector)
            ),

            new(
                Localization_GUIs.Options_Cursor_Name,
                Localization_GUIs.Options_Cursor_Description,
                TextureIndex.None,
                Rectangle.Empty,
                new Option(Localization_GUIs.Options_Cursor_Color_Name, Localization_GUIs.Options_Cursor_Color_Description, OptionType.ColorSelector),
                new Option(Localization_GUIs.Options_Cursor_BackgroundColor_Name, Localization_GUIs.Options_Cursor_BackgroundColor_Description, OptionType.ColorSelector),
                new Option(Localization_GUIs.Options_Cursor_Scale_Name, Localization_GUIs.Options_Cursor_Scale_Description, OptionType.Selector),
                new Option(Localization_GUIs.Options_Cursor_Opacity_Name, Localization_GUIs.Options_Cursor_Opacity_Description, OptionType.Slider)
            ),
        ];

        internal OptionsUI(
            ColorPickerUI colorPickerUI,
            CursorManager cursorManager,
            KeySelectorUI keySelectorUI,
            MessageUI messageUI,
            SliderUI sliderUI,
            TooltipBox tooltipBox,
            UIManager uiManager,
            VideoManager videoManager
        ) : base()
        {
            this.tooltipBox = tooltipBox;

            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI);
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

        }

        private void BuildOptionButtons()
        {

        }

        private void BuildPagination()
        {

        }
        protected override void OnResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
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

