using PixelDust.Core;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using PixelDust.Core.Scenes;
using PixelDust.Core.Managers;
using PixelDust.Core.Engine;
using PixelDust.Core.Elements;
using PixelDust.Core.Worlding;

using PixelDust.Game.Scenes;
using PixelDust.Game.Elements.Liquid;
using PixelDust.Game.Elements.Solid.Immovable;
using PixelDust.Game.Elements.Solid.Movable;

namespace PixelDust.Game.Managers
{
    [PManagerRegister]
    internal sealed class InputManager : PManager
    {
        public static StringBuilder DebugString => debugString;

        private static PElement elementSelected;
        private static PElement elementOver;

        private static float size = 1;

        private static StringBuilder debugString;

        private static readonly Dictionary<Keys, PElement> elementsKeys = new()
        {
            [Keys.D1] = PElementManager.GetElementByType<Dirt>(),
            [Keys.D2] = PElementManager.GetElementByType<Grass>(),
            [Keys.D3] = PElementManager.GetElementByType<Stone>(),
            [Keys.D4] = PElementManager.GetElementByType<Sand>(),
            [Keys.D5] = PElementManager.GetElementByType<Water>(),
            [Keys.D6] = PElementManager.GetElementByType<Lava>(),
            [Keys.D7] = PElementManager.GetElementByType<Acid>(),
            [Keys.D8] = PElementManager.GetElementByType<Wall>(),
        };

        protected override void OnUpdate()
        {
            Reset();
            MouseUpdate();
            KeyboardUpdate();
            PlaceElement();

            debugString = new();
            debugString.AppendLine($"Selected: {elementSelected?.Name}");
            debugString.AppendLine($"Mouse: {elementOver?.Name}");
            debugString.AppendLine($"Size: {(int)size}");
            base.OnUpdate();
        }

        private static void Reset()
        {
            if (PInput.KeyboardState.IsKeyDown(Keys.R))
            {
                PWorld.Clear();
            }
        }

        private static void MouseUpdate()
        {
            // Select element
            foreach (var item in elementsKeys)
            {
                if (PInput.KeyboardState.IsKeyDown(item.Key))
                {
                    elementSelected = item.Value;
                    break;
                }
            }
        }

        private void KeyboardUpdate()
        {
            // Scroll size
            if (PInput.KeyboardState.IsKeyDown(Keys.Add))
            {
                size += 0.2f;
            }

            if (PInput.KeyboardState.IsKeyDown(Keys.Subtract))
            {
                size -= 0.2f;
            }

            size = Math.Clamp(size, 0, 10);

            // Pause
            if (PInput.KeyboardState.IsKeyDown(Keys.Space))
            {
                if (PWorld.States.IsPaused) PWorld.Resume();
                else PWorld.Pause();
            }

            // Quit
            if (PInput.KeyboardState.IsKeyDown(Keys.Escape))
            {
                PEngine.Stop();
            }
        }

        private static void PlaceElement()
        {
            if (elementSelected == null)
                return;

            Vector2 mousePos = PInput.MouseState.Position.ToVector2() / PWorld.Scale;

            if (!PWorld.InsideTheWorldDimensions(mousePos))
                return;

            PWorld.TryGetElement(mousePos, out elementOver);

            // Place
            if (PInput.MouseState.LeftButton == ButtonState.Pressed)
            {
                if (size == 0)
                {
                    PWorld.TryInstantiate(mousePos, elementSelected.Id);
                    return;
                }

                for (int x = -(int)size; x < size; x++)
                {
                    for (int y = -(int)size; y < size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + mousePos;
                        if (!PWorld.InsideTheWorldDimensions(lpos))
                            continue;

                        PWorld.TryInstantiate(lpos, elementSelected.Id);
                    }
                }
            }

            // Earase
            if (PInput.MouseState.RightButton == ButtonState.Pressed)
            {
                if (size == 0)
                {
                    PWorld.TryDestroy(mousePos);
                    return;
                }

                for (int x = -(int)size; x < size; x++)
                {
                    for (int y = -(int)size; y < size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + mousePos;
                        if (!PWorld.InsideTheWorldDimensions(lpos))
                            continue;

                        PWorld.TryDestroy(lpos);
                    }
                }
            }
        }
    }
}
