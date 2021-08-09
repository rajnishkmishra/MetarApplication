# MetarApplication

Prerequisites:

1. Download Dotnet SDK from https://dotnet.microsoft.com/download/dotnet/2.2
2. Add ModHeader extension in Chrome.


STEPS TO RUN THE APPLICATION

1. Run Command Prompt as Administrator.

2. Go to the MetarApplication-main folder of the application using cd command.

![image](https://user-images.githubusercontent.com/40133554/128731507-e3774202-0ce0-4042-a36d-d5f6f673becc.png)

3. Run the dotnet command dotnet build to build the solution.

![image](https://user-images.githubusercontent.com/40133554/128731932-f01afa7f-94c3-48f5-b6dd-5878451a2ec0.png)

4. Once the build is successful, run dotnet command dotnet run -p MetarApplication to run the project.

![image](https://user-images.githubusercontent.com/40133554/128732468-c8a73a27-3f1d-4ee4-a55e-7b1f7c386aba.png)

Now application is running on localhost on port 5001.

5. You can now hit the local endpoint and get the response.

Ping:
https://localhost:5001/metar/ping

Sample Output:

![image](https://user-images.githubusercontent.com/40133554/128733405-a8da6d3c-76e5-49a6-bcbb-0bc33835f3a3.png)

Get the METAR data:
https://localhost:5001/metar/info?scode=KSGS

You can give othe station code and get the response. You can find list of other station codes here: https://www.cnrfc.noaa.gov/metar.php

Sample Output:

![image](https://user-images.githubusercontent.com/40133554/128733782-d757c88e-fcc1-4b89-a149-a020b238684a.png)


To pass the header, use ModHeader extension of chrome and get the response. 

![image](https://user-images.githubusercontent.com/40133554/128737850-1087492c-a5d8-4bc6-87ae-ea4ebb609966.png)

