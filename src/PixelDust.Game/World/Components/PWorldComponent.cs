using PixelDust.Game.Objects;

namespace PixelDust.Game.World.Components
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
