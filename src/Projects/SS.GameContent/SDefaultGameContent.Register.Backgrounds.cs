using Microsoft.Xna.Framework;

using StardustSandbox.Core.Ambient.Background;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

using System;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SDefaultGameContent
    {
        protected override void OnRegisterBackgrounds(ISGame game, ISBackgroundDatabase backgroundDatabase)
        {
            backgroundDatabase.RegisterBackground("main_menu", game.AssetDatabase.GetTexture("texture_background_1"), new Action<SBackground>((background) =>
            {
                // Settings
                background.IsAffectedByLighting = true;

                // Layers
                background.AddLayer(new Point(0, 0), new(2f, 0f), new(-16f, 0f), false, true);
            }));

            backgroundDatabase.RegisterBackground("ocean_1", game.AssetDatabase.GetTexture("texture_background_1"), new Action<SBackground>((background) =>
            {
                // Settings
                background.IsAffectedByLighting = true;

                // Layers
                background.AddLayer(new Point(0, 0), new(2f, 0f), Vector2.Zero, false, true);
            }));

            backgroundDatabase.RegisterBackground("credits", game.AssetDatabase.GetTexture("texture_background_3"), new Action<SBackground>((background) =>
            {
                // Layers
                background.AddLayer(new Point(0, 0), new(0f, 0f), new(-32f), false, false);
            }));
        }
    }
}
