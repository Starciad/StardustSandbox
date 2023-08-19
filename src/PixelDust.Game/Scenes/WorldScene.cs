using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;
using PixelDust.Core.Engine;
using PixelDust.Core.Scenes;
using PixelDust.Core.Worlding;
using PixelDust.Game.Managers;

namespace PixelDust.Game.Scenes
{
    internal sealed class WorldScene : PScene
    {
        protected override void OnLoad() { }
        protected override void OnUnload() { }
        protected override void OnUpdate() { }
        protected override void OnDraw()
        {
            PGraphics.SpriteBatch.DrawString(PFonts.Arial, InputManager.DebugString, Vector2.Zero, Color.White);
        }
    }
}
