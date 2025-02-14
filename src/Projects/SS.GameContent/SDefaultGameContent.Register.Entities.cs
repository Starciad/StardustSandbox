using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.GameContent.Entities.Specials;

namespace StardustSandbox.GameContent
{
    public sealed partial class SDefaultGameContent
    {
        protected override void OnRegisterEntities(ISGame game, ISEntityDatabase entityDatabase)
        {
            entityDatabase.RegisterEntityDescriptor(new SMagicCursorEntityDescriptor(SEntityConstants.MAGIC_CURSOR_IDENTIFIER));
        }
    }
}