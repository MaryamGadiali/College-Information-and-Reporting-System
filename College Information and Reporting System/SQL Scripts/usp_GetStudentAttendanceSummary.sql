USE [CIRS]
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
