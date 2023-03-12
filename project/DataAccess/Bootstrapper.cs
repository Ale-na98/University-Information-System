using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using DataAccess.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataAccess.Elasticsearch.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("UniversityDb")))
                .AddScoped<IUniversityRepository<TeacherDb>, UniversityRepository<TeacherDb>>()
                .AddScoped<IUniversityRepository<GroupDb>, UniversityRepository<GroupDb>>()
                .AddScoped<IAttendanceRepository, AttendanceRepository>()
                .AddScoped<ElasticsearchRepository<StudentDocument>>()
                .AddScoped<IHometaskRepository, HometaskRepository>()
                .AddScoped<ILectureRepository, LectureRepository>()
                .AddScoped<IStudentRepository, StudentRepository>();
        }
    }
}
