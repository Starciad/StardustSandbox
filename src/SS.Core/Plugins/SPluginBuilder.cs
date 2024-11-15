using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using StardustSandbox.Game.Databases;
using StardustSandbox.Game.Interfaces;

namespace StardustSandbox.Core
{
    public abstract partial class SPluginBuilder
    {
        internal void Initialize(ISGame game, ContentManager contentManager)
        {
            OnRegisterAssets(game, contentManager, game.AssetDatabase);
            OnRegisterElements(game, game.ElementDatabase);
            OnRegisterGUIs(game, game.GUIDatabase);
            OnRegisterItems(game, game.ItemDatabase);
            OnRegisterBackgrounds(game, game.BackgroundDatabase);
        }

        protected virtual void OnRegisterAssets(ISGame game, ContentManager contentManager, SAssetDatabase assetDatabase) { return; }
        protected virtual void OnRegisterElements(ISGame game, SElementDatabase elementDatabase) { return; }
        protected virtual void OnRegisterGUIs(ISGame game, SGUIDatabase guiDatabase) { return; }
        protected virtual void OnRegisterItems(ISGame game, SItemDatabase itemDatabase) { return; }
        protected virtual void OnRegisterBackgrounds(ISGame game, SBackgroundDatabase backgroundDatabase) { return; }
    }
}
