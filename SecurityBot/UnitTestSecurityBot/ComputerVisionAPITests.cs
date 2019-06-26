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
        public void CanImageBeValidated_ImageWithPhone_ReturnsFalse() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"C:\Users\v-ruigom\Desktop\test.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision);

            //Act
            computervision.ApiCall("analyze", localImagePath); //Analyze image
            var result = computervision.validateImage(); //Validate image

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanImageBeValidated_ImageWithoutPhone_ReturnsTrue() //Method under test _ Scenario tested _ Result
        {
            // Arrange
            const string localImagePath = @"C:\Users\v-ruigom\Desktop\test3.jpg";
            var computervision = new ComputerVisionAPI(subscriptionComputerVision);

            //Act
            computervision.ApiCall("analyze", localImagePath); //Analyze image
            var result = computervision.validateImage(); //Validate image

            //Assert
            Assert.IsTrue(result);
        }
    }
}
