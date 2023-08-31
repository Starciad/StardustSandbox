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
    internal sealed class PInputManager : PManager
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

        protected override void OnAwake()
        {
            // World Inputs
            // Keyboard
            PInputActionMap worldKeyboardActionMap = _actionHandler.AddActionMap("World", new(true));

            worldKeyboardActionMap.AddKeyAction("World_Camera_Up", new(Keys.W, Keys.Up)).SetAction(x =>
            {
                if (x.State == PKeyCallbackState.Performed)
                {
                    PWorldCamera.Camera.Position = new(PWorldCamera.Camera.Position.X, PWorldCamera.Camera.Position.Y + speed);
                }
            });

            worldKeyboardActionMap.AddKeyAction("World_Camera_Down", new(Keys.S, Keys.Down)).SetAction(x => {
                if (x.State == PKeyCallbackState.Performed)
                {
                    PWorldCamera.Camera.Position = new(PWorldCamera.Camera.Position.X - speed, PWorldCamera.Camera.Position.Y);
                }
            });

            worldKeyboardActionMap.AddKeyAction("World_Camera_Left", new(Keys.A, Keys.Left)).SetAction(x => {
                if (x.State == PKeyCallbackState.Performed)
                {
                    PWorldCamera.Camera.Position = new(PWorldCamera.Camera.Position.X, PWorldCamera.Camera.Position.Y - speed);
                }
            });

            worldKeyboardActionMap.AddKeyAction("World_Camera_Right", new(Keys.D, Keys.Right)).SetAction(x => {
                if (x.State == PKeyCallbackState.Performed)
                {
                    PWorldCamera.Camera.Position = new(PWorldCamera.Camera.Position.X + speed, PWorldCamera.Camera.Position.Y);
                }
            });

            // ======================= //

            worldKeyboardActionMap.AddKeyAction("World_Pause", new(Keys.Space)).SetAction(x => {
                if (x.State == PKeyCallbackState.Started)
                {
                    if (_world.Instance.States.IsPaused) _world.Instance.Resume();
                    else _world.Instance.Pause();
                }
            });

            worldKeyboardActionMap.AddKeyAction("World_Reset", new(Keys.R)).SetAction(x => {
                if (x.State == PKeyCallbackState.Started)
                {
                    _world.Instance.Clear();
                }
            });

            worldKeyboardActionMap.AddKeyAction("World_Quit", new(Keys.Escape)).SetAction(x => {
                if (x.State == PKeyCallbackState.Started)
                {
                    PEngine.Stop();
                }
            });
        }
        protected override void OnStart()
        {
            PManagersHandler.TryFindByType(out _world);
        }
        protected override void OnUpdate()
        {
            ClampCamera();
            _actionHandler.Update();

            debugString = new();
            debugString.AppendLine($"Selected: {elementSelected?.Name}");
            debugString.AppendLine($"Mouse: {elementOver?.Name}");
            debugString.AppendLine($"Size: {(int)size}");
        }

        private void KeyboardSelectElement()
        {
            foreach (var item in elementsKeys)
            {
                if (PInput.Keyboard.IsKeyDown(item.Key))
                {
                    elementSelected = item.Value;
                    break;
                }
            }
        }

        private void MousePlaceArea()
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
        private void MousePlaceElements()
        {
            if (elementSelected == null)
                return;

            Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInput.Mouse.Position.ToVector2());
            Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

            if (!_world.Instance.InsideTheWorldDimensions(worldPos)) return;

            _world.Instance.TryGetElement(worldPos, out elementOver);

            // Place
            if (PInput.Mouse.LeftButton == ButtonState.Pressed)
            {
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
            }
        }
        private void MouseEraseElements()
        {
            Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInput.Mouse.Position.ToVector2());
            Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

            if (!_world.Instance.InsideTheWorldDimensions(worldPos)) return;

            if (PInput.Mouse.RightButton == ButtonState.Pressed)
            {
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
            }
        }

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