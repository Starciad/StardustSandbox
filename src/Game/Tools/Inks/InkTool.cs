using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Elements;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.Tools.Inks
{
    internal abstract class InkTool : Tool
    {
        internal required Color InkColor { get; init; }

        internal override void Execute(ToolContext context)
        {
            if (!context.World.TryGetSlot(context.Position, out Slot slot) ||
                slot.GetLayer(context.Layer).HasState(ElementStates.IsEmpty))
            {
                return;
            }

            context.World.SetElementColorModifier(context.Position, context.Layer, this.InkColor);
        }
    }
}
