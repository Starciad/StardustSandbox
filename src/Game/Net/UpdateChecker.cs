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

using StardustSandbox.Constants;

using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace StardustSandbox.Net
{
    internal static class UpdateChecker
    {
        internal static bool IsUpdateAvailable => isUpdateAvailable;
        internal static Version LatestVersion => latestVersion;

        private static volatile bool isUpdateAvailable;
        private static volatile Version latestVersion;

        internal static void StartCheck()
        {
            _ = CheckForUpdateAsync();
        }

        private static async Task CheckForUpdateAsync()
        {
            await Task.Yield(); // ensure asynchronous continuation
            HttpClientProvider.GetJson(NetConstants.LATEST_ENDPOINT, OnJsonReceived, _ => ResetState());
        }

        private static void OnJsonReceived(JsonDocument document)
        {
            try
            {
                if (!document.RootElement.TryGetProperty("latest", out JsonElement versionElement) || versionElement.ValueKind != JsonValueKind.String)
                {
                    ResetState();
                    return;
                }

                string versionString = versionElement.GetString();

                if (!Version.TryParse(versionString, out Version remoteVersion))
                {
                    ResetState();
                    return;
                }

                latestVersion = remoteVersion;
                isUpdateAvailable = remoteVersion > GameConstants.VERSION;
            }
            finally
            {
                document.Dispose();
            }
        }

        private static void ResetState()
        {
            isUpdateAvailable = false;
            latestVersion = null;
        }
    }
}

