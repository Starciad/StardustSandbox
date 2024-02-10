using Microsoft.Xna.Framework;

using PixelDust.Game.InputSystem.Handlers;

namespace PixelDust.Game.InputSystem
{
    public sealed class PInputGameComponent
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            PInputHandler.Update();
        }
    }
}
