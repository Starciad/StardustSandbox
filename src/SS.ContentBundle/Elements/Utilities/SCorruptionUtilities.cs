using Microsoft.Xna.Framework;

using StardustSandbox.ContentBundle.Elements.Gases;
using StardustSandbox.ContentBundle.Elements.Liquids;
using StardustSandbox.ContentBundle.Elements.Solids.Immovables;
using StardustSandbox.ContentBundle.Elements.Solids.Movables;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Templates.Liquids;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Elements.Templates;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;

using System;
using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.Elements.Utilities
{
    internal static class SCorruptionUtilities
    {
        internal static bool CheckIfNeighboringElementsAreCorrupted(ReadOnlySpan<ISWorldSlot> neighbors, int length)
        {
            if (length == 0)
            {
                return true;
            }

            int corruptNeighboringElements = 0;

            for (int i = 0; i < length; i++)
            {
                if (neighbors[i].Element is ISCorruption)
                {
                    corruptNeighboringElements++;
                }
            }

            return corruptNeighboringElements == length;
        }

        internal static void InfectNeighboringElements(this ISElementContext context, ReadOnlySpan<ISWorldSlot> neighbors, int length)
        {
            if (length == 0)
            {
                return;
            }

            List<ISWorldSlot> targets = [];

            for (int i = 0; i < length; i++)
            {
                ISElement element = neighbors[i].Element;

                if (element is not ISCorruption &&
                    element is not SWall)
                {
                    targets.Add(neighbors[i]);
                }
            }

            if (targets.Count == 0)
            {
                return;
            }

            ISWorldSlot target = targets.Count == 0 ? targets[0] : targets[SRandomMath.Range(0, targets.Count)];

            switch (target.Element)
            {
                case SMovableSolid:
                    context.ReplaceElement<SMCorruption>(target.Position);
                    break;

                case SImmovableSolid:
                    context.ReplaceElement<SIMCorruption>(target.Position);
                    break;

                case SLiquid:
                    context.ReplaceElement<SLCorruption>(target.Position);
                    break;

                case SGas:
                    context.ReplaceElement<SGCorruption>(target.Position);
                    break;

                default:
                    context.ReplaceElement<SMCorruption>(target.Position);
                    break;
            }
        }
    }
}