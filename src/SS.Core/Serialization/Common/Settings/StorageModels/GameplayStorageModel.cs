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

using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Interfaces.Serialization.Morph;

namespace StardustSandbox.Core.Serialization.Common.Settings.StorageModels
{
    public sealed class GameplayStorageModel : IStorageModel
    {
        public bool EnableSmoothCameraMovement { get; set; }
        public byte GridOpacity { get; set; }
        public Color PreviewAreaColor { get; set; }
        public bool ShowGrid { get; set; }
        public bool ShowPreviewArea { get; set; }
        public bool ShowTemperatureColorVariations { get; set; }

        public GameplayStorageModel()
        {
            this.ShowPreviewArea = true;
            this.PreviewAreaColor = new(AAP64ColorPalette.DarkGray, 0.15f);
            this.ShowGrid = true;
            this.GridOpacity = 25;
            this.ShowTemperatureColorVariations = true;
            this.EnableSmoothCameraMovement = true;
        }
    }
}

