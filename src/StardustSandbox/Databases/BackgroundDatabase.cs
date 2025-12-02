using Microsoft.Xna.Framework;

using StardustSandbox.BackgroundSystem;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.BackgroundSystem;

using System;

namespace StardustSandbox.Databases
{
    internal static class BackgroundDatabase
    {
        private static Background[] backgrounds;

        private static bool isLoaded = false;

        internal static void Load()
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(BackgroundDatabase)} has already been loaded.");
            }

            backgrounds = [
                // [0] Main Menu
                new([
                    new(new(2.0f, 0.0f), new(-16.0f, 0.0f), false, true),
                ], true, AssetDatabase.GetTexture(TextureIndex.BackgroundOcean)),

                // [1] Ocean
                new([
                    new(new(2.0f, 0.0f), Vector2.Zero, false, true),
                ], true, AssetDatabase.GetTexture(TextureIndex.BackgroundOcean)),

                // [2] Credits
                new([
                    new(new(0.0f, 0.0f), new(-32.0f), false, false),
                ], false, AssetDatabase.GetTexture(TextureIndex.PatternDiamonds)),
            ];

            isLoaded = true;
        }

        internal static Background GetBackground(BackgroundIndex index)
        {
            return backgrounds[(int)index];
        }
    }
}
