using Microsoft.Xna.Framework;

using PixelDust.Core.Components;
using PixelDust.InputSystem.Handlers;

namespace PixelDust.InputSystem
{
    public sealed class PInputGameComponent : PGameComponent
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            PInputHandler.Update();
        }
    }
}
