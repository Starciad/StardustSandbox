using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Ambient.Background;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SBackgroundDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISBackgroundDatabase
    {
        private readonly Dictionary<string, SBackground> backgrounds = [];

        public void RegisterBackground(string identifier, Texture2D texture, Action<SBackground> builderAction)
        {
            SBackground background = new(this.SGameInstance, identifier, texture);

            builderAction.Invoke(background);

            this.backgrounds.Add(identifier, background);
        }

        public SBackground GetBackgroundById(string identifier)
        {
            return this.backgrounds[identifier];
        }
    }
}
