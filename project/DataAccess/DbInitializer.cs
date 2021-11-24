using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DbInitializer
    {
        public void FillDb(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentDb>().HasData(
                new StudentDb[]
                {
                    new StudentDb { Id = 1, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com", PhoneNumber = "89169475896" },
                    new StudentDb { Id = 2, FullName = "Maxim Zakharov", Email = "maximzakharov@gmail.com", PhoneNumber = "89129475994" }
                });

            modelBuilder.Entity<TeacherDb>().HasData(
                new TeacherDb[]
                {
                    new TeacherDb { Id = 1, FullName = "Ivan Ivanov", Email = "ivanivanov@gmail.com" },
                    new TeacherDb { Id = 2, FullName = "Peter Petrov", Email = "peterpetrov@gmail.com" }
                });

            modelBuilder.Entity<LectureDb>().HasData(
                new LectureDb[]
                {
                    new LectureDb { Id = 1, Name = "Maths", TeacherId = 1 },
                    new LectureDb { Id = 2, Name = "Physics", TeacherId = 2 }
                });

            modelBuilder.Entity<HometaskDb>().HasData(
                new HometaskDb[]
                {
                    new HometaskDb { Id = 1, HometaskDate = new DateTime(2013, 06, 12), Mark = 4, StudentId = 1, LectureId = 1 },
                    new HometaskDb { Id = 2, HometaskDate = new DateTime(2013, 06, 12), Mark = 5, StudentId = 2, LectureId = 1 },
                    new HometaskDb { Id = 3, HometaskDate = new DateTime(2013, 06, 12), Mark = 3, StudentId = 1, LectureId = 2 },
                    new HometaskDb { Id = 4, HometaskDate = new DateTime(2013, 06, 12), Mark = 4, StudentId = 2, LectureId = 2 }
                });

            modelBuilder.Entity<AttendanceDb>().HasData(
                new AttendanceDb[]
                {
                    new AttendanceDb { LectureId = 1, StudentId = 1, LectureDate = new DateTime(2013, 06, 12), Presence = true, HometaskDone = true },
                    new AttendanceDb { LectureId = 1, StudentId = 2, LectureDate = new DateTime(2013, 06, 13), Presence = false, HometaskDone = false },
                    new AttendanceDb { LectureId = 2, StudentId = 2, LectureDate = new DateTime(2013, 06, 14), Presence = false, HometaskDone = false }
                });
        }
    }
}
