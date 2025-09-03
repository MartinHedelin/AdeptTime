using System;
using AdeptTime.Shared.Models;

namespace AdeptTime.Shared.Interfaces
{
    public interface IParlayService
    {
        bool IsBetSelected(string matchId, string betType);
        List<Parlay> Parlays { get; }
        Parlay CurrentParlay { get; }
        event Action OnBetsChanged;

        void AddBet(Match match, string betType, string odds);
        void SwitchParlay(string parlayId);
        void RemoveMatchFromParlay(string matchId);
        void ClearCurrentParlay();
    }
}