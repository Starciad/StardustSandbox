using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.IO;

namespace PixelDust.Core.Engine.Assets
{
    /// <summary>
    /// Static wrapper of the <see cref="ContentManager"/> class for loading project contents, resources and assets.
    /// </summary>
    public static class PContent
    {
        /// <summary>
        /// Sprite Directory Content Manager.
        /// </summary>
        public static ContentManager Sprites => contentManagers["Sprites"];

        /// <summary>
        /// Effects Directory Content Manager.
        /// </summary>
        public static ContentManager Effects => contentManagers["Effects"];

        /// <summary>
        /// Fonts Directory Content Manager.
        /// </summary>
        public static ContentManager Fonts => contentManagers["Fonts"];

        /// <summary>
        /// Musics Directory Content Manager.
        /// </summary>
        public static ContentManager Musics => contentManagers["Musics"];

        /// <summary>
        /// Sounds Directory Content Manager.
        /// </summary>
        public static ContentManager Sounds => contentManagers["Sounds"];

        private static readonly Dictionary<string, ContentManager> contentManagers = [];
        private static readonly string[] contentDirectories = new string[] { "Sprites", "Effects", "Fonts", "Musics", "Sounds" };

        /// <summary>
        /// Builds and prepares all the project's internal specialized content managers.
        /// </summary>
        /// <param name="serviceProvider">The service provider to be used.</param>
        /// <param name="rootDirectory">The root path of the content directory.</param>
        internal static void Build(IServiceProvider serviceProvider, string rootDirectory)
        {
            foreach (string directory in contentDirectories)
            {
                contentManagers.Add(directory, new(serviceProvider, Path.Combine(rootDirectory, directory)));
            }
        }

        /// <summary>
        /// Unloads all instanced content managers from memory.
        /// </summary>
        internal static void Unload()
        {
            foreach (ContentManager contentManager in contentManagers.Values)
            {
                contentManager.Dispose();
            }
        }
    }
}
