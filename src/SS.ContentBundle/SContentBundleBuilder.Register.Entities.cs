using StardustSandbox.ContentBundle.Entities.Specials;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterEntities(ISGame game, ISEntityDatabase entityDatabase)
        {
            entityDatabase.RegisterEntityDescriptor(new SMagicCursorEntityDescriptor());
        }
    }
}