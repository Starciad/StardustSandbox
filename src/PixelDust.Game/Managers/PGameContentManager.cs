using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.GUI;
using PixelDust.Game.Databases;
using PixelDust.Game.Elements;
using PixelDust.Game.GUI;
using PixelDust.Game.Objects;

using System;
using System.Linq;
using System.Reflection;

namespace PixelDust.Game.Managers
{
    public sealed class PGameContentManager(Assembly assembly) : PGameObject
    {
        private PGUIManager _guiManager;
        private PElementDatabase _elementDatabase;

        protected override void OnAwake()
        {
            this._guiManager = this.Game.GUIManager;
            this._elementDatabase = this.Game.ElementDatabase;
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

            this._guiManager.RegisterGUISystem((PGUISystem)Activator.CreateInstance(type));
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

        }
    }
}
