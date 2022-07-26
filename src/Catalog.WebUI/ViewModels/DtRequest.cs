﻿using System.Text.Json.Serialization;

#nullable disable

namespace Catalog.WebUI.ViewModels
{
    public class DtRequest
    {
        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("search")]
        public DtSearch Search { get; set; }

        [JsonPropertyName("order")]
        public DtOrder[] Order { get; set; }

        [JsonPropertyName("columns")]
        public DtColumn[] Columns { get; set; }
    }

    public class DtOrder
    {
        [JsonPropertyName("column")]
        public int Column { get; set; }

        [JsonPropertyName("dir")]
        public string Dir { get; set; }
    }

    public class DtColumn
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("searchable")]
        public bool Searchable { get; set; }

        [JsonPropertyName("orderable")]
        public bool Orderable { get; set; }

        [JsonPropertyName("search")]
        public DtSearch Search { get; set; }
    }

    public class DtSearch
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("regex")]
        public bool Regex { get; set; }
    }
}
