This project is responsible for translating from any language to any language, as per your desire, by using the Cognitive Service Text Translator from Azure.

Like any Azure resource, you will have to get a free subscription (at least) and create a new Text Translator resource.
Afterwards, you simply have to replace the following variables with the needed values:

            string route = "/translate?api-version=3.0&to=<language to translate>";
            string subscriptionKey = "<subscription key>";
            
 NOTE: this solution is intended to work with big texts (larger than 5k chars per request) which is a known limitation from this service. 
 
 
