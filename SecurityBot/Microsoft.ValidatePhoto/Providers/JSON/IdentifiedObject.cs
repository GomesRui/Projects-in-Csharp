using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ValidatePhoto.Providers.JSON
{
    public class IdentifiedObject
    {
        public IdentifiedObject() { }

        [JsonProperty("faceId")]
        public string faceId { get; set; }

        [JsonProperty("candidates")]
        public Candidates[] candidate { get; set; }

    }

    public class Candidates
    {
        public Candidates() { }

        [JsonProperty("personId")]
        public string personId { get; set; }

        [JsonProperty("confidence")]
        public float confidence { get; set; }
    }
}


/*[
    {
        "faceId": "c5c24a82-6845-4031-9d5d-978df9175426",
        "candidates": [
            {
                "personId": "25985303-c537-4467-b41d-bdb45cd95ca1",
                "confidence": 0.92
            }
        ]
    },
    {
        "faceId": "65d083d4-9447-47d1-af30-b626144bf0fb",
        "candidates": [
            {
                "personId": "2ae4935b-9659-44c3-977f-61fac20d0538",
                "confidence": 0.89
            }
        ]
    }
]
*/