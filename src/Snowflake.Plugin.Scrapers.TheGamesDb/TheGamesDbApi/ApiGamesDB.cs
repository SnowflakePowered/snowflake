using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Snowflake.Plugin.Scrapers.TheGamesDb.TheGamesDbApi
{
    /// <summary>
    /// Fetches information from TheGamesDB.
    /// </summary>
    internal static class ApiGamesDb
    {
        /// <summary>
        /// The base image path that should be prepended to all the relative image paths to get the full paths to the images.
        /// </summary>
        public const string BaseImgURL = @"http://thegamesdb.net/banners/";

        /// <summary>
        /// Gets a collection of games matched up with loose search terms.
        /// </summary>
        /// <param name="name">The game title to search for</param>
        /// <param name="platform">Filters results by platform</param>
        /// <param name="genre">Filters results by genre</param>
        /// <returns>A collection of games that matched the search terms</returns>
        public static ICollection<ApiGameSearchResult> GetGames(string name, string platform = "", string genre = "")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/GetGamesList.php?name=" + name + @"&platform=" + platform + @"&genre=" + genre);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            List<ApiGameSearchResult> games = new List<ApiGameSearchResult>();

            // Iterate through all games
            XmlNode gameNode;
            while (ienum.MoveNext())
            {
                ApiGameSearchResult game = new ApiGameSearchResult();
                gameNode = (XmlNode)ienum.Current;

                IEnumerator ienumGame = gameNode.GetEnumerator();
                XmlNode attributeNode;
                while (ienumGame.MoveNext())
                {
                    attributeNode = (XmlNode)ienumGame.Current;

                    // Iterate through all game attributes
                    switch (attributeNode.Name)
                    {
                        case "id":
                            int.TryParse(attributeNode.InnerText, out game.ID);
                            break;
                        case "GameTitle":
                            game.Title = attributeNode.InnerText;
                            break;
                        case "ReleaseDate":
                            game.ReleaseDate = attributeNode.InnerText;
                            break;
                        case "Platform":
                            game.Platform = attributeNode.InnerText;
                            break;
                    }
                }

                games.Add(game);
            }

            return games;
        }

        /// <summary>
        /// Gets all games updated since the specified time.
        /// </summary>
        /// <param name="time">Last x seconds to get updated games for</param>
        /// <returns>A collection of game ID's for games that have been updated</returns>
        public static ICollection<int> GetUpdatedGames(int time)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/Updates.php?time=" + time);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();
            ienum.MoveNext();

            List<int> games = new List<int>();

            // Iterate through all games
            XmlNode gameNode;
            while (ienum.MoveNext())
            {
                gameNode = (XmlNode)ienum.Current;

                int game;
                int.TryParse(gameNode.InnerText, out game);

                games.Add(game);
            }

            return games;
        }

        /// <summary>
        /// Gets the data for a specific game.
        /// </summary>
        /// <param name="iD">The game ID to return data for</param>
        /// <returns>A Game-object containing all the data about the game, or null if no game was found</returns>
        public static ApiGame GetGame(int iD)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/GetGame.php?id=" + iD);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            XmlNode platformNode = root.FirstChild.NextSibling;
            ApiGame apiGame = new ApiGame();

            IEnumerator ienumGame = platformNode.GetEnumerator();
            XmlNode attributeNode;
            while (ienumGame.MoveNext())
            {
                attributeNode = (XmlNode)ienumGame.Current;

                // Iterate through all platform attributes
                switch (attributeNode.Name)
                {
                    case "id":
                        int.TryParse(attributeNode.InnerText, out apiGame.ID);
                        break;
                    case "Overview":
                        apiGame.Overview = attributeNode.InnerText;
                        break;
                    case "GameTitle":
                        apiGame.Title = attributeNode.InnerText;
                        break;
                    case "Platform":
                        apiGame.Platform = attributeNode.InnerText;
                        break;
                    case "ReleaseDate":
                        apiGame.ReleaseDate = attributeNode.InnerText;
                        break;
                    case "overview":
                        apiGame.Overview = attributeNode.InnerText;
                        break;
                    case "ESRB":
                        apiGame.ESRB = attributeNode.InnerText;
                        break;
                    case "Players":
                        apiGame.Players = attributeNode.InnerText;
                        break;
                    case "Publisher":
                        apiGame.Publisher = attributeNode.InnerText;
                        break;
                    case "Developer":
                        apiGame.Developer = attributeNode.InnerText;
                        break;
                    case "Rating":
                        // double.TryParse(attributeNode.InnerText, out game.Rating);
                        apiGame.Rating = attributeNode.InnerText;
                        break;
                    case "AlternateTitles":
                        IEnumerator ienumAlternateTitles = attributeNode.GetEnumerator();
                        while (ienumAlternateTitles.MoveNext())
                        {
                            apiGame.AlternateTitles.Add(((XmlNode)ienumAlternateTitles.Current).InnerText);
                        }

                        break;
                    case "Genres":
                        IEnumerator ienumGenres = attributeNode.GetEnumerator();
                        while (ienumGenres.MoveNext())
                        {
                            apiGame.Genres.Add(((XmlNode)ienumGenres.Current).InnerText);
                        }

                        break;
                    case "Images":
                        apiGame.Images.FromXmlNode(attributeNode);
                        break;
                }
            }

            return apiGame;
        }

        /// <summary>
        /// Gets the data for a specific game.
        /// </summary>
        /// <param name="game">The game to return data for</param>
        /// <returns>A Game-object containing all the data about the game, or null if no game was found</returns>
        public static ApiGame GetGame(ApiGameSearchResult game)
        {
            return ApiGamesDb.GetGame(game.ID);
        }

        /// <summary>
        /// Gets a collection of all the available platforms.
        /// </summary>
        /// <returns>A collection of all the available platforms</returns>
        public static ICollection<ApiPlatformSearchResult> GetPlatforms()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/GetPlatformsList.php");

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.FirstChild.NextSibling.GetEnumerator();

            List<ApiPlatformSearchResult> platforms = new List<ApiPlatformSearchResult>();

            // Iterate through all platforms
            XmlNode platformNode;
            while (ienum.MoveNext())
            {
                platformNode = (XmlNode)ienum.Current;

                ApiPlatformSearchResult platform = new ApiPlatformSearchResult();

                IEnumerator ienumPlatform = platformNode.GetEnumerator();
                XmlNode attributeNode;
                while (ienumPlatform.MoveNext())
                {
                    attributeNode = (XmlNode)ienumPlatform.Current;

                    // Iterate through all platform attributes
                    switch (attributeNode.Name)
                    {
                        case "id":
                            int.TryParse(attributeNode.InnerText, out int id);
                            platform.ID = id;
                            break;
                        case "name":
                            platform.Name = attributeNode.InnerText;
                            break;
                        case "alias":
                            platform.Alias = attributeNode.InnerText;
                            break;
                    }
                }

                platforms.Add(platform);
            }

            return platforms;
        }

        /// <summary>
        /// Gets all data for a specific platform.
        /// </summary>
        /// <param name="iD">The platform ID to return data for (can be found by using GetPlatformsList)</param>
        /// <returns>A Platform-object containing all the data about the platform, or null if no platform was found</returns>
        public static ApiPlatform GetPlatform(int iD)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/GetPlatform.php?id=" + iD);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            XmlNode platformNode = root.FirstChild.NextSibling;
            ApiPlatform platform = new ApiPlatform();

            IEnumerator ienumPlatform = platformNode.GetEnumerator();
            XmlNode attributeNode;
            while (ienumPlatform.MoveNext())
            {
                attributeNode = (XmlNode)ienumPlatform.Current;

                // Iterate through all platform attributes
                switch (attributeNode.Name)
                {
                    case "id":
                        int.TryParse(attributeNode.InnerText, out int id);
                        platform.ID = id;
                        break;
                    case "Platform":
                        platform.Name = attributeNode.InnerText;
                        break;
                    case "overview":
                        platform.Overview = attributeNode.InnerText;
                        break;
                    case "developer":
                        platform.Developer = attributeNode.InnerText;
                        break;
                    case "manufacturer":
                        platform.Manufacturer = attributeNode.InnerText;
                        break;
                    case "cpu":
                        platform.CPU = attributeNode.InnerText;
                        break;
                    case "memory":
                        platform.Memory = attributeNode.InnerText;
                        break;
                    case "graphics":
                        platform.Graphics = attributeNode.InnerText;
                        break;
                    case "sound":
                        platform.Sound = attributeNode.InnerText;
                        break;
                    case "display":
                        platform.Display = attributeNode.InnerText;
                        break;
                    case "media":
                        platform.Media = attributeNode.InnerText;
                        break;
                    case "maxcontrollers":
                        int.TryParse(attributeNode.InnerText, out int maxControllers);
                        platform.MaxControllers = maxControllers;
                        break;
                    case "Rating":
                        float.TryParse(attributeNode.InnerText, out float rating);
                        platform.Rating = rating;
                        break;
                    case "Images":
                        platform.Images.FromXmlNode(attributeNode);
                        break;
                }
            }

            return platform;
        }

        /// <summary>
        /// Gets all data for a specific platform.
        /// </summary>
        /// <param name="platform">The platform to return data for (can be found by using GetPlatformsList)</param>
        /// <returns>A Platform-object containing all the data about the platform, or null if no platform was found</returns>
        public static ApiPlatform GetPlatform(ApiPlatformSearchResult platform)
        {
            return ApiGamesDb.GetPlatform(platform.ID);
        }

        /// <summary>
        /// Gets all the games for a platform. The Platform field will not be filled.
        /// </summary>
        /// <param name="iD">The platform ID to return games for (can be found by using GetPlatformsList)</param>
        /// <returns>A collection of all the games on the platform</returns>
        public static ICollection<ApiGameSearchResult> GetPlatformGames(int iD)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/GetPlatformGames.php?platform=" + iD);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            List<ApiGameSearchResult> games = new List<ApiGameSearchResult>();

            // Iterate through all games
            XmlNode gameNode;
            while (ienum.MoveNext())
            {
                ApiGameSearchResult game = new ApiGameSearchResult();
                gameNode = (XmlNode)ienum.Current;

                IEnumerator ienumGame = gameNode.GetEnumerator();
                XmlNode attributeNode;
                while (ienumGame.MoveNext())
                {
                    attributeNode = (XmlNode)ienumGame.Current;

                    // Iterate through all game attributes
                    switch (attributeNode.Name)
                    {
                        case "id":
                            int.TryParse(attributeNode.InnerText, out game.ID);
                            break;
                        case "GameTitle":
                            game.Title = attributeNode.InnerText;
                            break;
                        case "ReleaseDate":
                            game.ReleaseDate = attributeNode.InnerText;
                            break;
                    }
                }

                games.Add(game);
            }

            return games;
        }

        /// <summary>
        /// Gets all the games for a platform.
        /// </summary>
        /// <param name="platform">The platform to return games for</param>
        /// <returns>A collection of all the games on the platform</returns>
        public static ICollection<ApiGameSearchResult> GetPlatformGames(ApiPlatform platform)
        {
            ICollection<ApiGameSearchResult> games = ApiGamesDb.GetPlatformGames(platform.ID);
            foreach (ApiGameSearchResult game in games)
            {
                game.Platform = platform.Name;
            }

            return games;
        }

        /// <summary>
        /// Gets all the games for a platform.
        /// </summary>
        /// <param name="platform">The platform to return games for</param>
        /// <returns>A collection of all the games on the platform</returns>
        public static ICollection<ApiGameSearchResult> GetPlatformGames(ApiPlatformSearchResult platform)
        {
            ICollection<ApiGameSearchResult> games = ApiGamesDb.GetPlatformGames(platform.ID);
            foreach (ApiGameSearchResult game in games)
            {
                game.Platform = platform.Name;
            }

            return games;
        }

        /// <summary>
        /// Gets all of a user's favorites.
        /// </summary>
        /// <param name="accountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <returns>Collection of game ID:s</returns>
        public static ICollection<int> GetUserFavorites(string accountIdentifier)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/User_Favorites.php?accountid=" + accountIdentifier);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            List<int> favorites = new List<int>();

            IEnumerator ienumGame = root.GetEnumerator();
            XmlNode gameNode;
            while (ienumGame.MoveNext())
            {
                gameNode = (XmlNode)ienumGame.Current;

                int favorite = 0;
                int.TryParse(gameNode.InnerText, out favorite);
                favorites.Add(favorite);
            }

            return favorites;
        }

        /// <summary>
        /// Adds a game to the user's favorites.
        /// </summary>
        /// <param name="accountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="gameID">ID of the game to add</param>
        public static void AddUserFavorite(string accountIdentifier, int gameID)
        {
            ApiGamesDb.SendRequest(@"http://thegamesdb.net/api/User_Favorites.php?accountid=" + accountIdentifier + @"&type=add&gameid=" + gameID);
        }

        /// <summary>
        /// Removes a game from the user's favorites.
        /// </summary>
        /// <param name="accountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="gameID">ID of the game to remove</param>
        public static void RemoveUserFavorite(string accountIdentifier, int gameID)
        {
            ApiGamesDb.SendRequest(@"http://thegamesdb.net/api/User_Favorites.php?accountid=" + accountIdentifier + @"&type=remove&gameid=" + gameID);
        }

        /// <summary>
        /// Gets a user's rating of a specific game.
        /// </summary>
        /// <param name="accountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="gameID">ID of the game to get the rating of</param>
        /// <returns>A rating of 1 to 10 (or 0 if the user has not rated the game)</returns>
        public static int GetUserRating(string accountIdentifier, int gameID)
        {
            // Create an XML document instance.
            XmlDocument doc = new XmlDocument();

            // Load the XML data from a URL.
            doc.Load(@"http://thegamesdb.net/api/User_Rating.php?accountid=58536D31278176DA&itemid=2");

            XmlNode root = doc.DocumentElement;

            int rating = 0;
            int.TryParse(root.FirstChild.FirstChild.InnerText, out rating);
            return rating;
        }

        /// <summary>
        /// Sets a user's rating of a specific game.
        /// </summary>
        /// <param name="accountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="gameID">ID of the game to rate</param>
        /// <param name="rating">A rating of 1 to 10</param>
        public static void SetUserRating(string accountIdentifier, int gameID, int rating)
        {
            if (rating < 1 || rating > 10)
            {
                throw new ArgumentOutOfRangeException();
            }

            ApiGamesDb.SendRequest(@"http://thegamesdb.net/api/User_Rating.php?accountid=" + accountIdentifier + @"&itemid=" + gameID + @"&rating=" + rating);
        }

        /// <summary>
        /// Removes a user's rating of a specific game.
        /// </summary>
        /// <param name="accountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="gameID">ID of the game to remove the rating for</param>
        public static void RemoveUserRating(string accountIdentifier, int gameID)
        {
            ApiGamesDb.SendRequest(@"http://thegamesdb.net/api/User_Rating.php?accountid=" + accountIdentifier + @"&itemid=" + gameID + @"&rating=0");
        }

        private static void SendRequest(string uRL)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(uRL);
        }
    }
}
