using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_SSD230.Models
{
    public class APIErrorData
    {
        [JsonProperty("error_description")]
        public string Message { get; set; }

        public Dictionary<string, string[]> ModelState { get; set; }
    }
}