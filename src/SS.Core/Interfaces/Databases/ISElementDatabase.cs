using StardustSandbox.Core.Interfaces.Elements;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISElementDatabase
    {
        void RegisterElement(ISElement element);

        ISElement GetElementByIdentifier(string identifier);
    }
}
