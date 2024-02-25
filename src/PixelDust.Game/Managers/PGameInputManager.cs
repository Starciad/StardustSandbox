using Microsoft.Xna.Framework;

using PixelDust.Game.Constants;
using PixelDust.Game.Databases;
using PixelDust.Game.Items;
using PixelDust.Game.Objects;
using PixelDust.Game.World;

using System;

namespace PixelDust.Game.Managers
{
    public sealed partial class PGameInputManager(PCameraManager cameraManager, PWorld world, PInputManager inputHandler) : PGameObject
    {
        public PItem ItemSelected => this.itemSelected;

        public int PenScale => this.penScale;
        public float CameraMovementSpeed => this.cameraMovementSpeed;
        public bool CanModifyEnvironment
        {
            get => this.canModifyEnvironment;
            set => this.canModifyEnvironment = value;
        }

        // Items
        private PItem itemSelected;

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
        public void SelectItem(PItem value)
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
            int totalX = (world.Infos.Size.Width * PWorldConstants.GRID_SCALE) - PScreenConstants.DEFAULT_SCREEN_WIDTH;
            int totalY = (world.Infos.Size.Height * PWorldConstants.GRID_SCALE) - PScreenConstants.DEFAULT_SCREEN_HEIGHT;

            cameraManager.ClampPosition(new Vector2(0, -totalY), new Vector2(totalX, 0));
        }
    }
}