using StardustSandbox.UI.Elements;

namespace StardustSandbox.UI.Information
{
    internal sealed class PlusMinusButtonInfo(UIElement plusElement, UIElement minusElement)
    {
        internal UIElement PlusElement => plusElement;
        internal UIElement MinusElement => minusElement;
    }
}
