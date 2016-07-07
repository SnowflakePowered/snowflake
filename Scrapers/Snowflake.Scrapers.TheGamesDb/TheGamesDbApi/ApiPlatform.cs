using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Snowflake.Scrapers.TheGamesDb.TheGamesDbApi
{
    /// <summary>
    /// Contains the data for one platform in the database.
    /// </summary>
    internal class ApiPlatform
    {
        /// <summary>
        /// Unique database ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// The name of the platform.
        /// </summary>
        public string Name;

        /// <summary>
        /// The max amount of controllers that can be connected to the device.
        /// </summary>
        public int MaxControllers;

        /// <summary>
        /// General overview of the platform.
        /// </summary>
        public string Overview;

        /// <summary>
        /// The developer(s) of the platform.
        /// </summary>
        public string Developer;

        /// <summary>
        /// The manufacturer(s) of the platform.
        /// </summary>
        public string Manufacturer;

        /// <summary>
        /// The CPU of the platform (for platforms which have one specific CPU).
        /// </summary>
        public string CPU;

        /// <summary>
        /// Information about the platform's memory.
        /// </summary>
        public string Memory;

        /// <summary>
        /// The platform's graphics card.
        /// </summary>
        public string Graphics;

        /// <summary>
        /// Information about the platform's sound capabilities.
        /// </summary>
        public string Sound;

        /// <summary>
        /// Display resolution (on the form: 'width'x'height')
        /// </summary>
        public string Display;

        /// <summary>
        /// The game media the platform reads (eg. 'Disc').
        /// </summary>
        public string Media;

        /// <summary>
        /// The average rating as rated by the users on TheGamesDB.net.
        /// </summary>
        public float Rating;

        /// <summary>
        /// A PlatformImages-object containing all the images for the platform.
        /// </summary>
        public PlatformImages Images;

        /// <summary>
        /// Creates a new Platform without any content.
        /// </summary>
        public ApiPlatform()
        {
            this.Images = new PlatformImages();
        }

        /// <summary>
        /// Represents the images for one platform in the database.
        /// </summary>
        public class PlatformImages
        {
            /// <summary>
            /// Path to the image of the console.
            /// </summary>
            public string ConsoleArt;

            /// <summary>
            /// Path to the image of the controller.
            /// </summary>
            public string ControllerArt;

            /// <summary>
            /// Boxart for the platform
            /// </summary>
            public PlatformImage Boxart;

            /// <summary>
            /// A list of all the fanart for the platform that have been uploaded to the database.
            /// </summary>
            public List<PlatformImage> Fanart;

            /// <summary>
            /// A list of all the banners for the platform that have been uploaded to the database.
            /// </summary>
            public List<PlatformImage> Banners;

            /// <summary>
            /// Creates a new PlatformImages without any content.
            /// </summary>
            public PlatformImages()
            {
                this.Fanart = new List<PlatformImage>();
                this.Banners = new List<PlatformImage>();
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
                            this.Fanart.Add(new PlatformImage(imageNode.FirstChild));
                            break;
                        case "banner":
                            this.Banners.Add(new PlatformImage(imageNode));
                            break;
                        case "boxart":
                            this.Boxart = new PlatformImage(imageNode);
                            break;
                        case "consoleart":
                            this.ConsoleArt = imageNode.InnerText;
                            break;
                        case "controllerart":
                            this.ControllerArt = imageNode.InnerText;
                            break;
                    }
                }
            }

            /// <summary>
            /// Represents one image
            /// </summary>
            public class PlatformImage
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
                /// Creates an image from an XmlNode.
                /// </summary>
                /// <param name="node">XmlNode to get data from</param>
                public PlatformImage(XmlNode node)
                {
                    this.Path = node.InnerText;

                    int.TryParse(node.Attributes.GetNamedItem("width").InnerText, out this.Width);
                    int.TryParse(node.Attributes.GetNamedItem("height").InnerText, out this.Height);
                }
            }
        }
    }
}
