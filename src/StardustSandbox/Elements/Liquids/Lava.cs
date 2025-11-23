using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Liquids
{
    internal sealed class Lava : Liquid
    {
        internal Lava() : base()
        {

        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue <= 500)
            {
                if (this.Context.SlotLayer.StoredElement == null)
                {
                    this.Context.ReplaceElement(ElementIndex.Stone);
                }
                else
                {
                    this.Context.ReplaceElement(this.Context.SlotLayer.StoredElement);
                }

                this.Context.SetElementTemperature(500);
            }
        }
    }
}