using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Stone : MovableSolid
    {
        internal Stone() : base()
        {

        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue > 600)
            {
                this.Context.ReplaceElement(ElementIndex.Lava);
            }
        }
    }
}
