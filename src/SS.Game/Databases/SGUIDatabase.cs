using StardustSandbox.Game.GameContent.GUI.Content.Hud;
using StardustSandbox.Game.GameContent.GUI.Content.Menus.ItemExplorer;
using StardustSandbox.Game.GUI;
using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Game.Databases
{
    public sealed class SGUIDatabase(SGame gameInstance) : SGameObject(gameInstance)
    {
        public IReadOnlyList<SGUISystem> RegisteredGUIs => this._registeredGUIs;

        private readonly List<SGUISystem> _registeredGUIs = [];

        protected override void OnInitialize()
        {
            RegisterGUISystem(new SGUI_HUD(this.SGameInstance, this.SGameInstance.GUIManager.GUIEvents));
            RegisterGUISystem(new SGUI_ItemExplorer(this.SGameInstance, this.SGameInstance.GUIManager.GUIEvents));

            this._registeredGUIs.ForEach(x => x.Initialize());
        }

        private void RegisterGUISystem(SGUISystem guiSystem)
        {
            this._registeredGUIs.Add(guiSystem);
        }

        public SGUISystem Find(string name)
        {
            return this._registeredGUIs.Find(x => x.Name == name);
        }
    }
}
