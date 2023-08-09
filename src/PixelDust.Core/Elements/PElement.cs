using Microsoft.Xna.Framework;

namespace PixelDust.Core.Elements
{
    public abstract class PElement
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Category { get; protected set; }
        public byte Id { get; internal set; }
        public Color Color { get; protected set; }

        public float DefaultTemperature { get; protected set; } = 20f;
        public int DefaultDispersionRate { get; protected set; } = 1;
        public bool HasColorVariation { get; protected set; } = true;

        internal void Build()
        {
            OnSettings();
        }

        internal void Update(PElementContext ctx)
        {
            OnBeforeStep(ctx);
            OnStep(ctx);
            OnDefaultBehaviourStep(ctx);
            OnAfterStep(ctx);
        }

        protected virtual void OnSettings() { return; }
        protected virtual void OnBeforeStep(PElementContext ctx) { return; }
        protected virtual void OnStep(PElementContext ctx) { return; }
        protected virtual void OnDefaultBehaviourStep(PElementContext ctx) { return; }
        protected virtual void OnAfterStep(PElementContext ctx) { return; }
    }
}
