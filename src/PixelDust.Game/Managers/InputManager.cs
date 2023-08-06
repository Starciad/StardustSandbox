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
using PixelDust.Core.World;

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

        private static PWorld _world = null;

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

            Vector2 mousePos = PInput.MouseState.Position.ToVector2() / PWorld.GridScale;

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
