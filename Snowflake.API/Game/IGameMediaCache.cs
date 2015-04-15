using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
namespace Snowflake.Game
{
    public interface IGameMediaCache
    {
        string RootPath { get; }
        void SetBoxartFront(Uri boxartFrontUri);
        void SetBoxartBack(Uri boxartBackUri);
        void SetBoxartFull(Uri boxartFrontUri);
        void SetGameFanart(Uri fanartUri);
        void SetGameVideo(Uri videoUri);
        void SetGameMusic(Uri musicUri);
        Image GetBoxartFrontImage();
        Image GetBoxartBackImage();
        Image GetBoxartFullImage();
        Image GetGameFanartImage();
    }
}
