using System.Text.Json.Serialization;

#nullable disable

namespace Catalog.WebUI.ViewModels
{
    public class DtResponse<T>
    {
        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        [JsonPropertyName("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonPropertyName("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
