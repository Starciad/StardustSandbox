using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Handlers;
using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World;

namespace StardustSandbox.Core.Controllers.GameInput
{
    public sealed partial class SGameInputController : SGameObject
    {
        public SSimulationPen Pen => this.pen;
        public SSimulationPlayer Player => this.player;

        private readonly SSimulationPen pen;
        private readonly SSimulationPlayer player;

        private readonly SCameraHandler cameraHandler;
        private readonly SSimulationHandler simulationHandler;
        private readonly SWorldHandler worldHandler;

        private readonly SInputManager inputManager;

        private readonly SInputActionMapHandler actionHandler;
        private readonly SWorld world;

        public SGameInputController(ISGame gameInstance) : base(gameInstance)
        {
            this.pen = new();
            this.player = new();

            this.cameraHandler = new(gameInstance.CameraManager);
            this.simulationHandler = new(gameInstance.GameManager);
            this.worldHandler = new(gameInstance.World, gameInstance.InputManager, gameInstance.CameraManager, player, pen, gameInstance.ElementDatabase);

            this.inputManager = gameInstance.InputManager;
            this.actionHandler = new(gameInstance);

            this.world = gameInstance.World;
        }

        public override void Initialize()
        {
            BuildKeyboardInputs();
            BuildMouseInputs();
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePlaceAreaSize();

            this.actionHandler.Update(gameTime);
        }

        private void UpdatePlaceAreaSize()
        {
            if (this.inputManager.GetDeltaScrollWheel() > 0)
            {
                this.pen.RemoveSize(1);
            }
            else if (this.inputManager.GetDeltaScrollWheel() < 0)
            {
                this.pen.AddSize(1);
            }
        }

        public void Activate()
        {
            this.cameraKeyboardActionMap.SetActive(true);
            this.simulationKeyboardActionMap.SetActive(true);
            this.worldKeyboardActionMap.SetActive(true);
            this.worldMouseActionMap.SetActive(true);
        }

        public void Deactivate()
        {
            this.cameraKeyboardActionMap.SetActive(false);
            this.simulationKeyboardActionMap.SetActive(false);
            this.worldKeyboardActionMap.SetActive(false);
            this.worldMouseActionMap.SetActive(false);
        }
    }
}
