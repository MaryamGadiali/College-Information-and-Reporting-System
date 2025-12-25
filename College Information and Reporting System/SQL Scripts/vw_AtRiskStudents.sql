CREATE VIEW vw_AtRiskStudents AS
SELECT *
FROM dbo.fn_GetStudentAttendanceRisk(
    '1900-01-01',
    '9999-12-31',
    70
);