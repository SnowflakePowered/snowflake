using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Snowflake.Plugin.Scrapers.TheGamesDb.TheGamesDbApi
{
    /// <summary>
    /// Contains the data for one game in the database.
    /// </summary>
    internal class ApiGame
    {
        /// <summary>
        /// Unique database ID
        /// </summary>
        public int ID;

        /// <summary>
        /// Title of the game.
        /// </summary>
        public string Title;

        /// <summary>
        /// Which platform the game is for.
        /// </summary>
        public string Platform;

        /// <summary>
        /// Which date the game was first released on.
        /// </summary>
        public string ReleaseDate;

        /// <summary>
        /// A general description of the game.
        /// </summary>
        public string Overview;

        /// <summary>
        /// ESRB rating for the game.
        /// </summary>
        public string ESRB;

        /// <summary>
        /// How many players the game supports. "1","2","3" or "4+".
        /// </summary>
        public string Players;

        /// <summary>
        /// The publisher(s) of the game.
        /// </summary>
        public string Publisher;

        /// <summary>
        /// The developer(s) of the game.
        /// </summary>
        public string Developer;

        /// <summary>
        /// The overall rating of the game as rated by users on TheGamesDB.net.
        /// </summary>
        public string Rating;

        /// <summary>
        /// A list of all the alternative titles of the game.
        /// </summary>
        public List<string> AlternateTitles;

        /// <summary>
        /// A list of all the game's genres.
        /// </summary>
        public IList<string> Genres;

        /// <summary>
        /// A GameImages-object containing all the images for the game.
        /// </summary>
        public ApiGameImages Images;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiGame"/> class.
        /// Creates a new Game without any content.
        /// </summary>
        public ApiGame()
        {
            this.AlternateTitles = new List<string>();
            this.Genres = new List<string>();
            this.Images = new ApiGameImages();
        }

        /// <summary>
        /// Represents the images for one game in the database.
        /// </summary>
        public class ApiGameImages
        {
            /// <summary>
            /// The art on the back of the box.
            /// </summary>
            public ApiGameImage BoxartBack;

            /// <summary>
            /// The art on the front of the box.
            /// </summary>
            public ApiGameImage BoxartFront;

            /// <summary>
            /// A list of all the fanart for the game that have been uploaded to the database.
            /// </summary>
            public List<ApiGameImage> Fanart;

            /// <summary>
            /// A list of all the banners for the game that have been uploaded to the database.
            /// </summary>
            public List<ApiGameImage> Banners;

            /// <summary>
            /// A list of all the screenshots for the game that have been uploaded to the database.
            /// </summary>
            public List<ApiGameImage> Screenshots;

            /// <summary>
            /// Initializes a new instance of the <see cref="ApiGameImages"/> class.
            /// Creates a new GameImages without any content.
            /// </summary>
            public ApiGameImages()
            {
                this.Fanart = new List<ApiGameImage>();
                this.Banners = new List<ApiGameImage>();
                this.Screenshots = new List<ApiGameImage>();
            }

            /// <summary>
            /// Adds all the images that can be found in the XmlNode
            /// </summary>
            /// <param name="node">the XmlNode to search through</param>
            public void FromXmlNode(XmlNode node)
            {
                IEnumerator ienumImages = node.GetEnumerator();
                while (ienumImages.MoveNext())
                {
                    XmlNode imageNode = (XmlNode)ienumImages.Current;

                    switch (imageNode.Name)
                    {
                        case "fanart":
                            this.Fanart.Add(new ApiGameImage(imageNode.FirstChild));
                            break;
                        case "banner":
                            this.Banners.Add(new ApiGameImage(imageNode));
                            break;
                        case "screenshot":
                            this.Screenshots.Add(new ApiGameImage(imageNode.FirstChild));
                            break;
                        case "boxart":
                            if (imageNode.Attributes.GetNamedItem("side").InnerText == "front")
                            {
                                this.BoxartFront = new ApiGameImage(imageNode);
                            }
                            else
                            {
                                this.BoxartBack = new ApiGameImage(imageNode);
                            }

                            break;
                    }
                }
            }

            /// <summary>
            /// Represents one image
            /// </summary>
            public class ApiGameImage
            {
                /// <summary>
                /// The width of the image in pixels.
                /// </summary>
                public int Width;

                /// <summary>
                /// The height of the image in pixels.
                /// </summary>
                public int Height;

                /// <summary>
                /// The relative path to the image.
                /// </summary>
                /// <seealso cref="ApiGamesDb.BaseImgURL"/>
                public string Path;

                /// <summary>
                /// Initializes a new instance of the <see cref="ApiGameImage"/> class.
                /// Creates an image from an XmlNode.
                /// </summary>
                /// <param name="node">XmlNode to get data from</param>
                public ApiGameImage(XmlNode node)
                {
                    this.Path = node.InnerText;

                    int.TryParse(node.Attributes.GetNamedItem("width").InnerText, out this.Width);
                    int.TryParse(node.Attributes.GetNamedItem("height").InnerText, out this.Height);
                }
            }
        }
    }
}
