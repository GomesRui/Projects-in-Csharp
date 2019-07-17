

namespace Microsoft.ValidatePhoto.Providers
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
