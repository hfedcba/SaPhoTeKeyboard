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
using System.Diagnostics;

namespace SaPhoTeKeyboard
{
    public class MouseHook : IDisposable
    {
        private int _HookHandle = 0;
        private WinAPI.HookProcDelegate _HookProc;

        public event EventHandler Click;
        public event EventHandler RightClick;

        public MouseHook()
        {
            HookMouse();
        }

        ~MouseHook()
        {
            Dispose(false); //Do not re-create Dispose clean-up code here.
        }

        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            WinAPI.MouseHookStruct MouseHookStruct;

            if (nCode < 0)
            {
                return WinAPI.CallNextHookEx(_HookHandle, nCode, wParam, lParam);
            }

            MouseHookStruct = (WinAPI.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(WinAPI.MouseHookStruct));

            switch (wParam.ToInt32())
            {
                case 513: //Down
                    Click(this, new EventArgs());
                    break;
                case 514: //Up
                    break;
                case 516: //Down
                    RightClick(this, new EventArgs());
                    break;
                case 517: //Up
                    break;
            }

            return WinAPI.CallNextHookEx(_HookHandle, nCode, wParam, lParam);
        }

        public void HookMouse()
        {
            _HookProc = new WinAPI.HookProcDelegate(MouseHookProc);
            _HookHandle = WinAPI.SetWindowsHookEx(WinAPI.WH_MOUSE_LL, _HookProc, IntPtr.Zero, 0);
            if (_HookHandle != 0)
            {
                Debug.WriteLine("Mouse hooked");
            }
        }

        public void UnhookMouse()
        {
            if (_HookHandle != 0)
            {
                WinAPI.UnhookWindowsHookEx(_HookHandle);
                _HookHandle = 0;
            }
        }

        private bool _Disposed = false;

        protected void Dispose(bool Disposing)
        {
            if (!_Disposed)
            {
                if (Disposing)
                {
                    //Dispose managed resources
                }

                //Dispose unmanaged resources
                UnhookMouse();
                _Disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class KeyboardHook : IDisposable
    {
        private Analysis _Analysis;
        private int _HookHandle = 0;
        private WinAPI.HookProcDelegate _HookProc;

        public KeyboardHook()
        {
            _Analysis = new Analysis();
            HookKeyboard();
        }

        public int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            WinAPI.KBDLLHOOKSTRUCT KBDLLHOOKSTRUCT;
            KBDLLHOOKSTRUCT = (WinAPI.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(WinAPI.KBDLLHOOKSTRUCT));

            if (nCode == WinAPI.HC_ACTION)
            {
                if (_Analysis.Analyze(KBDLLHOOKSTRUCT.vkCode, KBDLLHOOKSTRUCT.flags))
                {
                    return -1;
                }
            }

            return WinAPI.CallNextHookEx(_HookHandle, nCode, wParam, lParam);
        }

        public void HookKeyboard()
        {
            _HookProc = new WinAPI.HookProcDelegate(KeyboardHookProc);
            _HookHandle = WinAPI.SetWindowsHookEx(WinAPI.WH_KEYBOARD_LL, _HookProc, IntPtr.Zero, 0);
            if (_HookHandle != 0)
            {
                Debug.WriteLine("Keyboard hooked");
            }
        }

        public void UnhookKeyboard()
        {
            if (_HookHandle != 0)
            {
                WinAPI.UnhookWindowsHookEx(_HookHandle);
                _HookHandle = 0;
            }
        }

        private bool _Disposed = false;

        protected void Dispose(bool Disposing)
        {
            if (!_Disposed)
            {
                if (Disposing)
                {
                    //Dispose managed resources
                }

                //Dispose unmanaged resources
                UnhookKeyboard();
                _Disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}