using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World;

namespace StardustSandbox.ContentBundle.Components.Entities.AI
{
    internal sealed class SMagicCursorEntityAIComponent : SEntityComponent
    {
        private SElementId selectedElement;

        private readonly SWorld world;
        private readonly SSize2 worldSize;

        private readonly SEntityTransformComponent transformComponent;

        private static readonly SElementId[] allowedElements = [
            SElementId.Dirt,
            SElementId.Mud,
            SElementId.Water,
            SElementId.Stone,
            SElementId.Grass,
            SElementId.Ice,
            SElementId.Sand,
            SElementId.Snow,
            SElementId.MCorruption,
            SElementId.Lava,
            SElementId.Acid,
            SElementId.Glass,
            SElementId.Metal,
            SElementId.Wall,
            SElementId.Wood,
            SElementId.GCorruption,
            SElementId.LCorruption,
            SElementId.IMCorruption,
            SElementId.Steam,
            SElementId.Smoke,
            SElementId.RedBrick,
            SElementId.TreeLeaf,
            SElementId.MountingBlock,
            SElementId.Fire
        ];

        public SMagicCursorEntityAIComponent(ISGame gameInstance, SEntity entityInstance, SEntityTransformComponent transformComponent) : base(gameInstance, entityInstance)
        {
            this.world = gameInstance.World;
            this.transformComponent = transformComponent;
            this.worldSize = this.world.Infos.Size;

            this.selectedElement = allowedElements.GetRandomItem();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Move
            base.Update(gameTime);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}
