using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;
using AdeptTime.Shared.Services;

public class SubCategoryViewModel
{
    private readonly ICloudService _cloudService;
    public readonly IParlayService _parlayService;
    private readonly string _categoryName;
    private readonly string _parentCategory;  // Add this

    public List<Category> Categories { get; private set; }
    public List<Match> Matches { get; private set; }

    public SubCategoryViewModel(ICloudService cloudService, IParlayService parlayService,
        string categoryName, string parentCategory)
    {
        _cloudService = cloudService;
        _parlayService = parlayService;
        _categoryName = categoryName;
        _parentCategory = parentCategory;
        InitializeSubCategories();
        InitializeMatches();
    }

    private void InitializeSubCategories()
    {
        Categories = _cloudService.GetSubCategoriesByParent(_categoryName, _parentCategory);
    }

    private void InitializeMatches()
    {
        Matches = _cloudService.GetMatchesBySubCategory(_categoryName, _parentCategory);
    }
}