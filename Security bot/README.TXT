- Security bot based on 2 Cognitive Services from Microsoft Azure: Computer Vision and Face API with authentication purposes.

  Code solution is based on C# and, as per the Azure, you have to get a free subscription (at least) and create the two above services.
  Afterwards, you simply have to add the keys, API URL and the image that you're looking to test against the security measures.
  
  NOTE: the authentication method is using the methodology:
  
  1) Take a photo with your mobile device (or any other device)
  2) Send it to the solution for authentication
  3) Application looks for attempts of breaching security measures (example a picture from Facebook or from another mobile device) - 3 attempts are given
  4) After successful authentication, the user can login to the company's apps
  5) If not successful, the user is requests to put his credentials - 3 attempts;
  6) If not successful, the user receives a warning message and gets blocked.
  
  
 NOTE: UNDER DEVELOPMENT
  
