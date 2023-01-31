using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DbInitializer
    {
        public static void FillDb(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentDb>().HasData(
                new StudentDb[]
                {
                    new StudentDb { Id = 1, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com", PhoneNumber = "89169475896", GroupId = 1 },
                    new StudentDb { Id = 2, FullName = "Maxim Zakharov", Email = "maximzakharov@gmail.com", PhoneNumber = "89129475994", GroupId = 1 },
                    new StudentDb { Id = 3, FullName = "Kirill Morozov", Email = "kirillmorozov@gmail.com", PhoneNumber = "89159475789", GroupId = 1 },
                    new StudentDb { Id = 4, FullName = "Ivan Svistunov", Email = "ivansvistunov@gmail.com", PhoneNumber = "89129491486", GroupId = 1 },

                    new StudentDb { Id = 5, FullName = "Igor Sokolov", Email = "igorsokolov@gmail.com", PhoneNumber = "88329445692", GroupId = 2 },
                    new StudentDb { Id = 6, FullName = "Sergey Koshkin", Email = "sergeykoshkin@gmail.com", PhoneNumber = "89169435789", GroupId = 2 },
                    new StudentDb { Id = 7, FullName = "Anastasia Soboleva", Email = "anastasiasoboleva@gmail.com", PhoneNumber = "89139435787", GroupId = 2 },
                    new StudentDb { Id = 8, FullName = "Kristina Kereeva", Email = "kristinakereeva@gmail.com", PhoneNumber = "83129438452", GroupId = 2 },

                    new StudentDb { Id = 9, FullName = "Maria Larina", Email = "marialarina@gmail.com", PhoneNumber = "89459445623", GroupId = 3 },
                    new StudentDb { Id = 10, FullName = "Olga Smirnova", Email = "olgasmirnova@gmail.com", PhoneNumber = "89147835231", GroupId = 3 },
                    new StudentDb { Id = 11, FullName = "Viktor Mironov", Email = "viktormironov@gmail.com", PhoneNumber = "89157535221", GroupId = 3 },
                    new StudentDb { Id = 12, FullName = "Konstantin Gusev", Email = "konstantingusev@gmail.com", PhoneNumber = "89127534610", GroupId = 3 }
                });

            modelBuilder.Entity<TeacherDb>().HasData(
                new TeacherDb[]
                {
                    new TeacherDb { Id = 1, FullName = "Ivan Ivanov", Email = "ivanivanov@gmail.com", PhoneNumber = "89166975213" },
                    new TeacherDb { Id = 2, FullName = "Peter Petrov", Email = "peterpetrov@gmail.com", PhoneNumber = "88166975649" },
                    new TeacherDb { Id = 3, FullName = "Peter Ivanov", Email = "peterivanov@gmail.com", PhoneNumber = "88126932678" },
                    new TeacherDb { Id = 4, FullName = "Larisa Mironova", Email = "larisamironova@gmail.com", PhoneNumber = "89177875231" },
                    new TeacherDb { Id = 5, FullName = "Nikolay Girin", Email = "nikolaygirin@gmail.com", PhoneNumber = "88496575634" },
                    new TeacherDb { Id = 6, FullName = "Galina Semenova", Email = "galinasemenova@gmail.com", PhoneNumber = "88166991679" }
                });

            modelBuilder.Entity<LectureDb>().HasData(
                new LectureDb[]
                {
                    new LectureDb { Id = 1, Name = "Maths", TeacherId = 1 },
                    new LectureDb { Id = 2, Name = "Probability theory", TeacherId = 1 },
                    new LectureDb { Id = 3, Name = "Physics", TeacherId = 2 },
                    new LectureDb { Id = 4, Name = "History", TeacherId = 3 },
                    new LectureDb { Id = 5, Name = "Biology", TeacherId = 4 },
                    new LectureDb { Id = 6, Name = "Chemistry", TeacherId = 5 },
                    new LectureDb { Id = 7, Name = "English", TeacherId = 6 }
                });

            modelBuilder.Entity<HometaskDb>().HasData(
                new HometaskDb[]
                {
                    new HometaskDb { Id = 1, HometaskDate = new DateTime(2013, 06, 12), Mark = 4, StudentId = 1, LectureId = 1 },
                    new HometaskDb { Id = 2, HometaskDate = new DateTime(2013, 06, 12), Mark = 5, StudentId = 2, LectureId = 1 },
                    new HometaskDb { Id = 3, HometaskDate = new DateTime(2013, 06, 12), Mark = 3, StudentId = 1, LectureId = 2 },
                    new HometaskDb { Id = 4, HometaskDate = new DateTime(2013, 06, 12), Mark = 4, StudentId = 2, LectureId = 2 }
                });

            modelBuilder.Entity<GroupDb>().HasData(
                new GroupDb[]
                {
                    new GroupDb { Id = 1, Name = "6501" },
                    new GroupDb { Id = 2, Name = "6502" },
                    new GroupDb { Id = 3, Name = "6503" }
                });

            modelBuilder.Entity<AttendanceDb>().HasData(
                new AttendanceDb[]
                {
                    //Maths
                    new AttendanceDb { Id = 1, LectureId = 1, StudentId = 1, LectureDate = new DateTime(2013, 03, 12), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 2, LectureId = 1, StudentId = 2, LectureDate = new DateTime(2013, 03, 12), Presence = true, HometaskDone = false },
                    new AttendanceDb { Id = 3, LectureId = 1, StudentId = 3, LectureDate = new DateTime(2013, 03, 12), Presence = false, HometaskDone = true },
                    new AttendanceDb { Id = 4, LectureId = 1, StudentId = 4, LectureDate = new DateTime(2013, 03, 12), Presence = true, HometaskDone = false },

                    new AttendanceDb { Id = 5, LectureId = 1, StudentId = 5, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = false },
                    new AttendanceDb { Id = 6, LectureId = 1, StudentId = 6, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 7, LectureId = 1, StudentId = 7, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = false },
                    new AttendanceDb { Id = 8, LectureId = 1, StudentId = 8, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = true },

                    new AttendanceDb { Id = 9, LectureId = 1, StudentId = 9, LectureDate = new DateTime(2013, 10, 13), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 10, LectureId = 1, StudentId = 10, LectureDate = new DateTime(2013, 10, 13), Presence = false, HometaskDone = false },
                    new AttendanceDb { Id = 11, LectureId = 1, StudentId = 11, LectureDate = new DateTime(2013, 10, 13), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 12, LectureId = 1, StudentId = 12, LectureDate = new DateTime(2013, 10, 13), Presence = true, HometaskDone = false },

                    //Probability theory
                    new AttendanceDb { Id = 13, LectureId = 2, StudentId = 1, LectureDate = new DateTime(2013, 03, 12), Presence = true, HometaskDone = false },
                    new AttendanceDb { Id = 14, LectureId = 2, StudentId = 2, LectureDate = new DateTime(2013, 03, 12), Presence = false, HometaskDone = false },
                    new AttendanceDb { Id = 15, LectureId = 2, StudentId = 3, LectureDate = new DateTime(2013, 03, 12), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 16, LectureId = 2, StudentId = 4, LectureDate = new DateTime(2013, 03, 12), Presence = false, HometaskDone = false },

                    //Physics
                    new AttendanceDb { Id = 17, LectureId = 3, StudentId = 1, LectureDate = new DateTime(2013, 03, 12), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 18, LectureId = 3, StudentId = 2, LectureDate = new DateTime(2013, 03, 12), Presence = true, HometaskDone = false },
                    new AttendanceDb { Id = 19, LectureId = 3, StudentId = 3, LectureDate = new DateTime(2013, 03, 12), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 20, LectureId = 3, StudentId = 4, LectureDate = new DateTime(2013, 03, 12), Presence = false, HometaskDone = false },

                    //History
                    new AttendanceDb { Id = 21, LectureId = 4, StudentId = 5, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 22, LectureId = 4, StudentId = 6, LectureDate = new DateTime(2013, 04, 14), Presence = false, HometaskDone = false },
                    new AttendanceDb { Id = 23, LectureId = 4, StudentId = 7, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 24, LectureId = 4, StudentId = 8, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = false },

                    //Biology
                    new AttendanceDb { Id = 25, LectureId = 5, StudentId = 9, LectureDate = new DateTime(2013, 10, 13), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 26, LectureId = 5, StudentId = 10, LectureDate = new DateTime(2013, 10, 13), Presence = true, HometaskDone = false },
                    new AttendanceDb { Id = 27, LectureId = 5, StudentId = 11, LectureDate = new DateTime(2013, 10, 13), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 28, LectureId = 5, StudentId = 12, LectureDate = new DateTime(2013, 10, 13), Presence = true, HometaskDone = false },

                    //English
                    new AttendanceDb { Id = 29, LectureId = 6, StudentId = 5, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 30, LectureId = 6, StudentId = 6, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 31, LectureId = 6, StudentId = 7, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = true },
                    new AttendanceDb { Id = 32, LectureId = 6, StudentId = 8, LectureDate = new DateTime(2013, 04, 14), Presence = true, HometaskDone = true },
                });

            modelBuilder.Entity<ScheduleDb>().HasData(
                new ScheduleDb[]
                {
                    new ScheduleDb { LectureId = 1, GroupId = 1 },
                    new ScheduleDb { LectureId = 1, GroupId = 2 },
                    new ScheduleDb { LectureId = 1, GroupId = 3 },
                    new ScheduleDb { LectureId = 2, GroupId = 1 },
                    new ScheduleDb { LectureId = 3, GroupId = 1 },
                    new ScheduleDb { LectureId = 4, GroupId = 2 },
                    new ScheduleDb { LectureId = 5, GroupId = 3 },
                    new ScheduleDb { LectureId = 6, GroupId = 3 },
                    new ScheduleDb { LectureId = 7, GroupId = 2 }
                });
        }
    }
}
