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
    static class Helpers
    {
        public static void ShowError(String Function, String Message)
        {
            System.Windows.Forms.MessageBox.Show("Error in function " + Function + ": " + Message, "SaPhoTeKeyboard", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        }

        public static void ShowError(String Function, Exception ex)
        {
            System.Windows.Forms.MessageBox.Show("Error in function " + Function + ": " + ex.Message + "\n\n" + ex.StackTrace, "SaPhoTeKeyboard", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        }
    }
}
