using PixelDust.Core.Elements;

namespace PixelDust.Core.Worlding
{
    public abstract class PWorldComponent
    {
        protected PWorld WorldInstance { get; private set; }

        internal void Initialize(PWorld instance)
        {
            WorldInstance = instance;
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
