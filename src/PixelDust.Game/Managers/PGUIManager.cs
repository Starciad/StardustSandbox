using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelDust.Game.Managers
{
    public sealed class PGUIManager : PGameObject
    {
        private readonly List<Type> _guiTypes = [];
        private PGUISystem[] _guiSystems = [];

        protected override void OnAwake()
        {
            this._guiSystems = new PGUISystem[this._guiTypes.Count];

            for (int i = 0; i < this._guiSystems.Length; i++)
            {
                Type type = this._guiTypes[i];

                PGUISystem tempGUISystem = (PGUISystem)Activator.CreateInstance(type);
                tempGUISystem.Initialize(this.Game);

                this._guiSystems[i] = tempGUISystem;
            }

            this._guiSystems = [.. this._guiSystems.OrderBy(x => x.ZIndex)];
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = 0; i < this._guiSystems.Length; i++)
            {
                this._guiSystems[i].Update(gameTime);
            }
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this._guiSystems.Length; i++)
            {
                this._guiSystems[i].Draw(gameTime, spriteBatch);
            }
        }

        public void RegisterGUISystem<T>() where T : PGUISystem
        {
            RegisterGUISystem(typeof(T));
        }
        public void RegisterGUISystem(Type type)
        {
            this._guiTypes.Add(type);
        }

        public void ActivateGUI<T>() where T : PGUISystem
        {
            ActivateGUI(typeof(T));
        }
        public void ActivateGUI(Type type)
        {
            if (TryGetGUIByType(type, out PGUISystem guiSystem))
            {
                guiSystem.Activate();
            }
        }

        public void DisableGUI<T>() where T : PGUISystem
        {
            DisableGUI(typeof(T));
        }
        public void DisableGUI(Type type)
        {
            if (TryGetGUIByType(type, out PGUISystem guiSystem))
            {
                guiSystem.Disable();
            }
        }

        public void ShowGUI<T>() where T : PGUISystem
        {
            ShowGUI(typeof(T));
        }
        public void ShowGUI(Type type)
        {
            if (TryGetGUIByType(type, out PGUISystem guiSystem))
            {
                guiSystem.Show();
            }
        }

        public void CloseGUI<T>() where T : PGUISystem
        {
            CloseGUI(typeof(T));
        }
        public void CloseGUI(Type type)
        {
            if (TryGetGUIByType(type, out PGUISystem guiSystem))
            {
                guiSystem.Close();
            }
        }

        public PGUISystem GetGUIByType<T>() where T : PGUISystem
        {
            return GetGUIByType(typeof(T));
        }
        public PGUISystem GetGUIByType(Type type)
        {
            _ = TryGetGUIByType(type, out PGUISystem guiSystem);
            return guiSystem;
        }

        public bool TryGetGUIByType<T>(out PGUISystem guiSystem) where T : PGUISystem
        {
            return TryGetGUIByType(typeof(T), out guiSystem);
        }
        public bool TryGetGUIByType(Type type, out PGUISystem guiSystem)
        {
            PGUISystem target = Array.Find(this._guiSystems, x => x.GetType() == type);
            guiSystem = target;

            return target != null;
        }
    }
}
