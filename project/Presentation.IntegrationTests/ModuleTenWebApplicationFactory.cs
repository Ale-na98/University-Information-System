using System;
using DataAccess;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.IntegrationTests
{
    public class ModuleTenWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == 
                   typeof(DbContextOptions<DataAccess.AppContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<DataAccess.AppContext>(options => 
                options.UseInMemoryDatabase(databaseName: $"InMemoryDbForTesting{Guid.NewGuid()}"));
            });
        }
    }
}
