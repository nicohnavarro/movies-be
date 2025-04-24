using System;

namespace KindaMovies.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public int? ReleaseYear { get; set; }
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
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string AnalysisNeighborhood { get; set; }
        public string SupervisorDistrict { get; set; }
        public DateTime? DataAsOf { get; set; }
        public DateTime? DataLoadedAt { get; set; }
        public int? SFFindNeighborhoods { get; set; }
        public int? AnalysisNeighborhoods { get; set; }
        public int? CurrentSupervisorDistricts { get; set; }
    }
}