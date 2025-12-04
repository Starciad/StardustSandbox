using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Camera;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Inputs.Game.Handlers;
using StardustSandbox.Inputs.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.World;

namespace StardustSandbox.Inputs.Game
{
    internal sealed class InputController
    {
        internal Pen Pen => this.pen;
        internal Player Player => this.player;

        private SimulationHandler simulationHandler;
        private WorldHandler worldHandler;
        private InputActionMapHandler actionHandler;
        private InputManager inputManager;

        private readonly Pen pen;
        private readonly Player player;

        internal InputController()
        {
            this.pen = new();
            this.player = new();
        }

        private void BuildKeyboardInputs()
        {
            InputActionMap cameraKeyboardActionMap = this.actionHandler.AddActionMap("Camera_Keyboard", true);
            InputActionMap simulationKeyboardActionMap = this.actionHandler.AddActionMap("Simulation_Keyboard", true);
            InputActionMap worldKeyboardActionMap = this.actionHandler.AddActionMap("World_Keyboard", true);

            cameraKeyboardActionMap.AddAction("Move_Camera_Up", new InputAction(this.inputManager, Keys.W, Keys.Up)).OnPerformed += _ => SSCamera.Move(new Vector2(0, this.player.MovementSpeed));
            cameraKeyboardActionMap.AddAction("Move_Camera_Right", new InputAction(this.inputManager, Keys.D, Keys.Right)).OnPerformed += _ => SSCamera.Move(new Vector2(this.player.MovementSpeed, 0));
            cameraKeyboardActionMap.AddAction("Move_Camera_Down", new InputAction(this.inputManager, Keys.S, Keys.Down)).OnPerformed += _ => SSCamera.Move(new Vector2(0, -this.player.MovementSpeed));
            cameraKeyboardActionMap.AddAction("Move_Camera_Left", new InputAction(this.inputManager, Keys.A, Keys.Left)).OnPerformed += _ => SSCamera.Move(new Vector2(-this.player.MovementSpeed, 0));

            simulationKeyboardActionMap.AddAction("Toggle_Pause", new(this.inputManager, Keys.Space)).OnStarted += _ => this.simulationHandler.TogglePause();

            worldKeyboardActionMap.AddAction("Clear_World", new(this.inputManager, Keys.R)).OnStarted += _ => this.worldHandler.Clear();
        }

        private void BuildMouseInputs()
        {
            InputActionMap worldMouseActionMap = this.actionHandler.AddActionMap("World_Mouse", true);
            worldMouseActionMap.AddAction("Place_Item", new(this.inputManager, MouseButton.Left)).OnPerformed += _ => this.worldHandler.Modify(WorldModificationType.Adding);
            worldMouseActionMap.AddAction("Erase_Item", new(this.inputManager, MouseButton.Right)).OnPerformed += _ => this.worldHandler.Modify(WorldModificationType.Removing);
        }

        internal void Initialize(GameManager gameManager, InputManager inputManager, GameWorld world)
        {
            this.simulationHandler = new(gameManager);
            this.worldHandler = new(inputManager, this.player, this.pen, world);

            this.inputManager = inputManager;
            this.actionHandler = new();

            BuildKeyboardInputs();
            BuildMouseInputs();
        }

        internal void Update()
        {
            UpdatePlaceAreaSize();

            this.actionHandler.Update();
        }

        private void UpdatePlaceAreaSize()
        {
            if (this.inputManager.GetDeltaScrollWheel() > 0)
            {
                this.pen.Size--;
            }
            else if (this.inputManager.GetDeltaScrollWheel() < 0)
            {
                this.pen.Size++;
            }
        }

        internal void Activate()
        {
            this.actionHandler.ActivateAll();
        }

        internal void Disable()
        {
            this.actionHandler.DisableAll();
        }
    }
}
