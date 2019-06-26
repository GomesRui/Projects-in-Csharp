using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatePhoto
{
    abstract class API
    {
        private string url;
        private string requestParameters;
        private requestHeaders requestHeaders;
        //private string inputData;
        public static int attempts;
        private string resultContentApi;

        //public API(string subscription, string inputData)
        public API(string subscription)
        {
            //this.inputData = inputData;
            attempts = 3;
            this.requestHeaders = new requestHeaders(subscription);
            this.resultContentApi = "";
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
        //public string pInputData { get => inputData; set => inputData = value; }
        public int pAttempts { get => attempts; set => attempts = value; }
        public string pRequestParameters { get => requestParameters; set => requestParameters = value; }
        public string pResultContentApi { get => resultContentApi; set => resultContentApi = value; }
        public string pContentType { get => requestHeaders.ContentType; set => requestHeaders.ContentType = value; }

        public abstract void validateImage();

        public string ApiCall(string function, string inputData)
        {
            HttpClient client = new HttpClient();

            string resultContent;
            byte[] byteData;

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", requestHeaders.Subscription);

            // Assemble the URI for the REST API method.
            string URI = url + function + "?" + requestParameters; //adding parameters to the URI

            if (function == "verify")
                pContentType = "application/json";

            HttpResponseMessage response;

            if (pContentType == "application/octet-stream")
            {
                byteData = GetImageAsByteArray(inputData);
            }
            else
            {
                //ByteArrayContent content;
                //handle json file input data
            }

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(requestHeaders.ContentType);

                try
                {
                    // Execute the REST API call.
                    response = client.PostAsync(URI, content).Result;

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

                /*byte[] byteData = GetImageAsByteArray(inputData);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue(requestHeaders.ContentType);

                    try
                    {
                        // Execute the REST API call.
                        response = client.PostAsync(URI, content).Result;

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
                }*/

            }
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

        public void displayResults()
        {
            Console.WriteLine("\nResponse:\n\n{0}\n", JToken.Parse(resultContentApi).ToString());
        }

    }
}
