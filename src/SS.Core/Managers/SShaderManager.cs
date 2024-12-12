using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.Managers;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SShaderManager(ISGame gameInstance) : SManager(gameInstance), ISShaderManager
    {
        private Effect[] effects;
        private int effectsLength;

        public override void Initialize()
        {
            this.effects = ((SAssetDatabase)this.SGameInstance.AssetDatabase).GetAllEffects();
            this.effectsLength = this.effects.Length;
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.effectsLength; i++)
            {
                this.effects[i].Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }
        }

        public void Reset()
        {
            return;
        }
    }
}
