using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ValidatePhoto.Services;

namespace SecurityBot_UnitTests
{
    [TestClass]
    public class ComputerVisionAPITests
    {
        private const string subscriptionComputerVision = "fbd6957c05bd45f280165617c9d96c9b";        

        [TestMethod]
        public void CanImageBeValidated_ImageWithPhone_ReturnsTrue() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"C:\Users\v-ruigom\Desktop\test.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision);

            //Act
            var result = computervision.ApiCall("analyze", localImagePath); //Analyze image

            //Assert
            Assert.IsTrue(result.Contains("phone"));
        }

        [TestMethod]
        public void CanImageBeValidated_ImageWithoutPhone_ReturnsFalse() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"C:\Users\v-ruigom\Desktop\test3.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision);

            //Act
            var result = computervision.ApiCall("analyze", localImagePath); //Analyze image

            //Assert
            Assert.IsFalse(result.Contains("phone"));
        }

        [TestMethod]
        public void CanImageBeValidated_NoImage_ReturnsTrue() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"C:\Users\v-ruigom\Desktop\tes.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision);

            //Act
            var result = computervision.ApiCall("analyze", localImagePath); //Analyze image

            //Assert
            Assert.IsTrue(result.Contains(""));
        }
    }
}
