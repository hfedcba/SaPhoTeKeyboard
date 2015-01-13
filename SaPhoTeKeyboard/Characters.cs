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

namespace SaPhoTeKeyboard
{
    static class Characters
    {
        private static String[] _Consonants = { "క", "ఖ", "గ", "ఘ", "ఙ", "చ", "ఛ", "జ", "ఝ", "ఞ", "ట", "ఠ", "డ", "ఢ", "ణ", "త", "థ", "ద", "ధ", "న", "ప", "ఫ", "బ", "భ", "మ", "య", "ర", "ల", "ళ", "వ", "శ", "ష", "స", "హ" };
        private static int[] _IgnoredKeys = {
            8, //Backspace
	        19, //Pause
	        20, //Caps
	        33, //Bild rauf
	        34, //Bild runter
	        44, //Druck
	        45, //Einfg
	        91, //Windows li
	        92, //Windows re
	        93, //Re. Maustaste
	        144, //NumLk
	        145, //Rollen
	        160, //Shift
	        161, //Shift
	        162, //Strg, Fensterwechsel, AltGr 1
	        164, //Alt
	        165, //AltGr 2, wird manchmal gesendet
	        231  //Wird von SendKeys als Backspace gesendet. Sorgt fuer das unvorhersehbare Schreiben von Visarga
        };

        public static bool ConsonantsContain(String Character)
        {
            return _Consonants.Contains(Character);
        }

        public static bool IgnoredKeysContain(int KeyCode)
        {
            return _IgnoredKeys.Contains(KeyCode);
        }
    }
}
