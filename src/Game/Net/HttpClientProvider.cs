/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace StardustSandbox.Net
{
    internal static class HttpClientProvider
    {
        internal static HttpClient Client => client;

        private static SocketsHttpHandler socketsHandler;
        private static HttpClient client;
        private static CancellationTokenSource cts;
        private static bool isInitialized;

        internal static void Initialize()
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(HttpClientProvider)} is already initialized.");
            }

            cts = new();

            socketsHandler = new()
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = 10,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
                UseCookies = false,
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 3
            };

            client = new(socketsHandler, disposeHandler: false)
            {
                Timeout = TimeSpan.FromSeconds(30),
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"{GameConstants.TITLE}/{GameConstants.GetTitleAndVersionString()}/{GameConstants.VERSION}");
            client.DefaultRequestHeaders.Add("X-Platform", RuntimeInformation.OSDescription.Trim());
            client.DefaultRequestHeaders.Add("X-Arch", RuntimeInformation.ProcessArchitecture.ToString());

            isInitialized = true;
        }

        internal static void Dispose()
        {
            if (!isInitialized)
            {
                return;
            }

            try
            {
                cts?.Cancel();
            }
            catch { /* ignore */ }

            client?.Dispose();
            socketsHandler?.Dispose();
            cts?.Dispose();

            client = null;
            socketsHandler = null;
            cts = null;
            isInitialized = false;
        }

        internal static void GetJson(string uri, Action<JsonDocument> onSuccess = null, Action<Exception> onError = null)
        {
            _ = GetJsonInternalAsync(uri, onSuccess, onError);
        }

        private static async Task GetJsonInternalAsync(string uri, Action<JsonDocument> onSuccess, Action<Exception> onError)
        {
            try
            {
                using HttpResponseMessage resp = await Client.GetAsync(uri, cts?.Token ?? CancellationToken.None).ConfigureAwait(false);
                _ = resp.EnsureSuccessStatusCode();
                using Stream stream = await resp.Content.ReadAsStreamAsync(cts?.Token ?? CancellationToken.None).ConfigureAwait(false);
                JsonDocument doc = await JsonDocument.ParseAsync(stream, cancellationToken: cts?.Token ?? CancellationToken.None).ConfigureAwait(false);
                onSuccess?.Invoke(doc);
                doc.Dispose();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }
    }
}

