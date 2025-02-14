using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.GameContent.Localization.GUIs;
using StardustSandbox.GameContent.Localization.Statements;

namespace StardustSandbox.GameContent.GUISystem.GUIs.Hud.Complements.Information
{
    internal sealed partial class SGUI_Information
    {
        protected override void OnOpened()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = true;

            SSize2 worldSize = this.world.Infos.Size;

            uint limitOfElementsOnTheMap = (uint)(worldSize.Width * worldSize.Height * 2);
            uint limitOfElementsPerLayer = (uint)(worldSize.Width * worldSize.Height);

            this.infoElements[0].SetTextualContent(string.Concat(SLocalization_Statements.Size, ": ", worldSize));
            this.infoElements[1].SetTextualContent(string.Concat(SLocalization_Statements.Time, ": ", this.world.Time.CurrentTime));
            this.infoElements[2].SetTextualContent(string.Concat(SLocalization_Statements.Elements, ": ", this.world.GetTotalElementCount(), '/', limitOfElementsOnTheMap));
            this.infoElements[3].SetTextualContent(string.Concat(SLocalization_GUIs.HUD_Complements_Information_Field_ForegroundElements, ": ", this.world.GetTotalForegroundElementCount(), '/', limitOfElementsPerLayer));
            this.infoElements[4].SetTextualContent(string.Concat(SLocalization_GUIs.HUD_Complements_Information_Field_BackgroundElements, ": ", this.world.GetTotalBackgroundElementCount(), '/', limitOfElementsPerLayer));
        }

        protected override void OnClosed()
        {
            this.SGameInstance.GameManager.GameState.IsCriticalMenuOpen = false;
        }
    }
}
