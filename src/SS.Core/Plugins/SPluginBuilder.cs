using Microsoft.Xna.Framework.Content;

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Plugins
{
    public abstract partial class SPluginBuilder
    {
        internal void Initialize(ISGame game, ContentManager contentManager)
        {
            OnRegisterAssets(game, contentManager, game.AssetDatabase);
            OnRegisterElements(game, game.ElementDatabase);
            OnRegisterItems(game, game.ItemDatabase);
            OnRegisterGUIs(game, game.GUIDatabase);
            OnRegisterBackgrounds(game, game.BackgroundDatabase);
            OnRegisterEntities(game, game.EntityDatabase);
        }

        protected virtual void OnRegisterAssets(ISGame game, ContentManager contentManager, ISAssetDatabase assetDatabase) { return; }
        protected virtual void OnRegisterElements(ISGame game, ISElementDatabase elementDatabase) { return; }
        protected virtual void OnRegisterGUIs(ISGame game, ISGUIDatabase guiDatabase) { return; }
        protected virtual void OnRegisterItems(ISGame game, ISItemDatabase itemDatabase) { return; }
        protected virtual void OnRegisterBackgrounds(ISGame game, ISBackgroundDatabase backgroundDatabase) { return; }
        protected virtual void OnRegisterEntities(ISGame game, ISEntityDatabase entityDatabase) { return; }
    }
}
