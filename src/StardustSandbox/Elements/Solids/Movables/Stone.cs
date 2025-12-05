using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Stone : MovableSolid
    {
        internal Stone() : base()
        {

        }

        protected override void OnTemperatureChanged(in ElementContext context, double currentValue)
        {
            if (currentValue > 600)
            {
                context.ReplaceElement(ElementIndex.Lava);
            }
        }
    }
}
