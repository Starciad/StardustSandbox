using StardustSandbox.ContentBundle.Tools;
using StardustSandbox.Core.Bundles;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SDefaultGameBundle : SGameBundle
    {
        protected override void OnRegisterTools(ISGame game, ISToolDatabase toolDatabase)
        {
            toolDatabase.RegisterTool(new SHeatTool("heat_tool"));
            toolDatabase.RegisterTool(new SFreezeTool("freeze_tool"));
        }
    }
}
