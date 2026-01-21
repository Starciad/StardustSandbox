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

using StardustSandbox.Core.Localization;

using System;

namespace StardustSandbox.Core.Constants
{
    internal static class LocalizationConstants
    {
        internal static GameCulture DEFAULT_GAME_CULTURE => gameCultures[0];
        internal static GameCulture[] AVAILABLE_GAME_CULTURES => gameCultures;

        private static readonly GameCulture[] gameCultures =
        [
            new("en", "US"),
            new("pt", "BR"),
            new("es", "ES"),
            new("fr", "FR"),
            new("de", "DE"),
        ];

        internal static GameCulture GetGameCultureFromNativeName(string nativeName)
        {
            return Array.Find(gameCultures, x => x.CultureInfo.NativeName.Equals(nativeName, StringComparison.OrdinalIgnoreCase)) ?? DEFAULT_GAME_CULTURE;
        }

        internal static GameCulture GetGameCulture(string name)
        {
            return Array.Find(gameCultures, x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}

