/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using StardustSandbox.Core.Catalog;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class ItemSearchUI
    {
        private sealed class SearchIndexEntry
        {
            internal Item Item { get; }
            internal string NormalizedName { get; }
            internal string NormalizedDescription { get; }

            internal SearchIndexEntry(Item item)
            {
                this.Item = item;
                this.NormalizedName = Normalize(item.Name ?? string.Empty);
                this.NormalizedDescription = Normalize(item.Description ?? string.Empty);
            }
        }

        private readonly struct SearchMatch
        {
            internal readonly SearchIndexEntry Entry { get; }
            internal readonly int Score { get; }

            internal SearchMatch(SearchIndexEntry entry, int score)
            {
                this.Entry = entry;
                this.Score = score;
            }
        }
    }
}
