using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Snow : MovableSolid
    {
        internal Snow() : base()
        {

        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 8)
            {
                if (this.Context.SlotLayer.StoredElement == null)
                {
                    this.Context.ReplaceElement(ElementIndex.Water);
                }
                else
                {
                    this.Context.ReplaceElement(this.Context.SlotLayer.StoredElement);
                }

                this.Context.SetElementTemperature(12);
            }
        }
    }
}
