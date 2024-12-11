using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Interfaces.Elements;

using System;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISElementDatabase
    {
        void RegisterElement(SElement element);

        T GetElementById<T>(uint identifier) where T : ISElement;
        T GetElementByType<T>() where T : ISElement;
        uint GetIdOfElementType<T>() where T : ISElement;

        ISElement GetElementById(uint identifier);
        ISElement GetElementByType(Type type);
        uint GetIdOfElementType(Type type);
    }
}
