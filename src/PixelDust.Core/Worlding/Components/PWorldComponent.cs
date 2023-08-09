namespace PixelDust.Core.Worlding
{
    public abstract class PWorldComponent
    {
        protected PWorld World { get; private set; }

        internal void Initialize(PWorld world)
        {
            World = world;
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
