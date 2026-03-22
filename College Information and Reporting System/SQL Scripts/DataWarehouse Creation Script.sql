--Kimball Data Warehouse creation script

create database CIRSDW;

use CIRSDW;

--DimDate creation
CREATE TABLE DimDate (
    DateKey INT PRIMARY KEY, --YYYYMMDD format
    FullDate DATE NOT NULL,
    Year INT,
    Month INT,
    MonthName VARCHAR(20),
    Day INT,
    DayName VARCHAR(20),
);


--DimDate insertions from 2020 to 2030
DECLARE @StartDate DATE = '2020-01-01'
DECLARE @EndDate DATE = '2030-12-31'

WHILE @StartDate <= @EndDate
BEGIN
    INSERT INTO DimDate
    SELECT
        CONVERT(INT, FORMAT(@StartDate, 'yyyyMMdd')),
        @StartDate,
        YEAR(@StartDate),
        MONTH(@StartDate),
        DATENAME(MONTH, @StartDate),
        DAY(@StartDate),
        DATENAME(WEEKDAY, @StartDate)

    SET @StartDate = DATEADD(DAY, 1, @StartDate)
END

--dimdepartments creation
CREATE TABLE DimDepartment (
    DepartmentKey INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentID INT,
    DepartmentName VARCHAR(256)
);

--dimstudents creation
CREATE TABLE DimStudent (
    StudentKey INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT,
    StudentFullName VARCHAR(256),
	StudentFirstName VARCHAR(256),
	StudentLastName VARCHAR(256),
	StudentDob Date,
);

--dimcourses creation
CREATE TABLE DimCourse (
    CourseKey INT IDENTITY(1,1) PRIMARY KEY,
    CourseID INT,           
    CourseName VARCHAR(256),
    DepartmentName VARCHAR(256)
);


--fact attendance creation
CREATE TABLE FactAttendance (
    AttendanceKey INT IDENTITY(1,1) PRIMARY KEY,
    StudentKey INT NOT NULL,
    CourseKey INT NOT NULL,
    DateKey INT NOT NULL,
    AttendanceStatus VARCHAR(256),

    -- Foreign Keys
    CONSTRAINT FK_Fact_Student FOREIGN KEY (StudentKey)
        REFERENCES DimStudent(StudentKey),

    CONSTRAINT FK_Fact_Course FOREIGN KEY (CourseKey)
        REFERENCES DimCourse(CourseKey),

    CONSTRAINT FK_Fact_Date FOREIGN KEY (DateKey)
        REFERENCES DimDate(DateKey),
);

--factenrollment creation
CREATE TABLE FactEnrollment (
    EnrollmentKey INT IDENTITY(1,1) PRIMARY KEY,
    StudentKey INT NOT NULL,
    CourseKey INT NOT NULL,

    CONSTRAINT FK_Enroll_Student FOREIGN KEY (StudentKey)
        REFERENCES DimStudent(StudentKey),

    CONSTRAINT FK_Enroll_Course FOREIGN KEY (CourseKey)
        REFERENCES DimCourse(CourseKey),

);




