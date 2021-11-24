using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<UniversityContext>(options => options.UseNpgsql(connectionString))
                .AddScoped<IUniversityRepository<StudentDb>, UniversityRepository<StudentDb>>()
                .AddScoped<IUniversityRepository<TeacherDb>, UniversityRepository<TeacherDb>>()
                .AddScoped<IUniversityRepository<LectureDb>, UniversityRepository<LectureDb>>()
                .AddScoped<IUniversityRepository<HometaskDb>, UniversityRepository<HometaskDb>>()
                .AddScoped<IUniversityRepository<AttendanceDb>, UniversityRepository<AttendanceDb>>()
                .AddScoped<IStudentsRepository, StudentsRepository>()
                .AddScoped<ITeachersRepository, TeachersRepository>()
                .AddScoped<ILecturesRepository, LecturesRepository>()
                .AddScoped<IHometasksRepository, HometasksRepository>()
                .AddScoped<IAttendanceRepository, AttendanceRepository>();
        }
    }
}