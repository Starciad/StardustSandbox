using Microsoft.Xna.Framework;

namespace StardustSandbox.Elements.Energies
{
    internal sealed class Spark : Energy
    {
        protected override void OnStep(ElementContext context)
        {
            if (context.GetStoredElement() is null)
            {
                // If you are not on an element, fall.

                context.UpdateElementPosition(context.Position + new Point(0, 1));
            }
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {

        }
    }
}
