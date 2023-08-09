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
        public PWorld World => _world;
        private PWorld _world;

        protected override void OnLoad()
        {
            _world = new();
            _world.Initialize();
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
            InputManager.DebugString.AppendLine($"Count: {_world.Infos.TotalElements}");

            _world.Draw();
            PGraphics.SpriteBatch.DrawString(PFonts.Arial, InputManager.DebugString, Vector2.Zero, Color.White);
        }
    }
}
