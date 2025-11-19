using Microsoft.Xna.Framework;

using StardustSandbox.Elements;
using StardustSandbox.Enums.World;
using StardustSandbox.Interfaces.Collections;

namespace StardustSandbox.WorldSystem
{
    public sealed class Slot : IPoolableObject
    {
        internal bool IsEmpty => this.ForegroundLayer.IsEmpty && this.BackgroundLayer.IsEmpty;
        internal Point Position => this.position;

        internal SlotLayer ForegroundLayer => this.foregroundLayer;
        internal SlotLayer BackgroundLayer => this.backgroundLayer;

        private Point position;

        private readonly SlotLayer foregroundLayer = new();
        private readonly SlotLayer backgroundLayer = new();

        internal Slot()
        {

        }

        internal SlotLayer GetLayer(LayerType worldLayer)
        {
            return worldLayer switch
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

        internal void Instantiate(Point position, LayerType worldLayer, Element value)
        {
            this.position = position;
            GetLayer(worldLayer).Instantiate(value);
        }

        internal void Destroy(LayerType worldLayer)
        {
            GetLayer(worldLayer).Destroy();
        }

        internal void Copy(LayerType worldLayer, SlotLayer valueToCopy)
        {
            GetLayer(worldLayer).Copy(valueToCopy);
        }

        internal void SetTemperatureValue(LayerType worldLayer, short value)
        {
            GetLayer(worldLayer).SetTemperatureValue(value);
        }

        internal void SetFreeFalling(LayerType worldLayer, bool value)
        {
            GetLayer(worldLayer).SetFreeFalling(value);
        }

        internal void SetColorModifier(LayerType worldLayer, Color value)
        {
            GetLayer(worldLayer).SetColorModifier(value);
        }

        internal void SetStoredElement(LayerType worldLayer, Element value)
        {
            GetLayer(worldLayer).SetStoredElement(value);
        }

        internal void Reset(LayerType worldLayer)
        {
            GetLayer(worldLayer).Reset();
        }

        public void Reset()
        {
            this.foregroundLayer.Reset();
            this.backgroundLayer.Reset();
        }
    }
}