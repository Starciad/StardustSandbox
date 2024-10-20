using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Background;
using StardustSandbox.Game.Objects;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Game.Databases
{
    public sealed class SBackgroundDatabase(SGame gameInstance) : SGameObject(gameInstance)
    {
        private readonly Dictionary<string, SBackground> backgrounds = [];

        public override void Initialize()
        {
            AddBackground("ocean_1", this.SGameInstance.AssetDatabase.GetTexture("background_1"), new Action<SBackground>((background) =>
            {
                background.AddLayer(new Point(0, 0), new Vector2(2f, 0f), false, true);
            }));
        }

        private void AddBackground(string id, Texture2D texture, Action<SBackground> builderAction)
        {
            SBackground background = new(this.SGameInstance, texture);

            builderAction.Invoke(background);

            this.backgrounds.Add(id, background);
        }

        public SBackground GetBackgroundById(string id)
        {
            return this.backgrounds[id];
        }
    }
}
