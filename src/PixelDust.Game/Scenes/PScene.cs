using PixelDust.Game.Objects;

namespace PixelDust.Game.Scenes
{
    public abstract class PScene : PGameObject
    {
        public void Load()
        {
            OnLoad();
        }
        public void Unload()
        {
            OnUnload();
        }

        protected virtual void OnLoad() { return; }
        protected virtual void OnUnload() { return; }
    }
}