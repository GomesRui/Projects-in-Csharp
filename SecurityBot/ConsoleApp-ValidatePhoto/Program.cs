using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace ValidatePhoto
{
    /*struct requestHeaders
    {
        public string ContentType;
        public string Subscription;

        public requestHeaders(string Subscription)
        {
            this.Subscription = Subscription;
            this.ContentType = "application/octet-stream";
        }
    }

    class InvalidImageException : Exception
    {
        public InvalidImageException(string message) : base(message)
        {
            Console.WriteLine("The photo does not meet the security requisites. Authentication failed!");
        }
    }

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
                }

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

    class ComputerVisionAPI : API
    {
        private const string requestParameters = "visualFeatures=Objects&language=en";
        private const string url = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/"; //change region
        private const string ContentType = "application/octet-stream";

        //public ComputerVisionAPI(string subscription, string inputData) : base(subscription, inputData)
        public ComputerVisionAPI(string subscription) : base(subscription)
        {
            pUrl = url;
            pRequestParameters = requestParameters;
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
                Console.WriteLine("Authentication successful!");
            }
        }

    }

    class FaceAPI : API
    {

        private const string requestParameters = "recognitionModel=recognition_02&detectionModel=detection_02";
        private const string url = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/"; //change region

        //public FaceAPI(string subscription, string inputData) : base(subscription, inputData)
        public FaceAPI(string subscription) : base(subscription)
        {
            pUrl = url;
            pRequestParameters = requestParameters;
        }

        public override void validateImage()
        {


        }

    }*/

    class Program
    {
        private const string subscriptionKeyComputerVision = "fbd6957c05bd45f280165617c9d96c9b";
        private const string subscriptionKeyFace = "2eca3fe6b363437ab8b54db5daa11989";

        static void Main()
        {
            string localImagePath = @"C:\Users\v-ruigom\Desktop\test.jpg";
            //ComputerVisionAPI computerVisionApi = new ComputerVisionAPI(subscriptionKeyComputerVision, localImagePath);
            //FaceAPI faceApi = new FaceAPI(subscriptionKeyFace, localImagePath);

            ComputerVisionAPI computerVisionApi = new ComputerVisionAPI(subscriptionKeyComputerVision);
            FaceAPI faceApi = new FaceAPI(subscriptionKeyFace);

            // Get the path and filename to process from the user.
            do
            {
                Console.WriteLine("Analyzing image...");

                if (localImagePath == @"C:\Users\v-ruigom\Desktop\")
                {
                    Console.Write("Enter the path to the image you wish to analyze: ");
                    localImagePath += Console.ReadLine();
                }

                if (File.Exists(localImagePath))
                {
                    try
                    {
                        // Computer Vision Check
                        computerVisionApi.ApiCall("analyze", localImagePath); //Analyze image
                        computerVisionApi.validateImage(); //Validate image
                        API.attempts = 0;
                        computerVisionApi.displayResults();

                        // Face Check
                        string content = faceApi.ApiCall("detect", localImagePath); //Detect face in image
                        faceApi.ApiCall("verify", content); //Verify face in image
                        faceApi.validateImage(); //Validate comparison
                        API.attempts = 0;
                        faceApi.displayResults();
                    }
                    catch (InvalidImageException e)
                    {
                        API.attempts--;
                        Console.WriteLine("Exception: " + e.Message);
                        Console.WriteLine("Attempts remaining: " + API.attempts);
                        localImagePath = @"C:\Users\v-ruigom\Desktop\";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                        API.attempts = -1;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid file path");
                }


            } while (API.attempts > 0);

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}

    /*static void Main()
        {
            int attempts = 3;
            string localImagePath = @"C:\Users\v-ruigom\Desktop\test.jpg";
            // Get the path and filename to process from the user.
            do
            {
                Console.WriteLine("Analyze an image:");

                if (localImagePath == @"C:\Users\v-ruigom\Desktop\")
                {
                    Console.Write("Enter the path to the image you wish to analyze: ");
                    localImagePath += Console.ReadLine();
                }

                if (File.Exists(localImagePath))
                {
                    // Call the REST API method.
                    Console.WriteLine("\nWait a moment for the results to appear.\n");

                    try
                    {
                        MakeAnalysisRequest(localImagePath).Wait();
                        attempts = 0;                        
                    }
                    catch (Exception e)
                    {
                        attempts--;
                        Console.WriteLine("Attempts remaining: " + attempts);
                        localImagePath = @"C:\Users\v-ruigom\Desktop\";
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid file path");
                }


            } while (attempts > 0);

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }*/

       /*
            /// <summary>
            /// Gets the analysis of the specified image file by using
            /// the Computer Vision REST API.
            /// </summary>
            /// <param name="imageFilePath">The image file to analyze.</param>
            static async Task MakeAnalysisRequest(string imageFilePath)
        {

            try
            {

                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKeyComputerVision);
                
                // Request parameters. A third optional parameter is "details".
                // The Analyze Image method returns information about the following
                // visual features:
                // Categories:  categorizes image content according to a
                //              taxonomy defined in documentation.
                // Description: describes the image content with a complete
                //              sentence in supported languages.
                // Color:       determines the accent color, dominant color, 
                //              and whether an image is black & white.
                string requestParameters = "visualFeatures=Objects&language=en";

                // Assemble the URI for the REST API method.
                string uri = uriBaseComputerVision + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                displayResults(contentString);

                if (!validateImage(contentString))
                {                   
                    throw new Exception();
                }

                else
                {
                


                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nError Message:" + e.Message);
                throw;
            }
        }





        static void displayResults(string content)
        {
            Console.WriteLine("\nResponse:\n\n{0}\n", JToken.Parse(content).ToString());
        }

        static bool validateImage(string content)
        {         

            if (content.Contains("cell phone") || content.Contains("telephone"))
            {
                Console.WriteLine("Security measures are trying to be breached!");
                return false;
            }
            else if(!content.Contains("person"))
            {
                Console.WriteLine("No face was detected!");
                return false;
            }
            else
            {
                Console.WriteLine("Authentication successful!");
                return true;
            }
        }
    }*/

