using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Enums.GameInput.Pen;
using StardustSandbox.Core.Enums.Simulation;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.Main
{
    internal partial class SGUI_MainMenu
    {
        protected override void OnOpened()
        {
            this.SGameInstance.AmbientManager.BackgroundHandler.SetBackground(this.SGameInstance.BackgroundDatabase.GetBackgroundById("main_menu"));
            this.SGameInstance.AmbientManager.CloudHandler.IsActive = true;
            this.SGameInstance.AmbientManager.CelestialBodyHandler.IsActive = true;
            this.SGameInstance.AmbientManager.SkyHandler.IsActive = true;

            ResetElementPositions();

            this.SGameInstance.GameInputController.Pen.Tool = SPenTool.Visualization;
            this.SGameInstance.GameInputController.Disable();

            LoadAnimationValues();
            LoadMainMenuWorld();
            LoadMagicCursor();

            this.SGameInstance.GameManager.SetSimulationSpeed(SSimulationSpeed.Normal);
            this.SGameInstance.GameManager.GameState.IsPaused = false;
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = false;
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;

            this.SGameInstance.World.Time.SecondsPerFrames = 60f;
            this.SGameInstance.World.Time.IsFrozen = false;

            if (SSongEngine.State != MediaState.Playing || SSongEngine.CurrentSong != this.mainMenuSong)
            {
                SSongEngine.Play(this.mainMenuSong);
            }
        }

        protected override void OnClosed()
        {
            this.SGameInstance.World.Clear();
            this.SGameInstance.World.IsVisible = false;
        }
    }
}
