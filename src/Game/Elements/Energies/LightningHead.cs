using StardustSandbox.Generators;

namespace StardustSandbox.Elements.Energies
{
    internal sealed class LightningHead : Energy
    {
        protected override void OnInstantiated(ElementContext context)
        {
            LightningGenerator.Start(context, context.Position);
        }

        protected override void OnAfterStep(ElementContext context)
        {
            context.RemoveElement();
        }
    }
}