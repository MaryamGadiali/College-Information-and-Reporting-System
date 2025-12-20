
using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Models;

namespace College_Information_and_Reporting_System
{
    public class DatabaseSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.students.Any()) //prevents reseeding on every startup to avoid duplicates
            {
                var departments = SeedDepartments(context);
                var courses = SeedCourses(context, departments);
                var students = SeedStudents(context, courses);
                //_logger.LogInformation("Look here 3");
                //_logger.LogInformation(students.First().studentLastName);
                SeedAttendance(context, students, courses);

                await context.SaveChangesAsync(); //commits all updates
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) //runs when applicstion stops
        {
            return Task.CompletedTask;
        }

        private List<Department> SeedDepartments(AppDbContext context) {
            if (!context.departments.Any())
            {
                var departments = new List<Department>
                {
                    new Department { departmentName = "Math", departmentCode="MA", departmentHead="John Smith"},
                    new Department { departmentName = "English", departmentCode="EN", departmentHead="Joe Kurt"},
                    new Department { departmentName = "Science", departmentCode="SC", departmentHead="Mary Allen"}
                };


                context.departments.AddRange(departments);
                return departments;
            }
            else
            {
                return context.departments.ToList(); ;
            }
        }
        private List<Course> SeedCourses(AppDbContext context, List<Department> departments) {

            if (!context.courses.Any())
            {
                var mathDept = departments.First(d => d.departmentName == "Math");
                var englishDept = departments.First(d => d.departmentName == "English");
                var scienceDept = departments.First(d => d.departmentName == "Science");

                var courses = new List<Course>
                {
                    new Course { courseName = "Pure Maths", department = mathDept, budget = 50000 },
                    new Course { courseName = "Statistics", department = mathDept, budget = 40000 },
                    new Course { courseName = "Literature", department = englishDept, budget = 60000 },
                    new Course { courseName = "Language", department = englishDept, budget = 35000 },
                    new Course { courseName = "Chemistry", department = scienceDept, budget = 30000 },
                    new Course { courseName = "Physics", department = scienceDept, budget = 25000 }
                };

                context.courses.AddRange(courses);
                return courses;

                //var pureMaths = context.courses.First(p => p.courseName == "Pure Maths");
                //var Statistics = context.courses.First(p => p.courseName == "Statistics");
                //var Literature = context.courses.First(p => p.courseName == "Literature");
                //var Language = context.courses.First(p => p.courseName == "Language");
                //var Chemistry = context.courses.First(p => p.courseName == "Chemistry");
                //var Physics = context.courses.First(p => p.courseName == "Physics");

                //mathDept.courseList.Add(pureMaths);
                //mathDept.courseList.Add(Statistics);
                //englishDept.courseList.Add(Literature);
                //englishDept.courseList.Add(Language);
                //scienceDept.courseList.Add(Chemistry);
                //scienceDept.courseList.Add(Physics);

            }
            else
            {
                return context.courses.ToList();
            }
        
        }
        private List<Student> SeedStudents(AppDbContext context, List<Course> courses) {
            
            if (!context.students.Any())
            {
                var pureMaths = courses.First(p => p.courseName == "Pure Maths");
                var Statistics = courses.First(p => p.courseName == "Statistics");
                var Literature = courses.First(p => p.courseName == "Literature");
                var Language = courses.First(p => p.courseName == "Language");
                var Chemistry = courses.First(p => p.courseName == "Chemistry");
                var Physics = courses.First(p => p.courseName == "Physics");

                var students = new List<Student>
                {
                    new Student { studentFirstName = "John", studentLastName = "Doe", studentGender=Enums.Gender.Male, courses ={pureMaths,Physics}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2023-09-01"), studentEthnicity=Enums.Ethnicity.White },
                    new Student { studentFirstName = "Jane", studentLastName = "Smith", studentGender=Enums.Gender.Female, courses ={ Statistics, Physics}, status = Enums.StudentStatus.Completed, studentStartDate = DateOnly.Parse("2022-09-01"), studentEndDate = DateOnly.Parse("2024-01-01"), studentEthnicity=Enums.Ethnicity.BlackBritish },
                    new Student { studentFirstName = "Alice", studentLastName = "Johnson", studentGender=Enums.Gender.Female, courses ={ Literature, Chemistry}, status = Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2023-09-01"), studentEthnicity = Enums.Ethnicity.AsianOrAsianBritish },
                    new Student { studentFirstName = "Bob", studentLastName = "Lee",studentGender=Enums.Gender.Male,  courses ={ Literature, Language}, status = Enums.StudentStatus.Withdrawn, studentStartDate = DateOnly.Parse("2022-09-01"), studentEndDate=DateOnly.Parse("2022-11-01"), studentEthnicity = Enums.Ethnicity.PreferNotToSay },
                    new Student { studentFirstName = "Charlie", studentLastName = "Brown", studentGender=Enums.Gender.Other, courses ={ Chemistry, Physics}, status=Enums.StudentStatus.NotStarted, studentStartDate = DateOnly.Parse("2023-09-01"), studentEthnicity=Enums.Ethnicity.Black },
                    new Student { studentFirstName = "Vanessa", studentLastName = "Lee", studentGender=Enums.Gender.Female, courses ={ Statistics, Chemistry}, status = Enums.StudentStatus.Suspended, studentStartDate = DateOnly.Parse("2022-09-01"), studentEthnicity= Enums.Ethnicity.White },
                    new Student { studentFirstName = "Emma", studentLastName = "Davis", studentGender=Enums.Gender.Female, courses ={ Language, pureMaths}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2021-09-01"), studentEthnicity=Enums.Ethnicity.AsianOrAsianBritish },
                    new Student { studentFirstName = "Michael", studentLastName = "Wilson", studentGender=Enums.Gender.Male, courses ={ pureMaths, Statistics}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2022-09-01"), studentEthnicity=Enums.Ethnicity.White },
                    new Student { studentFirstName = "Sarah", studentLastName = "Taylor", studentGender=Enums.Gender.Female, courses ={ Language, Literature}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2023-09-01"), studentEthnicity=Enums.Ethnicity.White },
                    new Student { studentFirstName = "David", studentLastName = "Miller", studentGender=Enums.Gender.Male, courses ={ Physics, Chemistry}, status=Enums.StudentStatus.Completed, studentStartDate = DateOnly.Parse("2021-09-01"), studentEndDate = DateOnly.Parse("2023-06-01"), studentEthnicity=Enums.Ethnicity.White },
                    new Student { studentFirstName = "Priya", studentLastName = "Patel", studentGender=Enums.Gender.Female, courses ={ Statistics, Chemistry}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2022-09-01"), studentEthnicity=Enums.Ethnicity.AsianOrAsianBritish },
                    new Student { studentFirstName = "Ahmed", studentLastName = "Khan", studentGender=Enums.Gender.Male, courses ={ Physics, pureMaths}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2023-09-01"), studentEthnicity=Enums.Ethnicity.AsianOrAsianBritish },
                    new Student { studentFirstName = "Lucy", studentLastName = "Green", studentGender=Enums.Gender.Female, courses ={ Literature}, status=Enums.StudentStatus.NotStarted, studentStartDate = DateOnly.Parse("2024-09-01"), studentEthnicity=Enums.Ethnicity.White },
                    new Student { studentFirstName = "Tom", studentLastName = "Harris", studentGender=Enums.Gender.Male, courses ={ Language, Statistics}, status=Enums.StudentStatus.Withdrawn, studentStartDate = DateOnly.Parse("2022-09-01"), studentEndDate=DateOnly.Parse("2023-02-01"), studentEthnicity=Enums.Ethnicity.White },
                    new Student { studentFirstName = "Aisha", studentLastName = "Ali", studentGender=Enums.Gender.Female, courses ={ Chemistry}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2023-09-01"), studentEthnicity=Enums.Ethnicity.Black },
                    new Student { studentFirstName = "James", studentLastName = "Walker", studentGender=Enums.Gender.Male, courses ={ pureMaths}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2021-09-01"), studentEthnicity=Enums.Ethnicity.White },
                    new Student { studentFirstName = "Nina", studentLastName = "Lopez", studentGender=Enums.Gender.Female, courses ={ Statistics}, status=Enums.StudentStatus.Completed, studentStartDate = DateOnly.Parse("2020-09-01"), studentEndDate=DateOnly.Parse("2022-06-01"), studentEthnicity=Enums.Ethnicity.PreferNotToSay },
                    new Student { studentFirstName = "Leo", studentLastName = "Martin", studentGender=Enums.Gender.Male, courses ={ Physics}, status=Enums.StudentStatus.Active, studentStartDate = DateOnly.Parse("2023-09-01"), studentEthnicity=Enums.Ethnicity.White }
                };
                //_logger.LogInformation("Reached 1");
                //_logger.LogInformation(students.First().studentLastName);
                context.students.AddRange(students);
                
                return students;

            }
            else
            {
                //_logger.LogInformation("Reached 2");
                return context.students.ToList();
            }
        
        }
        private void SeedAttendance(AppDbContext context, List<Student> students, List<Course> courses) { 

            
            if (!context.attendances.Any())
            {
                var john = students.First(s => s.studentFirstName == "John");
                var jane = students.First(s => s.studentFirstName == "Jane");
                var alice = students.First(s => s.studentFirstName == "Alice");

                var pureMaths = courses.First(p => p.courseName == "Pure Maths");
                var Statistics = courses.First(p => p.courseName == "Statistics");
                var Literature = courses.First(p => p.courseName == "Literature");
                var Language = courses.First(p => p.courseName == "Language");
                var Chemistry = courses.First(p => p.courseName == "Chemistry");
                var Physics = courses.First(p => p.courseName == "Physics");

                var attendanceRecords = new List<Attendance>
                {
                    //new Attendance{attendanceTime=DateTime.Parse("2023-09-02 12:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=john, course=pureMaths},
                    //new Attendance{attendanceTime=DateTime.Parse("2023-09-03 12:00"), attendanceStatus=Enums.AttendanceStatus.Absent, student=john, course=Physics},
                    //new Attendance{attendanceTime=DateTime.Parse("2023-09-03 12:00"), attendanceStatus=Enums.AttendanceStatus.Absent, student=jane, course=Physics},
                    //new Attendance{attendanceTime=DateTime.Parse("2023-09-04 12:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=jane, course=Statistics},
                    //new Attendance{attendanceTime=DateTime.Parse("2023-09-05 12:00"), attendanceStatus=Enums.AttendanceStatus.Late, student=alice, course=Literature},
                    //new Attendance{attendanceTime=DateTime.Parse("2023-09-06 12:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=alice, course=Chemistry}

                    new Attendance{attendanceTime=DateTime.Parse("2023-09-02 09:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=john, course=pureMaths},
                    new Attendance{attendanceTime=DateTime.Parse("2023-09-04 09:00"), attendanceStatus=Enums.AttendanceStatus.Late, student=john, course=pureMaths},
                    new Attendance{attendanceTime=DateTime.Parse("2023-09-06 09:00"), attendanceStatus=Enums.AttendanceStatus.Absent, student=john, course=Physics},

                    new Attendance{attendanceTime=DateTime.Parse("2023-09-03 10:00"), attendanceStatus=Enums.AttendanceStatus.Absent, student=jane, course=Physics},
                    new Attendance{attendanceTime=DateTime.Parse("2023-09-05 10:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=jane, course=Statistics},
                    new Attendance{attendanceTime=DateTime.Parse("2023-09-07 10:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=jane, course=Statistics},

                    new Attendance{attendanceTime=DateTime.Parse("2023-09-02 11:00"), attendanceStatus=Enums.AttendanceStatus.Late, student=alice, course=Literature},
                    new Attendance{attendanceTime=DateTime.Parse("2023-09-04 11:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=alice, course=Chemistry},
                    new Attendance{attendanceTime=DateTime.Parse("2023-09-06 11:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=alice, course=Chemistry},

                    new Attendance{attendanceTime=DateTime.Parse("2023-09-03 09:00"), attendanceStatus=Enums.AttendanceStatus.Absent, student=students[8], course=Language},
                    new Attendance{attendanceTime=DateTime.Parse("2023-09-05 09:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=students[8], course=Literature},

                    new Attendance{attendanceTime=DateTime.Parse("2023-09-04 10:00"), attendanceStatus=Enums.AttendanceStatus.Present, student=students[10], course=Chemistry},
                    new Attendance{attendanceTime=DateTime.Parse("2023-09-06 10:00"), attendanceStatus=Enums.AttendanceStatus.Late, student=students[10], course=Statistics},

                    new Attendance{attendanceTime=DateTime.Parse("2023-09-05 12:00"), attendanceStatus=Enums.AttendanceStatus.Absent, student=students[12], course=Literature}

                  };
                context.attendances.AddRange(attendanceRecords);
            }
            ;
            }
        
        }
    }

