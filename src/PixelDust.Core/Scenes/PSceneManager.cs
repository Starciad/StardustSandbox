using System;

namespace PixelDust.Core.Scenes
{
    public static class PSceneManager
    {
        private static PScene _sceneInstance;

        public static void Load<T>() where T : PScene
        {
            _sceneInstance?.Unload();
            _sceneInstance = Activator.CreateInstance<T>();
            _sceneInstance.Load();
        }

        public static T GetCurrentScene<T>() where T : PScene
        {
            return (T)_sceneInstance;
        }

        internal static void Update()
        {
            _sceneInstance?.Update();
        }

        internal static void Draw()
        {
            _sceneInstance?.Draw();
        }
    }
}