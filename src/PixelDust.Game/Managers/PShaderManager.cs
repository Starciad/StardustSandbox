using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Databases;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PShaderManager(PAssetDatabase assetDatabase) : PGameObject
    {
        private Effect[] _shaders;
        private int _shadersLength;

        protected override void OnAwake()
        {
            this._shaders = assetDatabase.Shaders;
            this._shadersLength = this._shaders.Length;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = 0; i < this._shadersLength; i++)
            {
                this._shaders[i].Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }
        }
    }
}
