using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Background;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    public sealed class SBackgroundDatabase(ISGame gameInstance) : SGameObject(gameInstance)
    {
        private readonly Dictionary<string, SBackground> backgrounds = [];

        public void RegisterBackground(string id, Texture2D texture, Action<SBackground> builderAction)
        {
            SBackground background = new(this.SGameInstance, id, texture);

            builderAction.Invoke(background);

            this.backgrounds.Add(id, background);
        }

        public SBackground GetBackgroundById(string id)
        {
            return this.backgrounds[id];
        }
    }
}
