using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Background;
using StardustSandbox.Game.Interfaces;
using StardustSandbox.Game.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.Databases
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
