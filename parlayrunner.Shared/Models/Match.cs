using System;
namespace parlayrunner.Shared.Models
{
    public class Match
    {
        public string Time { get; set; }
        public DateTime ActualDateTime { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string HomeOdd { get; set; }
        public string DrawOdd { get; set; }
        public string AwayOdd { get; set; }
        public int MoreOdds { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public string SelectedBetType { get; set; } // "1", "X", "2"
        public string SelectedOdds { get; set; }
    }
}