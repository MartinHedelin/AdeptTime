using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;

namespace AdeptTime.Shared.Services
{
    public class CloudService : ICloudService
    {
        public async Task<List<Bookmaker>> GetBookmakers(string betType, List<Match> matches)
        {
            // Simulated delay to mimic DB call
            await Task.Delay(2500);

            return new List<Bookmaker>
            {
               new Bookmaker { Name = "BET365", Multiplier = 27, Logo = "https://pbs.twimg.com/profile_images/1313493454710857730/ddYgxm9j_400x400.jpg", RedirectURL = "https://www.bet365.com" },
               new Bookmaker { Name = "NORDICBET", Multiplier = 21, Logo = "https://pbs.twimg.com/profile_images/1313493454710857730/ddYgxm9j_400x400.jpg", RedirectURL = "https://www.nordicbet.com" },
               new Bookmaker { Name = "BETFAIR", Multiplier = 19, Logo = "https://pbs.twimg.com/profile_images/1313493454710857730/ddYgxm9j_400x400.jpg", RedirectURL = "https://www.betfair.com" },
               new Bookmaker { Name = "MR GREEN", Multiplier = 19, Logo = "https://pbs.twimg.com/profile_images/1313493454710857730/ddYgxm9j_400x400.jpg", RedirectURL = "https://www.mrgreen.com" },
               new Bookmaker { Name = "LEOVEGAS", Multiplier = 16, Logo = "https://pbs.twimg.com/profile_images/1313493454710857730/ddYgxm9j_400x400.jpg", RedirectURL = "https://www.leovegas.com" },
               new Bookmaker { Name = "BET25", Multiplier = 16, Logo = "https://pbs.twimg.com/profile_images/1313493454710857730/ddYgxm9j_400x400.jpg", RedirectURL = "https://www.bet25.com" },
               new Bookmaker { Name = "888SPORT", Multiplier = 15, Logo = "https://pbs.twimg.com/profile_images/1313493454710857730/ddYgxm9j_400x400.jpg", RedirectURL = "https://www.888sport.com" }
            };
        }

        public List<Match> GetAllScheduledMatchesWithLimit(DateTime endDate, int limit = 50)
        {
            var today = DateTime.Now.Date;
            var matches = new List<Match>
            {
                // Football Matches
                new Match
                {
                    Time = "Today 18:30",
                    ActualDateTime = today.AddHours(18).AddMinutes(30),
                    HomeTeam = "Arsenal",
                    AwayTeam = "Manchester City",
                    HomeOdd = "2.40",
                    DrawOdd = "3.30",
                    AwayOdd = "2.90",
                    MoreOdds = 345,
                    Category = "Football",
                    Id = "f1"
                },
                new Match
                {
                    Time = "Today 19:00",
                    ActualDateTime = today.AddHours(19),
                    HomeTeam = "Real Madrid",
                    AwayTeam = "Barcelona",
                    HomeOdd = "2.10",
                    DrawOdd = "3.40",
                    AwayOdd = "3.20",
                    MoreOdds = 412,
                    Category = "Football",
                    Id = "f2"
                },
                new Match
                {
                    Time = "Today 20:45",
                    ActualDateTime = today.AddHours(20).AddMinutes(45),
                    HomeTeam = "Bayern Munich",
                    AwayTeam = "Dortmund",
                    HomeOdd = "1.75",
                    DrawOdd = "3.80",
                    AwayOdd = "4.20",
                    MoreOdds = 378,
                    Category = "Football",
                    Id = "f3"
                },
                new Match
                {
                    Time = "Today 21:00",
                    ActualDateTime = today.AddHours(21),
                    HomeTeam = "PSG",
                    AwayTeam = "Marseille",
                    HomeOdd = "1.65",
                    DrawOdd = "3.90",
                    AwayOdd = "4.80",
                    MoreOdds = 356,
                    Category = "Football",
                    Id = "f4"
                },
                new Match
                {
                    Time = "Today 21:00",
                    ActualDateTime = today.AddHours(21),
                    HomeTeam = "Inter",
                    AwayTeam = "AC Milan",
                    HomeOdd = "2.20",
                    DrawOdd = "3.30",
                    AwayOdd = "3.10",
                    MoreOdds = 389,
                    Category = "Football",
                    Id = "f5"
                },

                // Tennis Matches
                new Match
                {
                    Time = "Today 14:00",
                    ActualDateTime = today.AddHours(14),
                    HomeTeam = "Djokovic",
                    AwayTeam = "Alcaraz",
                    HomeOdd = "1.85",
                    DrawOdd = "-",
                    AwayOdd = "1.95",
                    MoreOdds = 156,
                    Category = "Tennis",
                    Id = "t1"
                },
                new Match
                {
                    Time = "Today 16:30",
                    ActualDateTime = today.AddHours(16).AddMinutes(30),
                    HomeTeam = "Medvedev",
                    AwayTeam = "Zverev",
                    HomeOdd = "1.75",
                    DrawOdd = "-",
                    AwayOdd = "2.05",
                    MoreOdds = 142,
                    Category = "Tennis",
                    Id = "t2"
                },
                new Match
                {
                    Time = "Today 18:00",
                    ActualDateTime = today.AddHours(18),
                    HomeTeam = "Swiatek",
                    AwayTeam = "Sabalenka",
                    HomeOdd = "1.65",
                    DrawOdd = "-",
                    AwayOdd = "2.20",
                    MoreOdds = 134,
                    Category = "Tennis",
                    Id = "t3"
                },

                // Esport Matches
                new Match
                {
                    Time = "Today 16:00",
                    ActualDateTime = today.AddHours(16),
                    HomeTeam = "NAVI",
                    AwayTeam = "FaZe",
                    HomeOdd = "1.90",
                    DrawOdd = "3.50",
                    AwayOdd = "1.85",
                    MoreOdds = 125,
                    Category = "Esport",
                    Id = "e1"
                },
                new Match
                {
                    Time = "Today 17:30",
                    ActualDateTime = today.AddHours(17).AddMinutes(30),
                    HomeTeam = "G2",
                    AwayTeam = "Vitality",
                    HomeOdd = "1.95",
                    DrawOdd = "3.40",
                    AwayOdd = "1.80",
                    MoreOdds = 148,
                    Category = "Esport",
                    Id = "e2"
                },
                new Match
                {
                    Time = "Today 19:00",
                    ActualDateTime = today.AddHours(19),
                    HomeTeam = "Liquid",
                    AwayTeam = "Cloud9",
                    HomeOdd = "2.10",
                    DrawOdd = "3.30",
                    AwayOdd = "1.70",
                    MoreOdds = 156,
                    Category = "Esport",
                    Id = "e3"
                },

                // Boxing Matches
                new Match
                {
                    Time = "Today 22:00",
                    ActualDateTime = today.AddHours(22),
                    HomeTeam = "Fury",
                    AwayTeam = "Usyk",
                    HomeOdd = "1.75",
                    DrawOdd = "12.00",
                    AwayOdd = "2.10",
                    MoreOdds = 89,
                    Category = "Boxing",
                    Id = "b1"
                },
                new Match
                {
                    Time = "Today 23:30",
                    ActualDateTime = today.AddHours(23).AddMinutes(30),
                    HomeTeam = "Joshua",
                    AwayTeam = "Wilder",
                    HomeOdd = "1.90",
                    DrawOdd = "11.00",
                    AwayOdd = "1.85",
                    MoreOdds = 76,
                    Category = "Boxing",
                    Id = "b2"
                },

                // Basketball Matches
                new Match
                {
                    Time = "Today 19:30",
                    ActualDateTime = today.AddHours(19).AddMinutes(30),
                    HomeTeam = "Lakers",
                    AwayTeam = "Celtics",
                    HomeOdd = "1.85",
                    DrawOdd = "15.00",
                    AwayOdd = "1.95",
                    MoreOdds = 234,
                    Category = "Basketball",
                    Id = "bb1"
                },
                new Match
                {
                    Time = "Today 20:00",
                    ActualDateTime = today.AddHours(20),
                    HomeTeam = "Warriors",
                    AwayTeam = "Bucks",
                    HomeOdd = "2.05",
                    DrawOdd = "15.00",
                    AwayOdd = "1.75",
                    MoreOdds = 245,
                    Category = "Basketball",
                    Id = "bb2"
                },

                // Ice Hockey Matches
                new Match
                {
                    Time = "Today 19:00",
                    ActualDateTime = today.AddHours(19),
                    HomeTeam = "Maple Leafs",
                    AwayTeam = "Canadiens",
                    HomeOdd = "1.95",
                    DrawOdd = "4.20",
                    AwayOdd = "1.85",
                    MoreOdds = 167,
                    Category = "Ice Hockey",
                    Id = "ih1"
                },
                new Match
                {
                    Time = "Today 20:30",
                    ActualDateTime = today.AddHours(20).AddMinutes(30),
                    HomeTeam = "Bruins",
                    AwayTeam = "Rangers",
                    HomeOdd = "1.80",
                    DrawOdd = "4.30",
                    AwayOdd = "2.00",
                    MoreOdds = 178,
                    Category = "Ice Hockey",
                    Id = "ih2"
                },

                // Baseball Matches
                new Match
                {
                    Time = "Today 18:00",
                    ActualDateTime = today.AddHours(18),
                    HomeTeam = "Yankees",
                    AwayTeam = "Red Sox",
                    HomeOdd = "1.90",
                    DrawOdd = "10.00",
                    AwayOdd = "1.90",
                    MoreOdds = 156,
                    Category = "Baseball",
                    Id = "bs1"
                },
                new Match
                {
                    Time = "Today 19:30",
                    ActualDateTime = today.AddHours(19).AddMinutes(30),
                    HomeTeam = "Dodgers",
                    AwayTeam = "Giants",
                    HomeOdd = "1.85",
                    DrawOdd = "10.00",
                    AwayOdd = "1.95",
                    MoreOdds = 145,
                    Category = "Baseball",
                    Id = "bs2"
                },

                // Volleyball Matches
                new Match
                {
                    Time = "Today 17:00",
                    ActualDateTime = today.AddHours(17),
                    HomeTeam = "Italy",
                    AwayTeam = "Poland",
                    HomeOdd = "2.10",
                    DrawOdd = "4.50",
                    AwayOdd = "1.70",
                    MoreOdds = 89,
                    Category = "Volleyball",
                    Id = "v1"
                },
                new Match
                {
                    Time = "Today 18:30",
                    ActualDateTime = today.AddHours(18).AddMinutes(30),
                    HomeTeam = "Brazil",
                    AwayTeam = "USA",
                    HomeOdd = "1.75",
                    DrawOdd = "4.60",
                    AwayOdd = "2.05",
                    MoreOdds = 94,
                    Category = "Volleyball",
                    Id = "v2"
                }
            };

            return matches
                .Where(m => m.ActualDateTime <= endDate)
                .Take(limit)
                .ToList();
        }

        public List<Category> GetSportsCategories()
        {
            return new List<Category>
            {
                new Category { Name = "Football", Icon = "⚽" },
                new Category { Name = "Tennis", Icon = "🎾" },
                new Category { Name = "Esport", Icon = "🎮" },
                new Category { Name = "Boxing", Icon = "🥊" },
                new Category { Name = "Badminton", Icon = "🏸" },
                new Category { Name = "Badminton", Icon = "🏸" },
                new Category { Name = "Badminton", Icon = "🏸" },
                new Category { Name = "Badminton", Icon = "🏸" },
                new Category { Name = "Badminton", Icon = "🏸" }
            };
        }

        public List<Category> GetCategoriesByParent(string parentCategory)
        {
            // Example implementation - this would come from your data source
            switch (parentCategory.ToLower())
            {
                case "football":
                    return new List<Category>
                    {
                        new Category { Name = "England", Icon = "⚽" },
                        new Category { Name = "Italy", Icon = "⚽" },
                        new Category { Name = "Spain", Icon = "⚽" },
                        new Category { Name = "Germany", Icon = "⚽" },
                        new Category { Name = "France", Icon = "⚽" }
                    };
                case "tennis":
                    return new List<Category>
                    {
                        new Category { Name = "ATP", Icon = "🎾" },
                        new Category { Name = "WTA", Icon = "🎾" }
                    };
                default:
                    return new List<Category>();
            }
        }

        public List<Match> GetMatchesByCategory(string categoryName)
        {
            var today = DateTime.Now.Date;
            var allMatches = new List<Match>
            {
                // Football Matches
                new Match
                {
                    Time = "Today 18:30",
                    ActualDateTime = today.AddHours(18).AddMinutes(30),
                    HomeTeam = "Arsenal",
                    AwayTeam = "Manchester City",
                    HomeOdd = "2.40",
                    DrawOdd = "3.30",
                    AwayOdd = "2.90",
                    MoreOdds = 345,
                    Category = "Football",
                    Id = "f1"  // Same ID as in GetAllScheduledMatchesWithLimit
                },
                new Match
                {
                    Time = "Today 19:00",
                    ActualDateTime = today.AddHours(19),
                    HomeTeam = "Real Madrid",
                    AwayTeam = "Barcelona",
                    HomeOdd = "2.10",
                    DrawOdd = "3.40",
                    AwayOdd = "3.20",
                    MoreOdds = 412,
                    Category = "Football",
                    Id = "f2"
                },
                new Match
                {
                    Time = "Today 20:45",
                    ActualDateTime = today.AddHours(20).AddMinutes(45),
                    HomeTeam = "Bayern Munich",
                    AwayTeam = "Dortmund",
                    HomeOdd = "1.75",
                    DrawOdd = "3.80",
                    AwayOdd = "4.20",
                    MoreOdds = 378,
                    Category = "Football",
                    Id = "f3"
                },
                new Match
                {
                    Time = "Today 21:00",
                    ActualDateTime = today.AddHours(21),
                    HomeTeam = "PSG",
                    AwayTeam = "Marseille",
                    HomeOdd = "1.65",
                    DrawOdd = "3.90",
                    AwayOdd = "4.80",
                    MoreOdds = 356,
                    Category = "Football",
                    Id = "f4"
                },
                new Match
                {
                    Time = "Today 21:00",
                    ActualDateTime = today.AddHours(21),
                    HomeTeam = "Inter",
                    AwayTeam = "AC Milan",
                    HomeOdd = "2.20",
                    DrawOdd = "3.30",
                    AwayOdd = "3.10",
                    MoreOdds = 389,
                    Category = "Football",
                    Id = "f5"
                },

                // Tennis Matches
                new Match
                {
                    Time = "Today 14:00",
                    ActualDateTime = today.AddHours(14),
                    HomeTeam = "Djokovic",
                    AwayTeam = "Alcaraz",
                    HomeOdd = "1.85",
                    DrawOdd = "-",
                    AwayOdd = "1.95",
                    MoreOdds = 156,
                    Category = "Tennis",
                    Id = "t1"
                },
                new Match
                {
                    Time = "Today 16:30",
                    ActualDateTime = today.AddHours(16).AddMinutes(30),
                    HomeTeam = "Medvedev",
                    AwayTeam = "Zverev",
                    HomeOdd = "1.75",
                    DrawOdd = "-",
                    AwayOdd = "2.05",
                    MoreOdds = 142,
                    Category = "Tennis",
                    Id = "t2"
                },
                new Match
                {
                    Time = "Today 18:00",
                    ActualDateTime = today.AddHours(18),
                    HomeTeam = "Swiatek",
                    AwayTeam = "Sabalenka",
                    HomeOdd = "1.65",
                    DrawOdd = "-",
                    AwayOdd = "2.20",
                    MoreOdds = 134,
                    Category = "Tennis",
                    Id = "t3"
                },

                // Esport Matches
                new Match
                {
                    Time = "Today 16:00",
                    ActualDateTime = today.AddHours(16),
                    HomeTeam = "NAVI",
                    AwayTeam = "FaZe",
                    HomeOdd = "1.90",
                    DrawOdd = "3.50",
                    AwayOdd = "1.85",
                    MoreOdds = 125,
                    Category = "Esport",
                    Id = "e1"
                },
                new Match
                {
                    Time = "Today 17:30",
                    ActualDateTime = today.AddHours(17).AddMinutes(30),
                    HomeTeam = "G2",
                    AwayTeam = "Vitality",
                    HomeOdd = "1.95",
                    DrawOdd = "3.40",
                    AwayOdd = "1.80",
                    MoreOdds = 148,
                    Category = "Esport",
                    Id = "e2"
                },
                new Match
                {
                    Time = "Today 19:00",
                    ActualDateTime = today.AddHours(19),
                    HomeTeam = "Liquid",
                    AwayTeam = "Cloud9",
                    HomeOdd = "2.10",
                    DrawOdd = "3.30",
                    AwayOdd = "1.70",
                    MoreOdds = 156,
                    Category = "Esport",
                    Id = "e3"
                },

                // Boxing Matches
                new Match
                {
                    Time = "Today 22:00",
                    ActualDateTime = today.AddHours(22),
                    HomeTeam = "Fury",
                    AwayTeam = "Usyk",
                    HomeOdd = "1.75",
                    DrawOdd = "12.00",
                    AwayOdd = "2.10",
                    MoreOdds = 89,
                    Category = "Boxing",
                    Id = "b1"
                },
                new Match
                {
                    Time = "Today 23:30",
                    ActualDateTime = today.AddHours(23).AddMinutes(30),
                    HomeTeam = "Joshua",
                    AwayTeam = "Wilder",
                    HomeOdd = "1.90",
                    DrawOdd = "11.00",
                    AwayOdd = "1.85",
                    MoreOdds = 76,
                    Category = "Boxing",
                    Id = "b2"
                },

                // Basketball Matches
                new Match
                {
                    Time = "Today 19:30",
                    ActualDateTime = today.AddHours(19).AddMinutes(30),
                    HomeTeam = "Lakers",
                    AwayTeam = "Celtics",
                    HomeOdd = "1.85",
                    DrawOdd = "15.00",
                    AwayOdd = "1.95",
                    MoreOdds = 234,
                    Category = "Basketball",
                    Id = "bb1"
                },
                new Match
                {
                    Time = "Today 20:00",
                    ActualDateTime = today.AddHours(20),
                    HomeTeam = "Warriors",
                    AwayTeam = "Bucks",
                    HomeOdd = "2.05",
                    DrawOdd = "15.00",
                    AwayOdd = "1.75",
                    MoreOdds = 245,
                    Category = "Basketball",
                    Id = "bb2"
                },

                // Ice Hockey Matches
                new Match
                {
                    Time = "Today 19:00",
                    ActualDateTime = today.AddHours(19),
                    HomeTeam = "Maple Leafs",
                    AwayTeam = "Canadiens",
                    HomeOdd = "1.95",
                    DrawOdd = "4.20",
                    AwayOdd = "1.85",
                    MoreOdds = 167,
                    Category = "Ice Hockey",
                    Id = "ih1"
                },
                new Match
                {
                    Time = "Today 20:30",
                    ActualDateTime = today.AddHours(20).AddMinutes(30),
                    HomeTeam = "Bruins",
                    AwayTeam = "Rangers",
                    HomeOdd = "1.80",
                    DrawOdd = "4.30",
                    AwayOdd = "2.00",
                    MoreOdds = 178,
                    Category = "Ice Hockey",
                    Id = "ih2"
                },

                // Baseball Matches
                new Match
                {
                    Time = "Today 18:00",
                    ActualDateTime = today.AddHours(18),
                    HomeTeam = "Yankees",
                    AwayTeam = "Red Sox",
                    HomeOdd = "1.90",
                    DrawOdd = "10.00",
                    AwayOdd = "1.90",
                    MoreOdds = 156,
                    Category = "Baseball",
                    Id = "bs1"
                },
                new Match
                {
                    Time = "Today 19:30",
                    ActualDateTime = today.AddHours(19).AddMinutes(30),
                    HomeTeam = "Dodgers",
                    AwayTeam = "Giants",
                    HomeOdd = "1.85",
                    DrawOdd = "10.00",
                    AwayOdd = "1.95",
                    MoreOdds = 145,
                    Category = "Baseball",
                    Id = "bs2"
                },

                // Volleyball Matches
                new Match
                {
                    Time = "Today 17:00",
                    ActualDateTime = today.AddHours(17),
                    HomeTeam = "Italy",
                    AwayTeam = "Poland",
                    HomeOdd = "2.10",
                    DrawOdd = "4.50",
                    AwayOdd = "1.70",
                    MoreOdds = 89,
                    Category = "Volleyball",
                    Id = "v1"
                },
                new Match
                {
                    Time = "Today 18:30",
                    ActualDateTime = today.AddHours(18).AddMinutes(30),
                    HomeTeam = "Brazil",
                    AwayTeam = "USA",
                    HomeOdd = "1.75",
                    DrawOdd = "4.60",
                    AwayOdd = "2.05",
                    MoreOdds = 94,
                    Category = "Volleyball",
                    Id = "v2"
                }
            };

            // Return matches filtered by category
            return allMatches
                .Where(m => m.Category.ToLower() == categoryName.ToLower())
                .ToList();
        }


        // Add these methods to your CloudService class
        public List<Category> GetSubCategoriesByParent(string parentCategory, string grandParentCategory = null)
        {
           if (grandParentCategory.ToLower() == "football")
            {
                switch (parentCategory.ToLower())
                {
                    case "england":
                        return new List<Category>
                        {
                            new Category { Name = "Premier League", Icon = "🏆", ParentCategory = "England" },
                            new Category { Name = "Championship", Icon = "🏆", ParentCategory = "England" },
                            new Category { Name = "FA Cup", Icon = "🏆", ParentCategory = "England" },
                            new Category { Name = "EFL Cup", Icon = "🏆", ParentCategory = "England" },
                            new Category { Name = "EFL Trophy", Icon = "🏆", ParentCategory = "England" }
                        };
                        case "spain":
                            return new List<Category>
                            {
                                new Category { Name = "La Liga", Icon = "🏆", ParentCategory = "Spain" },
                                new Category { Name = "Copa del Rey", Icon = "🏆", ParentCategory = "Spain" }
                                // ... other Spanish competitions
                            };
                }
            }

            return new List<Category>();
        }

        public List<Match> GetMatchesBySubCategory(string subCategory, string parentCategory)
        {
            var today = DateTime.Now.Date;

            // Use both category and parent category to get the right matches
            if (subCategory.ToLower() == "england")
            {
                switch (subCategory.ToLower())
                {
                    case "premier league":
                        return new List<Match>
                {
                    new Match
                    {
                        Time = "Today 16:00",
                        ActualDateTime = today.AddHours(16),
                        HomeTeam = "Liverpool",
                        AwayTeam = "Chelsea",
                        HomeOdd = "1.90",
                        DrawOdd = "3.40",
                        AwayOdd = "3.80",
                        MoreOdds = 345,
                        Category = "Premier League",
                        Id = "pl1"
                    },
                    // ... more Premier League matches
                };
                    case "championship":
                        return new List<Match>
                {
                    new Match
                    {
                        Time = "Today 15:00",
                        ActualDateTime = today.AddHours(15),
                        HomeTeam = "Leeds",
                        AwayTeam = "Leicester",
                        HomeOdd = "2.10",
                        DrawOdd = "3.30",
                        AwayOdd = "3.40",
                        MoreOdds = 289,
                        Category = "Championship",
                        Id = "ch1"
                    },
                    // ... more Championship matches
                };
                }
            }

            return new List<Match>();
        }
    }
}