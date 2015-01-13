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
using System.Runtime.InteropServices;

namespace SaPhoTeKeyboard
{
    /*
       Flags:
           0:      Key down
           128:    Key up
           16:     Bildschirmtastatur-Tastendruck
           144:    Bildschirmtastatur loslassen
           33:     Alt-Gr-Druck
           1:      Pfeiltasten-Druck
           129:    Pfeiltasten-Loslassen
     */
    public class Analysis
    {
        private MouseHook _MouseHook;
        private List<String> _KeyCache = new List<String>();

        public Analysis()
        {
            _MouseHook = new MouseHook();
            _MouseHook.Click += new EventHandler(MouseHook_Click);
            _MouseHook.RightClick += new EventHandler(MouseHook_RightClick);
        }

        ~Analysis()
        {
            _MouseHook = null;
        }

        public bool Analyze(int KeyCode, int Flags) {
            uint HKL = (uint)WinAPI.GetKeyboardLayout(WinAPI.GetWindowThreadProcessId(WinAPI.GetForegroundWindow(), IntPtr.Zero)).ToInt32();
            HKL = (HKL >> 16) & 0x3FF; //Remove the sublanguage id
            if (HKL != WinAPI.LANG_TELUGU) return false;

            bool KeyDownFlag = false;
            //Normales Keydown (flags == 0), Bildschirmtastatur-KeyUp (flags == LLKHF_INJECTED + LLKHF_UP) oder Alt-Gr-KeyDown (flags = 32)
            if (Flags == 0 || (Flags == WinAPI.LLKHF_INJECTED + WinAPI.LLKHF_UP) || Flags == WinAPI.LLKHF_ALTDOWN) KeyDownFlag = true;

            KeyCode = SwitchYAndZ(KeyCode);

            if (KeyDownFlag && KeyCode == 8 && _KeyCache.Count > 0)
            {
                if(ProcessBackspace()) return true;
            }

            int IgnoredKey = 1;
            if (Characters.IgnoredKeysContain(KeyCode))
            {
                IgnoredKey = KeyCode;
            }
            else
            {
                if (Flags == 1) ClearKeyCache(); //Pfeiltasten
            }

            if (KeyDownFlag && GetKeyState(System.Windows.Forms.Keys.ShiftKey))
            {
                return ProcessShift(KeyCode, IgnoredKey);
            }
            else if (KeyDownFlag && GetKeyState(System.Windows.Forms.Keys.RMenu))
            {
                return ProcessAltGr(KeyCode, IgnoredKey);
            }
            else if (GetKeyState(System.Windows.Forms.Keys.ControlKey) || GetKeyState(System.Windows.Forms.Keys.RControlKey) || GetKeyState(System.Windows.Forms.Keys.Menu) || GetKeyState(System.Windows.Forms.Keys.RWin) || GetKeyState(System.Windows.Forms.Keys.LWin))
            {
                //Ignore
            }
            else if (KeyDownFlag)
            {
                return ProcessNormalKey(KeyCode, IgnoredKey);
            }

            return false;
        }

        private bool ProcessShift(int KeyCode, int IgnoredKey)
        {
            if (Keys.Shift.ContainsKey(KeyCode))
            {
                return ProcessKey(KeyCode, Keys.Shift[KeyCode]);
            }
            else
            {
                return ProcessSpecialKey(KeyCode, IgnoredKey);
            }
        }

        private bool ProcessAltGr(int KeyCode, int IgnoredKey)
        {
            if (Keys.AltGr.ContainsKey(KeyCode))
            {
                return ProcessKey(KeyCode, Keys.AltGr[KeyCode]);
            }
            else
            {
                return ProcessSpecialKey(KeyCode, IgnoredKey);
            }
        }

        private bool ProcessNormalKey(int KeyCode, int IgnoredKey)
        {
            if (Keys.Normal.ContainsKey(KeyCode))
            {
                return ProcessKey(KeyCode, Keys.Normal[KeyCode]);
            }
            else
            {
                return ProcessSpecialKey(KeyCode, IgnoredKey);
            }
        }

        private bool ProcessSpecialKey(int KeyCode, int IgnoredKey)
        {
            if (KeyCode == IgnoredKey) return false;
            switch (KeyCode)
            {
                case 9: //Tab
                    return ProcessWhitespace("\t");
                case 13: //Return
                    return ProcessWhitespace("\r");
                case 32: //Space
                    return ProcessWhitespace(" ");
                default:
                    ClearKeyCache();
                    break;
            }
            return false;
        }

        private bool ProcessWhitespace(String Whitespace)
        {
            if (_KeyCache.Count > 0 && Characters.ConsonantsContain(_KeyCache[_KeyCache.Count - 1])) SendKey("్");
            _KeyCache.Add(Whitespace);
            return false;
        }

        private bool ProcessKey(int KeyCode, Key Key)
        {
            switch (Key.KeyType)
            {
                case Key.KeyTypes.Vowel:
                    return ProcessVowel(Key.Telugu1, Key.Telugu2);
                case Key.KeyTypes.Consonant:
                    return ProcessConsonant(Key.Telugu1);
                case Key.KeyTypes.Number:
                    return ProcessNumber(Key.Telugu1);
                case Key.KeyTypes.Special:
                    switch (KeyCode)
                    {
                        case 65: //a
                            return ProcessA();
                        case 72: //h
                            return ProcessH();
                    }
                    break;
            }
            return false;
        }

        private bool ProcessA()
        {
            String LastCacheItem = "";
            if (_KeyCache.Count > 0) LastCacheItem = _KeyCache[_KeyCache.Count - 1];
            if (Characters.ConsonantsContain(LastCacheItem))
            {
                _KeyCache[_KeyCache.Count - 1] += "అ"; //Damit kein Konsonant mehr im Cache steht
            }
            else if (LastCacheItem == "్")
            {
                SendKey("\b");
                _KeyCache[_KeyCache.Count - 1] += "అ";
            }
            else
            {
                SendKey("అ");
            }
            return true;
        }

        private bool ProcessH()
        {
            if (_KeyCache.Count > 0)
            {
                switch (_KeyCache[_KeyCache.Count - 1])
                {
                    case "బ":
                        SendKey("\b", "భ");
                        break;
                    case "చ":
                        SendKey("\b", "ఛ");
                        break;
                    case "ద":
                        SendKey("\b", "ధ");
                        break;
                    case "డ":
                        SendKey("\b", "ఢ");
                        break;
                    case "గ":
                        SendKey("\b", "ఘ");
                        break;
                    case "జ":
                        SendKey("\b", "ఝ");
                        break;
                    case "క":
                        SendKey("\b", "ఖ");
                        break;
                    case "ప":
                        SendKey("\b", "ఫ");
                        break;
                    case "స":
                        SendKey("\b", "శ");
                        break;
                    case "త":
                        SendKey("\b", "థ");
                        break;
                    case "ట":
                        SendKey("\b", "ఠ");
                        break;
                    default:
                        ProcessConsonant("హ");
                        break;
                }
            }
            else
            {
                ProcessConsonant("హ");
            }
            return true;
        }

        private bool ProcessVowel(String Vowel, String VowelAppendix)
        {
            String LastCacheItem = "";
            if (_KeyCache.Count > 0) LastCacheItem = _KeyCache[_KeyCache.Count - 1];
            if (Characters.ConsonantsContain(LastCacheItem) || (LastCacheItem.Length == 2 && LastCacheItem.Substring(1, 1) == "అ"))
            {
                SendKey(VowelAppendix);
            }
            else if (LastCacheItem == "్")
            {
                SendKey("\b");
                SendKey(VowelAppendix);
            }
            else
            {
                SendKey(Vowel);
            }
            return true;
        }

        private bool ProcessConsonant(String Consonant)
        {
            String FirstKey = "";
            if (_KeyCache.Count > 0 && Characters.ConsonantsContain(_KeyCache[_KeyCache.Count - 1])) FirstKey = "్";
            SendKey(FirstKey, Consonant);
            return true;
        }

        private bool ProcessNumber(String Number)
        {
            SendKey(Number);
            return true;
        }

        private bool GetKeyState(System.Windows.Forms.Keys vKey)
        {
            ushort KeyState = WinAPI.GetAsyncKeyState(vKey);
            return (KeyState & 0x8000) > 0; //Only the MSB is interesting
        }

        public bool ProcessBackspace()
        {
            if (_KeyCache[_KeyCache.Count - 1].Length == 2)
            {
                if (_KeyCache[_KeyCache.Count - 1].Substring(1, 1) == "అ")
                { //Das Kurzform-A muss auch gelöscht werden
                    _KeyCache[_KeyCache.Count - 1] = _KeyCache[_KeyCache.Count - 1].Substring(0, 1);
                    return true;
                }
            }
            else
            {
                _KeyCache.RemoveAt(_KeyCache.Count - 1);
            }
            return false;
        }

        public void ClearKeyCache()
        {
            if (_KeyCache.Count > 0 && Characters.ConsonantsContain(_KeyCache[_KeyCache.Count - 1]))
            {
                SendKey("్");
            }
            _KeyCache.Clear();
        }

        public void SendKey(String Key1)
        {
            SendKey(Key1, "");
        }

        public void SendKey(String Key1, String Key2)
        {
            String Key = "";
            for (int i = 0; i < 2; i++)
            {
                Key = (i == 0) ? Key1 : Key2;
                if (Key == "\b")
                {
                    _KeyCache.RemoveAt(_KeyCache.Count - 1);
                    SendKeys.SendNonUnicode(8);
                }
                else if (Key == "\r")
                {
                    _KeyCache.Add(Key);
                    SendKeys.SendNonUnicode(13);
                }
                else if (Key == "\t")
                {
                    _KeyCache.Add(Key);
                    SendKeys.SendNonUnicode(9);
                }
                else
                {
                    for (int j = 0; j < Key.Length; j++)
                    {
                        _KeyCache.Add(Key.Substring(j, 1));
                        SendKeys.Send(Key[j]);
                    }
                }
                if (Key != "") System.Threading.Thread.Sleep(5);
                if (Key2 == "") return;
            }
        }

        private int SwitchYAndZ(int KeyCode)
        {
            if (Settings.KeyboardLayout == Settings.KeyboardLayouts.German)
            {
                if (KeyCode == 89)
                {
                    return 90;
                }
                else if (KeyCode == 90)
                {
                    return 89;
                }
            }
            return KeyCode;
        }

        void MouseHook_RightClick(object sender, EventArgs e)
        {
            ClearKeyCache();
        }

        void MouseHook_Click(object sender, EventArgs e)
        {
            ClearKeyCache();
        }
    }
}
