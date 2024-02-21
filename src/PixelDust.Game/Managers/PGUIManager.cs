using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.GUI;
using PixelDust.Game.GUI.Events;
using PixelDust.Game.Objects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelDust.Game.Managers
{
    public sealed class PGUIManager(PInputManager inputManager) : PGameObject
    {
        private readonly List<Type> _guiTypes = [];
        private PGUISystem[] _guiSystems = [];

        private readonly PGUIEvents _guiEvents = new(inputManager);

        protected override void OnAwake()
        {
            // Instantiate GUIs
            this._guiSystems = new PGUISystem[this._guiTypes.Count];

            for (int i = 0; i < this._guiSystems.Length; i++)
            {
                Type type = this._guiTypes[i];

                PGUISystem tempGUISystem = (PGUISystem)Activator.CreateInstance(type, this._guiEvents);
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

        public void ActivateGUI(string id)
        {
            if (TryGetGUIByName(id, out PGUISystem guiSystem))
            {
                guiSystem.Activate();
            }
        }

        public void DisableGUI(string id)
        {
            if (TryGetGUIByName(id, out PGUISystem guiSystem))
            {
                guiSystem.Disable();
            }
        }

        public void ShowGUI(string id)
        {
            if (TryGetGUIByName(id, out PGUISystem guiSystem))
            {
                guiSystem.Show();
            }
        }

        public void CloseGUI(string id)
        {
            if (TryGetGUIByName(id, out PGUISystem guiSystem))
            {
                guiSystem.Close();
            }
        }

        public PGUISystem GetGUIByName(string name)
        {
            _ = TryGetGUIByName(name, out PGUISystem guiSystem);
            return guiSystem;
        }

        public bool TryGetGUIByName(string name, out PGUISystem guiSystem)
        {
            PGUISystem target = Array.Find(this._guiSystems, x => x.Name == name);
            guiSystem = target;

            return target != null;
        }
    }
}
