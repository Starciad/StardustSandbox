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
using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Backgrounds;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

namespace StardustSandbox.Core.UI.Common.Menus
{
    internal sealed class AchievementsUI : UIBase
    {
        private Label titleLabelElement;
        private Image headerBackground;

        private SlotInfo exitButtonSlotInfo;

        private readonly Image[] achievementImages = new Image[UIConstants.ACHIEVEMENTS_PER_PAGE];

        private readonly ButtonInfo exitButtonInfo;

        private readonly AchievementSettings achievementSettings;

        private readonly TooltipBox tooltipBox;
        private readonly AmbientManager ambientManager;

        internal AchievementsUI(
            AmbientManager ambientManager,
            TooltipBox tooltipBox,
            UIManager uiManager
        ) : base()
        {
            this.ambientManager = ambientManager;
            this.tooltipBox = tooltipBox;

            this.achievementSettings = SettingsSerializer.Load<AchievementSettings>();
            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI);
        }

        protected override void OnBuild(Container root)
        {
            BuildHeader(root);
            BuildExitButton();
            BuildAchievementSlots();

            root.AddChild(this.tooltipBox);
        }

        private void BuildHeader(Container root)
        {
            // Background
            this.headerBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Color = new(AAP64ColorPalette.DarkGray, 196),
                Size = Vector2.One,
                Scale = new(GameScreen.GetViewport().X, 96.0f),
            };

            root.AddChild(this.headerBackground);

            // Title
            this.titleLabelElement = new()
            {
                Scale = new(0.15f),
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Alignment = UIDirection.West,
                Margin = new(32.0f, 0.0f),
                TextContent = Localization_GUIs.Achievements_Title,

                BorderColor = AAP64ColorPalette.DarkGray,
                BorderDirections = LabelBorderDirection.All,
                BorderOffset = 2.0f,
                BorderThickness = 2.0f,
            };

            this.headerBackground.AddChild(this.titleLabelElement);
        }

        private void BuildExitButton()
        {
            SlotInfo slot = UIBuilderUtility.BuildButtonSlot(new(-32.0f, 0.0f), this.exitButtonInfo);

            slot.Background.Alignment = UIDirection.East;
            slot.Icon.Alignment = UIDirection.Center;

            this.headerBackground.AddChild(slot.Background);
            slot.Background.AddChild(slot.Icon);

            this.exitButtonSlotInfo = slot;
        }

        private void BuildAchievementSlots()
        {
            Vector2 margin = new(48.0f, 136.0f);

            int rows = UIConstants.ACHIEVEMENTS_PER_ROW;
            int columns = UIConstants.ACHIEVEMENTS_PER_COLUMN;

            int index = 0;

            for (byte col = 0; col < columns; col++)
            {
                for (byte row = 0; row < rows; row++)
                {
                    Image image = new()
                    {
                        TextureIndex = TextureIndex.Achievements,
                        SourceRectangle = new(0, 0, 32, 32),
                        Alignment = UIDirection.Northwest,
                        Scale = new(2.0f),
                        Size = new(32.0f),
                        Margin = margin
                    };

                    this.headerBackground.AddChild(image);
                    margin.X += 80.0f;
                    this.achievementImages[index] = image;
                    index++;
                }

                margin.X = 48.0f;
                margin.Y += 80.0f;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
            UpdateAchievementSlots();
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

        private void UpdateAchievementSlots()
        {
            for (int i = 0; i < this.achievementImages.Length; i++)
            {
                Image image = this.achievementImages[i];

                if (!this.achievementImages[i].CanDraw)
                {
                    break;
                }

                Achievement achievement = AchievementDatabase.GetAchievement((AchievementIndex)i);

                if (Interaction.OnMouseOver(image))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(achievement.Title);
                    TooltipBoxContent.SetDescription(achievement.Description);

                    image.Scale = Vector2.Lerp(image.Scale, new(2.2f), 0.2f);
                }
                else
                {
                    image.Scale = Vector2.Lerp(image.Scale, new(2.0f), 0.2f);
                }
            }
        }

        private void ChangeAchievementsCatalog()
        {
            for (int i = 0; i < this.achievementImages.Length; i++)
            {
                Image image = this.achievementImages[i];

                if (image is null)
                {
                    continue;
                }

                if (i < (int)AchievementIndex.Length)
                {
                    AchievementIndex index = (AchievementIndex)i;

                    Achievement achievement = AchievementDatabase.GetAchievement(index);

                    image.CanDraw = true;

                    image.SourceRectangle = this.achievementSettings.IsUnlocked(index) ? achievement.AchievedIconSourceRectangle : achievement.NotAchievedIconSourceRectangle;
                }
                else
                {
                    image.CanDraw = false;
                }
            }
        }

        protected override void OnOpened()
        {
            this.titleLabelElement.TextContent = string.Concat(Localization_GUIs.Achievements_Title, " (", PercentageMath.PercentageFromValue((int)AchievementIndex.Length, this.achievementSettings.GetUnlockedCount()), "%)");

            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.Credits);
            ChangeAchievementsCatalog();
        }
    }
}
