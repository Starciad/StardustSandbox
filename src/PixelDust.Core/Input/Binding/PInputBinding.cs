using System.Data;

namespace PixelDust.Core.Input
{
    public abstract class PInputBinding
    {
        public PInputActionMap ActionMap => _actionMap;

        private PInputActionMap _actionMap;

        internal void SetActionMap(PInputActionMap map)
        {
            _actionMap = map;
        }

        internal virtual void Update() { return; }
    }
}
