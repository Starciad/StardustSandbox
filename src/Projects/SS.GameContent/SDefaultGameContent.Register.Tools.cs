using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Content;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.GameContent.Tools;

namespace StardustSandbox.GameContent
{
    public sealed partial class SDefaultGameContent : SGameContent
    {
        protected override void OnRegisterTools(ISGame game, ISToolDatabase toolDatabase)
        {
            toolDatabase.RegisterTool(new SHeatTool(SToolConstants.HEAT_IDENTIFIER));
            toolDatabase.RegisterTool(new SFreezeTool(SToolConstants.FREEZE_IDENTIFIER));
        }
    }
}
