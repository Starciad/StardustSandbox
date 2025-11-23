using StardustSandbox.Enums.Elements;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Ice : MovableSolid
    {
        internal Ice() : base()
        {

        }

        protected override void OnTemperatureChanged(double currentValue)
        {
            if (currentValue >= 0)
            {
                if (this.Context.SlotLayer.StoredElement == null)
                {
                    this.Context.ReplaceElement(ElementIndex.Water);
                }
                else
                {
                    this.Context.ReplaceElement(this.Context.SlotLayer.StoredElement);
                }

                this.Context.SetElementTemperature(13);
            }
        }
    }
}
