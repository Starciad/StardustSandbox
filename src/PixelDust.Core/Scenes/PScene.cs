namespace PixelDust.Core
{
    public abstract class PScene
    {
        internal void Load()
        {
            OnLoad();
        }

        internal void Unload()
        {
            OnUnload();
        }

        internal void Update()
        {
            OnUpdate();
        }

        internal void Draw()
        {
            OnDraw();
        }

        protected abstract void OnLoad();
        protected abstract void OnUnload();
        protected virtual void OnUpdate() { return; }
        protected virtual void OnDraw() { return; }
    }
}
