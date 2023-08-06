using Microsoft.Xna.Framework.Content;

namespace PixelDust.Core.Engine
{
    public static class PContent
    {
        private static ContentManager _contentManager;

        internal static void Build(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public static T Load<T>(string assetName)
        {
            return _contentManager.Load<T>(assetName);
        }
    }
}
