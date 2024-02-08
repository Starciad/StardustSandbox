namespace PixelDust.Core.Worlding.Components
{
    public abstract class PWorldComponent
    {
        protected PWorld WorldInstance { get; private set; }

        internal void Initialize(PWorld instance)
        {
            this.WorldInstance = instance;
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
