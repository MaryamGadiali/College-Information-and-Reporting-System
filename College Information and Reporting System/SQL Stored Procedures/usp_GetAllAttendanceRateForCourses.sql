USE [CIRS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Maryam.G
-- Create date: 20/12/2025
-- Description:	Gets all attendance rates for courses
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAllAttendanceRateForCourses]
AS
BEGIN
	SELECT 
	c.courseId,c.courseName,
	count(*) as totalSessions,
	SUM(CASE WHEN a.attendanceStatus=0 THEN 1 ELSE 0 END) as presentNumber
	from attendances a inner join courses c on a.courseId=c.courseId
	group by c.courseId, c.courseName
END
