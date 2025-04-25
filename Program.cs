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
    })
    .Build();

host.Run();
