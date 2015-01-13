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
    static class SendKeys
    {
        public static void Send(ushort KeyCode)
        {
            Send(KeyCode, true);
        }

        public static void Send(String Keys)
        {
            for (int i = 0; i < Keys.Length; i++)
            {
                Send(Keys[i], true);
            }
        }

        public static void Send(Char Key)
        {
            Send(Key, true);
        }

        public static void SendNonUnicode(ushort KeyCode)
        {
            Send(KeyCode, false);
        }

        public static void SendNonUnicode(String Keys)
        {
            for (int i = 0; i < Keys.Length; i++)
            {
                Send(Keys[i], false);
            }
        }

        public static void SendNonUnicode(Char Key)
        {
            Send(Key, false);
        }

        private static void Send(ushort KeyCode, bool Unicode)
        {
            WinAPI.INPUT[] Input = new WinAPI.INPUT[2];
            Input[0].type = WinAPI.INPUT_KEYBOARD;
            Input[0].ki = new WinAPI.KEYBDINPUT();
            Input[0].ki.time = 0;

            Input[1].type = WinAPI.INPUT_KEYBOARD;
            Input[1].ki = new WinAPI.KEYBDINPUT();
            Input[1].ki.time = 0;

            if (Unicode)
            {
                Input[0].ki.wVk = 0;
                Input[0].ki.wScan = KeyCode;
                Input[0].ki.dwFlags = WinAPI.KEYEVENTF_UNICODE;

                Input[1].ki.wVk = 0;
                Input[1].ki.wScan = KeyCode;
                Input[1].ki.dwFlags = WinAPI.KEYEVENTF_UNICODE + WinAPI.KEYEVENTF_KEYUP;
            }
            else
            {
                Input[0].ki.wVk = KeyCode;
                Input[0].ki.wScan = 0;
                Input[0].ki.dwFlags = 0;

                Input[1].ki.wVk = KeyCode;
                Input[1].ki.wScan = 0;
                Input[1].ki.dwFlags = WinAPI.KEYEVENTF_KEYUP;
            }

            uint result = WinAPI.SendInput(2, Input, Marshal.SizeOf(typeof(WinAPI.INPUT)));
            if (result == 0)
            {
                StringBuilder Buffer = new StringBuilder(200);
                WinAPI.FormatMessage(WinAPI.FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, Marshal.GetLastWin32Error(), WinAPI.LANG_NEUTRAL, Buffer, 200, IntPtr.Zero);
                Helpers.ShowError("Send", Buffer.ToString());
            }
        }
    }
}
