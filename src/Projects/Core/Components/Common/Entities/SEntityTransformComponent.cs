using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Components.Common.Entities
{
    public sealed class SEntityTransformComponent : SEntityComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        public SEntityTransformComponent(ISGame gameInstance, SEntity entityInstance) : base(gameInstance, entityInstance)
        {
            Reset();
        }

        public override void Reset()
        {
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Rotation = 0f;
        }
    }
}
