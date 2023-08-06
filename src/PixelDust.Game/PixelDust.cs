using PixelDust.Core;
using PixelDust.Core.Scenes;

using PixelDust.Game.Scenes;

namespace PixelDust.Game
{
    public class PixelDust : PEngineInstance
    {
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void OnStartup()
        {
            PSceneManager.Load<WorldScene>();
        }
    }
}