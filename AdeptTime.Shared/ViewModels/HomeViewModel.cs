using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;
using AdeptTime.Shared.Services;

namespace AdeptTime.Shared.ViewModels
{
    public class HomeViewModel
    {
        private readonly ICloudService _cloudService;
        public readonly IParlayService _parlayService;

        public List<Category> Sports { get; private set; }
        public List<Match> Matches { get; private set; }

        public HomeViewModel(ICloudService cloudService, IParlayService parlayService)
        {
            _cloudService = cloudService;
            _parlayService = parlayService;

            InitializeSports();
            InitializeMatches();
        }

        private void InitializeSports()
        {
            Sports = _cloudService.GetSportsCategories();
        }

        private void InitializeMatches()
        {
            var endDate = DateTime.Now.AddHours(24);
            Matches = _cloudService.GetAllScheduledMatchesWithLimit(endDate, 20);
        }
    }
}