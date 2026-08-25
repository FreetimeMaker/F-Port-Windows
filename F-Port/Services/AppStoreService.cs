using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using F_Port.Models;

namespace F_Port.Services
{
    public class AppStoreService
    {
        private readonly HttpClient _httpClient;
        private const string DefaultJsonUrl = "https://github.com/FreetimeMaker/F-Port-Windows/blob/main/sample_apps.json";

        public AppStoreService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<List<AppStoreItem>> GetAppsAsync(string? jsonUrl = null)
        {
            try
            {
                var url = jsonUrl ?? DefaultJsonUrl;
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();
                var apps = JsonSerializer.Deserialize<List<AppStoreItem>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return apps ?? new List<AppStoreItem>();
            }
            catch (Exception ex)
            {
                // Log error and return empty list
                System.Diagnostics.Debug.WriteLine($"Error fetching apps: {ex.Message}");
                return new List<AppStoreItem>();
            }
        }

        public async Task<byte[]> DownloadAppAsync(string downloadUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(downloadUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error downloading app: {ex.Message}");
                throw;
            }
        }
    }
}