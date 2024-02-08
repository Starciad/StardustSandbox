using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PixelDust.Core;
using PixelDust.Core.Elements;
using PixelDust.Core.Engine.Components;
using PixelDust.Core.Managers;
using PixelDust.Core.Managers.Attributes;
using PixelDust.Core.Worlding;
using PixelDust.Core.Worlding.World.Slots;
using PixelDust.Game.Elements.Liquid;
using PixelDust.Game.Elements.Solid.Immovable;
using PixelDust.Game.Elements.Solid.Movable;
using PixelDust.InputSystem.Actions;
using PixelDust.InputSystem.Enums;
using PixelDust.InputSystem.Handlers;
using PixelDust.Mathematics;

using System;
using System.Collections.Generic;
using System.Text;

namespace PixelDust.Game.Managers
{
    [PManagerRegister]
    internal sealed class InputManager : PManager
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
            [Keys.D1] = PElementsHandler.GetElementByType<Dirt>(),
            [Keys.D2] = PElementsHandler.GetElementByType<Grass>(),
            [Keys.D3] = PElementsHandler.GetElementByType<Stone>(),
            [Keys.D4] = PElementsHandler.GetElementByType<Sand>(),
            [Keys.D5] = PElementsHandler.GetElementByType<Water>(),
            [Keys.D6] = PElementsHandler.GetElementByType<Lava>(),
            [Keys.D7] = PElementsHandler.GetElementByType<Acid>(),
            [Keys.D8] = PElementsHandler.GetElementByType<Wall>(),
            [Keys.D9] = PElementsHandler.GetElementByType<MCorruption>(),
        };

        // Handlers
        private readonly PInputActionMapHandler _actionHandler = new();

        // Managers
        private WorldManager _world;

        protected override void OnStart()
        {
            _ = PManagersHandler.TryFindByType(out this._world);

            BuildKeyboardInputs();
            BuildMouseInputs();
        }
        protected override void OnUpdate()
        {
            UpdateMouse();

            this._actionHandler.Update();
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
            if (PInputHandler.GetDeltaScrollWheel() > 0)
            {
                this.size -= 1f;
            }
            else if (PInputHandler.GetDeltaScrollWheel() < 0)
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

            worldKeyboardActionMap.AddAction("World_Camera_Up", new(Keys.W, Keys.Up)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(0, this.speed));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Down", new(Keys.S, Keys.Down)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(0, -this.speed));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Left", new(Keys.A, Keys.Left)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(-this.speed, 0));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Right", new(Keys.D, Keys.Right)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(this.speed, 0));
            };

            #endregion

            #region Shortcuts

            worldKeyboardActionMap.AddAction("World_Pause", new(Keys.Space)).OnStarted += context =>
            {
                if (this._world.Instance.States.IsPaused)
                {
                    this._world.Instance.Resume();
                }
                else
                {
                    this._world.Instance.Pause();
                }
            };

            worldKeyboardActionMap.AddAction("World_Reset", new(Keys.R)).OnStarted += context =>
            {
                this._world.Instance.Clear();
            };

            worldKeyboardActionMap.AddAction("World_Quit", new(Keys.Escape)).OnStarted += context =>
            {
                PEngine.Stop();
            };

            #endregion

            #region Elements

            worldKeyboardActionMap.AddAction("World_Select_Element", new(Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0)).OnStarted += context =>
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

            worldMouseActionMap.AddAction("World_Place_Elements", new(PMouseButton.Left)).OnPerformed += context =>
            {
                Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInputHandler.Mouse.Position.ToVector2());
                Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

                if (!this._world.Instance.InsideTheWorldDimensions(worldPos) ||
                     this.elementSelected == null)
                {
                    return;
                }

                _ = this._world.Instance.TryGetElement(worldPos, out this.elementOver);

                if (this.size == 0)
                {
                    _ = this._world.Instance.TryInstantiateElement(worldPos, this.elementSelected.Id);
                    return;
                }

                for (int x = -(int)this.size; x < this.size; x++)
                {
                    for (int y = -(int)this.size; y < this.size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!this._world.Instance.InsideTheWorldDimensions(lpos))
                        {
                            continue;
                        }

                        _ = this._world.Instance.TryInstantiateElement(lpos, this.elementSelected.Id);
                    }
                }
            };

            worldMouseActionMap.AddAction("World_Erase_Elements", new(PMouseButton.Right)).OnPerformed += context =>
            {
                Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInputHandler.Mouse.Position.ToVector2());
                Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

                if (!this._world.Instance.InsideTheWorldDimensions(worldPos) ||
                     this.elementSelected == null)
                {
                    return;
                }

                _ = this._world.Instance.TryGetElement(worldPos, out this.elementOver);

                if (this.size == 0)
                {
                    _ = this._world.Instance.TryDestroyElement(worldPos);
                    return;
                }

                for (int x = -(int)this.size; x < this.size; x++)
                {
                    for (int y = -(int)this.size; y < this.size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!this._world.Instance.InsideTheWorldDimensions(lpos))
                        {
                            continue;
                        }

                        _ = this._world.Instance.TryDestroyElement(lpos);
                    }
                }
            };

            #endregion
        }

        #endregion

        private void GetMouseOverElement()
        {
            Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInputHandler.Mouse.Position.ToVector2());
            Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

            _ = this._world.Instance.TryGetElementSlot((Vector2Int)worldPos, out this.elementOverSlot);
        }

        private void ClampCamera()
        {
            // Clamp
            int totalX = (int)((this._world.Instance.Infos.Size.Width * PWorld.Scale) - PScreen.DefaultResolution.X);
            int totalY = (int)((this._world.Instance.Infos.Size.Height * PWorld.Scale) - PScreen.DefaultResolution.Y);

            PWorldCamera.Camera.Position = new(
                Math.Clamp(PWorldCamera.Camera.Position.X, 0, totalX),
                Math.Clamp(PWorldCamera.Camera.Position.Y, -totalY, 0)
            );
        }
    }
}