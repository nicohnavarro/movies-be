using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Functions.Worker;
using KindaMovies.Domain.Interfaces;
using KindaMovies.Infraestructure.Repositories;
using KindaMovies.Application.Services;
using KindaMovies.Infraestructure.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        
        // Register DbContext with SQL Server
        var connectionString = Environment.GetEnvironmentVariable("SqlDb");
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(connectionString));
        
        // Register Repository
        services.AddScoped<IMovieRepository, MovieRepository>();
        
        // Register Service
        services.AddScoped<IMovieService, MovieService>();
    })
    .Build();

host.Run();
