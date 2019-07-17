using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ValidatePhoto.Providers.Exceptions;
using Microsoft.ValidatePhoto.Providers;
using Microsoft.ValidatePhoto.Services.Face;
using Microsoft.ValidatePhoto.Providers.JSON;

namespace SecurityBotUnitTests
{
    [TestClass]
    public class FaceUnitTests
    {
        private const string subscriptionKeyFace = "2eca3fe6b363437ab8b54db5daa11989";

        [TestMethod]
        public void CanImageBeDetected_ImageWithPhone_ReturnsTrue() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"./ImagesToTest/WithPhone.jpg";
            var face = new FaceAPI(subscriptionKeyFace, localImagePath);

            //Act
            var result = face.ApiCall(); //Analyze image

            Console.WriteLine(result);

            //Assert
            Assert.IsTrue(result.Contains("faceId") && result.Contains("faceRectangle"));
        }


        [TestMethod]
        public void CanImageBeDetected_ImageWithoutPhoneWithFace_ReturnsTrue() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"./ImagesToTest/guard1.jpg";
            var face = new FaceAPI(subscriptionKeyFace, localImagePath);

            //Act
            var result = face.ApiCall(); //Analyze image

            //Assert
            Assert.IsTrue(result.Contains("faceId") && result.Contains("faceRectangle"));
        }

        [TestMethod]
        [ExpectedException(typeof(ImageNotFoundException))]
        public void CanImageBeDetected_NoImage_ReturnsException() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"./ImagesToTest/tes.jpg";
            var face = new FaceAPI(subscriptionKeyFace, localImagePath);

            //Act
            var result = face.ApiCall(); //Analyze image

            //Assert
            //Looks for exception
        }

        [TestMethod]
        public void CanImageBeDetected_NoFace_ReturnsFalse() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"./ImagesToTest/NoFace.jpg";
            var face = new FaceAPI(subscriptionKeyFace, localImagePath);

            //Act
            var result = face.ApiCall(); //Analyze image

            //Assert
            Assert.IsFalse(result.Contains("faceId") && result.Contains("faceRectangle"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidImageException))]
        public void CanImageBeValidated_WithPhone_ReturnsException() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            string content = System.IO.File.ReadAllText(@"./JSONToTestFace/WithPhone.txt");
            var face = new FaceAPI(subscriptionKeyFace, Functions.identify);
            face.pResultContentApi = content;

            //Act
            face.validateImage(); //Analyze image

            //Assert
            //Looks for exception
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidImageException))]
        public void CanImageBeValidated_WithNoFace_ReturnsException() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            string content = System.IO.File.ReadAllText(@"./JSONToTestFace/NoFace.txt");
            var face = new FaceAPI(subscriptionKeyFace, Functions.identify);
            face.pResultContentApi = content;

            //Act
            face.validateImage(); //Analyze image

            //Assert
            //Looks for exception
        }

        [TestMethod]
        public void CanImageBeValidated_WithFaceAndNoPhone_ReturnsSuccess() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            string content = System.IO.File.ReadAllText(@"./JSONToTestFace/Guard1.txt");
            var face = new FaceAPI(subscriptionKeyFace, Functions.identify);
            face.pResultContentApi = content;

            //Act
            try
            {
                face.validateImage(); //Analyze image
            }

            //Assert
            catch (InvalidImageException)
            { Assert.Fail(); }
        }

        [TestMethod]
        public void CanImageBeBuiltToAJsonFile_WithFace_ReturnsEqual() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            string inputData = "[{\"faceId\":\"daf80598-8a61-4afd-a3eb-9338dbf4b595\",\"faceRectangle\":{\"top\":279,\"left\":2328,\"width\":572,\"height\":827}}]";
            PersonGroupsID PGId = new PersonGroupsID(0);

            //Act            
            string res = FaceAPI.BuildJSONDetect(inputData, PGId);
            Console.Write(res);
            //Assert
            Assert.AreEqual(res, "{\"PersonGroupId\":\"employee_security_group1\",\"faceIds\":[\"daf80598-8a61-4afd-a3eb-9338dbf4b595\"],\"maxNumOfCandidatesReturned\":1,\"confidenceThreshold\":0.5}");
        }

        [TestMethod]
        [ExpectedException(typeof(JsonBuildingException))]
        public void CanImageBeBuiltToAJsonFile_WithFace_ReturnsException() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            string inputData = "[{\"faceId\":\"\",\"faceRectangle\":{\"top\":279,\"left\":2328,\"width\":572,\"height\":827}}]";
            PersonGroupsID PGId = new PersonGroupsID(0);

            //Act            
            string res = FaceAPI.BuildJSONDetect(inputData, PGId);

            //Assert
            //Throws Exception
        }

        // ------------------------------------------ TEST Identify

        [TestMethod]
        public void CanImageBeIdentified_WithFace_ReturnsEqual() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            var face = new FaceAPI(subscriptionKeyFace, Functions.identify);
            face.pInputData = "{\"PersonGroupId\":\"employee_security_group1\",\"faceIds\":[\"daf80598-8a61-4afd-a3eb-9338dbf4b595\"],\"maxNumOfCandidatesReturned\":1,\"confidenceThreshold\":0.5}";

            //Act
            var result = face.ApiCall(); //Analyze image

            Console.Write(result);

            //Assert
            Assert.IsTrue(result.Contains("personId") && result.Contains("confidence"));
        }

        

        [TestMethod]
        public void CanPeopleBeListed_WrongInput_ReturnsFalse() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            PersonGroupsID personGroup = new PersonGroupsID(0);
            var face = new FaceAPI(subscriptionKeyFace, Functions.list);
            string inputData = "[{\"faceId\": \"c5c24a82-6845-4031-9d5d-978df9175426\",\"candidates\": [{\"personId\": \"25985303-c537-4467-b41d-bdb45cd95ca1\",\"confidence\": 0.92}]},{\"faceId\": \"65d083d4-9447-47d1-af30-b626144bf0fb\",\"candidates\": [{\"personId\": \"2ae4935b-9659-44c3-977f-61fac20d0538\",\"confidence\": 0.89}]}]";

            //Act
            bool result = face.authenticateImage(inputData, personGroup); //Analyze image

            //Console.Write(result);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanPeopleBeListed_KnownInput_ReturnsTrue() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            PersonGroupsID personGroup = new PersonGroupsID(0);
            var face = new FaceAPI(subscriptionKeyFace, Functions.list);
            string inputData = "[{\"faceId\":\"e6f1f6bd-706d-4ca5-a537-978e226f0b73\",\"candidates\": [{\"personId\":\"c730bc36-37de-4dbc-8d65-3ac04cd3fd5f\",\"confidence\":0.80588}]}]";

            //Act
            bool result = face.authenticateImage(inputData, personGroup); //Analyze image

            //Console.Write(result);

            //Assert
            Assert.IsTrue(result);
        }

    }
}
