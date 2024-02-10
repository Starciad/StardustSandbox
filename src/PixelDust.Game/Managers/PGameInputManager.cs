using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Core.Worlding;
using PixelDust.Game.Camera;
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
    public sealed class PGameInputManager(POrthographicCamera orthographicCamera, PWorld world, PInputManager inputHandler, PElementDatabase elementDatabase) : PGameObject
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
            [Keys.D1] = elementDatabase.GetElementByType<PDirt>(),
            [Keys.D2] = elementDatabase.GetElementByType<PGrass>(),
            [Keys.D3] = elementDatabase.GetElementByType<PStone>(),
            [Keys.D4] = elementDatabase.GetElementByType<PSand>(),
            [Keys.D5] = elementDatabase.GetElementByType<PWater>(),
            [Keys.D6] = elementDatabase.GetElementByType<PLava>(),
            [Keys.D7] = elementDatabase.GetElementByType<PAcid>(),
            [Keys.D8] = elementDatabase.GetElementByType<PWall>(),
            [Keys.D9] = elementDatabase.GetElementByType<PMCorruption>(),
        };

        // Handlers
        private readonly PInputActionMapHandler _actionHandler = new();
        private readonly PInputManager _inputHandler = inputHandler;

        // Managers
        protected override void OnStart()
        {
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
            PInputActionMap worldKeyboardActionMap = this._actionHandler.AddActionMap("World_Keyboard", true);

            #region Camera

            worldKeyboardActionMap.AddAction("World_Camera_Up", new(_inputHandler, Keys.W, Keys.Up)).OnPerformed += context =>
            {
                orthographicCamera.Move(new(0, this.speed));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Down", new(_inputHandler, Keys.S, Keys.Down)).OnPerformed += context =>
            {
                orthographicCamera.Move(new(0, -this.speed));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Left", new(_inputHandler, Keys.A, Keys.Left)).OnPerformed += context =>
            {
                orthographicCamera.Move(new(-this.speed, 0));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Right", new(_inputHandler, Keys.D, Keys.Right)).OnPerformed += context =>
            {
                orthographicCamera.Move(new(this.speed, 0));
            };

            #endregion

            #region Shortcuts

            worldKeyboardActionMap.AddAction("World_Pause", new(_inputHandler, Keys.Space)).OnStarted += context =>
            {
                if (world.States.IsPaused)
                {
                    world.Resume();
                }
                else
                {
                    world.Pause();
                }
            };

            worldKeyboardActionMap.AddAction("World_Reset", new(_inputHandler, Keys.R)).OnStarted += context =>
            {
                world.Clear();
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
            PInputActionMap worldMouseActionMap = this._actionHandler.AddActionMap("World_Mouse", true);

            #region Elements (Place)

            worldMouseActionMap.AddAction("World_Place_Elements", new(_inputHandler, PMouseButton.Left)).OnPerformed += context =>
            {
                Vector2 screenPos = orthographicCamera.ScreenToWorld(this._inputHandler.MouseState.Position.ToVector2());
                Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorldConstants.GRID_SCALE;

                if (!world.InsideTheWorldDimensions(worldPos) ||
                     this.elementSelected == null)
                {
                    return;
                }

                this.elementOver = world.GetElement(worldPos);

                if (this.size == 0)
                {
                    world.InstantiateElement(worldPos, this.elementSelected.Id);
                    return;
                }

                for (int x = -(int)this.size; x < this.size; x++)
                {
                    for (int y = -(int)this.size; y < this.size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!world.InsideTheWorldDimensions(lpos))
                        {
                            continue;
                        }

                        world.InstantiateElement(lpos, this.elementSelected.Id);
                    }
                }
            };

            worldMouseActionMap.AddAction("World_Erase_Elements", new(_inputHandler, PMouseButton.Right)).OnPerformed += context =>
            {
                Vector2 screenPos = orthographicCamera.ScreenToWorld(this._inputHandler.MouseState.Position.ToVector2());
                Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorldConstants.GRID_SCALE;

                if (!world.InsideTheWorldDimensions(worldPos) ||
                     this.elementSelected == null)
                {
                    return;
                }

                this.elementOver = world.GetElement(worldPos);

                if (this.size == 0)
                {
                    world.DestroyElement(worldPos);
                    return;
                }

                for (int x = -(int)this.size; x < this.size; x++)
                {
                    for (int y = -(int)this.size; y < this.size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!world.InsideTheWorldDimensions(lpos))
                        {
                            continue;
                        }

                        world.DestroyElement(lpos);
                    }
                }
            };

            #endregion
        }

        #endregion

        private void GetMouseOverElement()
        {
            Vector2 screenPos = orthographicCamera.ScreenToWorld(this._inputHandler.MouseState.Position.ToVector2());
            Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorldConstants.GRID_SCALE;

            this.elementOverSlot = world.GetElementSlot((Vector2Int)worldPos);
        }

        private void ClampCamera()
        {
            // Clamp
            int totalX = (world.Infos.Size.Width * PWorldConstants.GRID_SCALE) - PScreenConstants.SCREEN_WIDTH;
            int totalY = (world.Infos.Size.Height * PWorldConstants.GRID_SCALE) - PScreenConstants.SCREEN_HEIGHT;

            orthographicCamera.Position = new Vector2(
                Math.Clamp(orthographicCamera.Position.X, 0, totalX),
                Math.Clamp(orthographicCamera.Position.Y, -totalY, 0)
            );
        }
    }
}