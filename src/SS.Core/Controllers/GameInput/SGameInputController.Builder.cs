using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.InputSystem;
using StardustSandbox.Core.InputSystem;

namespace StardustSandbox.Core.Controllers.GameInput
{
    public sealed partial class SGameInputController
    {
        private void BuildKeyboardInputs()
        {
            SInputActionMap cameraKeyboardActionMap = this.actionHandler.AddActionMap("Camera_Keyboard", true);
            SInputActionMap simulationKeyboardActionMap = this.actionHandler.AddActionMap("Simulation_Keyboard", true);
            SInputActionMap worldKeyboardActionMap = this.actionHandler.AddActionMap("World_Keyboard", true);

            cameraKeyboardActionMap.AddAction("Move_Camera_Up", new SInputAction(this.SGameInstance, this.inputManager, Keys.W, Keys.Up)).OnPerformed += _ => this.cameraHandler.MoveCamera(new Vector2(0, this.player.MovementSpeed));
            cameraKeyboardActionMap.AddAction("Move_Camera_Right", new SInputAction(this.SGameInstance, this.inputManager, Keys.D, Keys.Right)).OnPerformed += _ => this.cameraHandler.MoveCamera(new Vector2(this.player.MovementSpeed, 0));
            cameraKeyboardActionMap.AddAction("Move_Camera_Down", new SInputAction(this.SGameInstance, this.inputManager, Keys.S, Keys.Down)).OnPerformed += _ => this.cameraHandler.MoveCamera(new Vector2(0, -this.player.MovementSpeed));
            cameraKeyboardActionMap.AddAction("Move_Camera_Left", new SInputAction(this.SGameInstance, this.inputManager, Keys.A, Keys.Left)).OnPerformed += _ => this.cameraHandler.MoveCamera(new Vector2(-this.player.MovementSpeed, 0));

            simulationKeyboardActionMap.AddAction("Toggle_Pause", new(this.SGameInstance, this.inputManager, Keys.Space)).OnStarted += _ => this.simulationHandler.TogglePause();

            worldKeyboardActionMap.AddAction("Clear_World", new(this.SGameInstance, this.inputManager, Keys.R)).OnStarted += _ => this.worldHandler.Clear();
        }

        private void BuildMouseInputs()
        {
            SInputActionMap worldMouseActionMap = this.actionHandler.AddActionMap("World_Mouse", true);
            worldMouseActionMap.AddAction("Place_Item", new(this.SGameInstance, this.inputManager, SMouseButton.Left)).OnPerformed += _ => this.worldHandler.Modify(SWorldModificationType.Adding);
            worldMouseActionMap.AddAction("Erase_Item", new(this.SGameInstance, this.inputManager, SMouseButton.Right)).OnPerformed += _ => this.worldHandler.Modify(SWorldModificationType.Removing);
        }
    }
}
