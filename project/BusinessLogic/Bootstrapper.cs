using DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services, string connectionString)
        {
            return services
                .AddScoped<IStudentsService, StudentsService>()
                .AddScoped<ITeachersService, TeachersService>()
                .AddScoped<ILecturesService, LecturesService>()
                .AddScoped<IHometasksService, HometasksService>()
                .AddScoped<IAttendanceService, AttendanceService>()
                .AddScoped<IEmailProvider, EmailProvider>()
                .AddScoped<ISmsProvider, SmsProvider>()
                .AddAutoMapper(typeof(MapperProfile))
                .AddDataAccess(connectionString);
        }
    }
}
