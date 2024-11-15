using Microsoft.Xna.Framework.Content;

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces;

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
        }

        protected virtual void OnRegisterAssets(ISGame game, ContentManager contentManager, SAssetDatabase assetDatabase) { return; }
        protected virtual void OnRegisterElements(ISGame game, SElementDatabase elementDatabase) { return; }
        protected virtual void OnRegisterGUIs(ISGame game, SGUIDatabase guiDatabase) { return; }
        protected virtual void OnRegisterItems(ISGame game, SItemDatabase itemDatabase) { return; }
        protected virtual void OnRegisterBackgrounds(ISGame game, SBackgroundDatabase backgroundDatabase) { return; }
    }
}
