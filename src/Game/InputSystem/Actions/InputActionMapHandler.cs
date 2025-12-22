namespace StardustSandbox.InputSystem.Actions
{
    internal sealed class InputActionMapHandler
    {
        private readonly InputActionMap[] maps;

        internal InputActionMapHandler(InputActionMap[] maps)
        {
            this.maps = maps;
        }

        internal void Update()
        {
            for (int i = 0; i < this.maps.Length; i++)
            {
                if (!this.maps[i].IsActive)
                {
                    continue;
                }

                this.maps[i].Update();
            }
        }

        internal void Activate()
        {
            for (int i = 0; i < this.maps.Length; i++)
            {
                this.maps[i].IsActive = true;
            }
        }

        internal void Disable()
        {
            for (int i = 0; i < this.maps.Length; i++)
            {
                this.maps[i].IsActive = false;
            }
        }
    }
}
