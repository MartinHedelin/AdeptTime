using System;
namespace AdeptTime.Shared.Models
{
    // Parlay.cs
    public class Parlay
    {
        public string Id { get; set; }
        public List<Match> Matches { get; set; } = new();
        public string Type { get; set; } = "tripple";  // Default value
        public decimal TotalOdds => Matches.Any()
            ? Matches.Select(m => decimal.Parse(m.SelectedOdds)).Aggregate((a, b) => a * b)
            : 0;
    }
}