using Microsoft.Xna.Framework;

using SharpDX.Direct3D9;

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

        internal void SetPosition(Point position)
        {
            this.position = position;
        }

        internal SWorldSlotLayer GetLayer(SWorldLayer worldLayer)
        {
            return worldLayer switch
            {
                SWorldLayer.None => null,
                SWorldLayer.Foreground => this.foregroundLayer,
                SWorldLayer.Background => this.backgroundLayer,
                _ => null,
            };
        }

        internal void Instantiate(SWorldLayer worldLayer, Point position, ISElement value)
        {
            this.position = position;
            GetLayer(worldLayer).Instantiate(value);
        }

        internal void Destroy(SWorldLayer worldLayer)
        {
            GetLayer(worldLayer).Destroy();
        }

        internal void Copy(SWorldLayer worldLayer, ISWorldSlotLayer valueToCopy)
        {
            GetLayer(worldLayer).Copy(valueToCopy);
        }

        internal void SetTemperatureValue(SWorldLayer worldLayer, short value)
        {
            GetLayer(worldLayer).SetTemperatureValue(value);
        }

        internal void SetFreeFalling(SWorldLayer worldLayer, bool value)
        {
            GetLayer(worldLayer).SetFreeFalling(value);
        }

        internal void SetColorModifier(SWorldLayer worldLayer, Color value)
        {
            GetLayer(worldLayer).SetColorModifier(value);
        }

        internal void Reset(SWorldLayer worldLayer)
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