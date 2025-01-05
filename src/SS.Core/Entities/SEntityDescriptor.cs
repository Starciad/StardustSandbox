using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Entities
{
    public abstract class SEntityDescriptor(string identifier)
    {
        public string Identifier => identifier;

        public abstract SEntity CreateEntity(ISGame gameInstance);
    }
}