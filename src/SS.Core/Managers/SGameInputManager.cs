using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Items;
using StardustSandbox.Core.Objects;
using StardustSandbox.Core.World;

using System;

namespace StardustSandbox.Core.Managers
{
    public sealed partial class SGameInputManager(ISGame gameInstance) : SGameObject(gameInstance)
    {
        public SItem ItemSelected => this.itemSelected;

        public int PenScale => this.penScale;
        public float CameraMovementSpeed => this.cameraMovementSpeed;
        public bool CanModifyEnvironment
        {
            get => this.canModifyEnvironment;
            set => this.canModifyEnvironment = value;
        }

        // Items
        private SItem itemSelected;

        // Settings
        private int penScale = 1;
        private float cameraMovementSpeed = 10;
        private bool canModifyEnvironment = true;

        private readonly SCameraManager _cameraManager = gameInstance.CameraManager;
        private readonly SWorld _world = gameInstance.World;
        private readonly SInputManager _inputManager = gameInstance.InputManager;
        private readonly SInputActionMapHandler _actionHandler = new(gameInstance);

        public override void Initialize()
        {
            BuildKeyboardInputs();
            BuildMouseInputs();
        }

        public override void Update(GameTime gameTime)
        {
            // Inputs
            UpdatePlaceAreaSize();
            this._actionHandler.Update(gameTime);

            // Camera
            ClampCameraInTheWorld();
        }

        // Update
        private void UpdatePlaceAreaSize()
        {
            if (this._inputManager.GetDeltaScrollWheel() > 0)
            {
                this.penScale -= 1;
            }
            else if (this._inputManager.GetDeltaScrollWheel() < 0)
            {
                this.penScale += 1;
            }

            this.penScale = Math.Clamp(this.penScale, 0, 10);
        }

        // Settings
        public void SelectItem(SItem value)
        {
            this.itemSelected = value;
        }
        public void SetPenScale(int value)
        {
            this.penScale = value;
        }
        public void SetCameraMovementSpeed(float value)
        {
            this.cameraMovementSpeed = value;
        }

        // Utilities
        private void ClampCameraInTheWorld()
        {
            int totalWorldWidth = this._world.Infos.Size.Width * SWorldConstants.GRID_SCALE;
            int totalWorldHeight = this._world.Infos.Size.Height * SWorldConstants.GRID_SCALE;

            float visibleWidth = SScreenConstants.DEFAULT_SCREEN_WIDTH;
            float visibleHeight = SScreenConstants.DEFAULT_SCREEN_HEIGHT;

            float worldLeftLimit = 0f;
            float worldRightLimit = totalWorldWidth - visibleWidth;

            float worldBottomLimit = (totalWorldHeight - visibleHeight) * -1;
            float worldTopLimit = 0f;

            Vector2 cameraPosition = this._cameraManager.Position;

            cameraPosition.X = MathHelper.Clamp(cameraPosition.X, worldLeftLimit, worldRightLimit);
            cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, worldBottomLimit, worldTopLimit);

            this._cameraManager.Position = cameraPosition;
        }
    }
}