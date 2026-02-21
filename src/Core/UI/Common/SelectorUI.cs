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
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class SelectorUI : UIBase
    {
        private int currentPageIndex = 0, totalPages = 0;
        private IChoice[] choices, selectedChoices;

        private Action<IChoice> sendCallback;

        private Image panelBackground, shadowBackground;
        private Label title, pageIndexLabel;

        private SlotInfo exitButtonSlotInfo;

        private readonly SlotInfo[] choiceButtonSlotInfos, paginationButtonSlotInfos;

        private readonly ButtonInfo exitButtonInfo;
        private readonly ButtonInfo[] paginationButtonInfos;

        private readonly UIManager uiManager;

        internal SelectorUI(
            UIManager uiManager
        ) : base()
        {
            this.uiManager = uiManager;

            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, string.Empty, uiManager.CloseUI);

            this.paginationButtonInfos =
            [
                new(TextureIndex.IconUI, new(128, 160, 32, 32), Localization_Statements.Previous, string.Empty, () =>
                {
                    if (this.currentPageIndex > 0)
                    {
                        this.currentPageIndex--;
                    }
                    else
                    {
                        this.currentPageIndex = this.totalPages - 1;
                    }

                    RefreshContent();
                }),
                new(TextureIndex.IconUI, new(64, 160, 32, 32), Localization_Statements.Next, string.Empty, () =>
                {
                    if (this.currentPageIndex < this.totalPages - 1)
                    {
                        this.currentPageIndex++;
                    }
                    else
                    {
                        this.currentPageIndex = 0;
                    }

                    RefreshContent();
                }),
            ];

            this.paginationButtonSlotInfos = new SlotInfo[this.paginationButtonInfos.Length];
            this.choiceButtonSlotInfos = new SlotInfo[UIConstants.SELECTOR_CHOICES_PER_PAGE];
        }

        private void RefreshContent()
        {
            this.pageIndexLabel.TextContent = string.Concat(this.currentPageIndex + 1, " / ", this.totalPages);

            int startIndex = this.currentPageIndex * UIConstants.SELECTOR_CHOICES_PER_PAGE;
            int endIndex = Math.Min(startIndex + UIConstants.SELECTOR_CHOICES_PER_PAGE, this.choices.Length);
            int length = endIndex - startIndex;

            this.selectedChoices = new IChoice[length];

            Array.Copy(this.choices, startIndex, this.selectedChoices, 0, length);

            for (int i = 0; i < this.choiceButtonSlotInfos.Length; i++)
            {
                SlotInfo slotInfo = this.choiceButtonSlotInfos[i];

                if (i < length)
                {
                    slotInfo.Background.CanDraw = true;
                    slotInfo.Label.TextContent = this.selectedChoices[i].Name;
                }
                else
                {
                    slotInfo.Background.CanDraw = false;
                }
            }
        }

        internal void Setup(string title, Action<IChoice> sendCallback, params IChoice[] choices)
        {
            this.title.TextContent = title;

            this.sendCallback = sendCallback;
            this.choices = choices;

            this.currentPageIndex = 0;
            this.totalPages = (int)MathF.Max(1.0f, MathF.Ceiling(this.choices.Length / (float)UIConstants.SELECTOR_CHOICES_PER_PAGE));

            RefreshContent();
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildTitle();
            BuildExitButton();
            BuildChoiceButtons();
            BuildPagination();
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
                TextureIndex = TextureIndex.UIBackgroundSelector,
                Size = new(396.0f, 506.0f),
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
                Margin = new(16.0f, 10.0f),
                TextContent = "Selector",

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

        private void BuildChoiceButtons()
        {
            Vector2 margin = new(0.0f, 74.0f);

            for (int i = 0; i < this.choiceButtonSlotInfos.Length; i++)
            {
                Image background = new()
                {
                    TextureIndex = TextureIndex.UIButtons,
                    SourceRectangle = new(0, 361, 396, 91),
                    Size = new(396.0f, 89.0f),
                    Alignment = UIDirection.Northwest,
                    Margin = margin,
                };

                Label label = new()
                {
                    Scale = new(0.075f),
                    SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                    Alignment = UIDirection.Center,
                    TextContent = "Content",

                    BorderDirections = LabelBorderDirection.All,
                    BorderColor = AAP64ColorPalette.DarkGray,
                    BorderOffset = 2.0f,
                    BorderThickness = 2.0f,
                };

                this.panelBackground.AddChild(background);
                background.AddChild(label);

                this.choiceButtonSlotInfos[i] = new(background, null, label);

                margin.Y += background.Size.Y;
            }
        }

        private void BuildPagination()
        {
            this.pageIndexLabel = new()
            {
                Scale = new(0.1f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = UIDirection.South,
                Margin = new(0.0f, -12.0f),
                TextContent = "1 / 1",

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.panelBackground.AddChild(this.pageIndexLabel);

            for (int i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = new(
                    new()
                    {
                        TextureIndex = TextureIndex.UIButtons,
                        SourceRectangle = new(320, 140, 32, 32),
                        Scale = new(1.6f),
                        Size = new(32.0f),
                    },

                    new()
                    {
                        TextureIndex = this.paginationButtonInfos[i].TextureIndex,
                        SourceRectangle = this.paginationButtonInfos[i].TextureSourceRectangle,
                        Alignment = UIDirection.Center,
                        Size = new(32.0f)
                    }
                );

                // Spacing
                this.paginationButtonSlotInfos[i] = slot;

                // Adding
                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }

            SlotInfo left = this.paginationButtonSlotInfos[0];
            left.Background.Alignment = UIDirection.Southwest;
            left.Background.Margin = new(10.0f, -10.0f);

            SlotInfo right = this.paginationButtonSlotInfos[1];
            right.Background.Alignment = UIDirection.Southeast;
            right.Background.Margin = new(-10.0f);

            for (int i = 0; i < this.paginationButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                this.panelBackground.AddChild(slot.Background);
                slot.Background.AddChild(slot.Icon);
            }
        }

        protected override void OnResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateExitButton();
            UpdateChoiceButtons();
            UpdatePagination();
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
                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.HoverColor;
            }
            else
            {
                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.White;
            }
        }

        private void UpdateChoiceButtons()
        {
            for (int i = 0; i < this.selectedChoices.Length; i++)
            {
                SlotInfo slotInfo = this.choiceButtonSlotInfos[i];
                IChoice choice = this.selectedChoices[i];

                if (Interaction.OnMouseEnter(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slotInfo.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.sendCallback?.Invoke(choice);
                    this.uiManager.CloseUI();

                    break;
                }

                if (Interaction.OnMouseOver(slotInfo.Background))
                {
                    slotInfo.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slotInfo.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        private void UpdatePagination()
        {
            for (int i = 0; i < this.paginationButtonInfos.Length; i++)
            {
                SlotInfo slot = this.paginationButtonSlotInfos[i];

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Click);
                    this.paginationButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
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
