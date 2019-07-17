using Microsoft.ValidatePhoto.Providers;
using Microsoft.ValidatePhoto.Providers.Exceptions;
using Microsoft.ValidatePhoto.Providers.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.ValidatePhoto.Services.Face;

namespace Microsoft.ValidatePhoto.Services.Face
{
    public class FaceAPI : API
    {
        private const string detectRequestParameters = "recognitionModel=recognition_01&detectionModel=detection_01";
        private const string url = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/"; //change region
        private const float confidenceThresh = 0.5f;
        private const int numberOfCandidates = 1;

        //public FaceAPI(string subscription, string inputData) : base(subscription, inputData)
        public FaceAPI(string subscription, Functions function) : base(subscription, url, function)
        {
            if (function == Functions.list)
            {
                pRequestParameters = "persongroups/pid/persons";
                pContentType = "application/json";
            }
            else if (function == Functions.identify)
            {
                pRequestParameters = "";
                pContentType = "application/json";
            }
            else
            {
                pRequestParameters = "";
                pContentType = "application/octet-stream";
            }
        }

        public FaceAPI(string subscription, string inputData) : base(subscription, inputData, detectRequestParameters, url, Functions.detect)
        {

        }

        public override void validateImage()
        {
            if(!pResultContentApi.Contains("faceId") || !pResultContentApi.Contains("faceRectangle"))
            {
                throw new InvalidImageException ("No Face was detected!");
            }

            else
            {
                string[] contentArray = pResultContentApi.Split('{');
                int countFaces = 0, i = 0;

                do
                {

                    if (contentArray[i].Contains("faceId"))
                        countFaces++;

                    i++;

                } while (countFaces < 2 && i < contentArray.Length);

                if (countFaces > 1)
                    throw new InvalidImageException("More than one face was detected!");

                else
                    Console.WriteLine("Second Authentication method passed!");
            }
        }


        //NOTE
        //JSON FILE FROM DETECT
        /*
         [
            {
                "faceId": "62476bc0-f66c-461a-8b87-b556b7af9444",
                "faceRectangle": {
                "top": 483,
                "left": 716,
                "width": 783,
                "height": 1132
                }
            }
        ]*/

        //JSON FILE TO VERIFY
        /*[
        {
            "PersonGroupId": "employee_security_group1",
            "faceIds": [
                "2a15daff-4b22-41d7-9035-67cc98dfe228",
                "a0ca61ec-28a5-4a65-9ad3-e28575e30aea"
            ],
            "maxNumOfCandidatesReturned": 3,
            "confidenceThreshold": 0
        }
        ]*/

        public static string BuildJSONDetect(string inputData, PersonGroupsID PGId) // Builds the input json file to the identify function
        {
            List<DetectObject> desContent = new List<DetectObject>(); //input

            IdentifyObject serContent = new IdentifyObject();
            
            string json;

            try
            {
                desContent = JsonConvert.DeserializeObject<List<DetectObject>>(inputData);

                //adding the needed json parameters
                serContent.confidenceThreshold = confidenceThresh;
                serContent.maxNumOfCandidatesReturned = numberOfCandidates;
                serContent.faceIds = new string[] { desContent[0].faceId };
                serContent.PersonGroupId = PGId.pPersonGroupName;

                json = JsonConvert.SerializeObject(serContent);

                if (json == "[]" || json.Contains("\"PersonGroupId\":\"\"") || json.Contains("\"faceIds\":[\"\"]"))
                    throw new JsonBuildingException(); //default message stored
            }
            catch(JsonException je)
            {
                throw je; //Other json exceptions
            }

            return json;   
            
        }


        /*REPLY:
         * 
         * [
            {
                "faceId": "daf80598-8a61-4afd-a3eb-9338dbf4b595",
                "candidates": [
                    {
                        "personId": "c730bc36-37de-4dbc-8d65-3ac04cd3fd5f",
                        "confidence": 0.93172
                    }
                ]
            }
        ] */

        private static List<IdentifiedObject> BuildJSONIdentity(string inputData) // Builds the input json file to the identify function
        {
            List<IdentifiedObject> desContent = new List<IdentifiedObject>(); //input

            try
            {
                desContent = JsonConvert.DeserializeObject<List<IdentifiedObject>>(inputData);

                if (desContent.Count <= 0 || desContent[0].faceId == "" || desContent[0].candidate.Length <= 0 || desContent[0].candidate[0].personId == "")
                    throw new JsonBuildingException();//default message stored
            }
            catch (JsonException je)
            {
                throw je; //Other json exceptions
            }

            return desContent;

        }

        private static List<PersonList> BuildJSONPList(string inputData) // Builds the input json file to the PersonList function
        {
            List<PersonList> desContent = new List<PersonList>(); //input

            try
            {
                desContent = JsonConvert.DeserializeObject<List<PersonList>>(inputData);

                if (desContent.Count <= 0 || desContent[0].personId == "" || desContent[0].name == "" || desContent[0].persistedFaceIds.Length <= 0)
                    throw new JsonBuildingException();//default message stored
            }
            catch (JsonException je)
            {
                throw je; //Other json exceptions
            }

            return desContent;
        }

        public bool authenticateImage(string inputData, PersonGroupsID departmentID)
        {

            List<PersonList> personList = new List<PersonList>();
            List<IdentifiedObject> authenticationObj = new List<IdentifiedObject>();
            string stringPList = "";
            int personFound = -1;

            try
            {
                authenticationObj = BuildJSONIdentity(inputData);
                pRequestParameters = pRequestParameters.Replace("pid", departmentID.pPersonGroupName);
                stringPList = ApiCall(); //making api call to fetch the person List details
                personList = BuildJSONPList(stringPList);

                Console.WriteLine("You have {0} candidates with higher confidence score of {1}:", authenticationObj[0].candidate.Length, confidenceThresh);

                for (int i = 0; i < authenticationObj[0].candidate.Length; i++)
                {
                    for (int j = 0; j < personList.Count; j++)
                    {
                        if (authenticationObj[0].candidate[i].personId == personList[j].personId)
                        {
                            personFound = j;
                            Console.WriteLine("{0}: {1} [details: {2}]", personList[j].name, authenticationObj[0].candidate[i].confidence, personList[j].userData); //note, by default, the api returns always the candidates sorted by the confidence score
                        }

                    }            
                }                
            }   
            catch (Exception e)
            {
                throw e;
            }
            if (personFound == -1)
            {
                Console.WriteLine("Error during authentication! Access not granted!");
                pAttempts = 0;
                return false;
            }
            else
            {
                Console.WriteLine("Access granted! Welcome Mr(s). {0}", personList[personFound].userData.Substring(0, personList[personFound].userData.IndexOf(',')));
                return true;
            }
                
        }


    }
}
