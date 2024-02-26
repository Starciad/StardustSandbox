using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Objects;
using StardustSandbox.Game.World;

using System;

namespace StardustSandbox.Game.Managers
{
    public sealed partial class SGameInputManager(SCameraManager cameraManager, SWorld world, SInputManager inputHandler) : SGameObject
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

        protected override void OnAwake()
        {
            BuildKeyboardInputs();
            BuildMouseInputs();
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            // Inputs
            UpdatePlaceAreaSize();
            this._actionHandler.Update(gameTime);

            // External
            ClampCamera();
        }

        // Update
        private void UpdatePlaceAreaSize()
        {
            if (this._inputHandler.GetDeltaScrollWheel() > 0)
            {
                this.penScale -= 1;
            }
            else if (this._inputHandler.GetDeltaScrollWheel() < 0)
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
        private void ClampCamera()
        {
            int totalX = (int)(world.Infos.Size.Width * SWorldConstants.GRID_SCALE) - SScreenConstants.DEFAULT_SCREEN_WIDTH;
            int totalY = (int)(world.Infos.Size.Height * SWorldConstants.GRID_SCALE) - SScreenConstants.DEFAULT_SCREEN_HEIGHT;

            cameraManager.ClampPosition(new Vector2(0, -totalY), new Vector2(totalX, 0));
        }
    }
}