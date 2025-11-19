using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.AmbientSystem.BackgroundSystem;
using StardustSandbox.Managers;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Databases
{
    internal static class BackgroundDatabase
    {
        private static readonly Dictionary<string, Background> backgrounds = [];

        internal static void Load(CameraManager cameraManager)
        {
            RegisterBackground("main_menu", AssetDatabase.GetTexture("texture_background_1"), new Action<Background>((background) =>
            {
                // Settings
                background.IsAffectedByLighting = true;

                // Layers
                background.AddLayer(cameraManager, new Point(0, 0), new(2f, 0f), new(-16f, 0f), false, true);
            }));

            RegisterBackground("ocean_1", AssetDatabase.GetTexture("texture_background_1"), new Action<Background>((background) =>
            {
                // Settings
                background.IsAffectedByLighting = true;

                // Layers
                background.AddLayer(cameraManager, new Point(0, 0), new(2f, 0f), Vector2.Zero, false, true);
            }));

            RegisterBackground("credits", AssetDatabase.GetTexture("texture_background_3"), new Action<Background>((background) =>
            {
                // Layers
                background.AddLayer(cameraManager, new Point(0, 0), new(0f, 0f), new(-32f), false, false);
            }));
        }

        private static void RegisterBackground(string identifier, Texture2D texture, Action<Background> builderAction)
        {
            Background background = new(identifier, texture);

            builderAction.Invoke(background);

            backgrounds.Add(identifier, background);
        }

        internal static Background GetBackgroundById(string identifier)
        {
            return backgrounds[identifier];
        }
    }
}
