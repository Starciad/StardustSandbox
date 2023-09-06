using PixelDust.Core;
using PixelDust.Core.Scenes;

using PixelDust.Game.Scenes;
using PixelDust.InputSystem;

namespace PixelDust.Game
{
    public class PixelDust : PGame
    {
        protected override void Initialize()
        {
            AddComponent<PInputGameComponent>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void OnStartup()
        {
            PScenesHandler.Load<WorldScene>();
        }
    }
}