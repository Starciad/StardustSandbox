using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Background;

using System;

namespace StardustSandbox.Core.Interfaces.Databases
{
    public interface ISBackgroundDatabase
    {
        void RegisterBackground(string identifier, Texture2D texture, Action<SBackground> builderAction);

        SBackground GetBackgroundById(string identifier);
    }
}
