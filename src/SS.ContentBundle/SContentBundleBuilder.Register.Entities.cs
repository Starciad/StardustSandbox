using StardustSandbox.ContentBundle.Entities.Specials;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterEntities(ISGame game, SEntityDatabase entityDatabase)
        {
            entityDatabase.RegisterEntity(new SMagicCursorEntityDescriptor());
        }
    }
}