using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;
using StardustSandbox.GameContent.Elements.Utilities;
using StardustSandbox.GameContent.Enums.Elements.Utilities;

using System.Collections.Generic;

namespace StardustSandbox.GameContent.Elements.Solids.Immovables
{
    internal sealed class SHeater : SImmovableSolid
    {
        internal SHeater(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.DarkRed;
            this.texture = gameInstance.AssetDatabase.GetTexture("texture_element_37");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(gameInstance));
            this.enableNeighborsAction = true;
            this.enableFlammability = true;
            this.defaultTemperature = 0;
            this.defaultDensity = 1500;
            this.defaultExplosionResistance = 2.5f;
        }

        protected override void OnNeighbors(IEnumerable<SWorldSlot> neighbors)
        {
            STemperatureUtilities.ModifyNeighborsTemperature(this.Context, neighbors, STemperatureModifierMode.Warming);
        }
    }
}
