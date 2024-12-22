using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.World.Data
{
    public sealed class SWorldSlot : ISPoolableObject
    {
        public bool IsEmpty => this.ForegroundLayer.IsEmpty && this.BackgroundLayer.IsEmpty;
        public Point Position => this.position;

        public SWorldSlotLayer ForegroundLayer => this.foregroundLayer;
        public SWorldSlotLayer BackgroundLayer => this.backgroundLayer;

        private Point position;

        private readonly SWorldSlotLayer foregroundLayer = new();
        private readonly SWorldSlotLayer backgroundLayer = new();

        internal SWorldSlot()
        {

        }

        public SWorldSlotLayer GetLayer(SWorldLayer worldLayer)
        {
            return worldLayer switch
            {
                SWorldLayer.Foreground => this.foregroundLayer,
                SWorldLayer.Background => this.backgroundLayer,
                _ => null,
            };
        }

        public void SetPosition(Point position)
        {
            this.position = position;
        }

        public void Instantiate(Point position, SWorldLayer worldLayer, ISElement value)
        {
            this.position = position;
            GetLayer(worldLayer).Instantiate(value);
        }

        public void Destroy(SWorldLayer worldLayer)
        {
            GetLayer(worldLayer).Destroy();
        }

        public void Copy(SWorldLayer worldLayer, SWorldSlotLayer valueToCopy)
        {
            GetLayer(worldLayer).Copy(valueToCopy);
        }

        public void SetTemperatureValue(SWorldLayer worldLayer, short value)
        {
            GetLayer(worldLayer).SetTemperatureValue(value);
        }

        public void SetFreeFalling(SWorldLayer worldLayer, bool value)
        {
            GetLayer(worldLayer).SetFreeFalling(value);
        }

        public void SetColorModifier(SWorldLayer worldLayer, Color value)
        {
            GetLayer(worldLayer).SetColorModifier(value);
        }

        public void Reset(SWorldLayer worldLayer)
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