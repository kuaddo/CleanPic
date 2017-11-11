using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleaningPic.Models
{
    public class Result
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("category")]
        public Category Category { get; set; }

        [JsonProperty("tools")]
        public IList<Tool> Tools { get; set; }

        [JsonProperty("timeToFinish")]
        public int TimeToFinish { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("cautionText")]
        public string CautionText { get; set; }
    }

    public class Location
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Category
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Tool
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
