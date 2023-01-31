using DataAccess;
using BusinessLogic.Domain;
using BusinessLogic.Services;
using BusinessLogic.Providers;
using DataAccess.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<SmsProvider>()
                .AddScoped<GroupService>()
                .AddScoped<EmailProvider>()
                .AddScoped<StudentService>()
                .AddScoped<LectureService>()
                .AddScoped<AttendanceService>()
                .AddScoped<ElasticsearchService>()
                .AddScoped<ElasticsearchRepository<Student>>()
                .AddAutoMapper(typeof(MapperProfile))
                .AddElasticsearch(configuration)
                .AddDataAccess(configuration);
        }
    }
}
