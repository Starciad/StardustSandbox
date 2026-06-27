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

using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.UI.Builders;
using StardustSandbox.Core.UI.Dependencies;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Handlers;
using StardustSandbox.Core.UI.Models;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed class ExperimentalUI : UIBase<ExperimentalUIDependencies, ExperimentalUIModel>
    {
        internal ExperimentalUI(ExperimentalUIDependencies dependencies, GameScreen gameScreen, UIElementHandler elementHandler) : base(dependencies, gameScreen, elementHandler)
        {

        }

        private Container BuildPage1(UIBuildScope scope)
        {
            Container container = scope.AddContainer();
            container.Size = this.GameScreen.Viewport;

            Image panelImage = scope.AddImage();
            panelImage.Texture = this.Dependencies.AssetDatabase.GetTexture(TextureIndex.UIPanels);
            panelImage.SourceRectangle = new(0, 0, 542, 270);
            panelImage.Scale = new(1.0f);
            panelImage.Size = new(542.0f, 270.0f);
            panelImage.Alignment = UIAlignment.Center;

            container.AddChild(panelImage);

            return container;
        }

        protected override void OnBuild(UIBuildContext context, ExperimentalUIModel model)
        {
            using UIBuildScope scope = context.BeginLayout();

            Button nextPageButton = scope.AddButton();
            Button previousPageButton = scope.AddButton();

            Container[] containers =
            [
                BuildPage1(scope)
            ];
        }
    }
}
