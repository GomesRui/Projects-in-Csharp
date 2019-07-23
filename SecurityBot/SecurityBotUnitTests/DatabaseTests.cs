using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ValidatePhoto.Providers;
using Microsoft.ValidatePhoto.Services;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SecurityBotUnitTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void CanBeAuthenticatedWithPassword_ValidInput_ReturnsEqual()
        {

        string dataSource = "<server>.database.windows.net";
        string databaseString = "<database>";
        string oracleUser = "<username>";
        string oraclePass = "<password>";
        int department = 0;
        string username = "GomesRui";
        string password = "GomesRui";

            DatabaseConnection databaseConnection = new DatabaseConnection(dataSource, databaseString, oracleUser, oraclePass);
        
        Task <bool> T1 = Task.Run(() => databaseConnection.PasswordAuthentication(department, username, password));

            T1.Wait();
            Assert.IsTrue(T1.Result);   
    
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CanBeAuthenticatedWithPassword_InvalidConnectionStringDatabase_ReturnsException()
        {
            //Arrange
            string dataSource = "<server>.database.windows.net";
            string databaseString = "<database>";
            string oracleUser = "<username>";
            string oraclePass = "<password>";
            int department = 0;
            string username = "GomesRui";
            string password = "GomesRui";

            DatabaseConnection databaseConnection = new DatabaseConnection(dataSource, databaseString, oracleUser, oraclePass);

            //Act
            Task<bool> T1 = Task.Run(() => databaseConnection.PasswordAuthentication(department, username, password));

            T1.Wait();

            //Assert
            // Exception thrown

        }
        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CanBeAuthenticatedWithPassword_InvalidConnectionStringCredentials_ReturnsException()
        {
            //Arrange
            string dataSource = "<server>.database.windows.net";
            string databaseString = "<database>";
            string oracleUser = "<username>";
            string oraclePass = "<password>";
            int department = 0;
            string username = "GomesRui";
            string password = "GomesRui";

            DatabaseConnection databaseConnection = new DatabaseConnection(dataSource, databaseString, oracleUser, oraclePass);

            //Act
            Task<bool> T1 = Task.Run(() => databaseConnection.PasswordAuthentication(department, username, password));

            T1.Wait();

            //Assert
            // Exception thrown

        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CanBeAuthenticatedWithPassword_InvalidName_ReturnsException()
        {
            //Arrange
            string dataSource = "<server>.database.windows.net";
            string databaseString = "<database>";
            string oracleUser = "<username>";
            string oraclePass = "<password>";
            int department = 0;
            string username = "GomeRui";
            string password = "GomesRui";

            DatabaseConnection databaseConnection = new DatabaseConnection(dataSource, databaseString, oracleUser, oraclePass);

            //Act
            Task<bool> T1 = Task.Run(() => databaseConnection.PasswordAuthentication(department, username, password));

            T1.Wait();

            //Assert
            // Exception thrown

        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CanBeAuthenticatedWithPassword_InvalidPassword_ReturnsException()
        {
            //Arrange
            string dataSource = "<server>.database.windows.net";
            string databaseString = "<database>";
            string oracleUser = "<username>";
            string oraclePass = "<password>";
            int department = 0;
            string username = "GomesRui";
            string password = "GomeRui";

            DatabaseConnection databaseConnection = new DatabaseConnection(dataSource, databaseString, oracleUser, oraclePass);

            //Act
            Task<bool> T1 = Task.Run(() => databaseConnection.PasswordAuthentication(department, username, password));

            T1.Wait();

            //Assert
            // Exception thrown

        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CanBeAuthenticatedWithPassword_InvalidDepartment_ReturnsException()
        {
            //Arrange
            string dataSource = "<server>.database.windows.net";
            string databaseString = "<database>";
            string oracleUser = "<username>";
            string oraclePass = "<password>";
            int department = 1;
            string username = "GomesRui";
            string password = "GomesRui";

            DatabaseConnection databaseConnection = new DatabaseConnection(dataSource, databaseString, oracleUser, oraclePass);

            //Act
            Task<bool> T1 = Task.Run(() => databaseConnection.PasswordAuthentication(department, username, password));

            T1.Wait();

            //Assert
            // Exception thrown

        }
    }
}
