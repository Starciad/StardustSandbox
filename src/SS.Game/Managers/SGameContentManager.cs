using StardustSandbox.Game.Attributes.Elements;
using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.GUI;
using StardustSandbox.Game.Attributes.Items;
using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.GUI;
using StardustSandbox.Game.Objects;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace StardustSandbox.Game.Managers
{
    public sealed class SGameContentManager : SGameObject
    {
        // Databases
        private SGUIDatabase _guiDatabase;
        private SElementDatabase _elementDatabase;
        private SItemDatabase _itemDatabase;

        // Managers
        private SGUIManager _guiManager;
        private readonly Assembly assembly;

        public SGameContentManager(SGame gameInstance, Assembly assembly) : base(gameInstance)
        {
            this.assembly = assembly;
        }

        protected override void OnInitialize()
        {
            this._guiDatabase = this.SGameInstance.GUIDatabase;
            this._elementDatabase = this.SGameInstance.ElementDatabase;
            this._itemDatabase = this.SGameInstance.ItemDatabase;

            this._guiManager = this.SGameInstance.GUIManager;
        }

        [RequiresUnreferencedCode()]
        public void RegisterAllGameContent()
        {
            foreach (Type gameContentType in assembly.GetTypes().Where(x => x.GetCustomAttribute(typeof(SGameContentAttribute), true) != null))
            {
                RegisterGUI(gameContentType);
                RegisterElement(gameContentType);
                RegisterItem(gameContentType);
            }
        }

        private void RegisterGUI([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type)
        {
            if (type.GetCustomAttribute<SGUIRegisterAttribute>() == null)
            {
                return;
            }

            this._guiDatabase.RegisterGUISystem((SGUISystem)Activator.CreateInstance(type), this._guiManager.GUIEvents, this._guiManager.GUILayoutPool);
        }

        private void RegisterElement([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type)
        {
            if (type.GetCustomAttribute<SElementRegisterAttribute>() == null)
            {
                return;
            }

            this._elementDatabase.RegisterElement((SElement)Activator.CreateInstance(type));
        }

        private void RegisterItem(Type type)
        {
            SItemRegisterAttribute itemRegisterAttribute = type.GetCustomAttribute<SItemRegisterAttribute>();
            if (itemRegisterAttribute == null)
            {
                return;
            }

            this._itemDatabase.RegisterItem(itemRegisterAttribute.Item);
        }
    }
}
