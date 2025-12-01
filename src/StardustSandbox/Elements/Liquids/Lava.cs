using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Lava : Liquid
    {
        internal Lava() : base()
        {

        }

        protected override void OnTemperatureChanged(ElementContext context, double currentValue)
        {
            if (currentValue <= 500)
            {
                if (context.SlotLayer.StoredElement == null)
                {
                    context.ReplaceElement(ElementIndex.Stone);
                }
                else
                {
                    context.ReplaceElement(context.SlotLayer.StoredElement);
                }

                context.SetElementTemperature(500);
            }
        }
    }
}