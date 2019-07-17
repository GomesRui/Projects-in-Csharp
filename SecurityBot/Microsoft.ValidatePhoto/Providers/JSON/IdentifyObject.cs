using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ValidatePhoto.Providers.JSON
{
    public class IdentifyObject
    {
        public IdentifyObject() { }

        [JsonProperty("PersonGroupId")]
        public string PersonGroupId { get; set; }

        [JsonProperty("faceIds")]
        public string[] faceIds { get; set; }

        [JsonProperty("maxNumOfCandidatesReturned")]
        public int maxNumOfCandidatesReturned { get; set; }

        [JsonProperty("confidenceThreshold")]
        public float confidenceThreshold { get; set; }

    }
}


/*[
{
    "PersonGroupId": "employee_security_group1",
    "faceIds": [
        "2a15daff-4b22-41d7-9035-67cc98dfe228",
        "a0ca61ec-28a5-4a65-9ad3-e28575e30aea"
    ],
    "maxNumOfCandidatesReturned": 3,
    "confidenceThreshold": 0
}
]*/
