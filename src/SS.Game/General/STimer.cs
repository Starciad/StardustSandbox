using System;

namespace StardustSandbox.Game.General
{
    internal sealed class STimer
    {
        internal bool IsFinished => this.enable && this.current == 0;
        internal bool IsEnabled => this.enable;
        internal bool IsActive => this.active;
        internal float TargetDelay => this.target;
        internal float CurrentDelay => this.current;

        private bool enable = true;
        private bool active = false;

        private float target = 0;
        private float current = 0;

        internal STimer()
        {
            this.target = 0f;
            this.current = 0f;
        }

        internal STimer(float seconds)
        {
            this.target = seconds;
            this.current = seconds;
        }

        internal void Start()
        {
            this.active = true;
        }

        internal void Restart()
        {
            this.current = this.target;
            Start();
        }

        internal void Stop()
        {
            this.active = false;
        }

        internal void Update()
        {
            if (!this.active || !this.enable)
            {
                return;
            }

            this.current = Math.Max(0, this.current - 0.1f);

            if (this.current == 0)
            {
                this.active = false;
            }
        }

        internal void Enable()
        {
            this.enable = true;
        }

        internal void Disable()
        {
            this.enable = false;
        }

        internal void SetDelay(float seconds)
        {
            this.target = seconds;
            this.current = Math.Min(this.current, this.target);
        }

        public override string ToString()
        {
            return $"{{ {this.current}/{this.target} }}";
        }
    }
}
