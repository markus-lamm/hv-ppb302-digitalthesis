# Hv.Ppb302.DigitalThesis

## Background
This system was developed as an educational project by a team consisting of 6 people, to be delivered to the client Mona Tynkkinen at University West. The system is intended to be a visual representation of her thesis and all the unique elements therein. All information is updated as of 2024-05-27.

## Software Specifics

### Architecture
The system consists of a monolithic architecture built using one ASP.NET MVC project with .NET 8 using Visual Studio 2022 Enterprise. 

### Data Structure
The system uses a Microsoft SQL Server solution to store all system data on an IIS server run by the Economics & Informatics Department at University West. It uses a three-layered separation of concerns to establish clear boundaries between system components. The first is the interactive layer consisting of all HTML, JavaScript and CSS code. The second is the logic layer consisting primarily of the controllers. The third is the data layer consisting primarily of the repositories. This allocates tasks to different layers depending on their content, the interactive layer takes a user command, the logic layer processes it and the data layer executes an operation in the data storage.
