using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Camera;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
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

        private SimulationHandler simulationHandler;
        private WorldHandler worldHandler;
        private InputActionMapHandler actionHandler;

        private readonly Pen pen;
        private readonly Player player;

        internal InputController()
        {
            this.pen = new();
            this.player = new();
        }

        internal void Initialize(GameManager gameManager, World world)
        {
            this.simulationHandler = new(gameManager);
            this.worldHandler = new(this.player, this.pen, world);

            ControlSettings controlSettings = SettingsSerializer.LoadSettings<ControlSettings>();

            this.actionHandler = new([
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
                        OnStarted = _ => this.simulationHandler.TogglePause(),
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
            this.actionHandler.Update();
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
            this.actionHandler.ActivateAll();
        }

        internal void Disable()
        {
            this.actionHandler.DisableAll();
        }
    }
}
