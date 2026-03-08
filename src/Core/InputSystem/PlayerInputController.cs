/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Inputs;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.InputSystem.Actions;
using StardustSandbox.Core.InputSystem.Handlers;
using StardustSandbox.Core.InputSystem.Simulation;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.InputSystem
{
    internal sealed class PlayerInputController
    {
        internal Pen Pen => this.pen;
        internal Player Player => this.player;

        internal InputActionMapHandler GameplayInputHandler => this.gameplayInputHandler;
        internal InputActionMapHandler SystemInputHandler => this.systemInputHandler;

        private WorldHandler worldHandler;
        private InputActionMapHandler systemInputHandler, gameplayInputHandler;

        private readonly Pen pen;
        private readonly Player player;

        internal PlayerInputController()
        {
            this.pen = new();
            this.player = new();
        }

        internal void Initialize(ActorManager actorManager, Camera2D camera, VideoManager videoManager, World world)
        {
            this.worldHandler = new(actorManager, camera, this.pen, this.player, world);

            ControlSettings controlSettings = SettingsSerializer.Load<ControlSettings>();

            this.systemInputHandler = new(
                new InputActionMap("General",
                    new InputAction("Screenshot")
                    {
                        KeyboardBinding = controlSettings.ScreenshotKeyboardBinding,
                        OnStarted = _ =>
                        {
                            SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                            GameRenderer.RequestScreenshot();
                        },
                    },

                    new InputAction("ToggleFullscreen")
                    {
                        KeyboardBinding = controlSettings.ToggleFullscreenKeyboardBinding,
                        OnStarted = _ => videoManager.ToggleFullScreen(),
                    }
                )
            );

            this.gameplayInputHandler = new(
                new InputActionMap("Camera",
                    // Moving
                    new InputAction("MoveUp")
                    {
                        KeyboardBinding = controlSettings.MoveCameraUpKeyboardBinding,
                        OnPerformed = _ => camera.MoveUp(this.player.MovementSpeed),
                    },

                    new InputAction("MoveRight")
                    {
                        KeyboardBinding = controlSettings.MoveCameraRightKeyboardBinding,
                        OnPerformed = _ => camera.MoveRight(this.player.MovementSpeed),
                    },

                    new InputAction("MoveDown")
                    {
                        KeyboardBinding = controlSettings.MoveCameraDownKeyboardBinding,
                        OnPerformed = _ => camera.MoveDown(this.player.MovementSpeed),
                    },

                    new InputAction("MoveLeft")
                    {
                        KeyboardBinding = controlSettings.MoveCameraLeftKeyboardBinding,
                        OnPerformed = _ => camera.MoveLeft(this.player.MovementSpeed),
                    },

                    // Zooming
                    new InputAction("ZoomIn")
                    {
                        KeyboardBinding = controlSettings.ZoomCameraInKeyboardBinding,
                        OnPerformed = _ => camera.FadeIn(this.player.ZoomingSpeed),
                    },

                    new InputAction("ZoomOut")
                    {
                        KeyboardBinding = controlSettings.ZoomCameraOutKeyboardBinding,
                        OnPerformed = _ => camera.FadeOut(this.player.ZoomingSpeed),
                    },

                    // Running
                    new InputAction("MoveFast")
                    {
                        KeyboardBinding = controlSettings.MoveCameraFastKeyboardBinding,
                        OnStarted = _ => this.player.IsRunning = true,
                        OnCanceled = _ => this.player.IsRunning = false,
                    }
                ),

                new("Simulation",
                    new InputAction("TooglePause")
                    {
                        KeyboardBinding = controlSettings.TogglePauseKeyboardBinding,
                        OnStarted = _ => GameHandler.ToggleState(GameStates.IsSimulationPaused),
                    },

                    new InputAction("NextShape")
                    {
                        KeyboardBinding = controlSettings.NextShapeKeyboardBinding,
                        OnStarted = _ => this.pen.Shape = this.pen.Shape.Next(),
                    },

                    new InputAction("ClearWorld")
                    {
                        KeyboardBinding = controlSettings.ClearWorldKeyboardBinding,
                        OnStarted = _ => GameHandler.Reset(actorManager, world),
                    }
                ),

                // Mouse
                new("Mouse",
                    new InputAction("Adding")
                    {
                        MouseBinding = MouseButton.Left,
                        OnStarted = _ => this.worldHandler.Modify(WorldModificationType.Adding, InputState.Started),
                        OnPerformed = _ => this.worldHandler.Modify(WorldModificationType.Adding, InputState.Performed),
                    },

                    new InputAction("Removing")
                    {
                        MouseBinding = MouseButton.Right,
                        OnStarted = _ => this.worldHandler.Modify(WorldModificationType.Removing, InputState.Started),
                        OnPerformed = _ => this.worldHandler.Modify(WorldModificationType.Removing, InputState.Performed),
                    }
                )
            );
        }

        internal void Update()
        {
            UpdatePlaceAreaSize();

            this.systemInputHandler.Update();
            this.gameplayInputHandler.Update();
        }

        private void UpdatePlaceAreaSize()
        {
            if (GameHandler.HasState(GameStates.IsCriticalMenuOpen))
            {
                return;
            }

            if (InputEngine.GetDeltaScrollWheel() > 0)
            {
                this.pen.Size--;
            }
            else if (InputEngine.GetDeltaScrollWheel() < 0)
            {
                this.pen.Size++;
            }
        }

        internal void Enable()
        {
            this.gameplayInputHandler.EnableAll();
        }

        internal void Disable()
        {
            this.gameplayInputHandler.DisableAll();
        }
    }
}
