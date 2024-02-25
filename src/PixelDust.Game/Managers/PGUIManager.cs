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
        private readonly PGUIEvents _guiEvents = new(inputManager);
        private readonly PGUILayoutPool _layoutPool = new();

        private List<PGUISystem> _registeredGUIs = [];

        protected override void OnAwake()
        {
            this._registeredGUIs = [.. this._registeredGUIs.OrderBy(x => x.ZIndex)];
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (PGUISystem guiSystem in this._registeredGUIs)
            {
                if (guiSystem.IsActive)
                {
                    guiSystem.Update(gameTime);
                }
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (PGUISystem guiSystem in this._registeredGUIs)
            {
                if (guiSystem.IsActive || guiSystem.IsShowing)
                {
                    guiSystem.Draw(gameTime, spriteBatch);
                }
            }
        }

        public void RegisterGUISystem(PGUISystem guiSystem)
        {
            guiSystem.Configure(this._guiEvents, this._layoutPool);
            guiSystem.Initialize(this.Game);

            this._registeredGUIs.Add(guiSystem);
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
            PGUISystem target = this._registeredGUIs.Find(x => x.Name == name);
            guiSystem = target;

            return target != null;
        }
    }
}
