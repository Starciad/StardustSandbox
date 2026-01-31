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

using System.Globalization;

namespace StardustSandbox.Core.Localization
{
    public sealed class GameCulture
    {
        public CultureInfo CultureInfo => this.cultureInfo;
        public string Name => string.Concat(this.Language, '-', this.Region);
        public string Language => this.language;
        public string Region => this.region;

        private readonly CultureInfo cultureInfo;
        private readonly string language;
        private readonly string region;

        public GameCulture(string language, string region)
        {
            this.language = language;
            this.region = region;
            this.cultureInfo = new(this.Name);
        }

        public override string ToString()
        {
            return this.cultureInfo.NativeName;
        }
    }
}
