using StardustSandbox.Game.Objects;

namespace StardustSandbox.Game.World.Components
{
    public abstract class SWorldComponent : SGameObject
    {
        protected SWorld World { get; private set; }

        public void SetWorldInstance(SWorld instance)
        {
            this.World = instance;
        }
    }
}
