using Microsoft.Xna.Framework;

using System;

using PixelDust.Core.Worlding;
using System.Collections.Generic;

namespace PixelDust.Core.Elements
{
    public sealed partial class PElementContext
    {
        public bool TryInstantiate<T>(Vector2 pos) where T : PElement
        {
            return PWorld.TryInstantiate<T>(pos);
        }

        public bool TrySetPosition(Vector2 pos)
        {
            if (PWorld.TryUpdatePosition(_position, pos))
            {
                TryGetSlot(pos, out _slot);
                Update(_slot, _position);

                return true;
            }

            return false;
        }

        public bool TrySwitchPosition(Vector2 oldPos, Vector2 newPos)
        {
            if (PWorld.TrySwitchPosition(oldPos, newPos))
            {
                TryGetSlot(newPos, out _slot);
                Update(_slot, newPos);

                return true;
            }

            return false;
        }

        public bool TryDestroy(Vector2 pos)
        {
            return PWorld.TryDestroy(pos);
        }

        public bool TryGetElement(Vector2 pos, out PElement value)
        {
            return PWorld.TryGetElement(pos, out value);
        }

        public bool TryGetSlot(Vector2 pos, out PWorldSlot value)
        {
            return PWorld.TryGetSlot(pos, out value);
        }

        public bool TryReplace<T>(Vector2 pos) where T : PElement
        {
            if (!TryDestroy(pos)) return false;
            if (!TryInstantiate<T>(pos)) return false;

            return true;
        }

        public bool IsEmpty(Vector2 pos)
        {
            return PWorld.IsEmpty(pos);
        }

        public bool TryGetNeighbors(Vector2 pos, out (Vector2, PWorldSlot)[] neighbors)
        {
            List<(Vector2, PWorldSlot)> slotsFound = new();
            neighbors = Array.Empty<(Vector2, PWorldSlot)>();

            if (!PWorld.InsideTheWorldDimensions(pos))
                return false;

            Vector2[] neighborsPositions = new Vector2[]
            {
                // Top
                new(pos.X, pos.Y - 1),
                new(pos.X + 1, pos.Y - 1),
                new(pos.X - 1, pos.Y - 1),

                // Center
                new(pos.X + 1, pos.Y),
                new(pos.X - 1, pos.Y),

                // Down
                new(pos.X, pos.Y + 1),
                new(pos.X + 1, pos.Y + 1),
                new(pos.X - 1, pos.Y + 1),
            };

            foreach (Vector2 neighborPos in neighborsPositions)
            {
                if (TryGetSlot(neighborPos, out PWorldSlot value))
                    slotsFound.Add((neighborPos, value));
            }

            if (slotsFound.Count > 0)
            {
                neighbors = slotsFound.ToArray();
                return true;
            }

            return false;
        }
    }
}