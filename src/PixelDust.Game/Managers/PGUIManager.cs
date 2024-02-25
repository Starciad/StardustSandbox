using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Databases;
using PixelDust.Game.GUI;
using PixelDust.Game.GUI.Events;
using PixelDust.Game.Objects;

namespace PixelDust.Game.Managers
{
    public sealed class PGUIManager(PGUIDatabase guiDatabase, PInputManager inputManager) : PGameObject
    {
        public PGUIEvents GUIEvents => this._guiEvents;
        public PGUILayoutPool GUILayoutPool => this._guiLayoutPool;

        private readonly PGUIEvents _guiEvents = new(inputManager);
        private readonly PGUILayoutPool _guiLayoutPool = new();

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (PGUISystem guiSystem in guiDatabase.RegisteredGUIs)
            {
                if (guiSystem.IsActive)
                {
                    guiSystem.Update(gameTime);
                }
            }
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (PGUISystem guiSystem in guiDatabase.RegisteredGUIs)
            {
                if (guiSystem.IsActive || guiSystem.IsShowing)
                {
                    guiSystem.Draw(gameTime, spriteBatch);
                }
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
            PGUISystem target = guiDatabase.Find(name);
            guiSystem = target;

            return target != null;
        }
    }
}
