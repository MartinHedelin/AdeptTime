using System;
using AdeptTime.Shared.Models;

namespace AdeptTime.Shared.Interfaces
{
    public interface ICloudService
    {
        Task<List<Bookmaker>> GetBookmakers(string betType, List<Match> matches);

        // Category data
        List<Category> GetSportsCategories();
        List<Category> GetCategoriesByParent(string parentCategory);

        // Sub category data
        List<Category> GetSubCategoriesByParent(string parentCategory, string grandParentCategory = null);
        List<Match> GetMatchesBySubCategory(string subCategory, string parentCategory);


        // Match data
        List<Match> GetAllScheduledMatchesWithLimit(DateTime endDate, int limit = 10);
        List<Match> GetMatchesByCategory(string categoryName);
    }
}