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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Generators;
using StardustSandbox.Core.Managers;

namespace StardustSandbox.Core.Elements.Energies
{
    internal sealed class LightningHead : Energy
    {
        internal LightningHead(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementManager achievementManager, StatisticsManager statisticsManager) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementManager, statisticsManager)
        {
            this.InitialTemperature = TemperatureConstants.MAX_CELSIUS_VALUE;
            this.BaseDensity = 0.0f;
        }

        protected override void OnInstantiated(ElementContext context)
        {
            LightningGenerator.Start(context, context.CurrentPosition);
        }

        protected override void OnStep(ElementContext context)
        {
            context.RemoveElement();
        }
    }
}
