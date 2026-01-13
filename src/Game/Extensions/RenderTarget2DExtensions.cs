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

namespace StardustSandbox.Extensions
{
    internal static class RenderTarget2DExtensions
    {
        internal static void FlattenAlpha(this RenderTarget2D renderTarget)
        {
            int width = renderTarget.Width;
            int height = renderTarget.Height;

            Color[] data = new Color[width * height];
            renderTarget.GetData(data);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new(
                    data[i].R,
                    data[i].G,
                    data[i].B,
                    (byte)255
                );
            }

            renderTarget.SetData(data);
        }
    }
}

