using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppContext : DbContext
    {
        //private static volatile bool _initialized;

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            //if (!Database.IsRelational() || !_initialized)
            //{
            //    Database.EnsureDeleted();
            //    Database.EnsureCreated();
            //    _initialized = true;
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateStudents(modelBuilder);
            CreateTeachers(modelBuilder);
            CreateLectures(modelBuilder);
            CreateHometasks(modelBuilder);
            CreateGroups(modelBuilder);
            CreateAttendance(modelBuilder);
            CreateSchedule(modelBuilder);

            //DbInitializer.FillDb(modelBuilder);
        }

        private void CreateStudents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentDb>(entity =>
            {
                entity.ToTable("students");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id)
                    .HasColumnName("student_id")
                    .ValueGeneratedOnAdd();
                entity.Property(s => s.FullName)
                    .IsRequired().HasColumnName("full_name")
                    .HasColumnType("varchar");
                entity.Property(s => s.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar");
                entity.HasIndex(s => s.Email)
                    .IsUnique();
                entity.Property(s => s.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasColumnType("varchar");
                entity.HasIndex(s => s.PhoneNumber)
                    .IsUnique();
                entity.Property(s => s.GroupId)
                    .HasColumnName("group_id");
                entity.HasOne(s => s.Group)
                    .WithMany(g => g.Students)
                    .HasForeignKey(s => s.GroupId);
            });
        }

        private void CreateTeachers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherDb>(entity =>
            {
                entity.ToTable("teachers");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id)
                    .HasColumnName("teacher_id")
                    .ValueGeneratedOnAdd();
                entity.Property(t => t.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasColumnType("varchar");
                entity.Property(t => t.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar");
                entity.HasIndex(t => t.Email)
                    .IsUnique();
                entity.Property(t => t.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasColumnType("varchar");
                entity.HasIndex(t => t.PhoneNumber)
                    .IsUnique();
            });
        }

        private void CreateLectures(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LectureDb>(entity =>
            {
                entity.ToTable("lectures");
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Id)
                    .HasColumnName("lecture_id")
                    .ValueGeneratedOnAdd();
                entity.Property(l => l.Name)
                    .IsRequired()
                    .HasColumnName("lecture_name")
                    .HasColumnType("varchar");
                entity.Property(l => l.TeacherId)
                    .HasColumnName("teacher_id")
                    .HasColumnType("smallint");
                entity.HasOne(l => l.Teacher)
                    .WithMany(t => t.Lectures)
                    .HasForeignKey(l => l.TeacherId);
            });
        }

        private void CreateHometasks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HometaskDb>(entity =>
            {
                entity.ToTable("hometasks");
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Id)
                    .HasColumnName("hometask_id")
                    .ValueGeneratedOnAdd();
                entity.Property(h => h.HometaskDate)
                    .IsRequired()
                    .HasColumnName("date")
                    .HasColumnType("date");
                entity.Property(h => h.Mark)
                    .IsRequired()
                    .HasColumnName("mark")
                    .HasColumnType("smallint");
                entity.Property(h => h.StudentId)
                    .HasColumnName("student_id");
                entity.Property(h => h.LectureId)
                    .HasColumnName("lecture_id");
                entity.HasOne(h => h.Student)
                    .WithMany(s => s.Hometasks)
                    .HasForeignKey(h => h.StudentId);
                entity.HasOne(h => h.Lecture)
                    .WithMany(l => l.Hometasks)
                    .HasForeignKey(h => h.LectureId);
            });
        }

        private void CreateGroups(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupDb>(entity =>
            {
                entity.ToTable("groups");
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Id)
                    .HasColumnName("group_id")
                    .ValueGeneratedOnAdd();
                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasColumnName("group_name")
                    .HasColumnType("varchar");
                entity.HasIndex(g => g.Name)
                    .IsUnique();
            });
        }

        private void CreateAttendance(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceDb>(entity =>
            {
                entity.ToTable("attendance");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id)
                    .HasColumnName("attendance_id")
                    .ValueGeneratedOnAdd();
                entity.HasIndex(a => new { a.StudentId, a.LectureId, a.LectureDate })
                    .IsUnique();
                entity.Property(a => a.StudentId)
                    .HasColumnName("student_id");
                entity.Property(a => a.LectureId)
                    .HasColumnName("lecture_id");
                entity.Property(a => a.LectureDate)
                    .IsRequired()
                    .HasColumnName("date")
                    .HasColumnType("date");
                entity.Property(a => a.Presence)
                    .IsRequired()
                    .HasColumnName("presence")
                    .HasColumnType("boolean");
                entity.Property(a => a.HometaskDone)
                    .IsRequired()
                    .HasColumnName("hometask_done")
                    .HasColumnType("boolean");
                entity.HasOne(a => a.Student)
                    .WithMany(s => s.Lectures)
                    .HasForeignKey(a => a.StudentId);
                entity.HasOne(a => a.Lecture)
                    .WithMany(l => l.Students)
                    .HasForeignKey(a => a.LectureId);
            });
        }

        private void CreateSchedule(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduleDb>(entity =>
            {
                entity.ToTable("schedule");
                entity.HasKey(s => new { s.LectureId, s.GroupId });
                entity.Property(s => s.LectureId)
                    .HasColumnName("lecture_id");
                entity.Property(s => s.GroupId)
                    .HasColumnName("group_id");
                entity.HasOne(s => s.Lecture)
                    .WithMany(l => l.Schedule)
                    .HasForeignKey(a => a.LectureId);
                entity.HasOne(s => s.Group)
                    .WithMany(g => g.Schedule)
                    .HasForeignKey(a => a.GroupId);
            });
        }
    }
}
