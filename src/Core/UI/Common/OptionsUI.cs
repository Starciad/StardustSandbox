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
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

using System.Collections.Generic;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class OptionsUI : UIBase
    {
        private int selectedCategoryIndex = 0;

        private Image panelBackground, shadowBackground;
        private Label title;

        private SlotInfo exitButtonSlotInfo;
        private SlotInfo[] categoryButtonSlotInfos, optionButtonSlotInfos;

        private readonly ButtonInfo exitButtonInfo;
        private readonly ButtonInfo[] categoryButtonInfos;

        private readonly TooltipBox tooltipBox;

        private readonly Category[] categories =
        [
            new(
                Localization_GUIs.Options_General_Name,
                Localization_GUIs.Options_General_Description,
                TextureIndex.IconUI,
                new(0, 0, 32, 32),
                new Option(Localization_GUIs.Options_General_Language_Name, Localization_GUIs.Options_General_Language_Description, OptionType.Selector)
            ),

            new(
                Localization_GUIs.Options_Gameplay_Name,
                Localization_GUIs.Options_Gameplay_Description,
                TextureIndex.IconUI,
                new(0, 0, 32, 32),
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
                TextureIndex.IconUI,
                new(0, 0, 32, 32),
                new Option(Localization_GUIs.Options_Volume_MasterVolume_Name, Localization_GUIs.Options_Volume_MasterVolume_Description, OptionType.Slider),
                new Option(Localization_GUIs.Options_Volume_MusicVolume_Name, Localization_GUIs.Options_Volume_MusicVolume_Description, OptionType.Slider),
                new Option(Localization_GUIs.Options_Volume_SFXVolume_Name, Localization_GUIs.Options_Volume_SFXVolume_Description, OptionType.Slider)
            ),

            new(
                Localization_GUIs.Options_Video_Name,
                Localization_GUIs.Options_Video_Description,
                TextureIndex.IconUI,
                new(0, 0, 32, 32),
                new Option(Localization_GUIs.Options_Video_Framerate_Name, Localization_GUIs.Options_Video_Framerate_Description, OptionType.Selector),
                new Option(Localization_GUIs.Options_Video_Resolution_Name, Localization_GUIs.Options_Video_Resolution_Description, OptionType.Selector),
                new Option(Localization_GUIs.Options_Video_Fullscreen_Name, Localization_GUIs.Options_Video_Fullscreen_Description, OptionType.Toggle),
                new Option(Localization_GUIs.Options_Video_VSync_Name, Localization_GUIs.Options_Video_VSync_Description, OptionType.Toggle),
                new Option(Localization_GUIs.Options_Video_Borderless_Name, Localization_GUIs.Options_Video_Borderless_Description, OptionType.Toggle)
            ),

            new(
                Localization_GUIs.Options_Controls_Name,
                Localization_GUIs.Options_Controls_Description,
                TextureIndex.IconUI,
                new(0, 0, 32, 32),
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
                TextureIndex.IconUI,
                new(0, 0, 32, 32),
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
            this.categoryButtonInfos = new ButtonInfo[this.categories.Length];
            this.optionButtonSlotInfos = new SlotInfo[UIConstants.OPTIONS_PER_PAGE];

            for (int i = 0; i < this.categories.Length; i++)
            {
                this.categoryButtonInfos[i] = new(this.categories[i].TextureIndex, this.categories[i].TextureSourceRectangle, this.categories[i].Name, this.categories[i].Description, null);
            }
        }

        private void SelectCategory(int index)
        {
            if (index < 0 || index >= this.categories.Length)
            {
                return;
            }

            this.selectedCategoryIndex = index;
            Category category = this.categories[this.selectedCategoryIndex];

            this.title.TextContent = category.Name;

            for (int i = 0; i < this.optionButtonSlotInfos.Length; i++)
            {
                SlotInfo slotInfo = this.optionButtonSlotInfos[i];

                if (i < category.Options.Length)
                {
                    slotInfo.Background.CanDraw = true;
                    slotInfo.Label.TextContent = category.Options[i].Name.Truncate(32);
                }
                else
                {
                    slotInfo.Background.CanDraw = false;
                }
            }
        }

        internal void Setup()
        {
            SelectCategory(0);
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
            this.categoryButtonSlotInfos = UIBuilderUtility.BuildVerticalButtonLine(
                this.panelBackground,
                this.categoryButtonInfos,
                new(8.0f, 8.0f),
                74.0f,
                UIDirection.Northwest
            );
        }

        private void BuildOptionButtons()
        {
            Vector2 firstColumnMargin = new(96.0f, 92.0f);
            Vector2 secondColumnMargin = new(589.0f, 92.0f);
            Vector2 spacing = new(12.0f);
            Vector2 size = new(477.0f, 61.0f);

            int index = 0;

            for (int row = 0; row < UIConstants.OPTIONS_PER_ROW; row++)
            {
                for (int column = 0; column < UIConstants.OPTIONS_PER_COLUMN; column++)
                {
                    Vector2 margin = column % 2 == 0 ? firstColumnMargin : secondColumnMargin;
                    margin.Y += row * (size.Y + spacing.Y);

                    Image background = new()
                    {
                        TextureIndex = TextureIndex.UIButtons,
                        SourceRectangle = new(0, 300, 477, 61),
                        Size = size,
                        Alignment = UIDirection.Northwest,
                        Margin = margin,
                    };

                    Label label = new()
                    {
                        SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                        Scale = new(0.065f),
                        Margin = new(16.0f, 0.0f),
                        TextContent = "Title",
                        Alignment = UIDirection.West,

                        BorderColor = AAP64ColorPalette.DarkGray,
                        BorderDirections = LabelBorderDirection.All,
                        BorderOffset = 2.0f,
                        BorderThickness = 2.0f,
                    };

                    this.panelBackground.AddChild(background);
                    background.AddChild(label);

                    this.optionButtonSlotInfos[index] = new(background, null, label);
                    index++;
                }
            }
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
            UpdateCategoryButtons();
            UpdateOptionButtons();
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

        private void UpdateCategoryButtons()
        {
            for (int i = 0; i < this.categoryButtonSlotInfos.Length; i++)
            {
                SlotInfo slotInfo = this.categoryButtonSlotInfos[i];
                ButtonInfo buttonInfo = this.categoryButtonInfos[i];

                if (Interaction.OnMouseEnter(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    SelectCategory(i);
                    break;
                }

                if (Interaction.OnMouseOver(slotInfo.Background))
                {
                    this.tooltipBox.CanDraw = true;
                    TooltipBoxContent.SetTitle(buttonInfo.Name);
                    TooltipBoxContent.SetDescription(buttonInfo.Description);
                    slotInfo.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slotInfo.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdateOptionButtons()
        {
            Category category = this.categories[this.selectedCategoryIndex];

            for (int i = 0; i < category.Options.Length; i++)
            {
                SlotInfo slotInfo = this.optionButtonSlotInfos[i];

                if (slotInfo.Background == null)
                {
                    continue;
                }

                if (Interaction.OnMouseEnter(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseOver(slotInfo.Background))
                {
                    this.tooltipBox.CanDraw = true;
                    TooltipBoxContent.SetTitle(category.Options[i].Name);
                    TooltipBoxContent.SetDescription(category.Options[i].Description);
                    slotInfo.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slotInfo.Background.Color = AAP64ColorPalette.White;
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

