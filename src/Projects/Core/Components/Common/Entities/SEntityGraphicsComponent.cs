using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Components.Common.Entities
{
    public sealed class SEntityGraphicsComponent : SEntityComponent
    {
        public Texture2D Texture => this.texture;

        private Texture2D texture;

        public SEntityGraphicsComponent(ISGame gameInstance, SEntity entityInstance) : base(gameInstance, entityInstance)
        {
            Reset();
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public override void Reset()
        {
            this.texture = null;
        }
    }
}
