using PixelDust.Core;
using Microsoft.Xna.Framework;

namespace PixelDust
{
    internal sealed class WorldScene : PScene
    {
        public World World => _world;
        private World _world;

        protected override void OnLoad()
        {
            _world = new();
        }

        protected override void OnUnload()
        {
            _world.Unload();
            _world = null;
        }

        protected override void OnUpdate()
        {
            _world.Update();
        }

        protected override void OnDraw()
        {
            InputManager.DebugString.AppendLine($"Count: {_world.TotalElements}");

            _world.Draw();
            PGraphics.SpriteBatch.DrawString(PFonts.Arial, InputManager.DebugString, Vector2.Zero, Color.White);
        }
    }
}
