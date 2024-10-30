# Hv.Ppb302.DigitalThesis

## Background
This system was developed as an educational project by a team consisting of 6 people, to be delivered to the client Mona Tynkkinen at University West. The system is intended to be a visual representation of her thesis and all the unique elements therein.

## Software Specifics

### Architecture
The system consists of a monolithic architecture built using one ASP.NET MVC project with .NET 8 using Visual Studio 2022 Enterprise. The system is currently hosted (as of 2024-10-30) on an IIS server at University West.

### Data Structure
The system uses a Microsoft SQL Server solution to store all system data on a server at University West (as of 2024-10-30). It uses a three-layered separation of concerns to establish clear boundaries between system components. The first is the interactive layer consisting of all HTML, JavaScript and CSS code. The second is the logic layer consisting primarily of the controllers and other logic in C#. The third is the data layer consisting primarily of the repositories also in C#. This allocates tasks to different layers depending on their content, the interactive layer takes a user command, the logic layer processes it and the data layer executes an operation in the data storage.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details. The license only applies to the code in this repository. The thesis content is not included in this license.