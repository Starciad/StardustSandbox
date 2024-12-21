using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.Core.World.Data
{
    internal sealed class SWorldSlot : ISWorldSlot
    {
        public bool IsEmpty => this.ForegroundLayer.IsEmpty && this.BackgroundLayer.IsEmpty;
        public Point Position => this.position;

        public ISWorldSlotLayer ForegroundLayer => this.foregroundLayer;
        public ISWorldSlotLayer BackgroundLayer => this.backgroundLayer;

        private Point position;

        private readonly SWorldSlotLayer foregroundLayer = new();
        private readonly SWorldSlotLayer backgroundLayer = new();

        internal SWorldSlot()
        {

        }

        public ISWorldSlotLayer GetLayer(SWorldLayer worldLayer)
        {
            return worldLayer switch
            {
                SWorldLayer.Foreground => this.foregroundLayer,
                SWorldLayer.Background => this.backgroundLayer,
                _ => null,
            };
        }

        internal void SetPosition(Point position)
        {
            this.position = position;
        }

        internal void Instantiate(Point position, SWorldLayer worldLayer, ISElement value)
        {
            this.position = position;
            ((SWorldSlotLayer)GetLayer(worldLayer)).Instantiate(value);
        }

        internal void Destroy(SWorldLayer worldLayer)
        {
            ((SWorldSlotLayer)GetLayer(worldLayer)).Destroy();
        }

        internal void Copy(SWorldLayer worldLayer, ISWorldSlotLayer valueToCopy)
        {
            ((SWorldSlotLayer)GetLayer(worldLayer)).Copy(valueToCopy);
        }

        internal void SetTemperatureValue(SWorldLayer worldLayer, short value)
        {
            ((SWorldSlotLayer)GetLayer(worldLayer)).SetTemperatureValue(value);
        }

        internal void SetFreeFalling(SWorldLayer worldLayer, bool value)
        {
            ((SWorldSlotLayer)GetLayer(worldLayer)).SetFreeFalling(value);
        }

        internal void SetColorModifier(SWorldLayer worldLayer, Color value)
        {
            ((SWorldSlotLayer)GetLayer(worldLayer)).SetColorModifier(value);
        }

        internal void Reset(SWorldLayer worldLayer)
        {
            ((SWorldSlotLayer)GetLayer(worldLayer)).Reset();
        }

        public void Reset()
        {
            this.foregroundLayer.Reset();
            this.backgroundLayer.Reset();
        }
    }
}