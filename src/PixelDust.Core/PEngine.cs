namespace PixelDust.Core
{
    public static class PEngine
    {
        public static PEngineInstance Instance => _instance;
        public static CancellationToken CancellationToken => _cancellationTokenSource.Token;

        private static PEngineInstance _instance;
        private static readonly CancellationTokenSource _cancellationTokenSource = new();

        public static void SetEngineInstance<T>() where T : PEngineInstance, new()
        {
            _instance = new T();
        }

        public static void Start()
        {
            _instance.Run();
        }

        public static void Stop()
        {
            _cancellationTokenSource.Cancel();
            _instance.Exit();
        }
    }
}
