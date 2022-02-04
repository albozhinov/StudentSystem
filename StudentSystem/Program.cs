using Microsoft.EntityFrameworkCore;
using StudentSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentSystem
{
    class Program
    {
        private static Random random = new Random();
        static void Main()
        {
            using (var db = new StudentsSystemDbContext())
            {
                //db.Database.Migrate();
                //SeedDate(db);
                //PrintAllStudentsAndTheirHomeworksSub(db);
                //PrintAllCoursesWithResources(db);
                PrintAllCoursesWith5Recources(db);

            }
        }      

        private static void PrintAllStudentsAndTheirHomeworksSub(StudentsSystemDbContext db)
        {
            var allStudents = db.Students
                .Select(s => new
                {
                    Name = s.Name,
                    Homeworks = s.Homeworks.Select(h => new 
                    {
                        Content = h.Content,
                        Type = h.Type
                    })
                })
                .ToList();

            foreach (var student in allStudents)
            {
                Console.WriteLine($"Student name: {student.Name}");

                foreach (var homework in student.Homeworks)
                {
                    Console.WriteLine($"Content: {homework.Content}");
                    Console.WriteLine($"Type: {homework.Type}");
                }

                Console.WriteLine("===================================");

            }
        }
        private static void PrintAllCoursesWithResources(StudentsSystemDbContext db)
        {
            var allCourses = db.Courses
                               .OrderBy(c => c.Startdate)
                               .ThenByDescending(c => c.Enddate)
                               .Select(c => new
                               {
                                   Name = c.Name,
                                   Description = c.Description,
                                   Resources = c.Resources.Select(r => new
                                   {
                                       r.Name,
                                       r.URL,
                                       r.Type
                                   })
                               })
                               .ToList();

            foreach (var course in allCourses)
            {
                Console.WriteLine($"Course name: {course.Name} /n Description: {course.Description}");

                foreach (var recource in course.Resources)
                {
                    Console.WriteLine($"Resource: {recource.Name}; Type: {recource.Type}; Url: {recource.URL};");

                }
                Console.WriteLine("======================================================");
            }
        }

        private static void PrintAllCoursesWith5Recources(StudentsSystemDbContext db)
        {
            var result = db.Courses
                           .Where(c => c.Resources.Count > 5)
                           .OrderByDescending(c => c.Resources.Count)
                           .ThenByDescending(c => c.Startdate)
                           .Select(c => new
                           {
                               c.Name,
                               recourcesCount = c.Resources.Count
                           })
                           .ToList();

            foreach (var course in result)
            {
                Console.WriteLine($"Course name: {course.Name}; Resource count: {course.recourcesCount}");
            }
        }


        private static void SeedDate(StudentsSystemDbContext db)
        {
            const int totalStudents = 25;
            const int courses = 10;
            var currDate = DateTime.Now;



            //Students
            for (int i = 0; i < totalStudents; i++)
            {
                db.Add(new Student
                {
                    Name = $"Student {i}",
                    RegistrationDate = currDate.AddDays(i),
                    PhoneNumber = $"Random phone {i}"
                });
            }

            db.SaveChanges();

            //Courses
            var addedCourse = new List<Course>();

            for (int i = 0; i < courses; i++)
            {
                var course = new Course
                {
                    Name = $"Course {i}",
                    Description = $"Course Details {i}",
                    Price = 100 + i,
                    Startdate = currDate.AddDays(i),
                    Enddate = currDate.AddDays(20 + i),
                };
                addedCourse.Add(course);
                db.Courses.Add(course);
            }
            db.SaveChanges();

            //Student in courses
            var studentsIds = db
                                .Students
                                .Select(s => s.Id)
                                .ToList();

            for (int c = 0; c < courses; c++)
            {
                var currentCourse = addedCourse[c];
                var studentInCourse = random.Next(2, totalStudents / 2);

                for (int j = 0; j < studentInCourse; j++)
                {
                    var studentId = studentsIds[random.Next(0, studentsIds.Count)];

                    if (!currentCourse.Students.Any(s => s.StudentId == studentId))
                    {
                        currentCourse.Students.Add(new StudentCourse
                        {
                            StudentId = studentId,
                            CourseId = currentCourse.Id
                        });
                    }
                    else
                    {
                        j--;
                    }
                }

                var resourcesIncourse = random.Next(2, 20);
                var types = new[] { 0, 1, 2, 999 };

                for (int i = 0; i < resourcesIncourse; i++)
                {
                    currentCourse.Resources.Add(new Resource
                    {
                        Name = $"Recourse {c} {i}",
                        URL = $"URL {c} {i}",
                        Type = (RecourceType)types[random.Next(0, types.Length)]
                    });
                }
            }
            db.SaveChanges();

            // Homeworks

            for (int i = 0; i < courses; i++)
            {
                var currCoirse = addedCourse[i];
                var studentsInCourseId = currCoirse
                    .Students
                    .Select(s => s.StudentId)
                    .ToList();

                for (int j = 0; j < studentsInCourseId.Count; j++)
                {
                    var totalHomeworks = random.Next(2, 5);

                    for (int k = 0; k < totalHomeworks; k++)
                    {
                        db.Homeworks.Add(new Homework
                        {
                            Content = $"Content homework {i}",
                            Submissiondate = currDate.AddDays(-i),
                            Type = ContentType.Zip,
                            StudentId = studentsInCourseId[j],
                            CourseId = currCoirse.Id
                        });
                    }
                    db.SaveChanges();
                }
                db.SaveChanges();
            }
        }
    }
}