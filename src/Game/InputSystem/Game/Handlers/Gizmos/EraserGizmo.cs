using Microsoft.Xna.Framework;

using StardustSandbox.Actors;
using StardustSandbox.Constants;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
{
    internal sealed class EraserGizmo : Gizmo
    {
        internal EraserGizmo(ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler) : base(actorManager, pen, world, worldHandler)
        {

        }

        internal override void Execute(in WorldModificationType worldModificationType, in InputState inputState, in ItemContentType contentType, in int contentIndex, in Point position)
        {
            switch (contentType)
            {
                case ItemContentType.Element:
                    EraseElements(this.pen.GetShapePoints(position));
                    break;

                case ItemContentType.Actor:
                    EraseActors(this.pen.GetShapePoints(position));
                    break;

                default:
                    break;
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.RemoveElement(position, this.pen.Layer);
            }
        }

        private void EraseActors(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                Rectangle targetRectangle = new(position.X * SpriteConstants.SPRITE_DEFAULT_WIDTH, position.Y * SpriteConstants.SPRITE_DEFAULT_HEIGHT, SpriteConstants.SPRITE_DEFAULT_WIDTH, SpriteConstants.SPRITE_DEFAULT_HEIGHT);

                foreach (Actor actor in this.actorManager.InstantiatedActors)
                {
                    if (targetRectangle.Intersects(actor.SelfRectangle))
                    {
                        this.actorManager.Destroy(actor);
                    }
                }
            }
        }
    }
}
