using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UniversityContext : DbContext
    {
        private static volatile bool _initialized;
        public DbSet<StudentDb> Students { get; set; }
        public DbSet<TeacherDb> Teachers { get; set; }
        public DbSet<LectureDb> Lectures { get; set; }
        public DbSet<HometaskDb> Hometasks { get; set; }
        public DbSet<AttendanceDb> Attendance { get; set; }
        
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {            
            if (!Database.IsRelational() || !_initialized)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
                _initialized = true;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateStudents(modelBuilder);
            CreateTeachers(modelBuilder);
            CreateLectures(modelBuilder);
            CreateHometasks(modelBuilder);
            CreateAttendance(modelBuilder);

            var dataInitializer = new DbInitializer();
            dataInitializer.FillDb(modelBuilder);
        }

        private void CreateStudents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentDb>(entity =>
            {
                entity.ToTable("students");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasColumnName("student_id").ValueGeneratedOnAdd();
                entity.Property(s => s.FullName).IsRequired().HasColumnName("full_name").HasColumnType("varchar(30)");
                entity.Property(s => s.Email).IsRequired().HasColumnName("email").HasColumnType("varchar(30)");
                entity.Property(s => s.PhoneNumber).IsRequired().HasColumnName("phone_number").HasColumnType("varchar(18)");
            });
        }

        private void CreateTeachers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherDb>(entity =>
            {
                entity.ToTable("teachers");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasColumnName("teacher_id").ValueGeneratedOnAdd();
                entity.Property(t => t.FullName).IsRequired().HasColumnName("full_name").HasColumnType("varchar(30)");
                entity.Property(t => t.Email).IsRequired().HasColumnName("email").HasColumnType("varchar(30)");
            });
        }

        private void CreateLectures(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LectureDb>(entity =>
            {
                entity.ToTable("lectures");
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Id).HasColumnName("lecture_id").ValueGeneratedOnAdd();
                entity.Property(l => l.Name).IsRequired().HasColumnName("lecture_name").HasColumnType("varchar(30)");
                entity.Property(l => l.TeacherId).HasColumnName("teacher_id").HasColumnType("smallint");
                entity.HasOne(l => l.Teacher).WithMany(t => t.Lectures).HasForeignKey(l => l.TeacherId);
            });
        }

        private void CreateHometasks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HometaskDb>(entity =>
            {
                entity.ToTable("hometasks");
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Id).HasColumnName("hometask_id").ValueGeneratedOnAdd();
                entity.Property(h => h.HometaskDate).IsRequired().HasColumnName("date").HasColumnType("date");
                entity.Property(h => h.Mark).IsRequired().HasColumnName("mark").HasColumnType("smallint");
                entity.HasOne(h => h.Student).WithMany(s => s.Hometasks).HasForeignKey(h => h.StudentId);
                entity.Property(h => h.StudentId).HasColumnName("student_id");
                entity.HasOne(h => h.Lecture).WithMany(l => l.Hometasks).HasForeignKey(h => h.LectureId);
                entity.Property(h => h.LectureId).HasColumnName("lecture_id");
            });
        }
        private void CreateAttendance(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceDb>(entity =>
            {
                entity.ToTable("attendance");
                entity.HasKey(a => new { a.StudentId, a.LectureId, a.LectureDate });
                entity.Property(a => a.StudentId).HasColumnName("student_id");
                entity.Property(a => a.LectureId).HasColumnName("lecture_id");
                entity.Property(a => a.LectureDate).IsRequired().HasColumnName("date").HasColumnType("date");
                entity.Property(a => a.Presence).IsRequired().HasColumnName("presence").HasColumnType("boolean");
                entity.Property(a => a.HometaskDone).IsRequired().HasColumnName("hometask_done").HasColumnType("boolean");
                entity.HasOne(a => a.Student).WithMany(s => s.Lectures).HasForeignKey(a => a.StudentId);
                entity.HasOne(a => a.Lecture).WithMany(l => l.Students).HasForeignKey(a => a.LectureId);
            });
        }
    }
}