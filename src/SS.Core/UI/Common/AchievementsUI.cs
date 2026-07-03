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

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization.Common.Progress.StorageModels;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

using System;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class AchievementsUI : UIBase
    {
        private int currentPageIndex = 0, totalPages = 0;
        private Range achievementsRange;

        private Label title, pageIndexLabel;
        private Image panelBackground;

        private SlotInfo exitButtonSlotInfo;
        private readonly SlotInfo[] paginationButtonSlotInfos;

        private readonly Image[] achievementImages = new Image[UIConstants.ACHIEVEMENTS_PER_PAGE];

        private readonly ButtonInfo exitButtonInfo;
        private readonly ButtonInfo[] paginationButtonInfos;

        private readonly TooltipBox tooltipBox;

        private readonly AchievementDatabase achievementDatabase;
        private readonly AssetDatabase assetDatabase;
        private readonly AmbientManager ambientManager;
        private readonly AchievementStorageModel achievementProgress;
        private readonly SoundEffectManager soundEffectManager;

        internal AchievementsUI(
            AchievementDatabase achievementDatabase,
            AchievementStorageModel achievementProgress,
            AssetDatabase assetDatabase,
            AmbientManager ambientManager,
            GameScreen gameScreen,
            SoundEffectManager soundEffectManager,
            TooltipBox tooltipBox,
            UIManager uiManager
        ) : base(assetDatabase, gameScreen)
        {
            this.achievementDatabase = achievementDatabase;
            this.achievementProgress = achievementProgress;
            this.assetDatabase = assetDatabase;
            this.ambientManager = ambientManager;
            this.soundEffectManager = soundEffectManager;
            this.tooltipBox = tooltipBox;

            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI);

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
        }

        private void RefreshContent()
        {
            this.pageIndexLabel.TextContent = string.Concat(this.currentPageIndex + 1, " / ", this.totalPages);

            this.achievementsRange = new(
                this.currentPageIndex * UIConstants.ACHIEVEMENTS_PER_PAGE,
                Math.Min(
                    (this.totalPages * UIConstants.ACHIEVEMENTS_PER_PAGE) + UIConstants.ACHIEVEMENTS_PER_PAGE,
                    this.achievementDatabase.Length
                )
            );

            int length = this.achievementsRange.End.Value - this.achievementsRange.Start.Value;

            for (int i = 0; i < this.achievementImages.Length; i++)
            {
                Image image = this.achievementImages[i];

                if (i < length)
                {
                    image.CanDraw = true;
                    image.Texture = this.assetDatabase.GetTexture(TextureIndex.Achievements);

                    AchievementIndex index = (AchievementIndex)(this.achievementsRange.Start.Value + i + 1);

                    Achievement achievement = this.achievementDatabase.GetAchievement(index);

                    image.SourceRectangle = this.achievementProgress.IsUnlocked(index)
                        ? achievement.AchievedIconSourceRectangle
                        : achievement.NotAchievedIconSourceRectangle;
                }
                else
                {
                    image.CanDraw = false;
                }
            }
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildExitButton();
            BuildAchievementSlots();
            BuildPagination();

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            // Background
            this.panelBackground = new(this.assetDatabase.GetTexture(TextureIndex.UIBackgroundAchievements))
            {
                Alignment = UIDirection.Center,
                Size = new(420.0f, 568.0f),
            };

            // Title
            this.title = new(this.assetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm))
            {
                Scale = new(0.1f),
                Margin = new(16.0f, 4.0f),
                TextContent = Localization_GUIs.Achievements_Title,

                BorderDirections = LabelBorderDirection.All,
                BorderColor = AAP64ColorPalette.DarkGray,
                BorderOffset = 3.0f,
                BorderThickness = 3.0f,
            };

            root.AddChild(this.panelBackground);
            this.panelBackground.AddChild(this.title);
        }

        private void BuildExitButton()
        {
            SlotInfo slot = this.ElementFactory.BuildButtonSlot(new(-4.0f, 6.5f), this.exitButtonInfo);

            slot.Background.Alignment = UIDirection.Northeast;
            slot.Icon.Alignment = UIDirection.Center;

            this.panelBackground.AddChild(slot.Background);
            slot.Background.AddChild(slot.Icon);

            this.exitButtonSlotInfo = slot;
        }

        private void BuildAchievementSlots()
        {
            int rows = UIConstants.ACHIEVEMENTS_PER_ROW;
            int columns = UIConstants.ACHIEVEMENTS_PER_COLUMN;

            for (int i = 0; i < rows * columns; i++)
            {
                int row = i % rows;
                int column = i / rows;

                Image image = new(this.assetDatabase.GetTexture(TextureIndex.Achievements), new(0, 0, 32, 32))
                {
                    Alignment = UIDirection.Northwest,
                    Scale = new(2.0f),
                    Size = new(32.0f),
                    Margin = new(16.0f + (row * 80.0f), 92.0f + (column * 80.0f))
                };

                this.panelBackground.AddChild(image);
                this.achievementImages[i] = image;
            }
        }

        private void BuildPagination()
        {
            this.pageIndexLabel = new(this.assetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm))
            {
                Scale = new(0.1f),
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
                    new(this.assetDatabase.GetTexture(TextureIndex.UIButtons), new(320, 140, 32, 32))
                    {
                        Scale = new(1.6f),
                        Size = new(32.0f),
                    },

                    new(this.assetDatabase.GetTexture(this.paginationButtonInfos[i].TextureIndex), this.paginationButtonInfos[i].TextureSourceRectangle)
                    {
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

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
            UpdateAchievementSlots();
            UpdatePagination();
        }

        private void UpdateExitButton()
        {
            if (Interaction.OnMouseEnter(this.exitButtonSlotInfo.Background))
            {
                this.soundEffectManager.Play(SoundEffectIndex.GUI_Hover);
            }

            if (Interaction.OnMouseLeftClick(this.exitButtonSlotInfo.Background))
            {
                this.soundEffectManager.Play(SoundEffectIndex.GUI_Click);
                this.exitButtonInfo.ClickAction?.Invoke();
            }

            if (Interaction.OnMouseOver(this.exitButtonSlotInfo.Background))
            {
                this.tooltipBox.CanDraw = true;

                this.tooltipBox.SetTitle(this.exitButtonInfo.Name);
                this.tooltipBox.SetDescription(this.exitButtonInfo.Description);

                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.HoverColor;
            }
            else
            {
                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.White;
            }
        }

        private void UpdateAchievementSlots()
        {
            for (int i = this.achievementsRange.Start.Value; i < this.achievementsRange.End.Value; i++)
            {
                Image image = this.achievementImages[i % UIConstants.ACHIEVEMENTS_PER_PAGE];
                Achievement achievement = this.achievementDatabase.GetAchievement((AchievementIndex)(i + 1));

                if (Interaction.OnMouseOver(image))
                {
                    this.tooltipBox.CanDraw = true;

                    this.tooltipBox.SetTitle(achievement.Title);
                    this.tooltipBox.SetDescription(achievement.Description);

                    image.Scale = Vector2.Lerp(image.Scale, new(2.2f), 0.2f);
                }
                else
                {
                    image.Scale = Vector2.Lerp(image.Scale, new(2.0f), 0.2f);
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
                    this.soundEffectManager.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.soundEffectManager.Play(SoundEffectIndex.GUI_Click);
                    this.paginationButtonInfos[i].ClickAction?.Invoke();
                    break;
                }

                slot.Background.Color = Interaction.OnMouseOver(slot.Background) ? AAP64ColorPalette.HoverColor : AAP64ColorPalette.White;
            }
        }

        protected override void OnOpened()
        {
            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.Credits);

            this.currentPageIndex = 0;
            this.totalPages = (int)MathF.Max(1.0f, MathF.Ceiling(this.achievementDatabase.Length / (float)UIConstants.ACHIEVEMENTS_PER_PAGE));

            RefreshContent();
        }
    }
}
