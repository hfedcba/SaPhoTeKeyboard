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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SaPhoTeKeyboard
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            RegistryKey Key = Registry.CurrentUser;
            Key = Key.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion", true);
            Key = Key.CreateSubKey("Run");
            if (Key.GetValueNames().Contains("SaPhoTeKeyboard") && (String)Key.GetValue("SaPhoTeKeyboard") == Application.ExecutablePath) chkAutostart.Checked = true;

            if (Settings.KeyboardLayout == Settings.KeyboardLayouts.German) rdbGerman.Checked = true;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkAutostart_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Key = Registry.CurrentUser;
                Key = Key.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (chkAutostart.Checked)
                {
                    Key.SetValue("SaPhoTeKeyboard", Application.ExecutablePath, RegistryValueKind.String);
                }
                else
                {
                    Key.DeleteValue("SaPhoTeKeyboard");
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowError("chkAutostart_CheckedChanged", ex);
            }
        }

        private void rdbGerman_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbGerman.Checked) Settings.KeyboardLayout = Settings.KeyboardLayouts.German; else Settings.KeyboardLayout = Settings.KeyboardLayouts.English;
            Settings.SaveSettings();
        }
    }
}
