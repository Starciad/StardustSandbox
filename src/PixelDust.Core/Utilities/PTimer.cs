using System;

namespace PixelDust.Core.Utilities
{
    /// <summary>
    /// Represents a timer that can be used for various timing operations.
    /// </summary>
    public sealed class PTimer
    {
        /// <summary>
        /// Gets a value indicating whether the timer has finished counting down.
        /// </summary>
        public bool IsFinished => this.enable && this.current == 0;

        /// <summary>
        /// Gets a value indicating whether the timer is enabled.
        /// </summary>
        public bool IsEnabled => this.enable;

        /// <summary>
        /// Gets a value indicating whether the timer is actively counting down.
        /// </summary>
        public bool IsActive => this.active;

        /// <summary>
        /// Gets the target delay value for the timer.
        /// </summary>
        public float TargetDelay => this.target;

        /// <summary>
        /// Gets the current delay value for the timer.
        /// </summary>
        public float CurrentDelay => this.current;

        private bool enable = true;
        private bool active = false;

        private float target = 0;
        private float current = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PTimer"/> class with a target delay of 0.
        /// </summary>
        public PTimer()
        {
            this.target = 0f;
            this.current = 0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PTimer"/> class with a specified target delay.
        /// </summary>
        /// <param name="seconds">The target delay value in seconds.</param>
        public PTimer(float seconds)
        {
            this.target = seconds;
            this.current = seconds;
        }

        /// <summary>
        /// Starts the timer, allowing it to count down.
        /// </summary>
        public void Start()
        {
            this.active = true;
        }

        /// <summary>
        /// Restarts the timer, setting the current delay to the target delay and starting it.
        /// </summary>
        public void Restart()
        {
            this.current = this.target;
            Start();
        }

        /// <summary>
        /// Stops the timer, preventing it from counting down further.
        /// </summary>
        public void Stop()
        {
            this.active = false;
        }

        /// <summary>
        /// Updates the timer's current delay if it is active and enabled.
        /// </summary>
        public void Update()
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

        /// <summary>
        /// Enables the timer, allowing it to count down if it is active.
        /// </summary>
        public void Enable()
        {
            this.enable = true;
        }

        /// <summary>
        /// Disables the timer, preventing it from counting down even if it is active.
        /// </summary>
        public void Disable()
        {
            this.enable = false;
        }

        /// <summary>
        /// Sets the target delay value for the timer and adjusts the current delay accordingly.
        /// </summary>
        /// <param name="value">The new target delay value in seconds.</param>
        public void SetDelay(float value)
        {
            this.target = value;
            this.current = Math.Min(this.current, this.target);
        }

        /// <summary>
        /// Returns a string representation of the timer's current and target delay values.
        /// </summary>
        public override string ToString()
        {
            return $"{{ {this.current}/{this.target} }}";
        }
    }
}
