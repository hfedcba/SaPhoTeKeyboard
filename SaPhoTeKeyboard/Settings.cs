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
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace SaPhoTeKeyboard
{
    public static class Settings
    {
        private static String _AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SaPhoTeKeyboard\";
        private static String _FullPath = _AppDataPath + "Settings.dat";

        public enum KeyboardLayouts
        {
            English,
            German
        }

        public static KeyboardLayouts KeyboardLayout = KeyboardLayouts.English;

        public static void LoadSettings()
        {
            try
            {
                if (!File.Exists(_FullPath)) return;
                using (StreamReader StreamReader = new StreamReader(_FullPath, Encoding.UTF8, false))
                {
                    if (StreamReader.ReadLine().Trim() == "1") KeyboardLayout = KeyboardLayouts.German; else KeyboardLayout = KeyboardLayouts.English;
                }
            }
            catch (IOException ex)
            {
                Helpers.ShowError("LoadSettings", ex);
            }
            catch (Exception ex)
            {
                Helpers.ShowError("LoadSettings", ex);
            }
        }

        public static void DeleteSettings()
        {
            try
            {
                String[] Keys = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList").GetSubKeyNames();
                List<User> Users = new List<User>();
                RegistryKey Key;
                for (int i = 0; i < Keys.Length; i++)
                {
                    User User = new User();
                    User.SID = Keys[i];
                    Key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList\\" + Keys[i]);
                    if (Key != null)
                    {
                        User.ProfilePath = (String)Key.GetValue("ProfileImagePath");
                        if(User.ProfilePath != null) Users.Add(User);
                    }
                }

                for (int i = 0; i < Users.Count; i++)
                {
                    try
                    {
                        Key = Registry.Users.OpenSubKey(Users[i].SID + "\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders");
                        if (Key != null)
                        {
                            String UserAppDataPath = Key.GetValue("AppData", null, RegistryValueOptions.DoNotExpandEnvironmentNames).ToString();
                            if (UserAppDataPath != null)
                            {
                                UserAppDataPath = UserAppDataPath.Replace("%USERPROFILE%", Users[i].ProfilePath);
                                try
                                {
                                    if (Directory.Exists(UserAppDataPath + "\\SaPhoTeKeyboard")) Directory.Delete(UserAppDataPath + "\\SaPhoTeKeyboard", true);
                                }
                                catch
                                {
                                }
                            }
                        }

                        Key = Registry.Users.OpenSubKey(Users[i].SID + "\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                        if (Key != null)
                        {
                            Key.DeleteValue("SaPhoTeKeyboard", false);
                        }
                    }
                    catch (IOException ex)
                    {
                        Helpers.ShowError("DeleteSettings", ex);
                    }
                    catch (Exception ex)
                    {
                        Helpers.ShowError("DeleteSettings", ex);
                    }
                }
            }
            catch (IOException ex)
            {
                Helpers.ShowError("DeleteSettings", ex);
            }
            catch (Exception ex)
            {
                Helpers.ShowError("DeleteSettings", ex);
            }
        }

        public static void SaveSettings()
        {
            try
            {
                if (!Directory.Exists(_AppDataPath))
                {
                    Directory.CreateDirectory(_AppDataPath);
                }
                using (StreamWriter StreamWriter = new StreamWriter(_FullPath, false, Encoding.UTF8))
                {
                    if (KeyboardLayout == KeyboardLayouts.German) StreamWriter.WriteLine("1"); else StreamWriter.WriteLine("0");
                }
            }
            catch (IOException ex)
            {
                Helpers.ShowError("SaveSettings", ex);
            }
            catch (Exception ex)
            {
                Helpers.ShowError("SaveSettings", ex);
            }
        }
    }

    public class User
    {
        public String SID;
        public String ProfilePath;
    }
}
