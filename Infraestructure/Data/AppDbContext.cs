using System;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using KindaMovies.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KindaMovies.Infraestructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            var currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current Directory: {currentDirectory}");
            
            var csvPath = Path.Combine(currentDirectory, "Film_Locations_in_San_Francisco_20250424.csv");
            Console.WriteLine($"Looking for CSV at: {csvPath}");
            Console.WriteLine($"File exists: {File.Exists(csvPath)}");
            
            if (File.Exists(csvPath))
            {
                Console.WriteLine("Starting to read CSV file...");
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    Delimiter = ","
                };

                using var reader = new StreamReader(csvPath);
                using var csv = new CsvReader(reader, config);

                var records = csv.GetRecords<MovieCsvRecord>().ToList();
                Console.WriteLine($"Found {records.Count} records in CSV");
                
                var movies = new List<Movie>();
                var id = 1;

                foreach (var record in records)
                {
                    movies.Add(new Movie
                    {
                        Id = id++,
                        Title = string.IsNullOrEmpty(record.Title) ? "Unknown" : record.Title,
                        ReleaseYear = ParseInt(record.ReleaseYear),
                        Locations = string.IsNullOrEmpty(record.Locations) ? "Unknown" : record.Locations,
                        FunFacts = string.IsNullOrEmpty(record.FunFacts) ? "Unknown" : record.FunFacts,
                        ProductionCompany = string.IsNullOrEmpty(record.ProductionCompany) ? "Unknown" : record.ProductionCompany,
                        Distributor = string.IsNullOrEmpty(record.Distributor) ? "Unknown" : record.Distributor,
                        Director = string.IsNullOrEmpty(record.Director) ? "Unknown" : record.Director,
                        Writer = string.IsNullOrEmpty(record.Writer) ? "Unknown" : record.Writer,
                        Actor1 = string.IsNullOrEmpty(record.Actor1) ? "Unknown" : record.Actor1,
                        Actor2 = string.IsNullOrEmpty(record.Actor2) ? "Unknown" : record.Actor2,
                        Actor3 = string.IsNullOrEmpty(record.Actor3) ? "Unknown" : record.Actor3,
                        Point = string.IsNullOrEmpty(record.Point) ? "Unknown" : record.Point,
                        Longitude = ParseDouble(record.Longitude),
                        Latitude = ParseDouble(record.Latitude),
                        AnalysisNeighborhood = string.IsNullOrEmpty(record.AnalysisNeighborhood) ? "Unknown" : record.AnalysisNeighborhood,
                        SupervisorDistrict = string.IsNullOrEmpty(record.SupervisorDistrict) ? "Unknown" : record.SupervisorDistrict,
                        DataAsOf = ParseDateTime(record.DataAsOf),
                        DataLoadedAt = ParseDateTime(record.DataLoadedAt),
                        SFFindNeighborhoods = ParseInt(record.SFFindNeighborhoods),
                        AnalysisNeighborhoods = ParseInt(record.AnalysisNeighborhoods),
                        CurrentSupervisorDistricts = ParseInt(record.CurrentSupervisorDistricts)
                    });
                }

                Console.WriteLine($"Adding {movies.Count} movies to database...");
                modelBuilder.Entity<Movie>().HasData(movies);
                Console.WriteLine("Movies added successfully");
            }
            else
            {
                Console.WriteLine("CSV file not found!");
            }
        }

        private int? ParseInt(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return int.TryParse(value, out int result) ? result : null;
        }

        private double? ParseDouble(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return double.TryParse(value, out double result) ? result : null;
        }

        private DateTime? ParseDateTime(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return DateTime.TryParse(value, out DateTime result) ? result : null;
        }
    }

    public class MovieCsvRecord
    {
        public string Title { get; set; }
        public string ReleaseYear { get; set; }
        public string Locations { get; set; }
        public string FunFacts { get; set; }
        public string ProductionCompany { get; set; }
        public string Distributor { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actor1 { get; set; }
        public string Actor2 { get; set; }
        public string Actor3 { get; set; }
        public string Point { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AnalysisNeighborhood { get; set; }
        public string SupervisorDistrict { get; set; }
        public string DataAsOf { get; set; }
        public string DataLoadedAt { get; set; }
        public string SFFindNeighborhoods { get; set; }
        public string AnalysisNeighborhoods { get; set; }
        public string CurrentSupervisorDistricts { get; set; }
    }
}