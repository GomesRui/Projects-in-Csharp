using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ValidatePhoto.Providers.JSON
{
        public class PersonList
        {
            public PersonList() { }

            [JsonProperty("personId")]
            public string personId { get; set; }

            [JsonProperty("persistedFaceIds")]
            public string[] persistedFaceIds { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("userData")]
            public string userData { get; set; }

        }
}
