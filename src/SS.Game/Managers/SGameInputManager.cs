﻿using Microsoft.Xna.Framework;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.InputSystem;
using StardustSandbox.Game.Items;
using StardustSandbox.Game.Objects;
using StardustSandbox.Game.World;

using System;

namespace StardustSandbox.Game.Managers
{
    public sealed partial class SGameInputManager(SGame gameInstance) : SGameObject(gameInstance)
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

            // External
            ClampCamera();
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
        private void ClampCamera()
        {
            int totalX = (int)(this._world.Infos.Size.Width * SWorldConstants.GRID_SCALE) - SScreenConstants.DEFAULT_SCREEN_WIDTH;
            int totalY = (int)(this._world.Infos.Size.Height * SWorldConstants.GRID_SCALE) - SScreenConstants.DEFAULT_SCREEN_HEIGHT;

            this._cameraManager.ClampPosition(new Vector2(0, -totalY), new Vector2(totalX, 0));
        }
    }
}