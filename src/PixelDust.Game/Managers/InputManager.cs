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

namespace PixelDust.Game.Managers
{
    [PManagerRegister]
    internal sealed class InputManager : PManager
    {
        public static StringBuilder DebugString => debugString;

        private static PElement elementSelected;
        private static PElement elementOver;

        private static float size = 1;
        private readonly static float speed = 10;

        private static StringBuilder debugString;

        private static readonly Dictionary<Keys, PElement> elementsKeys = new()
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

        protected override void OnUpdate()
        {
            MouseUpdate();
            KeyboardUpdate();

            debugString = new();
            debugString.AppendLine($"Selected: {elementSelected?.Name}");
            debugString.AppendLine($"Mouse: {elementOver?.Name}");
            debugString.AppendLine($"Size: {(int)size}");
            base.OnUpdate();
        }

        private static void KeyboardUpdate()
        {
            KeyboardSelectElement();
            KeyboardPause();
            KeyboardCameraMovement();
            KeyboardResetWorld();
            KeyboardQuit();
        }
        private static void MouseUpdate()
        {
            MousePlaceArea();
            MousePlaceElements();
            MouseEraseElements();
        }

        #region Keyboard
        private static void KeyboardCameraMovement()
        {
            if (PInput.Keyboard.IsKeyDown(Keys.W))
            {
                PWorldCamera.Camera.Position = new(PWorldCamera.Camera.Position.X, PWorldCamera.Camera.Position.Y + speed);
            }

            if (PInput.Keyboard.IsKeyDown(Keys.A))
            {
                PWorldCamera.Camera.Position = new(PWorldCamera.Camera.Position.X - speed, PWorldCamera.Camera.Position.Y);
            }

            if (PInput.Keyboard.IsKeyDown(Keys.S))
            {
                PWorldCamera.Camera.Position = new(PWorldCamera.Camera.Position.X, PWorldCamera.Camera.Position.Y - speed);
            }

            if (PInput.Keyboard.IsKeyDown(Keys.D))
            {
                PWorldCamera.Camera.Position = new(PWorldCamera.Camera.Position.X + speed, PWorldCamera.Camera.Position.Y);
            }

            // Clamp
            int totalX = (int)(PWorld.Infos.Width * PWorld.Scale - PScreen.DefaultResolution.X);
            int totalY = (int)(PWorld.Infos.Height * PWorld.Scale - PScreen.DefaultResolution.Y);

            PWorldCamera.Camera.Position = new(
                Math.Clamp(PWorldCamera.Camera.Position.X, 0, totalX),
                Math.Clamp(PWorldCamera.Camera.Position.Y, -totalY, 0)
            );
        }
        private static void KeyboardPause()
        {
            if (PInput.Keyboard.IsKeyDown(Keys.Space))
            {
                if (PWorld.States.IsPaused) PWorld.Resume();
                else PWorld.Pause();
            }
        }
        private static void KeyboardResetWorld()
        {
            if (PInput.Keyboard.IsKeyDown(Keys.R))
            {
                PWorld.Clear();
            }
        }
        private static void KeyboardQuit()
        {
            if (PInput.Keyboard.IsKeyDown(Keys.Escape))
            {
                PEngine.Stop();
            }
        }
        private static void KeyboardSelectElement()
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
        #endregion

        #region Mouse
        private static void MousePlaceArea()
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
        private static void MousePlaceElements()
        {
            if (elementSelected == null)
                return;

            Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInput.Mouse.Position.ToVector2());
            Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

            if (!PWorld.InsideTheWorldDimensions(worldPos)) return;

            PWorld.TryGetElement(worldPos, out elementOver);

            // Place
            if (PInput.Mouse.LeftButton == ButtonState.Pressed)
            {
                if (size == 0)
                {
                    PWorld.TryInstantiate(worldPos, elementSelected.Id);
                    return;
                }

                for (int x = -(int)size; x < size; x++)
                {
                    for (int y = -(int)size; y < size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!PWorld.InsideTheWorldDimensions(lpos))
                            continue;

                        PWorld.TryInstantiate(lpos, elementSelected.Id);
                    }
                }
            }
        }
        private static void MouseEraseElements()
        {
            Vector2 screenPos = PWorldCamera.Camera.ScreenToWorld(PInput.Mouse.Position.ToVector2());
            Vector2 worldPos = new Vector2(screenPos.X, screenPos.Y) / PWorld.Scale;

            if (!PWorld.InsideTheWorldDimensions(worldPos)) return;

            if (PInput.Mouse.RightButton == ButtonState.Pressed)
            {
                if (size == 0)
                {
                    PWorld.TryDestroy(worldPos);
                    return;
                }

                for (int x = -(int)size; x < size; x++)
                {
                    for (int y = -(int)size; y < size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + worldPos;
                        if (!PWorld.InsideTheWorldDimensions(lpos))
                            continue;

                        PWorld.TryDestroy(lpos);
                    }
                }
            }
        }
        #endregion
    }
}