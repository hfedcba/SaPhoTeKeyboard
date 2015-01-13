/*
Copyright (C) 2004-2011 Lopeware

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

Am Schützenplatz 3
24211 Preetz
Germany

E-Mail: mail@sathya.de
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace SaPhoTeKeyboard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            String CommandLine = Environment.CommandLine;
            if (CommandLine.Trim().ToLower().EndsWith("/deletesettings"))
            {
                Settings.DeleteSettings();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                UserInterface UserInterface = new UserInterface();
                Application.Run();
            }
        }
    }

    class UserInterface
    {
        private Container _Container = new Container();
        private NotifyIcon _NotifyIcon;
        private KeyboardHook _KeyboardHook;

        public UserInterface()
        {
            Settings.LoadSettings();
            Keys.LoadKeys();

            _NotifyIcon = new NotifyIcon(_Container);
            _NotifyIcon.Visible = true;
            _NotifyIcon.Icon = new Icon(this.GetType(), "SaPhoTeKeyboard.ico");
            AddContextMenuEntries();

            _KeyboardHook = new KeyboardHook();
        }

        ~UserInterface()
        {
            _KeyboardHook = null;
        }

        private void AddContextMenuEntries() {
            _NotifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            _NotifyIcon.MouseDoubleClick += new MouseEventHandler(ShowSettings);

            MenuItem MenuItem = new MenuItem("Show key map", new EventHandler(ShowKeyMap));
            _NotifyIcon.ContextMenu.MenuItems.Add(MenuItem);

            MenuItem = new MenuItem("About", new EventHandler(ShowAbout));
            _NotifyIcon.ContextMenu.MenuItems.Add(MenuItem);

            MenuItem = new MenuItem("Settings", new EventHandler(ShowSettings));
            _NotifyIcon.ContextMenu.MenuItems.Add(MenuItem);

            MenuItem = new MenuItem("Exit", new EventHandler(delegate(object Object, EventArgs e) {
                _NotifyIcon.Visible = false;
                Application.Exit();
            }));
            _NotifyIcon.ContextMenu.MenuItems.Add(MenuItem);
        }

        private void ShowSettings(Object sender, EventArgs e) {
            frmSettings Settings = new frmSettings();
            Settings.Show();
        }

        private void ShowAbout(Object sender, EventArgs e)
        {
            frmAbout About = new frmAbout();
            About.Show();
        }

        private void ShowKeyMap(Object sender, EventArgs e)
        {
            frmKeyMap KeyMap = new frmKeyMap();
            KeyMap.Show();
        }
    }
}
