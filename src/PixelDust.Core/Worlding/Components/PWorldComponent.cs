namespace PixelDust.Core.Worlding
{
    public abstract class PWorldComponent
    {
        internal void Initialize()
        {
            OnInitialize();
        }

        internal void Update()
        {
            OnUpdate();
        }

        internal void Draw()
        {
            OnDraw();
        }

        protected virtual void OnInitialize() { return; }
        protected virtual void OnUpdate() { return; }
        protected virtual void OnDraw() { return; }
    }
}
