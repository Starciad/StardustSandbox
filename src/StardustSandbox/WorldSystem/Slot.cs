using Microsoft.Xna.Framework;

using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Collections;

namespace StardustSandbox.WorldSystem
{
    public sealed class Slot : IPoolableObject
    {
        internal bool IsEmpty => this.IsBackgroundEmpty && this.IsForegroundEmpty;

        internal bool IsForegroundEmpty => this.Foreground.HasState(ElementStates.IsEmpty);
        internal bool IsBackgroundEmpty => this.Background.HasState(ElementStates.IsEmpty);

        internal Point Position => this.position;

        internal SlotLayer Foreground => this.foreground;
        internal SlotLayer Background => this.background;

        private Point position;

        private readonly SlotLayer foreground = new();
        private readonly SlotLayer background = new();

        internal SlotLayer GetLayer(Layer layer)
        {
            return layer switch
            {
                Layer.Foreground => this.foreground,
                Layer.Background => this.background,
                _ => null,
            };
        }

        internal void SetPosition(Point position)
        {
            this.position = position;
        }

        internal void Instantiate(Point position, Layer layer, Element value)
        {
            this.position = position;
            GetLayer(layer).Instantiate(value);
        }

        internal void Destroy(Layer layer)
        {
            GetLayer(layer).Destroy();
        }

        internal void Copy(Layer layer, SlotLayer valueToCopy)
        {
            GetLayer(layer).Copy(valueToCopy);
        }

        internal void SetTemperatureValue(Layer layer, float value)
        {
            GetLayer(layer).SetTemperatureValue(value);
        }

        internal void SetColorModifier(Layer layer, Color value)
        {
            GetLayer(layer).SetColorModifier(value);
        }

        internal void SetStoredElement(Layer layer, Element value)
        {
            GetLayer(layer).SetStoredElement(value);
        }

        internal bool HasState(Layer layer, ElementStates value)
        {
            return GetLayer(layer).HasState(value);
        }

        internal void SetState(Layer layer, ElementStates value)
        {
            GetLayer(layer).SetState(value);
        }

        internal void RemoveState(Layer layer, ElementStates value)
        {
            GetLayer(layer).RemoveState(value);
        }

        internal void ClearStates(Layer layer)
        {
            GetLayer(layer).ClearStates();
        }

        internal void ToggleState(Layer layer, ElementStates value)
        {
            GetLayer(layer).ToggleState(value);
        }

        internal void Reset(Layer layer)
        {
            GetLayer(layer).Reset();
        }

        public void Reset()
        {
            this.foreground.Reset();
            this.background.Reset();
        }
    }
}