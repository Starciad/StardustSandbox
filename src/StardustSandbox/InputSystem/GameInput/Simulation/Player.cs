using StardustSandbox.Catalog;

namespace StardustSandbox.InputSystem.GameInput.Simulation
{
    internal sealed class Player
    {
        internal Item SelectedItem => this.selectedItem;
        internal float MovementSpeed => this.movementSpeed;
        internal bool CanModifyEnvironment { get; set; }

        private readonly float movementSpeed = 10;
        private Item selectedItem;

        internal void SelectItem(Item item)
        {
            this.selectedItem = item;
        }
    }
}
