using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;

namespace QuickRatings
{
    public class QuickRatings : Plugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        private QuickRatingsSettings settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("bdb15f97-ae65-42d0-8a51-ec6d1e5834b6");

        private static Dictionary<string, System.Guid> RatingDictionary = new Dictionary<string, Guid>();

        public QuickRatings(IPlayniteAPI api) : base(api)
        {
            settings = new QuickRatingsSettings(this);
        }

        public override void OnGameInstalled(Game game)
        {
            // Add code to be executed when game is finished installing.
        }

        public override void OnGameStarted(Game game)
        {
            // Add code to be executed when game is started running.
        }

        public override void OnGameStarting(Game game)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameStopped(Game game, long elapsedSeconds)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameUninstalled(Game game)
        {
            // Add code to be executed when game is uninstalled.
        }

        public override void OnApplicationStarted()
        {
            // Add code to be executed when Playnite is initialized.
            UpdateTagIdForRating("☆");
            UpdateTagIdForRating("☆½");
            UpdateTagIdForRating("★");
            UpdateTagIdForRating("★½");
            UpdateTagIdForRating("★★");
            UpdateTagIdForRating("★★½");
            UpdateTagIdForRating("★★★");
            UpdateTagIdForRating("★★★½");
            UpdateTagIdForRating("★★★★");
            UpdateTagIdForRating("★★★★½");
            UpdateTagIdForRating("★★★★★");            
        }

        private void UpdateTagIdForRating(string rating)
        {
            if (PlayniteApi.Database.Tags.Any(x => x.Name == rating))
                RatingDictionary[rating] = PlayniteApi.Database.Tags.First(x => x.Name == rating).Id;
        }

        public override void OnApplicationStopped()
        {
            // Add code to be executed when Playnite is shutting down.
        }

        public override void OnLibraryUpdated()
        {
            // Add code to be executed when library is updated.
        }

        public class CustomGameMenuItem : GameMenuItem
        {
            public bool? Odd { get; set; }
        }

        private CustomGameMenuItem GetGameMenuItem(string rating, int numericalRating, bool isOdd)
        {
            return new CustomGameMenuItem
            {
                MenuSection = "Rating",
                Description = rating,
                Odd = isOdd,
                Action = (arg) =>
                {
                    System.Guid? tagIdToSet = null;
                    if (settings.AddStarsAsTags && numericalRating >= 0)
                    {
                        var tagExists = PlayniteApi.Database.Tags.Any(x => x.Name == rating);
                        if (tagExists)
                        {
                            tagIdToSet = PlayniteApi.Database.Tags.First(x => x.Name == rating).Id;
                        }
                        else
                        {
                            tagIdToSet = PlayniteApi.Database.Tags.Add(rating).Id;
                            RatingDictionary[rating] = tagIdToSet.Value;
                        }
                    }
                    arg.Games.ForEach(g =>
                    {
                        if (numericalRating < 0)
                        {
                            g.UserScore = null;
                            if (g.TagIds != null)
                            {
                                g.TagIds.RemoveAll(x => RatingDictionary.Values.Contains(x));
                            }
                        }
                        else
                        {
                            g.UserScore = numericalRating;
                            if (tagIdToSet.HasValue)
                            {
                                if (g.TagIds == null)
                                {
                                    g.TagIds = new List<Guid>();
                                }
                                g.TagIds.RemoveAll(x => RatingDictionary.Values.Contains(x) && tagIdToSet.Value != x);
                                g.TagIds.Add(tagIdToSet.Value);
                            }
                        }                        
                        PlayniteApi.Database.Games.Update(g);
                    });
                }
            };
        }

        public override List<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
        {
            var items = new List<CustomGameMenuItem>
            {
                GetGameMenuItem("☆", 0, false),
                GetGameMenuItem("☆½", 10, true),
                GetGameMenuItem("★", 20, false),
                GetGameMenuItem("★½", 30, true),
                GetGameMenuItem("★★", 40, false),
                GetGameMenuItem("★★½", 50, true),
                GetGameMenuItem("★★★", 60, false),
                GetGameMenuItem("★★★½", 70, true),
                GetGameMenuItem("★★★★", 80, false),
                GetGameMenuItem("★★★★½", 90, true),
                GetGameMenuItem("★★★★★", 100, false),
                GetGameMenuItem("Clear rating", -1, false)
            };

            return settings.IncludeHalfStars ? items.Cast<GameMenuItem>().ToList() : items.Where(x => !x.Odd.HasValue || !x.Odd.Value).Cast<GameMenuItem>().ToList();

        }
        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new QuickRatingsSettingsView();
        }
    }
}