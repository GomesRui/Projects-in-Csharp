using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
// Install Newtonsoft.Json with NuGet
using Newtonsoft.Json;

namespace TextTranslatorFixed
{

    public class GlobalConstants
    {
        public static int maxChars = 20;
    }

    public class TranslationResult
    {
        public DetectedLanguage DetectedLanguage { get; set; }
        public TextResult SourceText { get; set; }
        public Translation[] Translations { get; set; }
    }

    public class DetectedLanguage
    {
        public string Language { get; set; }
        public float Score { get; set; }
    }

    public class TextResult
    {
        public string Text { get; set; }
        public string Script { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public TextResult Transliteration { get; set; }
        public string To { get; set; }
        public Alignment Alignment { get; set; }
        public SentenceLength SentLen { get; set; }
    }

    public class Alignment
    {
        public string Proj { get; set; }
    }

    public class SentenceLength
    {
        public int[] SrcSentLen { get; set; }
        public int[] TransSentLen { get; set; }
    }

    class Program
    {
        

        static async Task Main(string[] args)
        {
            string[] textParsed;
             int nextWord = 0;
            // This is our main function.
            // Output languages are defined in the route.
            // For a complete list of options, see API reference.
            // https://docs.microsoft.com/azure/cognitive-services/translator/reference/v3-0-translate
            string host = "https://api.cognitive.microsofttranslator.com";
            string route = "/translate?api-version=3.0&from=<text in the language>&to=<language to translate>"; //ex from=en&to=de (from english to german)
            string subscriptionKey = "<subscription key>";
            // Prompts you for text to translate. 
            Console.Write("Type the phrase you'd like to translate? ");
            string textToTranslate = Console.ReadLine();

            if (textToTranslate.Length > GlobalConstants.maxChars)
                textParsed = textToTranslate.Split('.', '!', '?', ';');
            else
                textParsed = new[] { textToTranslate };

            do
            {
                var content = await ParseText(textParsed, nextWord); //get first text to translate

                await TranslateTextRequest(subscriptionKey, host, route, content.Item2); //translation

                nextWord = content.Item1; //looking for the next words

            } while (nextWord < textParsed.Length);

            
            Console.ReadLine();

        }

        static public async Task<(int, string)> ParseText(string[] text, int count)
        {
            int totalChars = 0;
            string finalText = text[count] + " ";
            int finalCount = count;

            totalChars = text[count].Length;
            
            for (int it = (count + 1); it < text.Length; it++)
            {
                if ((totalChars + text[it].Length + 1) <= GlobalConstants.maxChars) //1 = indicates the '.'
                {
                    totalChars += text[it].Length + 1;
                    finalCount++;
                }
                else
                    break;

                finalText += text[it] + " ";
            }

            Console.Write("Text to be translated: " + finalText);

            return ((finalCount + 1), finalText);
        }

        // This sample requires C# 7.1 or later for async/await.
        // Async call to the Translator Text API
        static public async Task TranslateTextRequest(string subscriptionKey, string host, string route, string inputText)
        {


            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                // Set the method to Post.
                request.Method = HttpMethod.Post;
                // Construct the URI and add headers.
                request.RequestUri = new Uri(host + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                // Deserialize the response using the classes created earlier.
                TranslationResult[] deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);
                // Iterate over the deserialized results.
                foreach (TranslationResult o in deserializedOutput)
                {
                    // Print the detected input language and confidence score.
                    Console.WriteLine("Detected input language: {0}\nConfidence score: {1}\n", o.DetectedLanguage.Language, o.DetectedLanguage.Score);
                    // Iterate over the results and print each translation.
                    foreach (Translation t in o.Translations)
                    {
                        Console.WriteLine("Translated to {0}: {1}", t.To, t.Text);
                    }
                }
            }
        }
    }
}
