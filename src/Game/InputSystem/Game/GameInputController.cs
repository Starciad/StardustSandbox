using Microsoft.Xna.Framework;

using StardustSandbox.Audio;
using StardustSandbox.Camera;
using StardustSandbox.Enums.Assets;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.States;
using StardustSandbox.InputSystem.Actions;
using StardustSandbox.InputSystem.Game.Handlers;
using StardustSandbox.InputSystem.Game.Simulation;
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

        internal void Initialize(World world)
        {
            this.worldHandler = new(this.player, this.pen, world);

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
                    new(controlSettings.MoveCameraUp)
                    {
                        OnPerformed = _ => SSCamera.Move(new Vector2(0, this.player.MovementSpeed)),
                    },

                    new(controlSettings.MoveCameraRight)
                    {
                        OnPerformed = _ => SSCamera.Move(new Vector2(this.player.MovementSpeed, 0)),
                    },

                    new(controlSettings.MoveCameraDown)
                    {
                        OnPerformed = _ => SSCamera.Move(new Vector2(0, -this.player.MovementSpeed)),
                    },

                    new(controlSettings.MoveCameraLeft)
                    {
                        OnPerformed = _ => SSCamera.Move(new Vector2(-this.player.MovementSpeed, 0)),
                    },
                ]),

                // Simulation
                new([
                    new(controlSettings.TogglePause)
                    {
                        OnStarted = _ => GameHandler.ToggleState(GameStates.IsSimulationPaused),
                    },
                ]),
                
                // World
                new([
                    new(controlSettings.ClearWorld)
                    {
                        OnStarted = _ => this.worldHandler.Clear(),
                    },
                ]),

                #endregion

                #region Mouse

                new([
                    new(MouseButton.Left)
                    {
                        OnPerformed = _ => this.worldHandler.Modify(WorldModificationType.Adding),
                    },

                    new(MouseButton.Right)
                    {
                        OnPerformed = _ => this.worldHandler.Modify(WorldModificationType.Removing),
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

        internal void Activate()
        {
            this.gameplayInputHandler.Activate();
        }

        internal void Disable()
        {
            this.gameplayInputHandler.Disable();
        }
    }
}
