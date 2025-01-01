using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Handlers;
using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.Objects;

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

        private readonly SInputActionMapHandler actionHandler;

        private readonly ISInputManager inputManager;

        public SGameInputController(ISGame gameInstance) : base(gameInstance)
        {
            this.pen = new();
            this.player = new();

            this.cameraHandler = new(gameInstance.CameraManager);
            this.simulationHandler = new(gameInstance);
            this.worldHandler = new(gameInstance, this.player, this.pen);

            this.inputManager = gameInstance.InputManager;
            this.actionHandler = new(gameInstance);
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
                this.pen.Size--;
            }
            else if (this.inputManager.GetDeltaScrollWheel() < 0)
            {
                this.pen.Size++;
            }
        }

        public void Activate()
        {
            this.actionHandler.ActivateAll();
        }

        public void Disable()
        {
            this.actionHandler.DisableAll();
        }
    }
}
