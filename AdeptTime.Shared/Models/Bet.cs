using System;
namespace AdeptTime.Shared.Models
{
	public class Bet
	{
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Match Match { get; set; }
        public string BetType { get; set; } // "1", "X", or "2"
        public string Odds { get; set; }
    }
}