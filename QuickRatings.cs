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

        public override List<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
        {
            var items = new List<CustomGameMenuItem>
    {
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "☆",
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 0;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
                {
                    MenuSection = "Rating",
                    Description = "☆½",
                    Odd = true,
                    Action = (arg) =>
                    {
                        arg.Games.ForEach(g => {
                            g.UserScore = 10;
                            PlayniteApi.Database.Games.Update(g);
                        });
                    }
                },

        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★",
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 20;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★½",
            Odd = true,
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 30;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★★",
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 40;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★★½",
            Odd = true,
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 50;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★★★",
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 60;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★★★½",
            Odd = true,
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 70;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★★★★",
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 80;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★★★★½",
            Odd = true,
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 90;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        },
        new CustomGameMenuItem
        {
            MenuSection = "Rating",
            Description = "★★★★★",
            Action = (arg) =>
            {
                arg.Games.ForEach(g => {
                    g.UserScore = 100;
                    PlayniteApi.Database.Games.Update(g);
                });
            }
        }
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