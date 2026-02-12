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

        private WorldHandler worldHandler;

        private InputActionMapHandler systemInputHandler;
        private InputActionMapHandler gameplayInputHandler;

        private readonly Pen pen;
        private readonly Player player;

        internal PlayerInputController()
        {
            this.pen = new();
            this.player = new();
        }

        internal void Initialize(ActorManager actorManager, World world)
        {
            this.worldHandler = new(actorManager, this.pen, this.player, world);

            ControlSettings controlSettings = SettingsSerializer.Load<ControlSettings>();

            this.systemInputHandler = new([
                #region Keyboard

                new([
                    new(controlSettings.Screenshot)
                    {
                        OnStarted = _ =>
                        {
                            SoundEngine.Play(SoundEffectIndex.GUI_Accepted);
                            GameRenderer.RequestScreenshot();
                        },
                    },
                ]),

                #endregion
            ]);

            this.gameplayInputHandler = new([
                #region Keyboard

                // Camera
                new([
                    // Moving
                    new(controlSettings.MoveCameraUp)
                    {
                        OnPerformed = _ => Camera.MoveUp(this.player.MovementSpeed),
                    },

                    new(controlSettings.MoveCameraRight)
                    {
                        OnPerformed = _ => Camera.MoveRight(this.player.MovementSpeed),
                    },

                    new(controlSettings.MoveCameraDown)
                    {
                        OnPerformed = _ => Camera.MoveDown(this.player.MovementSpeed),
                    },

                    new(controlSettings.MoveCameraLeft)
                    {
                        OnPerformed = _ => Camera.MoveLeft(this.player.MovementSpeed),
                    },

                    // Zooming
                    new(controlSettings.ZoomCameraIn)
                    {
                        OnPerformed = _ => Camera.FadeIn(this.player.ZoomingSpeed),
                    },

                    new(controlSettings.ZoomCameraOut)
                    {
                        OnPerformed = _ => Camera.FadeOut(this.player.ZoomingSpeed),
                    },

                    // Running
                    new(controlSettings.MoveCameraFast)
                    {
                        OnStarted = _ => this.player.IsRunning = true,
                        OnCanceled = _ => this.player.IsRunning = false,
                    },
                ]),

                // Simulation
                new([
                    new(controlSettings.TogglePause)
                    {
                        OnStarted = _ => GameHandler.ToggleState(GameStates.IsSimulationPaused),
                    },

                    new(controlSettings.NextShape)
                    {
                        OnStarted = _ => this.pen.Shape = this.pen.Shape.Next(),
                    },
                ]),
                
                // World
                new([
                    new(controlSettings.ClearWorld)
                    {
                        OnStarted = _ => GameHandler.Reset(actorManager, world),
                    },
                ]),

                #endregion

                #region Mouse

                new([
                    new(MouseButton.Left)
                    {
                        OnStarted = _ => this.worldHandler.Modify(WorldModificationType.Adding, InputState.Started),
                        OnPerformed = _ => this.worldHandler.Modify(WorldModificationType.Adding, InputState.Performed),
                    },

                    new(MouseButton.Right)
                    {
                        OnStarted = _ => this.worldHandler.Modify(WorldModificationType.Removing, InputState.Started),
                        OnPerformed = _ => this.worldHandler.Modify(WorldModificationType.Removing, InputState.Performed),
                    },
                ]),

                #endregion
            ]);
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
            this.gameplayInputHandler.Enable();
        }

        internal void Disable()
        {
            this.gameplayInputHandler.Disable();
        }
    }
}
