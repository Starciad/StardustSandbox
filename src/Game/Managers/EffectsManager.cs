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
