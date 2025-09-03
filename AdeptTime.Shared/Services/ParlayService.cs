using System;
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;

namespace AdeptTime.Shared.Services
{
    public class ParlayService : IParlayService
    {
        private List<Parlay> _parlays = new();
        private Parlay _currentParlay;
        private List<Match> _matches = new();

        public event Action OnBetsChanged;
        public List<Parlay> Parlays => _parlays;
        public Parlay CurrentParlay => _currentParlay;

        public ParlayService()
        {
            CreateNewParlay();
        }

        public void CreateNewParlay()
        {
            _currentParlay = new Parlay { Id = Guid.NewGuid().ToString() };
            _parlays.Add(_currentParlay);
            OnBetsChanged?.Invoke();
        }


        // Add method to check if a bet is selected
        public bool IsBetSelected(string matchId, string betType)
        {
            var match = _matches.FirstOrDefault(m => m.Id == matchId);
            if (match != null)
            {
                return match.SelectedBetType == betType;
            }

            // Also check current parlay in case match isn't in _matches yet
            var parlayMatch = _currentParlay.Matches.FirstOrDefault(m => m.Id == matchId);
            return parlayMatch?.SelectedBetType == betType;
        }

            // Modify AddBet to update the original match state
            public void AddBet(Match match, string betType, string odds)
            {
                // Keep track of original match and update its state
                var existingMatch = _matches.FirstOrDefault(m => m.Id == match.Id);
                if (existingMatch == null)
                {
                    match.SelectedBetType = betType;
                    match.SelectedOdds = odds;
                    _matches.Add(match);
                }
                else
                {
                    existingMatch.SelectedBetType = betType;
                    existingMatch.SelectedOdds = odds;
                }

                // Rest of your existing AddBet logic
                if (string.IsNullOrEmpty(betType))
                {
                    var matchInParlay = _currentParlay.Matches.FirstOrDefault(m => m.Id == match.Id);
                    if (matchInParlay != null)
                    {
                        _currentParlay.Matches.Remove(matchInParlay);
                    }
                }
                else
                {
                    var matchForParlay = new Match
                    {
                        Id = match.Id,
                        HomeTeam = match.HomeTeam,
                        AwayTeam = match.AwayTeam,
                        Time = match.Time,
                        Category = match.Category,
                        HomeOdd = match.HomeOdd,
                        DrawOdd = match.DrawOdd,
                        AwayOdd = match.AwayOdd,
                        SelectedBetType = betType,
                        SelectedOdds = odds
                    };

                    var existingInParlay = _currentParlay.Matches.FirstOrDefault(m => m.Id == match.Id);
                    if (existingInParlay != null)
                    {
                        _currentParlay.Matches.Remove(existingInParlay);
                    }
                    _currentParlay.Matches.Add(matchForParlay);
                }

                OnBetsChanged?.Invoke();
            }



        public void RemoveMatchFromParlay(string matchId)
        {
            var matchInParlay = _currentParlay.Matches.FirstOrDefault(m => m.Id == matchId);
            if (matchInParlay != null)
            {
                _currentParlay.Matches.Remove(matchInParlay);

                // Clear selection from original match
                var originalMatch = _matches.FirstOrDefault(m => m.Id == matchId);
                if (originalMatch != null)
                {
                    originalMatch.SelectedBetType = null;
                    originalMatch.SelectedOdds = null;
                }

                OnBetsChanged?.Invoke();
            }
        }

        public void SwitchParlay(string parlayId)
        {
            var parlay = _parlays.FirstOrDefault(p => p.Id == parlayId);
            if (parlay != null)
            {
                _currentParlay = parlay;
                OnBetsChanged?.Invoke();
            }
        }

        public void ClearCurrentParlay()
        {
            // Clear selections from original matches
            foreach (var match in _currentParlay.Matches)
            {
                var originalMatch = _matches.FirstOrDefault(m => m.Id == match.Id);
                if (originalMatch != null)
                {
                    originalMatch.SelectedBetType = null;
                    originalMatch.SelectedOdds = null;
                }
            }

            _currentParlay.Matches.Clear();
            OnBetsChanged?.Invoke();
        }
    }
}