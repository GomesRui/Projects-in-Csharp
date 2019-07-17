using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ValidatePhoto.Providers.Exceptions;
using Microsoft.ValidatePhoto.Providers;
using Microsoft.ValidatePhoto.Services.ComputerVision;


namespace SecurityBotUnitTests
{
    [TestClass]
    public class ComputerVisionUnitTests
    {
        private const string subscriptionComputerVision = "066af27a42d84df8ad9404d17ff9a1c5";

        [TestMethod]
        public void CanImageBeAnalyzed_ImageWithPhone_ReturnsTrue() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"./ImagesToTest/WithPhone.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision, localImagePath);

            //Act
            var result = computervision.ApiCall(); //Analyze image

            //Assert
            Assert.IsTrue(result.Contains("phone") || result.Contains("cell") || result.Contains("pose"));
        }

        [TestMethod]
        public void CanImageBeAnalyzed_ImageWithoutPhoneWithFace_ReturnsTrue() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"./ImagesToTest/guard1.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision, localImagePath);

            //Act
            var result = computervision.ApiCall(); //Analyze image

            //Assert
            Assert.IsTrue(result.Contains("person") && !(result.Contains("phone") || result.Contains("cell") || result.Contains("pose")));
        }

        [TestMethod]
        [ExpectedException(typeof(ImageNotFoundException))]
        public void CanImageBeAnalyzed_NoImage_ReturnsException() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"./ImagesToTest/tes.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision, localImagePath);

            //Act
            var result = computervision.ApiCall(); //Analyze image

            //Assert
            //Looks for exception
        }

        [TestMethod]
        public void CanImageBeAnalyzed_NoFace_ReturnsFalse() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"./ImagesToTest/NoFace.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision, localImagePath);

            //Act
            var result = computervision.ApiCall(); //Analyze image

            Console.WriteLine(result);

            //Assert
            Assert.IsFalse(result.Contains("person"));
        }
    }
}
