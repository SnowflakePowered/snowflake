using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Snowflake.Plugin.Scrapers.TheGamesDb.TheGamesDbApi
{
    /// <summary>
    /// Contains the data for one platform in the database.
    /// </summary>
    internal class ApiPlatform
    {
        /// <summary>
        /// Gets or sets unique database ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the platform.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the max amount of controllers that can be connected to the device.
        /// </summary>
        public int MaxControllers { get; set; }

        /// <summary>
        /// Gets or sets general overview of the platform.
        /// </summary>
        public string Overview { get; set; }

        /// <summary>
        /// Gets or sets the developer(s) of the platform.
        /// </summary>
        public string Developer { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer(s) of the platform.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the CPU of the platform (for platforms which have one specific CPU).
        /// </summary>
        public string CPU { get; set; }

        /// <summary>
        /// Gets or sets information about the platform's memory.
        /// </summary>
        public string Memory { get; set; }

        /// <summary>
        /// Gets or sets the platform's graphics card.
        /// </summary>
        public string Graphics { get; set; }

        /// <summary>
        /// Gets or sets information about the platform's sound capabilities.
        /// </summary>
        public string Sound { get; set; }

        /// <summary>
        /// Gets or sets display resolution (on the form: 'width'x'height')
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// Gets or sets the game media the platform reads (eg. 'Disc').
        /// </summary>
        public string Media { get; set; }

        /// <summary>
        /// Gets or sets the average rating as rated by the users on TheGamesDB.net.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Gets or sets a PlatformImages-object containing all the images for the platform.
        /// </summary>
        public PlatformImages Images { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiPlatform"/> class.
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
            /// Gets or sets path to the image of the console.
            /// </summary>
            public string ConsoleArt { get; set; }

            /// <summary>
            /// Gets or sets path to the image of the controller.
            /// </summary>
            public string ControllerArt { get; set; }

            /// <summary>
            /// Gets or sets boxart for the platform
            /// </summary>
            public PlatformImage Boxart { get; set; }

            /// <summary>
            /// Gets or sets a list of all the fanart for the platform that have been uploaded to the database.
            /// </summary>
            public List<PlatformImage> Fanart { get; set; }

            /// <summary>
            /// Gets or sets a list of all the banners for the platform that have been uploaded to the database.
            /// </summary>
            public List<PlatformImage> Banners { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="PlatformImages"/> class.
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
                /// Gets or sets the width of the image in pixels.
                /// </summary>
                public int Width { get; set; }

                /// <summary>
                /// Gets or sets the height of the image in pixels.
                /// </summary>
                public int Height { get; set; }

                /// <summary>
                /// Gets or sets the relative path to the image.
                /// </summary>
                /// <seealso cref="ApiGamesDb.BaseImgURL"/>
                public string Path { get; set; }

                /// <summary>
                /// Initializes a new instance of the <see cref="PlatformImage"/> class.
                /// Creates an image from an XmlNode.
                /// </summary>
                /// <param name="node">XmlNode to get data from</param>
                public PlatformImage(XmlNode node)
                {
                    this.Path = node.InnerText;

                    int.TryParse(node.Attributes.GetNamedItem("width").InnerText, out int width);
                    int.TryParse(node.Attributes.GetNamedItem("height").InnerText, out int height);
                    this.Width = width;
                    this.Height = height;
                }
            }
        }
    }
}
