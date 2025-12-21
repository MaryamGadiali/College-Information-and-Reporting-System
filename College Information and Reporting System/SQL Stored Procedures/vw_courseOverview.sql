
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


