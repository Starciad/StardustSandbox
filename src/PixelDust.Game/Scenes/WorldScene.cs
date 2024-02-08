using Microsoft.Xna.Framework;

using PixelDust.Core.Engine.Assets;
using PixelDust.Core.Engine.Components;
using PixelDust.Core.Scenes;
using PixelDust.Game.Managers;

namespace PixelDust.Game.Scenes
{
    internal sealed class WorldScene : PScene
    {
        protected override void OnLoad()
        {
            //PGUIEngine.ActiveGUI<PGUIHud>();
        }
        protected override void OnUnload() { }
        protected override void OnUpdate() { }
        protected override void OnDraw()
        {
            PGraphics.SpriteBatch.DrawString(PFonts.Arial, InputManager.DebugString, Vector2.Zero, Color.White);
        }
    }
}
