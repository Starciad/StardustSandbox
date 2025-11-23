using Microsoft.Xna.Framework;

using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Collections;

namespace StardustSandbox.WorldSystem
{
    public sealed class Slot : IPoolableObject
    {
        internal bool IsEmpty => this.ForegroundLayer.HasState(ElementStates.IsEmpty) && this.BackgroundLayer.HasState(ElementStates.IsEmpty);
        internal Point Position => this.position;

        internal SlotLayer ForegroundLayer => this.foregroundLayer;
        internal SlotLayer BackgroundLayer => this.backgroundLayer;

        private Point position;

        private readonly SlotLayer foregroundLayer = new();
        private readonly SlotLayer backgroundLayer = new();

        internal Slot()
        {

        }

        internal SlotLayer GetLayer(LayerType layer)
        {
            return layer switch
            {
                LayerType.Foreground => this.foregroundLayer,
                LayerType.Background => this.backgroundLayer,
                _ => null,
            };
        }

        internal void SetPosition(Point position)
        {
            this.position = position;
        }

        internal void Instantiate(Point position, LayerType layer, Element value)
        {
            this.position = position;
            GetLayer(layer).Instantiate(value);
        }

        internal void Destroy(LayerType layer)
        {
            GetLayer(layer).Destroy();
        }

        internal void Copy(LayerType layer, SlotLayer valueToCopy)
        {
            GetLayer(layer).Copy(valueToCopy);
        }

        internal void SetTemperatureValue(LayerType layer, double value)
        {
            GetLayer(layer).SetTemperatureValue(value);
        }

        internal void SetColorModifier(LayerType layer, Color value)
        {
            GetLayer(layer).SetColorModifier(value);
        }

        internal void SetStoredElement(LayerType layer, Element value)
        {
            GetLayer(layer).SetStoredElement(value);
        }

        internal bool HasState(LayerType layer, ElementStates value)
        {
            return GetLayer(layer).HasState(value);
        }

        internal void SetState(LayerType layer, ElementStates value)
        {
            GetLayer(layer).SetState(value);
        }

        internal void RemoveState(LayerType layer, ElementStates value)
        {
            GetLayer(layer).RemoveState(value);
        }

        internal void ClearStates(LayerType layer)
        {
            GetLayer(layer).ClearStates();
        }

        internal void ToggleState(LayerType layer, ElementStates value)
        {
            GetLayer(layer).ToggleState(value);
        }

        internal void Reset(LayerType layer)
        {
            GetLayer(layer).Reset();
        }

        public void Reset()
        {
            this.foregroundLayer.Reset();
            this.backgroundLayer.Reset();
        }
    }
}