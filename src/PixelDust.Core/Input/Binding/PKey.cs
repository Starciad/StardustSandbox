using Microsoft.Xna.Framework.Input;

using PixelDust.Core.Engine;

using System;

using static PixelDust.Core.Input.PKey;
using static System.Windows.Forms.AxHost;

namespace PixelDust.Core.Input
{
    public enum PKeyCallbackState
    {
        Started,
        Performed,
        Canceled
    }

    public sealed class PKey : PInputBinding
    {
        private readonly Keys[] keys;
        private Action<Callback> action;

        private bool started = false;
        private bool performed = false;
        private bool canceled = true;

        private Callback previousCallback;

        public PKey(params Keys[] keys)
        {
            this.keys = keys;
        }

        internal override void Update()
        {
            Callback callback = new();

            foreach (Keys key in keys)
            {
                // Starting
                if (!PInput.PreviousKeyboard.IsKeyDown(key) &&
                     PInput.Keyboard.IsKeyDown(key) &&
                     !started && !performed && canceled)
                {
                    callback.State = PKeyCallbackState.Started;

                    action?.Invoke(callback);
                    previousCallback = callback;

                    started = true;
                    canceled = false;
                }

                // Performed
                if (PInput.PreviousKeyboard.IsKeyDown(key) &&
                    PInput.Keyboard.IsKeyDown(key) &&
                    started && !canceled)
                {
                    callback.State = PKeyCallbackState.Performed;

                    action?.Invoke(callback);
                    previousCallback = callback;

                    performed = true;
                }

                // Canceled
                if (PInput.PreviousKeyboard.IsKeyDown(key) &&
                   !PInput.Keyboard.IsKeyDown(key) &&
                   started && performed && !canceled)
                {
                    callback.State = PKeyCallbackState.Canceled;

                    action?.Invoke(callback);
                    previousCallback = callback;

                    started = false;
                    performed = false;
                    canceled = true;
                }
            }
        }

        public void SetAction(Action<Callback> action)
        {
            this.action = action;
        }

        public struct Callback
        {
            public PKeyCallbackState State { get; internal set; }
        }
    }
}
