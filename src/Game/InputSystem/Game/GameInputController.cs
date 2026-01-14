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

using StardustSandbox.Audio;
using StardustSandbox.Core;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Generators;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.States;
using StardustSandbox.Extensions;
using StardustSandbox.Generators;
using StardustSandbox.InputSystem.Actions;
using StardustSandbox.InputSystem.Game.Handlers;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.InputSystem.Game
{
    internal sealed class InputController
    {
        internal Pen Pen => this.pen;
        internal Player Player => this.player;

        private WorldHandler worldHandler;

        private InputActionMapHandler systemInputHandler;
        private InputActionMapHandler gameplayInputHandler;

        private readonly Pen pen;
        private readonly Player player;

        internal InputController()
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
                    // Movement
                    new(controlSettings.MoveCameraUp)
                    {
                        OnPerformed = _ => Camera.Move(new(0, this.player.MovementSpeed)),
                    },

                    new(controlSettings.MoveCameraRight)
                    {
                        OnPerformed = _ => Camera.Move(new(this.player.MovementSpeed, 0)),
                    },

                    new(controlSettings.MoveCameraDown)
                    {
                        OnPerformed = _ => Camera.Move(new(0, -this.player.MovementSpeed)),
                    },

                    new(controlSettings.MoveCameraLeft)
                    {
                        OnPerformed = _ => Camera.Move(new(-this.player.MovementSpeed, 0)),
                    },

                    // Zoom
                    new(controlSettings.CameraZoomIn)
                    {
                        OnPerformed = _ => Camera.ZoomIn(this.player.ZoomSpeed),
                    },

                    new(controlSettings.CameraZoomOut)
                    {
                        OnPerformed = _ => Camera.ZoomOut(this.player.ZoomSpeed),
                    },

                    // Running
                    new(controlSettings.CameraRunning)
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

                    new(controlSettings.GenerateWorld)
                    {
                        OnStarted = _ => WorldGenerator.Start(actorManager, world, WorldGenerationPreset.Plain),
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
                        OnCanceled = _ => this.worldHandler.Modify(WorldModificationType.Adding, InputState.Canceled),
                    },

                    new(MouseButton.Right)
                    {
                        OnStarted = _ => this.worldHandler.Modify(WorldModificationType.Removing, InputState.Started),
                        OnPerformed = _ => this.worldHandler.Modify(WorldModificationType.Removing, InputState.Performed),
                        OnCanceled = _ => this.worldHandler.Modify(WorldModificationType.Removing, InputState.Canceled),
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

            if (Input.GetDeltaScrollWheel() > 0)
            {
                this.pen.Size--;
            }
            else if (Input.GetDeltaScrollWheel() < 0)
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

