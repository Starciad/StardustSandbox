namespace StardustSandbox.InputSystem
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

        internal void ActivateAll()
        {
            for (int i = 0; i < this.maps.Length; i++)
            {
                this.maps[i].IsActive = true;
            }
        }

        internal void DisableAll()
        {
            for (int i = 0; i < this.maps.Length; i++)
            {
                this.maps[i].IsActive = false;
            }
        }
    }
}
