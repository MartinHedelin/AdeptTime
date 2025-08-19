using parlayrunner.Shared.Interfaces;
using parlayrunner.Shared.Models;
using parlayrunner.Shared.Services;

public class CategoryViewModel
{
    private readonly ICloudService _cloudService;
    public readonly IParlayService _parlayService;
    private readonly string _categoryName;

    public List<Category> Categories { get; private set; }
    public List<Match> Matches { get; private set; }

    public CategoryViewModel(ICloudService cloudService, IParlayService parlayService, string categoryName)
    {
        _cloudService = cloudService;
        _parlayService = parlayService;
        _categoryName = categoryName;

        InitializeCategories();
        InitializeMatches();
    }

    private void InitializeCategories()
    {
        Categories = _cloudService.GetCategoriesByParent(_categoryName);
    }

    private void InitializeMatches()
    {
        Matches = _cloudService.GetMatchesByCategory(_categoryName);
    }
}