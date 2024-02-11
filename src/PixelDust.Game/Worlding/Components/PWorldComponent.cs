using PixelDust.Core.Worlding;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Worlding.Components
{
    public abstract class PWorldComponent : PGameObject
    {
        protected PWorld World { get; private set; }

        public void SetWorldInstance(PWorld instance)
        {
            this.World = instance;
        }
    }
}
