namespace PixelDust.Core
{
    public abstract class PManager
    {
        internal void Build()
        {
            OnAwake();
        }

        internal void Initializer()
        {
            OnStart();
        }

        internal void Update()
        {
            OnUpdate();
        }

        protected virtual void OnAwake() { return; }
        protected virtual void OnStart() { return; }
        protected virtual void OnUpdate() { return; }
    }
}
