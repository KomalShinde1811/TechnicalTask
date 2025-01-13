# XML File Monitor Application

This console application is designed to monitor a specified folder for newly added XML files. When a new XML file is detected, the application logs the filename and its full path to the console.

### Features
- Monitors a folder for the addition of XML files.
- Outputs the file name and its full path to the console.
- Generate the xml file & store in output folder 

## Prerequisites

Before you start, make sure you have the following installed on your machine:
- **Visual Studio** or any C# development environment.
- **.NET Core** or **.NET Framework** (depending on your project setup).

## Getting Started

### 1. Clone the Repository

Clone this repository to your local machine using Git:

```bash
git clone https://github.com/yourusername/your-repository-name.git
```
### 2. Configure the App.config File
In the repository, you'll find an App.config file where you need to set the paths for your InputFolder and OutputFolder.

Open the App.config file.
Modify the InputFolder and OutputFolder values to reflect the paths on your machine:
```xml
<configuration>
  <appSettings>
    <!-- Path where new XML files will be added -->
    <add key="InputFolder" value="C:\path\to\input\folder" />
    
    <!-- Path to move the files after processing (optional) -->
    <add key="OutputFolder" value="C:\path\to\output\folder" />
  </appSettings>
</configuration>
Make sure the folders you specify already exist.
```
### 3. Build the Application
Open the solution in Visual Studio.
Build the solution by selecting Build > Build Solution or pressing Ctrl + Shift + B.
### 4. Run the Application
Press F5 or Ctrl + F5 to run the application.
The application will begin monitoring the folder specified in InputFolder.
### 5. Test the Application
To test the functionality:
Add a new XML file to the folder specified as InputFolder.
You will see the following output in the console:
```bash
File added: newfile.xml
File is stored in location: C:\path\to\output\folder\newfile-Result.xml
```
