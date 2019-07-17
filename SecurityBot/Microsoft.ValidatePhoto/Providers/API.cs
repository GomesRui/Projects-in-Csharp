using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.ValidatePhoto.Providers.Exceptions;
using Microsoft.ValidatePhoto.Providers.JSON;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.ValidatePhoto.Providers
{
    public abstract class API
    {
        private string url;
        private string requestParameters;
        private requestHeaders requestHeaders;
        public static int attempts;
        private string resultContentApi;
        private string inputData = "";
        private Functions currentFunction;

        public API(string subscription, string url, Functions currentFunction)
        {
            this.inputData = "";
            attempts = 3;
            this.requestHeaders = new requestHeaders(subscription);
            this.resultContentApi = "";
            this.requestParameters = "";
            this.url = url;
            this.currentFunction = currentFunction;
        }
        public API(string subscription, string requestParameters, string url, Functions currentFunction)
        {
            this.inputData = "";
            attempts = 3;
            this.requestHeaders = new requestHeaders(subscription);
            this.resultContentApi = "";
            this.requestParameters = requestParameters;
            this.url = url;
            this.currentFunction = currentFunction;
        }
        public API(string subscription, string inputData, string requestParameters, string url, Functions currentFunction)
        {
            this.inputData = inputData;
            attempts = 3;
            this.requestHeaders = new requestHeaders(subscription);
            this.resultContentApi = "";
            this.requestParameters = requestParameters;
            this.url = url;
            this.currentFunction = currentFunction;
        }

        public string pSubscription
        {
            get
            {
                return requestHeaders.Subscription;
            }

            set
            {
                requestHeaders.Subscription = value;
            }

        }

        public string pUrl { get => url; set => url = value; }
        public int pAttempts { get => attempts; set => attempts = value; }
        public string pRequestParameters { get => requestParameters; set => requestParameters = value; }
        public string pResultContentApi { get => resultContentApi; set => resultContentApi = value; }
        public string pContentType { get => requestHeaders.ContentType; set => requestHeaders.ContentType = value; }
        public string pInputData { get => inputData; set => inputData = value; }
        public Functions pCurrentFunction { get => currentFunction; set => currentFunction = value; }

        public abstract void validateImage();

        public string ApiCall()
        {
            string resultContent = "", URI = "";
            byte[] byteData;
            dynamic content;
            dynamic method;
   
            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            HttpRequestMessage request = new HttpRequestMessage();

            switch (currentFunction)
            {
                case (Functions.identify):
                {
                    if (inputData != "[]")
                    {
                        URI += url + currentFunction;
                        content = new StringContent(inputData, Encoding.UTF8, pContentType);
                        method = HttpMethod.Post;
                    }
                    else
                    {
                        throw new ImageNotFoundException(inputData);
                    }
                    break;
                }
                case (Functions.list):
                {
                    if (inputData != "[]")
                    {
                        content = null;
                        URI += url + pRequestParameters;
                        method = HttpMethod.Get;
                    }
                    else
                    {
                        throw new ImageNotFoundException(inputData);
                    }
                    break;
                }
                default:

                    if (File.Exists(inputData))
                    {
                        URI += url + currentFunction + "?" + requestParameters;
                        byteData = GetImageAsByteArray(inputData);
                        content = new ByteArrayContent(byteData);
                        method = HttpMethod.Post;
                    }
                    else
                    {
                        throw new ImageNotFoundException(inputData);
                    }
                    break;
            }

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", requestHeaders.Subscription);
            request.Method = method;
            request.RequestUri = new Uri(URI);

            if (pCurrentFunction != Functions.list)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(pContentType);
                request.Content = content;
            }

            try
            {
                // Execute the REST API call.

                response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode == false)
                    throw new HttpRequestException("Error code: " + response.StatusCode + "\nReason message: " + response.ReasonPhrase + "\n Details: " + response.RequestMessage);
              
                // Get the JSON response.                        
                resultContent = response.Content.ReadAsStringAsync().Result;
                resultContentApi = resultContent;
            }
            catch (Exception e)
            {
                throw e;
                //Console.WriteLine("Exception: " + e.Message);
            }                

            return resultContent;
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        public void DisplayResults()
        {
            Console.WriteLine("\nResponse:\n\n{0}\n", JToken.Parse(resultContentApi).ToString());
        }
    }
}


