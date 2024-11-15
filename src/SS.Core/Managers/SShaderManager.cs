using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.Managers
{
    public sealed class SShaderManager(ISGame gameInstance) : SGameObject(gameInstance)
    {
        private Effect[] shaders;
        private int shadersLength;

        public override void Initialize()
        {
            this.shaders = this.SGameInstance.AssetDatabase.Shaders;
            this.shadersLength = this.shaders.Length;
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.shadersLength; i++)
            {
                this.shaders[i].Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }
        }
    }
}
