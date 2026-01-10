# College Information and Reporting System

# Overview
This is an end-to-end demonstration project designed to reflect how schools can manage, analyse, and report on student attendance and budget data.
It includes an API, a user friendly front end, PowerBI dashboard and SQL Sprocs/views/functions.
It is developed using ASP.NET in an MVC style.

# Set up and running the project
1. Open SSMS and create a database called CIRS.
2. Download the project, unzip it, and open in VS2022.
3. In the solution explorer, open College Information and Reporting System > appsetting.json > appsettings.Development.json
4. Edit line 9 with your connection string details.
5. On the top naivgation bar of VS2022 open View > Terminal.
6. Use "cd" to navigate to where the project is
7. Run "dotnet ef database update" and wait for it to finish
8. Now you can go to SSMS and open a query tab.
9. Run "Use CIRS".
10. Run "select * from students;" - if it runs successfully, then that means the db has set up correctly.
11. Now go back to the VS2022 window and go through the solution explorer to find College Information and Reporting System > SQL Scripts > MasterScript.sql
12. Copy and paste the contents of the file to SSMS and click execute.
13. Now you can click the green arrow and start running the application in VS2022. "https://localhost:7010/swagger/index.html" will automatically open, which contains the REST API
14. Navigate to "https://localhost:7010/home" to see the web interface
