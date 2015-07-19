using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace Snowflake.Shell.Windows
{
    internal class ShellIcon
    {
        public NotifyIcon notifyIcon;
        internal ShellIcon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Text = "Snowflake is running.";
            this.notifyIcon.ContextMenu = new ContextMenu();
            Bitmap bmp = Properties.Resources.icon;
            this.notifyIcon.Icon = Icon.FromHandle(bmp.GetHicon());
            this.notifyIcon.Visible = true;  // Shows the notify icon in the system tray
        }

        public void AddMenuItem(string label, EventHandler menuEventHandler)
        {
            this.notifyIcon.ContextMenu.MenuItems.Add(new MenuItem(label, menuEventHandler));
        }
    }
}
