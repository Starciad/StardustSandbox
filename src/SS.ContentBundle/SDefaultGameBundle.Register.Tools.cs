using StardustSandbox.ContentBundle.Tools;
using StardustSandbox.Core.Bundles;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SDefaultGameBundle : SGameBundle
    {
        protected override void OnRegisterTools(ISGame game, ISToolDatabase toolDatabase)
        {
            toolDatabase.RegisterTool(new SHeatTool(SToolConstants.HEAT_IDENTIFIER));
            toolDatabase.RegisterTool(new SFreezeTool(SToolConstants.FREEZE_IDENTIFIER));
        }
    }
}
