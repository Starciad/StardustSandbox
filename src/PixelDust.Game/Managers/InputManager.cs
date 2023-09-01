using PixelDust.Core;
using PixelDust.Core.Managers;
using PixelDust.Core.Engine;
using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;

using PixelDust.Game.Elements.Liquid;
using PixelDust.Game.Elements.Solid.Immovable;
using PixelDust.Game.Elements.Solid.Movable;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;
using PixelDust.Core.Input;

namespace PixelDust.Game.Managers
{
    [PManagerRegister]
    internal sealed class InputManager : PManager
    {
        public static StringBuilder DebugString => debugString;
        private static StringBuilder debugString;

        private PElement elementSelected;
        private PElement elementOver;

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
            [Keys.D9] = PElementsHandler.GetElementByType<Corruption>(),
        };

        // Handlers
        private readonly PInputActionMapHandler _actionHandler = new();

        // Managers
        private WorldManager _world;

        protected override void OnStart()
        {
            PManagersHandler.TryFindByType(out _world);

            BuildKeyboardInputs();
            BuildMouseInputs();
        }
        protected override void OnUpdate()
        {
            UpdateMouse();
            ClampCamera();
            _actionHandler.Update();

            debugString = new();
            debugString.AppendLine($"Selected: {elementSelected?.Name}");
            debugString.AppendLine($"Mouse: {elementOver?.Name}");
            debugString.AppendLine($"Size: {(int)size}");
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
            if (PInput.GetDeltaScrollWheel() > 0)
            {
                size -= 1f;
            }
            else if (PInput.GetDeltaScrollWheel() < 0)
            {
                size += 1f;
            }

            size = Math.Clamp(size, 0, 10);
        }

        #endregion

        #region Keyboard

        private void BuildWorldKeyboard()
        {
            PInputActionMap worldKeyboardActionMap = _actionHandler.AddActionMap("World_Keyboard", new(true));

            #region Camera

            worldKeyboardActionMap.AddAction("World_Camera_Up", new(Keys.W, Keys.Up)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(0, speed));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Down", new(Keys.S, Keys.Down)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(0, -speed));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Left", new(Keys.A, Keys.Left)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(-speed, 0));
            };

            worldKeyboardActionMap.AddAction("World_Camera_Right", new(Keys.D, Keys.Right)).OnPerformed += context =>
            {
                PWorldCamera.Camera.Move(new(speed, 0));
            };

            #endregion

            #region Shortcuts

            worldKeyboardActionMap.AddAction("World_Pause", new(Keys.Space)).OnStarted += context =>
            {
                if (_world.Instance.States.IsPaused) _world.Instance.Resume();
                else _world.Instance.Pause();
            };

            worldKeyboardActionMap.AddAction("World_Reset", new(Keys.R)).OnStarted += context =>
            {
                _world.Instance.Clear();
            };

            worldKeyboardActionMap.AddAction("World_Quit", new(Keys.Escape)).OnStarted += context =>
            {
                PEngine.Stop();
            };

            #endregion

            #region Elements

            worldKeyboardActionMap.AddAction("World_Select_Element", new(Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0)).OnStarted += context =>
            {
                if (elementsKeys.TryGetValue(context.CapturedKey, out PElement value))
                {
                    if (value == null)
                        return;

                    elementSelected = value;
                }
            };

            #endregion
        }

        #endregion

        #region Mouse

        private void BuildWorldMouse()
        {
            PInputActionMap worldMouseActionMap = _actionHandler.AddActionMap("World_Mouse", new(true));

            #region Elements (Place)

            worldMouseActionMap.AddAction("World_Place_Elements", new(PMouseButton.Left)).OnPerformed += context =>
            {
                Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInput.Mouse.Position.ToVector2());
                Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

                if (!_world.Instance.InsideTheWorldDimensions(worldPos) ||
                     elementSelected == null)
                    return;

                _world.Instance.TryGetElement(worldPos, out elementOver);

                if (size == 0)
                {
                    _world.Instance.TryInstantiate(worldPos, elementSelected.Id);
                    return;
                }

                for (int x = -(int)size; x < size; x++)
                {
                    for (int y = -(int)size; y < size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!_world.Instance.InsideTheWorldDimensions(lpos))
                            continue;

                        _world.Instance.TryInstantiate(lpos, elementSelected.Id);
                    }
                }
            };

            worldMouseActionMap.AddAction("World_Erase_Elements", new(PMouseButton.Right)).OnPerformed += context =>
            {
                Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInput.Mouse.Position.ToVector2());
                Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

                if (!_world.Instance.InsideTheWorldDimensions(worldPos) ||
                     elementSelected == null)
                    return;

                _world.Instance.TryGetElement(worldPos, out elementOver);

                if (size == 0)
                {
                    _world.Instance.TryDestroy(worldPos);
                    return;
                }

                for (int x = -(int)size; x < size; x++)
                {
                    for (int y = -(int)size; y < size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!_world.Instance.InsideTheWorldDimensions(lpos))
                            continue;

                        _world.Instance.TryDestroy(lpos);
                    }
                }
            };

            #endregion
        }

        #endregion

        private void ClampCamera()
        {
            // Clamp
            int totalX = (int)(_world.Instance.Infos.Width * PWorld.Scale - PScreen.DefaultResolution.X);
            int totalY = (int)(_world.Instance.Infos.Height * PWorld.Scale - PScreen.DefaultResolution.Y);

            PWorldCamera.Camera.Position = new(
                Math.Clamp(PWorldCamera.Camera.Position.X, 0, totalX),
                Math.Clamp(PWorldCamera.Camera.Position.Y, -totalY, 0)
            );
        }
    }
}