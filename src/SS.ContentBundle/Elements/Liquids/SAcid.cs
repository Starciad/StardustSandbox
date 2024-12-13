using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Enums.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.ContentBundle.Elements.Liquids
{
    internal class SAcid : SLiquid
    {
        internal SAcid(ISGame gameInstance) : base(gameInstance)
        {
            this.identifier = (uint)SElementId.Acid;
            this.referenceColor = new(059, 167, 005, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_11");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 10;
            this.enableNeighborsAction = true;
        }

        protected override void OnNeighbors(ReadOnlySpan<ISWorldSlot> neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                ISWorldSlot slot = neighbors[i];

                switch (slot.Element)
                {
                    case SAcid:
                    case SWall:
                        continue;
                }

                this.Context.DestroyElement();
                this.Context.DestroyElement(slot.Position);
            }
        }
    }
}