
-- =============================================
-- Author:		Maryam.G
-- Create date: 20/12/2025
-- Description:	Gets all attendance rates for courses
-- =============================================
CREATE VIEW vw_courseAttendanceSummary
AS
	SELECT 
	c.courseId,c.courseName,
	count(*) as totalSessions,
	SUM(CASE WHEN a.attendanceStatus=0 THEN 1 ELSE 0 END) as presentNumber
	from attendances a inner join courses c on a.courseId=c.courseId
	group by c.courseId, c.courseName

