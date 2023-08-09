using PixelDust.Core.Elements;

namespace PixelDust.Core.Worlding
{
    public sealed class PWorldSlot
    {
        public PElement Element => _element;
        public PWorldSlotInfos Infos => _infos;

        // Header
        private PElement _element;
        private PWorldSlotInfos _infos;

        internal PWorldSlot(uint id)
        {
            Instantiate(id);
        }

        internal PWorldSlot(PElement value)
        {
            Instantiate(value);
        }

        internal void Instantiate(uint id)
        {
            Instantiate(PElementManager.GetElementById<PElement>(id));
        }
        internal void Instantiate(PElement value)
        {
            _element = value;

            _infos ??= new();
            _infos.Instantiate(_element);
        }
    }
}