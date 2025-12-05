using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Databases;

using System;

namespace StardustSandbox.Managers
{
    internal sealed class ShaderManager
    {
        private Effect[] effects;
        private int effectsLength;

        internal void Initialize()
        {
            this.effects = AssetDatabase.GetEffects();
            this.effectsLength = this.effects.Length;
        }

        internal void Update(in GameTime gameTime)
        {
            for (int i = 0; i < this.effectsLength; i++)
            {
                this.effects[i].Parameters["Time"]?.SetValue(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds));
            }
        }
    }
}
