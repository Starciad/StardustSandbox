using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;

using System.Collections.Generic;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.WorldTools
{
    internal sealed class SWorldPencilTool : SWorldTool
    {
        internal SWorldPencilTool(SWorldHandler worldHandler, ISGame gameInstance, SSimulationPen simulationPen) : base(worldHandler, gameInstance, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position)
        {
            IEnumerable<Point> targetPoints = this.simulationPen.GetShapePoints(position);

            switch (contentType)
            {
                case SItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case SWorldModificationType.Adding:
                            DrawElements(this.gameInstance.ElementDatabase.GetElementByIdentifier(referencedItemIdentifier), targetPoints);
                            break;

                        case SWorldModificationType.Removing:
                            EraseElements(targetPoints);
                            break;

                        default:
                            break;
                    }
                    break;

                case SItemContentType.Tool:
                    ExecuteTool(referencedItemIdentifier, targetPoints);
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

        // ============================================ //
        // Tools

        private void ExecuteTool(string identifier, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.worldHandler.ToolContext.Update(position, this.simulationPen.Layer);
                this.gameInstance.ToolDatabase.GetToolByIdentifier(identifier).Execute(this.worldHandler.ToolContext);
            }
            
        }
    }
}
