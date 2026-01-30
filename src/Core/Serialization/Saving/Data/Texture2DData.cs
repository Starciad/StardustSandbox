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

using MessagePack;

using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustSandbox.Core.Serialization.Saving.Data
{
    [Serializable]
    [MessagePackObject]
    public sealed class Texture2DData
    {
        [Key("Data")]
        public byte[] Data { get; set; }

        [Key("Height")]
        public int Height { get; set; }

        [Key("Width")]
        public int Width { get; set; }

        public Texture2DData()
        {

        }

        public Texture2DData(Texture2D texture2d)
        {
            this.Width = texture2d.Width;
            this.Height = texture2d.Height;
            this.Data = new byte[this.Width * this.Height * 4]; // RGBA

            texture2d.GetData(this.Data);
        }

        public Texture2D ToTexture2D(GraphicsDevice graphicsDevice)
        {
            Texture2D texture2d = new(graphicsDevice, this.Width, this.Height);
            texture2d.SetData(this.Data);
            return texture2d;
        }
    }
}

