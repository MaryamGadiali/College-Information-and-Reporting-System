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