namespace PixelDust.Core.Worlding
{
    public class PWorldInfos
    {
        public float Temperature { get; private set; }
        public int TotalElements { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public PWorldInfos()
        {
            Temperature = 20f;
            TotalElements = 0;

            Width = 0;
            Height = 0;
        }

        internal void SetTemperature(float value)
        {
            Temperature = value;
        }

        internal void SetTotalElements(int value)
        {
            TotalElements = value;
        }

        internal void SetWidth(uint value)
        {
            Width = (int)value;
        }

        internal void SetHeight(uint value)
        {
            Height = (int)value;
        }
    }
}
