using StardustSandbox.Core.Content;

using System;

namespace StardustSandbox.Core
{
    public sealed class SStardustSandboxEngine : IDisposable
    {
        private readonly SGame game = new();
        private bool disposedValue;

        public void AddContent(SGameContent content)
        {
            this.game.AddContent(content);
        }

        public void Start()
        {
            this.game.Run();
        }

        public void Stop()
        {
            this.game.Exit();
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.game.Dispose();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
