using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Core.Worlding;
using PixelDust.Game.Constants;
using PixelDust.Game.Elements;
using PixelDust.Game.Elements.Common.Liquid;
using PixelDust.Game.Elements.Common.Solid.Immovable;
using PixelDust.Game.Elements.Common.Solid.Movable;
using PixelDust.Game.Engine;
using PixelDust.Game.InputSystem.Actions;
using PixelDust.Game.InputSystem.Enums;
using PixelDust.Game.Mathematics;
using PixelDust.Game.Objects;
using PixelDust.Game.Worlding;
using PixelDust.Game.Worlding.World.Slots;

using System;
using System.Collections.Generic;
using System.Text;

namespace PixelDust.Game.Managers
{
    public sealed class PGameInputManager(PInputManager inputHandler) : PGameObject
    {
        public static StringBuilder DebugString => debugString;
        private static StringBuilder debugString;

        private PElement elementSelected;
        private PElement elementOver;
        private PWorldElementSlot elementOverSlot;

        private float size = 1;
        private readonly float speed = 10;

        private readonly Dictionary<Keys, PElement> elementsKeys = new()
        {
            [Keys.D1] = PElementDatabase.GetElementByType<PDirt>(),
            [Keys.D2] = PElementDatabase.GetElementByType<PGrass>(),
            [Keys.D3] = PElementDatabase.GetElementByType<PStone>(),
            [Keys.D4] = PElementDatabase.GetElementByType<PSand>(),
            [Keys.D5] = PElementDatabase.GetElementByType<PWater>(),
            [Keys.D6] = PElementDatabase.GetElementByType<PLava>(),
            [Keys.D7] = PElementDatabase.GetElementByType<PAcid>(),
            [Keys.D8] = PElementDatabase.GetElementByType<PWall>(),
            [Keys.D9] = PElementDatabase.GetElementByType<PMCorruption>(),
        };

        // Handlers
        private readonly PInputActionMapHandler _actionHandler = new();
        private readonly PInputManager _inputHandler = inputHandler;

        // Managers
        private PWorld _world;

        protected override void OnStart()
        {
            this._world = this.Game.World;

            BuildKeyboardInputs();
            BuildMouseInputs();
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            UpdateMouse();
            this._actionHandler.Update(gameTime);

            ClampCamera();
            GetMouseOverElement();

            debugString = new();
            _ = debugString.AppendLine($"Selected: {this.elementSelected?.Name}");
            _ = debugString.AppendLine($"Mouse: {this.elementOver?.Name}");
            _ = debugString.AppendLine($"Temperature: {this.elementOverSlot.Temperature.ToString("0.##")}°C");
            _ = debugString.AppendLine($"Size: {(int)this.size}");
        }

        private void BuildKeyboardInputs()
        {
            BuildWorldKeyboard();
        }
        private void BuildMouseInputs()
        {
            BuildWorldMouse();
        }

        #region Updates
        private void UpdateMouse()
        {
            UpdatePlaceAreaSize();
        }

        private void UpdatePlaceAreaSize()
        {
            if (this._inputHandler.GetDeltaScrollWheel() > 0)
            {
                this.size -= 1f;
            }
            else if (this._inputHandler.GetDeltaScrollWheel() < 0)
            {
                this.size += 1f;
            }

            this.size = Math.Clamp(this.size, 0, 10);
        }

        #endregion

        #region Keyboard

        private void BuildWorldKeyboard()
        {
            PInputActionMap worldKeyboardActionMap = this._actionHandler.AddActionMap("World_Keyboard", new(true));

            #region Camera

            worldKeyboardActionMap.AddAction("World_Camera_Up", new(_inputHandler, Keys.W, Keys.Up)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(0, this.speed));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Down", new(_inputHandler, Keys.S, Keys.Down)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(0, -this.speed));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Left", new(_inputHandler, Keys.A, Keys.Left)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(-this.speed, 0));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Right", new(_inputHandler, Keys.D, Keys.Right)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(this.speed, 0));
            };

            #endregion

            #region Shortcuts

            worldKeyboardActionMap.AddAction("World_Pause", new(_inputHandler, Keys.Space)).OnStarted += context =>
            {
                if (this._world.States.IsPaused)
                {
                    this._world.Resume();
                }
                else
                {
                    this._world.Pause();
                }
            };

            worldKeyboardActionMap.AddAction("World_Reset", new(_inputHandler, Keys.R)).OnStarted += context =>
            {
                this._world.Clear();
            };

            #endregion

            #region Elements

            worldKeyboardActionMap.AddAction("World_Select_Element", new(_inputHandler, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0)).OnStarted += context =>
            {
                if (this.elementsKeys.TryGetValue(context.CapturedKey, out PElement value))
                {
                    if (value == null)
                    {
                        return;
                    }

                    this.elementSelected = value;
                }
            };

            #endregion
        }

        #endregion

        #region Mouse

        private void BuildWorldMouse()
        {
            PInputActionMap worldMouseActionMap = this._actionHandler.AddActionMap("World_Mouse", new(true));

            #region Elements (Place)

            worldMouseActionMap.AddAction("World_Place_Elements", new(_inputHandler, PMouseButton.Left)).OnPerformed += context =>
            {
                Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(this._inputHandler.MouseState.Position.ToVector2());
                Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorldConstants.GRID_SCALE;

                if (!this._world.InsideTheWorldDimensions(worldPos) ||
                     this.elementSelected == null)
                {
                    return;
                }

                _ = this._world.TryGetElement(worldPos, out this.elementOver);

                if (this.size == 0)
                {
                    _ = this._world.TryInstantiateElement(worldPos, this.elementSelected.Id);
                    return;
                }

                for (int x = -(int)this.size; x < this.size; x++)
                {
                    for (int y = -(int)this.size; y < this.size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!this._world.InsideTheWorldDimensions(lpos))
                        {
                            continue;
                        }

                        _ = this._world.TryInstantiateElement(lpos, this.elementSelected.Id);
                    }
                }
            };

            worldMouseActionMap.AddAction("World_Erase_Elements", new(_inputHandler, PMouseButton.Right)).OnPerformed += context =>
            {
                Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(this._inputHandler.MouseState.Position.ToVector2());
                Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorldConstants.GRID_SCALE;

                if (!this._world.InsideTheWorldDimensions(worldPos) ||
                     this.elementSelected == null)
                {
                    return;
                }

                _ = this._world.TryGetElement(worldPos, out this.elementOver);

                if (this.size == 0)
                {
                    _ = this._world.TryDestroyElement(worldPos);
                    return;
                }

                for (int x = -(int)this.size; x < this.size; x++)
                {
                    for (int y = -(int)this.size; y < this.size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!this._world.InsideTheWorldDimensions(lpos))
                        {
                            continue;
                        }

                        _ = this._world.TryDestroyElement(lpos);
                    }
                }
            };

            #endregion
        }

        #endregion

        private void GetMouseOverElement()
        {
            Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(this._inputHandler.MouseState.Position.ToVector2());
            Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorldConstants.GRID_SCALE;

            _ = this._world.TryGetElementSlot((Vector2Int)worldPos, out this.elementOverSlot);
        }

        private void ClampCamera()
        {
            // Clamp
            int totalX = (this._world.Infos.Size.Width * PWorldConstants.GRID_SCALE) - PScreenConstants.SCREEN_WIDTH;
            int totalY = (this._world.Infos.Size.Height * PWorldConstants.GRID_SCALE) - PScreenConstants.SCREEN_HEIGHT;

            PWorldCamera.Camera.Position = new(
                Math.Clamp(PWorldCamera.Camera.Position.X, 0, totalX),
                Math.Clamp(PWorldCamera.Camera.Position.Y, -totalY, 0)
            );
        }
    }
}