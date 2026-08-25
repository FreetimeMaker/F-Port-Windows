using System;
using System.Text.Json.Serialization;

namespace F_Port.Models
{
    public class AppStoreItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("developer")]
        public string Developer { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; } = string.Empty;

        [JsonPropertyName("downloadUrl")]
        public string DownloadUrl { get; set; } = string.Empty;

        [JsonPropertyName("installSize")]
        public string InstallSize { get; set; } = string.Empty;

        [JsonPropertyName("isInstalled")]
        public bool IsInstalled { get; set; }

        [JsonIgnore]
        public string InstallButtonText => IsInstalled ? "Open" : "Install";
    }
}