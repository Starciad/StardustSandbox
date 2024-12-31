using Microsoft.Xna.Framework;

using StardustSandbox.Core.Background;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

using System;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        protected override void OnRegisterBackgrounds(ISGame game, ISBackgroundDatabase backgroundDatabase)
        {
            backgroundDatabase.RegisterBackground("main_menu", game.AssetDatabase.GetTexture("background_1"), new Action<SBackground>((background) =>
            {
                background.AddLayer(new Point(0, 0), new(2f, 0f), new(-16f, 0f), false, true);
            }));

            backgroundDatabase.RegisterBackground("ocean_1", game.AssetDatabase.GetTexture("background_1"), new Action<SBackground>((background) =>
            {
                background.AddLayer(new Point(0, 0), new(2f, 0f), Vector2.Zero, false, true);
            }));

            backgroundDatabase.RegisterBackground("credits", game.AssetDatabase.GetTexture("background_3"), new Action<SBackground>((background) =>
            {
                background.AddLayer(new Point(0, 0), new(0f, 0f), new(-32f), false, false);
            }));
        }
    }
}
