using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using Microsoft.ValidatePhoto.Services.Face;
using Microsoft.ValidatePhoto.Services.ComputerVision;
using Microsoft.ValidatePhoto.Providers;
using Microsoft.ValidatePhoto.Providers.Exceptions;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace ValidatePhoto
{
    class Program
    {
        private const string subscriptionKeyComputerVision = "<computer vision key>";
        private const string subscriptionKeyFace = "<face key>";

        //SQL Database connection string
        private const string dataSource = "<server>.database.windows.net"; 
        private const string databaseString = "<database>";
        private const string oracleUser = "<username>"; 
        private const string oraclePass = "<password>"; 
        
        static async Task Main(string[] args)
        {
            string directoryPath = @"<first image to authenticate location>";
            string imageName = "<name of the file>";
            string fullImagePath = directoryPath + imageName;
            int department = 0;
            bool exit = false;

            ComputerVisionAPI computerVisionApi = new ComputerVisionAPI(subscriptionKeyComputerVision, fullImagePath);
            FaceAPI faceDetectApi = new FaceAPI(subscriptionKeyFace, fullImagePath);
            FaceAPI faceIdentifyApi = new FaceAPI(subscriptionKeyFace, Functions.identify);
            FaceAPI faceListApi = new FaceAPI(subscriptionKeyFace, Functions.list);

            // Get the path and filename to process from the user.
            do
            {
                do
                {
                    
                    Console.WriteLine("Please state your department: ");
                    Console.WriteLine("[0] - Security");
                    Console.WriteLine("[1] - HR");
                    Console.WriteLine("[2] - Development");
                    Console.WriteLine("[3] - Janitors");
                    Console.Write("Department ID: ");
                    department = Convert.ToInt16(Console.ReadLine());
                    
                    if (department > 3 || department < 0)
                    {
                        Console.WriteLine("ERROR: That department doesn't exist. Please select the keys that correspond to your department");
                    }
                    else
                    {
                        exit = true;
                    }

                } while (!exit);

                exit = false;
                PersonGroupsID DepartmentID = new PersonGroupsID(department); //link the department ID with the personGroupID for Face API

                if (imageName == "")
                {
                    Console.Write("Enter the name of the new image you wish to analyze: ");
                    imageName = Console.ReadLine();
                    fullImagePath = directoryPath + imageName;
                    computerVisionApi.pInputData = fullImagePath;
                }
                    try
                    {
                        // Computer Vision Check
                        computerVisionApi.ApiCall(); //Analyze image
                        computerVisionApi.validateImage(); //Validate image
                        computerVisionApi.DisplayResults();

                        // Face Check
                        faceDetectApi.pInputData = computerVisionApi.pInputData; //using the same image as the Computer Vision
                        faceDetectApi.ApiCall(); //Detect face in image
                        faceDetectApi.validateImage(); //Validate Picture
                        faceDetectApi.DisplayResults();

                        faceIdentifyApi.pInputData = FaceAPI.BuildJSONDetect(faceDetectApi.pResultContentApi, DepartmentID); //Input data based on an application json
                        faceIdentifyApi.ApiCall(); //Verify face in image
                        faceIdentifyApi.DisplayResults();
                        faceListApi.authenticateImage(faceIdentifyApi.pResultContentApi, DepartmentID); //Authenticate
                        
                        API.attempts = 0;
                        exit = true; //End of a successful face authentication
                    }
                    catch (Exception e)
                    {
                        if (e is ImageNotFoundException || e is InvalidImageException || e is HttpRequestException) //application's input data exceptions
                        {
                            API.attempts--;
                            Console.WriteLine("Exception: " + e.Message);
                            Console.WriteLine("Attempts remaining: " + API.attempts);
                            imageName = "";
                        }
                        else //application's bugs
                        {
                            Console.WriteLine("Code exception! Please contact support team to debug the logs!");
                            Console.WriteLine("Exception: " + e.Message);
                            API.attempts = -1;
                        }

                    }

            } while (API.attempts > 0);

            if (!exit)
            {
                Console.WriteLine("Face Authentication failed after 3 tries!");

                try
                {
                    API.attempts = 3;
                    string username = "", password = "";
                    DatabaseConnection databaseConnection = new DatabaseConnection(dataSource, databaseString, oracleUser, oraclePass);

                    do
                    {
                        Console.WriteLine("\nPlease login with your credentials.");
                        Console.Write("\nUsername: ");
                        username = Console.ReadLine();
                        Console.Write("Password: ");
                        do
                        {
                            ConsoleKeyInfo key = Console.ReadKey(true);
                            // Backspace Should Not Work
                            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                            {
                                password += key.KeyChar;
                                Console.Write("*");
                            }
                            else
                            {
                                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                                {
                                    password = password.Substring(0, (password.Length - 1));
                                    Console.Write("\b \b");
                                }
                                else if (key.Key == ConsoleKey.Enter)
                                {
                                    break;
                                }
                            }
                        } while (true);

                        await databaseConnection.PasswordAuthentication(department, username, password);

                    } while (API.attempts > 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nDatabase exception! Please contact support team to debug the logs!");
                    Console.WriteLine("Exception: " + e.Message);
                    API.attempts = -1;
                }

            }

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}