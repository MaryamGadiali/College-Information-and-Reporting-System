USE [CIRS]
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