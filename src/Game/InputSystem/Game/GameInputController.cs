using Microsoft.Xna.Framework;

using StardustSandbox.Audio;
using StardustSandbox.Camera;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Generators;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.States;
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

                new([
                    new(Microsoft.Xna.Framework.Input.Keys.K)
                    {
                        OnStarted = _ =>
                        {
                            Screenshot.WorldScreenshot.Capture(world);
                        },
                    },
                ]),

                #endregion
            ]);

            this.gameplayInputHandler = new([
                #region Keyboard

                // Camera
                new([
                    new(controlSettings.MoveCameraUp)
                    {
                        OnPerformed = _ => SSCamera.Move(new(0, this.player.MovementSpeed)),
                    },

                    new(controlSettings.MoveCameraRight)
                    {
                        OnPerformed = _ => SSCamera.Move(new(this.player.MovementSpeed, 0)),
                    },

                    new(controlSettings.MoveCameraDown)
                    {
                        OnPerformed = _ => SSCamera.Move(new(0, -this.player.MovementSpeed)),
                    },

                    new(controlSettings.MoveCameraLeft)
                    {
                        OnPerformed = _ => SSCamera.Move(new(-this.player.MovementSpeed, 0)),
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
