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
        // private const string EXPERIMENTAL_STRING = "<c:emeraldgreen>Dirt</c> is a loose, granular material that piles up naturally under gravity.<br/><b>It can absorb water</b>, slowly turning into <c:brown>mud</c>, <i>and supports plant growth when</i><br/>combined with <c:forestgreen>seeds</c>.";
        private const string EXPERIMENTAL_STRING = "A";

        internal ExperimentalUI(ExperimentalUIDependencies dependencies, GameScreen gameScreen, UIElementHandler elementHandler) : base(dependencies, gameScreen, elementHandler)
        {

        }

        protected override void OnBuild(UIBuildContext context, ExperimentalUIModel model)
        {
            using UIBuildScope scope = context.BeginLayout();

            ImageElement image = scope.AddImage();
            image.Texture = this.Dependencies.AssetDatabase.GetTexture(TextureIndex.Pixel);
            image.Color = AAP64ColorPalette.DarkRed;

            TextElement text = scope.AddText();
            text.SpriteFont = this.Dependencies.AssetDatabase.GetSpriteFont(SpriteFontIndex.Font_01);
            text.Scale = new(0.08f);
            text.Color = AAP64ColorPalette.White;
            text.WrapContent = true;
            text.AreaSize = new(485.0f, 328.0f);
            text.SetTextContent(EXPERIMENTAL_STRING);

            image.Position = text.Position;
            image.Scale = text.Size;
        }
    }
}
