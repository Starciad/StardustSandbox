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
