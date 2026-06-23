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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.UI.Builders;
using StardustSandbox.Core.UI.Dependencies;
using StardustSandbox.Core.UI.Elements.Common;
using StardustSandbox.Core.UI.Handlers;
using StardustSandbox.Core.UI.Models;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class AchievementsUI : UIBase<AchievementsUIDependencies, AchievementsUIModel>
    {
        internal AchievementsUI(AchievementsUIDependencies dependencies, UIElementHandler elementHandler, GameScreen gameScreen) : base(dependencies, elementHandler, gameScreen)
        {

        }

        protected override void OnBuild(UIBuildContext context, AchievementsUIModel model)
        {
            // BUILD
            using UIBuildScope scope = context.BeginLayout();

            Image panel = BuildPanelBackground(scope);
            BuildExitButton(scope, panel);
            BuildAchievementSlots(scope, panel);
            BuildPagination(scope, panel);
        }

        private Image BuildPanelBackground(UIBuildScope scope)
        {
            // Background
            Image panel = scope.AddImage();

            panel.Alignment = UIDirection.Center;
            panel.Size = new(420.0f, 568.0f);
            panel.Texture = this.Dependencies.AssetDatabase.GetTexture(TextureIndex.UIBackgroundAchievements);

            // Title
            Label title = scope.AddLabel();

            title.SpriteFont = this.Dependencies.AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm);
            title.Scale = new(0.1f);
            title.Margin = new(16.0f, 4.0f);
            title.TextContent = Localization_GUIs.Achievements_Title;

            title.BorderDirections = LabelBorderDirection.All;
            title.BorderColor = AAP64ColorPalette.DarkGray;
            title.BorderOffset = 3.0f;
            title.BorderThickness = 3.0f;

            // Adding
            panel.AddChild(title);

            return panel;
        }

        private void BuildExitButton(UIBuildScope scope, Image panel)
        {
            Button exitButton = scope.AddButton();

            exitButton.Alignment = UIDirection.Northeast;
            exitButton.Margin = new(-4.0f, 6.5f);

            exitButton.SetBackground(this.Dependencies.AssetDatabase.GetTexture(TextureIndex.UIButtons), new(320, 140, 32, 32));
            exitButton.SetIcon(this.Dependencies.AssetDatabase.GetTexture(TextureIndex.IconUI), new(224, 0, 32, 32), UIDirection.Center);

            panel.AddChild(exitButton);
        }

        private void BuildAchievementSlots(UIBuildScope scope, Image panel)
        {
            int rows = UIConstants.ACHIEVEMENTS_PER_ROW;
            int columns = UIConstants.ACHIEVEMENTS_PER_COLUMN;

            for (int i = 0; i < rows * columns; i++)
            {
                int row = i % rows;
                int column = i / rows;

                Image image = scope.AddImage();

                image.Texture = this.Dependencies.AssetDatabase.GetTexture(TextureIndex.Achievements);
                image.SourceRectangle = new(0, 0, 32, 32);

                image.Alignment = UIDirection.Northwest;
                image.Scale = new(2.0f);
                image.Size = new(32.0f);
                image.Margin = new(16.0f + (row * 80.0f), 92.0f + (column * 80.0f));

                panel.AddChild(image);
            }
        }

        private void BuildPagination(UIBuildScope scope, Image panel)
        {
            Label pageIndexLabel = scope.AddLabel();
            pageIndexLabel.SpriteFont = this.Dependencies.AssetDatabase.GetSpriteFont(SpriteFontIndex.BigApple3pm);
            pageIndexLabel.Scale = new(0.1f);
            pageIndexLabel.Alignment = UIDirection.South;
            pageIndexLabel.Margin = new(0.0f, -12.0f);
            pageIndexLabel.TextContent = "1 / 1";
            pageIndexLabel.BorderDirections = LabelBorderDirection.All;
            pageIndexLabel.BorderColor = AAP64ColorPalette.DarkGray;
            pageIndexLabel.BorderOffset = 2.0f;
            pageIndexLabel.BorderThickness = 2.0f;

            Button leftButton = scope.AddButton();
            leftButton.Alignment = UIDirection.Southwest;
            leftButton.Margin = new(10.0f, -10.0f);

            Button rightButton = scope.AddButton();
            rightButton.Alignment = UIDirection.Southeast;
            rightButton.Margin = new(-10.0f);

            panel.AddChild(pageIndexLabel);
            panel.AddChild(leftButton);
            panel.AddChild(rightButton);
        }

        protected override void OnOpened()
        {
            this.Dependencies.AmbientManager.BackgroundHandler.SetBackground(BackgroundIndex.Credits);
        }
    }
}
