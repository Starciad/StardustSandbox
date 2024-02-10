using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PShaderManager : PGameObject
    {
        private Effect[] _shaders;
        private int _shadersLength;

        protected override void OnAwake()
        {
            this._shaders = this.Game.AssetsDatabase.Shaders;
            this._shadersLength = this._shaders.Length;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _shadersLength; i++)
            {
                _shaders[i].Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }
        }
    }
}
