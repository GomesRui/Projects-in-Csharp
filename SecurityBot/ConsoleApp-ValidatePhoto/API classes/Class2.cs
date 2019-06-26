using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatePhoto.API_classes
{
    struct requestHeaders
    {
        public string ContentType;
        public string Subscription;

        public requestHeaders(string Subscription)
        {
            this.Subscription = Subscription;
            this.ContentType = "application/octet-stream";
        }
    }
}
