using PixelDust.Core;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace PixelDust
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
            [Keys.D1] = PElements.GetElementByType<Dirt>(),
            [Keys.D2] = PElements.GetElementByType<Grass>(),
            [Keys.D3] = PElements.GetElementByType<Stone>(),
            [Keys.D4] = PElements.GetElementByType<Sand>(),
            [Keys.D5] = PElements.GetElementByType<Water>(),
            [Keys.D6] = PElements.GetElementByType<Lava>(),
            [Keys.D7] = PElements.GetElementByType<Acid>(),
            [Keys.D8] = PElements.GetElementByType<Wall>(),
        };

        private static World _world = null;

        protected override void OnUpdate()
        {
            _world = PSceneManager.GetCurrentScene<WorldScene>().World;
            if (_world == null)
                return;

            Reset();
            MouseUpdate();
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
                _world.Clear();
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
        }

        private static void PlaceElement()
        {
            if (elementSelected == null)
                return;

            Vector2 mousePos = PInput.MouseState.Position.ToVector2() / World.GridScale;

            if (!PSceneManager.GetCurrentScene<WorldScene>().World.InsideTheWorldDimensions(mousePos))
                return;

            _world.TryGetElement(mousePos, out elementOver);

            // Place
            if (PInput.MouseState.LeftButton == ButtonState.Pressed)
            {
                if (size == 0)
                {
                    _world.TryInstantiate(elementSelected.Id, mousePos);
                    return;
                }

                for (int x = -(int)size; x < size; x++)
                {
                    for (int y = -(int)size; y < size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + mousePos;
                        if (!_world.InsideTheWorldDimensions(lpos))
                            continue;

                        _world.TryInstantiate(elementSelected.Id, lpos);
                    }
                }
            }

            // Earase
            if (PInput.MouseState.RightButton == ButtonState.Pressed)
            {
                if (size == 0)
                {
                    _world.TryDestroy(mousePos);
                    return;
                }

                for (int x = -(int)size; x < size; x++)
                {
                    for (int y = -(int)size; y < size; y++)
                    {
                        Vector2 lpos = new Vector2(x, y) + mousePos;
                        if (!_world.InsideTheWorldDimensions(lpos))
                            continue;

                        _world.TryDestroy(lpos);
                    }
                }
            }
        }
    }
}
