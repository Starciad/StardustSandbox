using PixelDust.Core;

namespace PixelDust
{
    public class Game : PEngineInstance
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