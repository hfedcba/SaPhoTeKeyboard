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
using System.Windows.Forms;
using System.IO;

namespace SaPhoTeKeyboard
{
    public static class Keys
    {
        private static String _FilePath = Application.StartupPath + "\\Keys.ini";

        private static Dictionary<int, Key> _Normal = new Dictionary<int, Key>();
        public static Dictionary<int, Key> Normal { get { return _Normal; } }

        private static Dictionary<int, Key> _Shift = new Dictionary<int, Key>();
        public static Dictionary<int, Key> Shift { get { return _Shift; } }

        private static Dictionary<int, Key> _AltGr = new Dictionary<int, Key>();
        public static Dictionary<int, Key> AltGr { get { return _AltGr; } }

        public static void LoadKeys()
        {
            ParseKeyFile();
        }

        private static void ParseKeyFile()
        {
            try
            {
                String[] Rows = LoadKeyFile();
                if (Rows == null || Rows.Length == 0) return;
                String[] Row;

                for (int i = 0; i < Rows.Length; i++)
                {
                    if (Rows[i].StartsWith("#")) continue;
                    Row = Rows[i].Split(";".ToCharArray());
                    if (Row.Length < 6) continue;

                    //0: Key code
                    //1: Telugu1
                    //2: Telugu2
                    //3: Type
                    //4: Shift
                    //5: AltGr
                    //6: SpecialFunction

                    Key Key = new Key();
                    Key.Telugu1 = Row[1].Trim();
                    Key.Telugu2 = Row[2].Trim();

                    switch (Row[3].Trim().ToLower())
                    {
                        case "number":
                            Key.KeyType = SaPhoTeKeyboard.Key.KeyTypes.Number;
                            break;
                        case "consonant":
                            Key.KeyType = SaPhoTeKeyboard.Key.KeyTypes.Consonant;
                            break;
                        case "vowel":
                            Key.KeyType = SaPhoTeKeyboard.Key.KeyTypes.Vowel;
                            break;
                    }

                    if (Row[6].Trim().ToLower() == "1") Key.KeyType = SaPhoTeKeyboard.Key.KeyTypes.Special;
                    if (Row[4].Trim().ToLower() == "1")
                    {
                        _Shift.Add(int.Parse(Row[0]), Key);
                    }
                    else if (Row[5].Trim().ToLower() == "1")
                    {
                        _AltGr.Add(int.Parse(Row[0]), Key);
                    }
                    else
                    {
                        _Normal.Add(int.Parse(Row[0]), Key);
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowError("ParseKeyFile", ex);
            }
        }

        private static String[] LoadKeyFile()
        {
            try
            {
                String[] Rows;
                using (FileStream FileStream = new FileStream(_FilePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader StreamReader = new StreamReader(FileStream, Encoding.UTF8))
                    {
                        Rows = StreamReader.ReadToEnd().Split("\n".ToCharArray());
                    }
                }
                return Rows;
            }
            catch (IOException ex)
            {
                Helpers.ShowError("LoadKeyFile", ex);
            }
            catch (Exception ex)
            {
                Helpers.ShowError("LoadKeyFile", ex);
            }
            return null;
        }
    }

    public class Key
    {
        public enum KeyTypes
        {
            Vowel,
            Consonant,
            Number,
            Special
        }

        public String Telugu1 = "";
        public String Telugu2 = "";
        public KeyTypes KeyType;
    }
}
