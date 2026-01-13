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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Colors;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Assets;

using System;

namespace StardustSandbox.Managers
{
    internal sealed class EffectsManager
    {
        private Effect[] effects;
        private int effectsLength;

        internal void Initialize()
        {
            this.effects = AssetDatabase.GetEffects();
            this.effectsLength = this.effects.Length;
        }

        private void UpdateTime(GameTime gameTime)
        {
            for (int i = 0; i < this.effectsLength; i++)
            {
                this.effects[i].Parameters["Time"]?.SetValue(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds));
            }
        }

        private static void UpdateGradientTransitionEffect(GradientColorMap gradientColorMap, TimeSpan timeSpan)
        {
            float interpolation = gradientColorMap.GetInterpolationFactor(timeSpan);

            Effect effect = AssetDatabase.GetEffect(EffectIndex.GradientTransition);

            effect.Parameters["StartColor1"].SetValue(gradientColorMap.GradientStartColor.Start.ToVector4());
            effect.Parameters["StartColor2"].SetValue(gradientColorMap.GradientEndColor.Start.ToVector4());
            effect.Parameters["EndColor1"].SetValue(gradientColorMap.GradientStartColor.End.ToVector4());
            effect.Parameters["EndColor2"].SetValue(gradientColorMap.GradientEndColor.End.ToVector4());
            effect.Parameters["TimeNormalized"].SetValue(interpolation);
        }

        internal void Update(GameTime gameTime, TimeSpan currentTime)
        {
            UpdateTime(gameTime);
            UpdateGradientTransitionEffect(GradientConstants.GetBackgroundGradientByTime(currentTime), currentTime);
        }
    }
}

