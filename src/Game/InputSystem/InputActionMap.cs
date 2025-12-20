namespace StardustSandbox.InputSystem
{
    internal sealed class InputActionMap
    {
        internal bool IsActive { get; set; }

        private readonly InputAction[] actions;

        internal InputActionMap(InputAction[] actions)
        {
            this.IsActive = true;
            this.actions = actions;
        }

        internal void Update()
        {
            for (int i = 0; i < this.actions.Length; i++)
            {
                this.actions[i].Update();
            }
        }
    }
}
