using StardustSandbox.Mathematics;

using System;
using System.Diagnostics;

namespace StardustSandbox.WorldSystem
{
    internal sealed class Temperature
    {
        private sealed class TemperatureRange(TimeSpan startTime, TimeSpan endTime)
        {
            internal TimeSpan StartTime => startTime;
            internal TimeSpan EndTime => endTime;
            internal float Temperature
            {
                get => temperature;
                set => temperature = TemperatureMath.Clamp(value);
            }
            internal bool CanApplyTemperature
            {
                get => canApplyTemperature;
                set => canApplyTemperature = value;
            }

            private float temperature = 30.0f;
            private bool canApplyTemperature = false;
        }

        private readonly TemperatureRange[] temperatureRanges =
        [
            new(new(0, 0, 0), new(3, 0, 0)),
            new(new(3, 0, 0), new(6, 0, 0)),
            new(new(6, 0, 0), new(8, 0, 0)),
            new(new(8, 0, 0), new(12, 0, 0)),
            new(new(12, 0, 0), new(15, 0, 0)),
            new(new(15, 0, 0), new(18, 0, 0)),
            new(new(18, 0, 0), new(20, 0, 0)),
            new(new(20, 0, 0), new(24, 0, 0)),
        ];

        internal bool CanApplyTemperature => GetTemperatureRangeByTime(this.time.CurrentTime).CanApplyTemperature;
        internal float CurrentTemperature => this.currentTemperature;

        private float currentTemperature;

        private readonly Time time;

        internal Temperature(Time time)
        {
            this.time = time;
        }

        internal void Update()
        {
            TemperatureRange currentRange = GetTemperatureRangeByTime(this.time.CurrentTime);

            if (currentRange.CanApplyTemperature)
            {
                this.currentTemperature = float.Lerp(this.currentTemperature, currentRange.Temperature, 0.01f);
            }
        }

        internal void SetTemperatureValue(TimeSpan time, float temperature, bool canApplyTemperature)
        {
            TemperatureRange range = GetTemperatureRangeByTime(time);
            
            range.Temperature = temperature;
            range.CanApplyTemperature = canApplyTemperature;
        }

        private TemperatureRange GetTemperatureRangeByTime(TimeSpan time)
        {
            return Array.Find(this.temperatureRanges, x => time >= x.StartTime && time < x.EndTime);
        }
    }
}
