using System;
using System.Linq;
using Microsoft.ValidatePhoto.Providers;
using Microsoft.ValidatePhoto.Providers.Exceptions;

namespace Microsoft.ValidatePhoto.Services.ComputerVision
{
    public class ComputerVisionAPI : API
    {
        private const string requestParameters = "visualFeatures=Objects&language=en";
        private const string url = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/"; //change region
        private const string ContentType = "application/octet-stream";
        private const Functions computerVisionFunction = Functions.analyze;

        //public ComputerVisionAPI(string subscription, string inputData) : base(subscription, inputData)
        public ComputerVisionAPI(string subscription) : base(subscription, requestParameters, url, computerVisionFunction)
        {

        }

        public ComputerVisionAPI(string subscription, string inputData) : base(subscription, inputData, requestParameters, url, computerVisionFunction)
        {

        }

        public override void validateImage()
        {
            if (pResultContentApi.Contains("cell phone") || pResultContentApi.Contains("telephone"))
            {
                throw new InvalidImageException("Security measures are trying to be breached!");
            }
            else if (!pResultContentApi.Contains("person"))
            {
                throw new InvalidImageException("No face was detected!");
            }
            else
            {
                Console.WriteLine("First Authentication method passed!");
            }
        }
    }
}
