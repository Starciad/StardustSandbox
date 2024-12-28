using StardustSandbox.ContentBundle.Entities.Living.Animals;
using StardustSandbox.ContentBundle.Entities.Specials;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterEntities(ISGame game, ISEntityDatabase entityDatabase)
        {
            entityDatabase.RegisterEntityDescriptor(new SMagicCursorEntityDescriptor(SEntityConstants.IDENTIFIER_MAGIC_CURSOR));
            entityDatabase.RegisterEntityDescriptor(new SAntEntityDescriptor(SEntityConstants.IDENTIFIER_ANT));
        }
    }
}