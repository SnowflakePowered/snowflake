using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snowflake.Shell.Windows
{
    internal class ShellIcon
    {
        public NotifyIcon notifyIcon;
        internal ShellIcon()
        {
            this.notifyIcon = new NotifyIcon
            {
                Text = "Snowflake is running.",
                ContextMenu = new ContextMenu()
            };
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
