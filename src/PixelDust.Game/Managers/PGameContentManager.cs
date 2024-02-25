using PixelDust.Game.Attributes.Elements;
using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.GUI;
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

        protected override void OnAwake()
        {
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

            this._guiManager.RegisterGUISystem((PGUISystem)Activator.CreateInstance(type));
        }

        private void RegisterElement(Type type)
        {

        }

        private void RegisterItem(Type type)
        {

        }
    }
}
