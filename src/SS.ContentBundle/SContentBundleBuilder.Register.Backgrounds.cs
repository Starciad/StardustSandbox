using Microsoft.Xna.Framework;

using StardustSandbox.Game.Background;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Interfaces;

using System;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterBackgrounds(ISGame game, SBackgroundDatabase backgroundDatabase)
        {
            backgroundDatabase.RegisterBackground("ocean_1", game.AssetDatabase.GetTexture("background_1"), new Action<SBackground>((background) =>
            {
                background.AddLayer(new Point(0, 0), new Vector2(2f, 0f), false, true);
            }));
        }
    }
}
