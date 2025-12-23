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
                if (!this.maps[i].IsActivated)
                {
                    continue;
                }

                this.maps[i].Update();
            }
        }

        internal void Enable()
        {
            for (int i = 0; i < this.maps.Length; i++)
            {
                this.maps[i].IsActivated = true;
            }
        }

        internal void Disable()
        {
            for (int i = 0; i < this.maps.Length; i++)
            {
                this.maps[i].IsActivated = false;
            }
        }
    }
}
