using StardustSandbox.Game.Databases;
using StardustSandbox.Game.GameContent.Elements.Gases;
using StardustSandbox.Game.GameContent.Elements.Liquids;
using StardustSandbox.Game.GameContent.Elements.Solids.Immovables;
using StardustSandbox.Game.GameContent.Elements.Solids.Movables;
using StardustSandbox.Game.GameContent.GUI.Content.Hud;
using StardustSandbox.Game.GameContent.GUI.Content.Menus.ItemExplorer;
using StardustSandbox.Game.GameContent.Items.Elements.Gases;
using StardustSandbox.Game.GameContent.Items.Elements.Liquids;
using StardustSandbox.Game.GameContent.Items.Elements.Solids.Immovables;
using StardustSandbox.Game.GameContent.Items.Elements.Solids.Movables;
using StardustSandbox.Game.Objects;

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

        public SGameContentManager(SGame gameInstance) : base(gameInstance)
        {

        }

        protected override void OnInitialize()
        {
            this._guiDatabase = this.SGameInstance.GUIDatabase;
            this._elementDatabase = this.SGameInstance.ElementDatabase;
            this._itemDatabase = this.SGameInstance.ItemDatabase;

            this._guiManager = this.SGameInstance.GUIManager;
        }

        public void RegisterAllGameContent()
        {
            RegisterGUIs();
            RegisterElements();
            RegisterItems();
        }

        private void RegisterElements()
        {
            /* ID : 00 */
            this._elementDatabase.RegisterElement(new SDirt(this.SGameInstance));
            /* ID : 01 */
            this._elementDatabase.RegisterElement(new SMud(this.SGameInstance));
            /* ID : 02 */
            this._elementDatabase.RegisterElement(new SWater(this.SGameInstance));
            /* ID : 03 */
            this._elementDatabase.RegisterElement(new SStone(this.SGameInstance));
            /* ID : 04 */
            this._elementDatabase.RegisterElement(new SGrass(this.SGameInstance));
            /* ID : 05 */
            this._elementDatabase.RegisterElement(new SIce(this.SGameInstance));
            /* ID : 06 */
            this._elementDatabase.RegisterElement(new SSand(this.SGameInstance));
            /* ID : 07 */
            this._elementDatabase.RegisterElement(new SSnow(this.SGameInstance));
            /* ID : 08 */
            this._elementDatabase.RegisterElement(new SMCorruption(this.SGameInstance));
            /* ID : 09 */
            this._elementDatabase.RegisterElement(new SLava(this.SGameInstance));
            /* ID : 10 */
            this._elementDatabase.RegisterElement(new SAcid(this.SGameInstance));
            /* ID : 11 */
            this._elementDatabase.RegisterElement(new SGlass(this.SGameInstance));
            /* ID : 12 */
            this._elementDatabase.RegisterElement(new SMetal(this.SGameInstance));
            /* ID : 13 */
            this._elementDatabase.RegisterElement(new SWall(this.SGameInstance));
            /* ID : 14 */
            this._elementDatabase.RegisterElement(new SWood(this.SGameInstance));
            /* ID : 15 */
            this._elementDatabase.RegisterElement(new SGCorruption(this.SGameInstance));
            /* ID : 16 */
            this._elementDatabase.RegisterElement(new SLCorruption(this.SGameInstance));
            /* ID : 17 */
            this._elementDatabase.RegisterElement(new SIMCorruption(this.SGameInstance));
            /* ID : 18 */
            this._elementDatabase.RegisterElement(new SSteam(this.SGameInstance));
            /* ID : 19 */
            this._elementDatabase.RegisterElement(new SSmoke(this.SGameInstance));
        }

        private void RegisterGUIs()
        {
            this._guiDatabase.RegisterGUISystem(new SGUI_HUD(this.SGameInstance), this._guiManager.GUIEvents);
            this._guiDatabase.RegisterGUISystem(new SGUI_ItemExplorer(this.SGameInstance), this._guiManager.GUIEvents);
        }

        private void RegisterItems()
        {
            // Elements

            this._itemDatabase.RegisterItem(new SDirtItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SMudItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SWaterItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SStoneItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SGrassItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SIceItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SSandItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SSnowItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SMCorruptionItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SLavaItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SAcidItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SGlassItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SMetalItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SWallItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SWoodItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SGCorruptionItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SLCorruptionItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SIMCorruptionItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SSteamItem(this.SGameInstance));
            this._itemDatabase.RegisterItem(new SSmokeItem(this.SGameInstance));
        }
    }
}
