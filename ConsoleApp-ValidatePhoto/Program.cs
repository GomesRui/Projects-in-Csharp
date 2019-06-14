using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;

namespace CSHttpClientSample
{

    class Program
    {
        // subscriptionKey = "0123456789abcdef0123456789ABCDEF"
        private const string subscriptionKey = "fbd6957c05bd45f280165617c9d96c9b";

        // localImagePath = @"C:\Documents\LocalImage.jpg"
        

        // Specify the features to return
        private const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/analyze";

        static void Main()
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
        }

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
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                
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
                string uri = uriBase + "?" + requestParameters;

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



            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nError Message:" + e.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
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

        static void displayResults(string content)
        {
            Console.WriteLine("\nResponse:\n\n{0}\n", JToken.Parse(content).ToString());
        }

        static bool validateImage(string content)
        {         

            if (content.Contains("cell phone") || content.Contains("telephone"))
            {
                Console.WriteLine("Security measures trying to be breached!");
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
    }
}