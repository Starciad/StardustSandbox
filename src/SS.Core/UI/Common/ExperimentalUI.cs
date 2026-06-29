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
            panelImage.Texture = this.Dependencies.AssetDatabase.GetTexture(TextureIndex.UI);
            panelImage.SourceRectangle = new(1024, 0, 542, 270);
            panelImage.Scale = new(2.0f);
            panelImage.Size = new(542.0f, 270.0f);
            panelImage.Alignment = UIAlignment.Center;

            container.AddChild(panelImage);

            Text panelText = scope.AddText();
            panelText.SpriteFont = this.Dependencies.AssetDatabase.GetSpriteFont(SpriteFontIndex.Font_01);
            panelText.Scale = new(0.15f);
            panelText.LineHeight = 360.0f;
            panelText.TextContent = "Hello World! [BreakLine] Experimental [SetColor:255,32,96,255] Changes. [ResetColor] [BreakLine] Omg, what's [SetColor:50,80,53,255] this place? [BreakLine] I'm [ResetColor] not...";
            
            panelImage.AddChild(panelText);

            return container;
        }

        protected override void OnBuild(UIBuildContext context, ExperimentalUIModel model)
        {
            using UIBuildScope scope = context.BeginLayout();

            Container[] containers =
            [
                BuildPage1(scope)
            ];
        }
    }
}
