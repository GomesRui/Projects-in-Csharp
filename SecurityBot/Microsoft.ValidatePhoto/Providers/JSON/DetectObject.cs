using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.ValidatePhoto.Providers.JSON
{
    public class DetectObject
    {
        public DetectObject() { }

        [JsonProperty("faceId")]
        public string faceId { get; set; }

        [JsonProperty("faceRectangle")]
        public Rectangle faceRectangle { get; set; }

    }

    public class Rectangle
    {
        public Rectangle() { }

        [JsonProperty("top")]
        public int top { get; set; }

        [JsonProperty("left")]
        public int left { get; set; }

        [JsonProperty("width")]
        public int width { get; set; }

        [JsonProperty("height")]
        public int height { get; set; }
    }
}


/*
 [
    {
        "faceId": "62476bc0-f66c-461a-8b87-b556b7af9444",
        "faceRectangle": {
        "top": 483,
        "left": 716,
        "width": 783,
        "height": 1132
        }
    }
]*/
