using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;

using System.Collections.Generic;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SPencilTool : STool
    {
        internal SPencilTool(ISGame game, SSimulationPen simulationPen) : base(game, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position)
        {
            switch (contentType)
            {
                case SItemContentType.Element:
                    IEnumerable<Point> targetPoints = this.simulationPen.GetPenShapePoints(position);

                    switch (worldModificationType)
                    {
                        case SWorldModificationType.Adding:
                            DrawElements(this.game.ElementDatabase.GetElementByIdentifier(referencedItemIdentifier), targetPoints);
                            break;

                        case SWorldModificationType.Removing:
                            EraseElements(targetPoints);
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        // ============================================ //
        // Elements

        private void DrawElements(ISElement element, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.InstantiateElement(position, this.simulationPen.Layer, element);
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.DestroyElement(position, this.simulationPen.Layer);
            }
        }

        // ============================================ //
        // Entities

        //private void DrawEntities(string entityIdentifier, Vector2 position)
        //{
        //    _ = this.world.InstantiateEntity(entityIdentifier, (entity) =>
        //    {
        //        SEntityTransformComponent transformComponent = entity.ComponentContainer.GetComponent<SEntityTransformComponent>();

        //        transformComponent.Position = position;
        //    });
        //}

        //private void EraseEntities()
        //{

        //}
    }
}
