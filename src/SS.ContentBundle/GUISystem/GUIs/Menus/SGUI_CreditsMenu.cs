using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_CreditsMenu : SGUISystem
    {
        private readonly Texture2D gameTitleTexture;
        private readonly Texture2D starciadCharacterTexture;

        private readonly Song creditsMenuSong;

        public SGUI_CreditsMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.gameTitleTexture = gameInstance.AssetDatabase.GetTexture("game_title_1");
            this.starciadCharacterTexture = gameInstance.AssetDatabase.GetTexture("character_1");
            this.creditsMenuSong = this.SGameInstance.AssetDatabase.GetSong("song_2");
        }

        protected override void OnLoad()
        {
            this.SGameInstance.BackgroundManager.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("credits"));

            SSongEngine.Play(this.creditsMenuSong);
        }
    }
}
