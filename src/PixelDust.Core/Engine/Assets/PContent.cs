using Microsoft.Xna.Framework.Content;

namespace PixelDust.Core.Engine
{
    /// <summary>
    /// Static wrapper of the <see cref="ContentManager"/> class for loading project contents, resources and assets.
    /// </summary>
    public static class PContent
    {
        private static ContentManager _contentManager;

        internal static void Build(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        /// <summary>
        /// Loads a desired file based on a generic type and the path where it is located.
        /// </summary>
        /// <typeparam name="T">Type of file to be uploaded.</typeparam>
        /// <param name="assetName">Relative path where the file is in the project.</param>
        /// <returns>Asset requested.</returns>
        public static T Load<T>(string assetName)
        {
            return _contentManager.Load<T>(assetName);
        }
    }
}
