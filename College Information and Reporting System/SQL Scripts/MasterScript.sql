USE [CIRS]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAttendanceRateForCourse]    Script Date: 20/12/2025 17:16:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Maryam.G
-- Create date: 20/12/2025
-- Description:	Gets the attendance rate for a specific course
-- Returns: Present, Absent, Late, Authorised absence amount for a specific course
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAttendanceRateForCourse]
	@courseId INT
AS
BEGIN
	SELECT 
	c.courseId, c.courseName,
	count(*) as totalSessions,
	SUM(case when a.attendanceStatus=0 Then 1 else 0 end) as [Present],
	SUM(case when a.attendanceStatus=1 Then 1 else 0 end) as [Absent],
	SUM(case when a.attendanceStatus=2 Then 1 else 0 end) as [Late],
	SUM(case when a.attendanceStatus=3 Then 1 else 0 end) as [AuthorisedAbsence]
	from attendances a left join courses c on a.courseId=c.courseId
	where a.courseId=@courseId
	group by c.courseId, c.courseName;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_GetStudentAttendanceRisk]    Script Date: 20/12/2025 21:49:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Maryam.G
-- Create date: 20/12/2025
-- Description:	Outputs each student and their attendance risk level:
--				>85 - Acceptable
--				70 - 85 - Monitor
--				<70 (or user can enter optional risk amount) - At risk

-- =============================================
CREATE PROCEDURE [dbo].[usp_GetStudentAttendanceRisk]
	@startDate date = '1900-01-01',
	@endDate date = '9999-12-31',
	@riskLevel decimal(5,2) = 70
AS
BEGIN
	--Gathers all attendance summaries for each student within a certain date interval
	with attendanceCTE as(
		 select
		 s.studentid, s.studentFirstName, s.studentLastName,
		 count(a.AttendanceId) as TotalSessions,
         sum(case when a.AttendanceStatus = 0 then 1 else 0 end) as PresentSessions
		 from attendances a left join students s on a.studentid=s.studentid
	     where a.attendanceTime between @startdate and @enddate
		 group by s.studentid, s.studentfirstname,s.studentlastname
	)

	--Selects Risk level based off the attendance summaries and outputs it
	SELECT 
	studentid, studentfirstname,studentlastname,
	cast((PresentSessions*100.00 / nullif(TotalSessions,0)) as decimal(5,2)) as attendanceAvg,
	case 
		when (cast((PresentSessions*100.00 / nullif(TotalSessions,0)) as decimal(5,2))) < @riskLevel then 'At risk'
		when (cast((PresentSessions*100.00 / nullif(TotalSessions,0)) as decimal(5,2))) between 70 and 85 then 'Monitor'
		else 'Acceptable'
	end as 'RiskStatus'
	from attendanceCTE
	order by attendanceAvg asc;

END


GO
/****** Object:  StoredProcedure [dbo].[usp_GetStudentAttendanceSummary]    Script Date: 20/12/2025 17:28:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Maryam.G
-- Create date: 18/12/2025
-- Description:	Gets the attendance rows for a specific student
-- Returns: Present, Absent, Late, Authorised absence amount for student
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetStudentAttendanceSummary]
	@studentId INT
AS
BEGIN
	SELECT 
	s.studentId, s.studentfirstname, s.studentLastName,
	count(a.attendanceId) as TotalSessions,
	SUM(case when a.attendanceStatus=0 Then 1 else 0 end) as [Present],
	SUM(case when a.attendanceStatus=1 Then 1 else 0 end) as [Absent],
	SUM(case when a.attendanceStatus=2 Then 1 else 0 end) as [Late],
	SUM(case when a.attendanceStatus=3 Then 1 else 0 end) as [AuthorisedAbsence]

	from Students s 
	left join Attendances a ON s.studentId=a.studentId
	where s.studentId=@studentId
	GROUP BY s.StudentId, studentFirstName, studentLastName;
END




-- =============================================
-- Author:		Maryam.G
-- Create date: 20/12/2025
-- Description:	Gets all attendance rates for courses
-- =============================================
GO
CREATE VIEW vw_courseAttendanceSummary
AS
	SELECT 
	c.courseId,c.courseName,
	count(*) as totalSessions,
	SUM(CASE WHEN a.attendanceStatus=0 THEN 1 ELSE 0 END) as presentNumber
	from attendances a inner join courses c on a.courseId=c.courseId
	group by c.courseId, c.courseName


GO
create view vw_courseOverview
as
SELECT
    fq.courseName,
    fq.courseId,
    fq.departmentName,
    fq.departmentCode,
    fq.departmentHead,
    fq.totalStudents,
    fq.budgetPerStudent,
    sq.avgAttendance
FROM (

	--First query
	select 
	c.courseName,c.courseId,d.departmentName,d.departmentCode,d.departmentHead,
	COUNT(cs.studentsstudentId) as totalStudents,
	cast(c.budget/CAST(COUNT(cs.studentsstudentId)as decimal(10,2)) as decimal(10,2)) as budgetPerStudent
	from courses c inner join departments d on c.departmentId=d.departmentId
	left join CourseStudent cs on c.courseId=cs.coursescourseId
	GROUP BY
		c.courseId,
		c.courseName,
		c.budget,
		d.departmentName,
		d.departmentCode,
		d.departmentHead) fq

	--Second query
	left join (
		select 
		a.courseId, 
		cast((SUM(case when a.attendanceStatus=0 then 1 else 0 end)/cast(COUNT(a.attendanceId) as decimal))*100 as decimal(5,2)) as avgAttendance
		from attendances a
		group by courseId
	) sq on fq.courseId=sq.courseid;

GO

create function [dbo].[fn_GetStudentAttendanceRisk]
(
	@startDate date = '1900-01-01',
	@endDate date = '9999-12-31',
	@riskLevel decimal(5,2) = 70
) returns table
AS
return (
	--Gathers all attendance summaries for each student within a certain date interval
	with attendanceCTE as(
		 select
		 s.studentid, s.studentFirstName, s.studentLastName,
		 count(a.AttendanceId) as TotalSessions,
         sum(case when a.AttendanceStatus = 0 then 1 else 0 end) as PresentSessions
		 from attendances a left join students s on a.studentid=s.studentid
	     where a.attendanceTime between @startdate and @enddate
		 group by s.studentid, s.studentfirstname,s.studentlastname
	)

	--Selects Risk level based off the attendance summaries and outputs it
	SELECT 
	studentid, studentfirstname,studentlastname,
	cast((PresentSessions*100.00 / nullif(TotalSessions,0)) as decimal(5,2)) as attendanceAvg,
	case 
		when (cast((PresentSessions*100.00 / nullif(TotalSessions,0)) as decimal(5,2))) < @riskLevel then 'At risk'
		when (cast((PresentSessions*100.00 / nullif(TotalSessions,0)) as decimal(5,2))) between 70 and 85 then 'Monitor'
		else 'Acceptable'
	end as 'RiskStatus'
	from attendanceCTE

);


GO

CREATE VIEW vw_AtRiskStudents AS
SELECT *
FROM dbo.fn_GetStudentAttendanceRisk(
    '1900-01-01',
    '9999-12-31',
    70
);

--Kimball Data Warehouse creation script
create database CIRSDW;

use cirsdw;

--DimDate creation
CREATE TABLE DimDate (
    DateKey INT PRIMARY KEY, --YYYYMMDD format
    FullDate DATETIME2 NOT NULL,
    Year INT,
    Month INT,
    MonthName VARCHAR(20),
    Day INT,
    DayName VARCHAR(20),
);


--DimDate insertions from 2020 to 2030
DECLARE @StartDate DATETIME2 = '2020-01-01'
DECLARE @EndDate DATETIME2 = '2030-12-31'

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
    DepartmentName nvarchar(MAX)
);

--dimstudents creation
CREATE TABLE DimStudent (
    StudentKey INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT,
    StudentFullName nvarchar(MAX),
	StudentFirstName nvarchar(MAX),
	StudentLastName nvarchar(MAX),
	StudentDob date,
);

--dimcourses creation
CREATE TABLE DimCourse (
    CourseKey INT IDENTITY(1,1) PRIMARY KEY,
    CourseID INT,           
    CourseName nvarchar(MAX),
    DepartmentName nvarchar(MAX)
);


--fact attendance creation
CREATE TABLE FactAttendance (
    AttendanceKey INT IDENTITY(1,1) PRIMARY KEY,
    StudentKey INT NOT NULL,
    CourseKey INT NOT NULL,
    DateKey INT NOT NULL,
    AttendanceStatus nvarchar(MAX),

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