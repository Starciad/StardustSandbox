using PixelDust.Game.Objects;

namespace PixelDust.Game.Scenes
{
    public abstract class PScene : PGameObject
    {
        public void Unload()
        {
            OnUnload();
        }

        protected virtual void OnUnload() { return; }
    }
}