using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.Components.AI.Entities.Living.Animals;
using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Entities.Living.Animals
{
    internal sealed class SAntEntityDescriptor : SEntityDescriptor
    {
        internal SAntEntityDescriptor(string identifier) : base(identifier)
        {

        }

        public override SEntity CreateEntity(ISGame gameInstance)
        {
            return new SAntEntity(gameInstance, this.Identifier);
        }
    }

    internal sealed class SAntEntity : SEntity
    {
        private readonly Texture2D texture;

        private readonly SEntityTransformComponent transformComponent;
        private readonly SEntityGraphicsComponent graphicsComponent;
        private readonly SEntityRenderingComponent renderingComponent;

        internal SAntEntity(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.transformComponent = this.ComponentContainer.AddComponent(new SEntityTransformComponent(this.SGameInstance, this));
            this.graphicsComponent = this.ComponentContainer.AddComponent(new SEntityGraphicsComponent(this.SGameInstance, this));
            this.renderingComponent = this.ComponentContainer.AddComponent(new SEntityRenderingComponent(this.SGameInstance, this, this.transformComponent, this.graphicsComponent));
            _ = this.ComponentContainer.AddComponent(new SAntEntityAIComponent(this.SGameInstance, this));

            // Graphics
            this.texture = gameInstance.AssetDatabase.GetTexture("entity_1");
            this.graphicsComponent.SetTexture(this.texture);
            this.renderingComponent.ClipArea = new Rectangle(new(0), new(32));
        }
    }
}
