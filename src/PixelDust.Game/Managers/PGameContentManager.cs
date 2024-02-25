using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.GUI;
using PixelDust.Game.Attributes.Items;
using PixelDust.Game.Databases;
using PixelDust.Game.Elements;
using PixelDust.Game.GUI;
using PixelDust.Game.Items;
using PixelDust.Game.Objects;

using System;
using System.Linq;
using System.Reflection;

namespace PixelDust.Game.Managers
{
    public sealed class PGameContentManager(Assembly assembly) : PGameObject
    {
        // Databases
        private PGUIDatabase _guiDatabase;
        private PElementDatabase _elementDatabase;
        private PItemDatabase _itemDatabase;

        // Managers
        private PGUIManager _guiManager;

        protected override void OnAwake()
        {
            this._guiDatabase = this.Game.GUIDatabase;
            this._elementDatabase = this.Game.ElementDatabase;
            this._itemDatabase = this.Game.ItemDatabase;

            this._guiManager = this.Game.GUIManager;
        }

        public void RegisterAllGameContent()
        {
            foreach (Type gameContentType in assembly.GetTypes().Where(x => x.GetCustomAttribute(typeof(PGameContentAttribute), true) != null))
            {
                RegisterGUI(gameContentType);
                RegisterElement(gameContentType);
                RegisterItem(gameContentType);
            }
        }

        private void RegisterGUI(Type type)
        {
            if (type.GetCustomAttribute<PGUIRegisterAttribute>() == null)
            {
                return;
            }

            this._guiDatabase.RegisterGUISystem((PGUISystem)Activator.CreateInstance(type), this._guiManager.GUIEvents, this._guiManager.GUILayoutPool);
        }

        private void RegisterElement(Type type)
        {
            if (type.GetCustomAttribute<PElementRegisterAttribute>() == null)
            {
                return;
            }

            this._elementDatabase.RegisterElement((PElement)Activator.CreateInstance(type));
        }

        private void RegisterItem(Type type)
        {
            if (type.GetCustomAttribute<PItemRegisterAttribute>() == null)
            {
                return;
            }

            this._itemDatabase.RegisterItem((PItem)Activator.CreateInstance(type));
        }
    }
}
