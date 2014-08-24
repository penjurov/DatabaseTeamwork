#Databases - Practical Teamwork Project 2014

A factory of your choice holds information about its products in MongoDB database consisting of at least 3 tables. For testing purposes please fill between 10 and 50 records in each table. Try to use real-world data. You may use sequential IDs for the primary key or any other primary key notation.

## Assignment

Your assignment is to design, develop and test a **C# application** for importing **Excel** reports from a **ZIP** file and the product data from **MongoDB** into **SQL Server**, generate **XML** reports and **PDF** reports, create reports as **JSON** documents and also load them into **MySQL**, load additional information by your choice from XML file, read other information by your choice from **SQLite** and calculate aggregated results and write them into Excel file. All reports should be different from each other and are by your choice. They can be sales reports, taxes reports, vendor reports, etc. Try to use real-world example.

## Problem #1 - Load Excel Reports from ZIP File
Your task is to write a C# program to **load Excel in MS SQL Server**. You may need to preliminary design a database schema to hold all data about the products (data from the MongoDB database and data from the Excel files) or use the "code-first" approach to move the DB schema from MongoDB to SQL Server. Your C# program should also move the products data from MongoDB to SQL Server. The Excel files are given inside a ZIP archive holding subfolders named as the dates of the report in format dd-MMM-yyyy (see the example reports archive Sample-Sales-Reports.zip).
Note that the ZIP file could contain few hundred dates (folders), each holding few hundreds Excel files, each holding thousands of data.

**Input**: MongoDB database; ZIP file with Excel 2003 reports. **Output**: data loaded in the SQL Server database.

For example you may have the **MongoDB database "Supermarket"** holding information about some vendors and some products and **a set of Excel files** (*.xls) holding information about the sales in the different super¬markets.

## Problem #2 - Generate **PDF Reports**
Your task is to generate a **PDF** reports summarizing information from the SQL Server.

**Input**: SQL Server database. **Output**: PDF report.

## Problem #3 - Generate **XML Report**
Your task is to create a C# program to generate report in **XML** format.
Save the report in an "xml" file.

**Input**: SQL Server database. **Output**: XML report.

## Problem #4 - JSON Reports

Your task is to write a program to create report for each product in **JSON format** and save all reports in **MySQL**. All reports may look like the sample below and should be saved in the MySQL database as well as in the file system (in a folder called "Json-Reports", in files named "XX.json" where XX is the ID).

**Input**: SQL Server database. **Output**: a set of JSON files; data loaded in MySQL database.

## Problem #5 - Load data from XML
You must create XML file holding additional information by your choice. Your task is to read the **XML** file, parse it and save the data in the MongoDB database and in the SQL Server. Please think how your database schema / document model will support the additional data.

**Input**: XML file. **Output**: data loaded in the SQL Server; data loaded in the MongoDB.


## Problem #6 -Excel data
You are given a **SQLite** database holding more information for each product. Write a program to read the MySQL database of reports, read the information from SQLite and generate a single Excel 2007 file holding some information by your choice. You are not allowed to connect to the SQL Server or MongoDB databases to read information.

**Input**: SQLite database; MySQL database. **Output**: Excel 2007 file (.xlsx).

## Additional Requirements
 * Your main program logic should be a **C#** application (a set of modules, executed sequentially one after another).
 * Use non-commercial library to read the **ZIP** file.
 * For reading the Excel 2003 files (.xls) use **ADO.NET** (without ORM or third-party libraries).
 * MySQL should be accessed through **OpenAccess ORM** (research it). 
 * SQL Server should be accessed through **Entity Framework**.
 * You are free to use "code first" or "database first" approach or both for the ORM frameworks.
 * For the **PDF export** use a non-commercial third party framework.
 * The **XML** files should be read / written through the standard .NET parsers (by your choice).
 * For **JSON** serializations use a non-commercial library / framework of your choice.
 * **MongoDB** should be accessed through the Official MongoDB C# Driver.
 * The **SQLite** embedded database should be accesses though its Entity Framework provider.
 * For creating the **Excel 2007** files (.xlsx) use a third-party non-commercial library.