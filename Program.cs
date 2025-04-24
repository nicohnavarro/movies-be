using KindaMovies.Infraestructure.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var connectionString = Environment.GetEnvironmentVariable("SqlDb");
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(connectionString));

        // Apply migrations
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
    })
    .Build();

host.Run();
