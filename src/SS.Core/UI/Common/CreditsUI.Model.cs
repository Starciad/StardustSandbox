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
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class CreditsUI
    {
        private enum CreditContentType : byte
        {
            Text,
            Title,
            Image
        }

        private sealed class CreditSection(string title, CreditContent[] contents)
        {
            internal string Title => title;
            internal CreditContent[] Contents => contents;
        }

        private sealed class CreditContent
        {
            internal CreditContentType ContentType { get; init; }
            internal string Text { get; init; }
            internal Texture2D Texture { get; init; }
            internal Vector2 TextureScale { get; init; }
            internal Vector2 Margin { get; init; }

            public CreditContent()
            {
                this.ContentType = CreditContentType.Text;
                this.TextureScale = Vector2.One;
            }
        }
    }
}
