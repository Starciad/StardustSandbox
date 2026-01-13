/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using MessagePack;

using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class Texture2DData
    {
        [Key(0)]
        public int Width { get; init; }

        [Key(1)]
        public int Height { get; init; }

        [Key(2)]
        public byte[] PixelData { get; init; }

        public Texture2DData()
        {

        }

        public Texture2DData(Texture2D texture2d)
        {
            this.Width = texture2d.Width;
            this.Height = texture2d.Height;
            this.PixelData = new byte[this.Width * this.Height * 4]; // RGBA

            texture2d.GetData(this.PixelData);
        }

        public Texture2D ToTexture2D(GraphicsDevice graphicsDevice)
        {
            Texture2D texture2d = new(graphicsDevice, this.Width, this.Height);
            texture2d.SetData(this.PixelData);
            return texture2d;
        }
    }
}

