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

namespace StardustSandbox.Core.Constants
{
    internal static class NetConstants
    {
        internal const string ITCH_URL = "https://starciad.itch.io/stardust-sandbox";

#if SS_WINDOWS
        internal const string LATEST_ENDPOINT = "https://itch.io/api/1/x/wharf/latest?target=starciad/stardust-sandbox&channel_name=windows-x64";
#elif SS_LINUX
        internal const string LATEST_ENDPOINT = "https://itch.io/api/1/x/wharf/latest?target=starciad/stardust-sandbox&channel_name=linux-x64";
#else
        // default to windows channel when no platform symbol provided
        internal const string LATEST_ENDPOINT = "https://itch.io/api/1/x/wharf/latest?target=starciad/stardust-sandbox&channel_name=windows-x64";
#endif
    }
}
