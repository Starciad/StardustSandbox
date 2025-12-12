using StardustSandbox.Generators;

namespace StardustSandbox.Elements.Energies
{
    internal sealed class LightningHead : Energy
    {
        protected override void OnInstantiated(in ElementContext context)
        {
            LightningGenerator.Start(context, context.Position);
        }

        protected override void OnAfterStep(in ElementContext context)
        {
            context.RemoveElement();
        }
    }
}