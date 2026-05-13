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

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Enums.Elements;

namespace StardustSandbox.Core.Elements.Solids.Movables
{
    internal sealed class Dirt : MovableSolid
    {
        internal Dirt(AchievementSystem achievementSystem) : base(achievementSystem)
        {
            Index = ElementIndex.Dirt,
                    Category = ElementCategory.MovableSolid,
                    Characteristics = ElementCharacteristics.HasTemperature |
                                      ElementCharacteristics.IsCorruptible |
                                      ElementCharacteristics.IsPushable,
                    RenderingType = ElementRenderingType.Blob,
                    TextureOriginOffset = new(0, 0),
                    ReferenceColor = AAP64ColorPalette.Clay,
                    DefaultTemperature = 20.0f,
                    DefaultDensity = 1.6f,
                    DefaultExplosionResistance = 1.0f,
        }
    }
}
