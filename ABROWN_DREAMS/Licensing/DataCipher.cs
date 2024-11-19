using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABROWN_DREAMS
{
    public class DataCipher
    {
        public static string DataEncrypt(string DataValue)
        {
            string CipherValue = null;
            foreach (char str in DataValue)
            {
                CipherValue = CipherValue + CipherEncrypt(str.ToString());
            }
            return CipherValue;
        }

        public static string DataDecrypt(string DataValue)
        {
            string CipherValue = null;
            int cnt = DataValue.Length;

            for (int i = 0; i < cnt; i = i + 2)
            {
                CipherValue = CipherValue + CipherDecrypt(DataValue.Substring(i, 2));
            }

            return CipherValue;
        }

        private static string CipherDecrypt(string CipherValue)
        {
            string CipherEncrypt = CipherValue.Replace("_", "");
            switch (CipherValue)
            {
                case "Z0":
                    CipherEncrypt = "A";
                    break;
                case "2B":
                    CipherEncrypt = "B";
                    break;
                case "X8":
                    CipherEncrypt = "C";
                    break;
                case "4d":
                    CipherEncrypt = "D";
                    break;
                case "V6":
                    CipherEncrypt = "E";
                    break;
                case "7f":
                    CipherEncrypt = "F";
                    break;
                case "T3":
                    CipherEncrypt = "G";
                    break;
                case "9h":
                    CipherEncrypt = "H";
                    break;
                case "R1":
                    CipherEncrypt = "I";
                    break;
                case "4j":
                    CipherEncrypt = "J";
                    break;
                case "P8":
                    CipherEncrypt = "K";
                    break;
                case "2l":
                    CipherEncrypt = "L";
                    break;
                case "N0":
                    CipherEncrypt = "M";
                    break;
                case "1n":
                    CipherEncrypt = "N";
                    break;
                case "L9":
                    CipherEncrypt = "O";
                    break;
                case "3p":
                    CipherEncrypt = "P";
                    break;
                case "J7":
                    CipherEncrypt = "Q";
                    break;
                case "5r":
                    CipherEncrypt = "R";
                    break;
                case "H0":
                    CipherEncrypt = "S";
                    break;
                case "2t":
                    CipherEncrypt = "T";
                    break;
                case "F8":
                    CipherEncrypt = "U";
                    break;
                case "4v":
                    CipherEncrypt = "V";
                    break;
                case "D6":
                    CipherEncrypt = "W";
                    break;
                case "7x":
                    CipherEncrypt = "X";
                    break;
                case "B3":
                    CipherEncrypt = "Y";
                    break;
                case "9z":
                    CipherEncrypt = "Z";
                    break;

                case "a0":
                    CipherEncrypt = "a";
                    break;
                case "Y2":
                    CipherEncrypt = "b";
                    break;
                case "c8":
                    CipherEncrypt = "c";
                    break;
                case "W4":
                    CipherEncrypt = "d";
                    break;
                case "e6":
                    CipherEncrypt = "e";
                    break;
                case "U7":
                    CipherEncrypt = "f";
                    break;
                case "g3":
                    CipherEncrypt = "g";
                    break;
                case "S9":
                    CipherEncrypt = "h";
                    break;
                case "i1":
                    CipherEncrypt = "i";
                    break;
                case "Q4":
                    CipherEncrypt = "j";
                    break;
                case "k8":
                    CipherEncrypt = "k";
                    break;
                case "O2":
                    CipherEncrypt = "l";
                    break;
                case "m0":
                    CipherEncrypt = "m";
                    break;
                case "M1":
                    CipherEncrypt = "n";
                    break;
                case "o9":
                    CipherEncrypt = "o";
                    break;
                case "K3":
                    CipherEncrypt = "p";
                    break;
                case "q7":
                    CipherEncrypt = "q";
                    break;
                case "I5":
                    CipherEncrypt = "r";
                    break;
                case "s0":
                    CipherEncrypt = "s";
                    break;
                case "G2":
                    CipherEncrypt = "t";
                    break;
                case "u8":
                    CipherEncrypt = "u";
                    break;
                case "E4":
                    CipherEncrypt = "v";
                    break;
                case "w6":
                    CipherEncrypt = "w";
                    break;
                case "C7":
                    CipherEncrypt = "x";
                    break;
                case "y3":
                    CipherEncrypt = "y";
                    break;
                case "A9":
                    CipherEncrypt = "z";
                    break;

                case "/=":
                    CipherEncrypt = "1";
                    break;
                case "o#":
                    CipherEncrypt = "2";
                    break;
                case "sG":
                    CipherEncrypt = "3";
                    break;
                case "=P":
                    CipherEncrypt = "4";
                    break;
                case "t/":
                    CipherEncrypt = "5";
                    break;
                case "8x":
                    CipherEncrypt = "6";
                    break;
                case "pG":
                    CipherEncrypt = "7";
                    break;
                case "jD":
                    CipherEncrypt = "8";
                    break;
                case "2L":
                    CipherEncrypt = "9";
                    break;
                case "zR":
                    CipherEncrypt = "0";
                    break;

                case "3e":
                    CipherEncrypt = "'";
                    break;
                case "3d":
                    CipherEncrypt = "-";
                    break;
                case "3c":
                    CipherEncrypt = "!";
                    break;
                case "2b":
                    CipherEncrypt = "\"";
                    break;
                case "7e":
                    CipherEncrypt = "#";
                    break;
                case "7d":
                    CipherEncrypt = "$";
                    break;
                case "7c":
                    CipherEncrypt = "%";
                    break;
                case "7b":
                    CipherEncrypt = "&";
                    break;
                case "60":
                    CipherEncrypt = "(";
                    break;
                case "5f":
                    CipherEncrypt = ")";
                    break;
                case "5e":
                    CipherEncrypt = "*";
                    break;
                case "5d":
                    CipherEncrypt = ",";
                    break;
                case "5c":
                    CipherEncrypt = ".";
                    break;
                case "5b":
                    CipherEncrypt = ":";
                    break;
                case "40":
                    CipherEncrypt = ";";
                    break;
                case "3f":
                    CipherEncrypt = "?";
                    break;
                case "3b":
                    CipherEncrypt = "@";
                    break;
                case "3a":
                    CipherEncrypt = "[";
                    break;
                case "2e":
                    CipherEncrypt = "\\";
                    break;
                case "2c":
                    CipherEncrypt = "]";
                    break;
                case "2a":
                    CipherEncrypt = "^";
                    break;
                case "29":
                    CipherEncrypt = "_";
                    break;
                case "28":
                    CipherEncrypt = "`";
                    break;
                case "26":
                    CipherEncrypt = "{";
                    break;
                case "25":
                    CipherEncrypt = "|";
                    break;
                case "24":
                    CipherEncrypt = "}";
                    break;
                case "23":
                    CipherEncrypt = "~";
                    break;
                case "22":
                    CipherEncrypt = "+";
                    break;
                case "21":
                    CipherEncrypt = "<";
                    break;
                case "2d":
                    CipherEncrypt = "=";
                    break;
                case "27":
                    CipherEncrypt = ">";
                    break;
            }

            return CipherEncrypt;
        }

        private static string CipherEncrypt(string CipherValue)
        {
            string CipherEncrypt = CipherValue + "_";
            switch (CipherValue)
            {
                case "A":
                    CipherEncrypt = "Z0";
                    break;
                case "B":
                    CipherEncrypt = "2B";
                    break;
                case "C":
                    CipherEncrypt = "X8";
                    break;
                case "D":
                    CipherEncrypt = "4d";
                    break;
                case "E":
                    CipherEncrypt = "V6";
                    break;
                case "F":
                    CipherEncrypt = "7f";
                    break;
                case "G":
                    CipherEncrypt = "T3";
                    break;
                case "H":
                    CipherEncrypt = "9h";
                    break;
                case "I":
                    CipherEncrypt = "R1";
                    break;
                case "J":
                    CipherEncrypt = "4j";
                    break;
                case "K":
                    CipherEncrypt = "P8";
                    break;
                case "L":
                    CipherEncrypt = "2l";
                    break;
                case "M":
                    CipherEncrypt = "N0";
                    break;
                case "N":
                    CipherEncrypt = "1n";
                    break;
                case "O":
                    CipherEncrypt = "L9";
                    break;
                case "P":
                    CipherEncrypt = "3p";
                    break;
                case "Q":
                    CipherEncrypt = "J7";
                    break;
                case "R":
                    CipherEncrypt = "5r";
                    break;
                case "S":
                    CipherEncrypt = "H0";
                    break;
                case "T":
                    CipherEncrypt = "2t";
                    break;
                case "U":
                    CipherEncrypt = "F8";
                    break;
                case "V":
                    CipherEncrypt = "4v";
                    break;
                case "W":
                    CipherEncrypt = "D6";
                    break;
                case "X":
                    CipherEncrypt = "7x";
                    break;
                case "Y":
                    CipherEncrypt = "B3";
                    break;
                case "Z":
                    CipherEncrypt = "9z";
                    break;

                case "a":
                    CipherEncrypt = "a0";
                    break;
                case "b":
                    CipherEncrypt = "Y2";
                    break;
                case "c":
                    CipherEncrypt = "c8";
                    break;
                case "d":
                    CipherEncrypt = "W4";
                    break;
                case "e":
                    CipherEncrypt = "e6";
                    break;
                case "f":
                    CipherEncrypt = "U7";
                    break;
                case "g":
                    CipherEncrypt = "g3";
                    break;
                case "h":
                    CipherEncrypt = "S9";
                    break;
                case "i":
                    CipherEncrypt = "i1";
                    break;
                case "j":
                    CipherEncrypt = "Q4";
                    break;
                case "k":
                    CipherEncrypt = "k8";
                    break;
                case "l":
                    CipherEncrypt = "O2";
                    break;
                case "m":
                    CipherEncrypt = "m0";
                    break;
                case "n":
                    CipherEncrypt = "M1";
                    break;
                case "o":
                    CipherEncrypt = "o9";
                    break;
                case "p":
                    CipherEncrypt = "K3";
                    break;
                case "q":
                    CipherEncrypt = "q7";
                    break;
                case "r":
                    CipherEncrypt = "I5";
                    break;
                case "s":
                    CipherEncrypt = "s0";
                    break;
                case "t":
                    CipherEncrypt = "G2";
                    break;
                case "u":
                    CipherEncrypt = "u8";
                    break;
                case "v":
                    CipherEncrypt = "E4";
                    break;
                case "w":
                    CipherEncrypt = "w6";
                    break;
                case "x":
                    CipherEncrypt = "C7";
                    break;
                case "y":
                    CipherEncrypt = "y3";
                    break;
                case "z":
                    CipherEncrypt = "A9";
                    break;

                case "1":
                    CipherEncrypt = "/=";
                    break;
                case "2":
                    CipherEncrypt = "o#";
                    break;
                case "3":
                    CipherEncrypt = "sG";
                    break;
                case "4":
                    CipherEncrypt = "=P";
                    break;
                case "5":
                    CipherEncrypt = "t/";
                    break;
                case "6":
                    CipherEncrypt = "8x";
                    break;
                case "7":
                    CipherEncrypt = "pG";
                    break;
                case "8":
                    CipherEncrypt = "jD";
                    break;
                case "9":
                    CipherEncrypt = "2L";
                    break;
                case "0":
                    CipherEncrypt = "zR";
                    break;

                case "'":
                    CipherEncrypt = "3e";
                    break;
                case "-":
                    CipherEncrypt = "3d";
                    break;
                case "!":
                    CipherEncrypt = "3c";
                    break;
                case "\"":
                    CipherEncrypt = "2b";
                    break;
                case "#":
                    CipherEncrypt = "7e";
                    break;
                case "$":
                    CipherEncrypt = "7d";
                    break;
                case "%":
                    CipherEncrypt = "7c";
                    break;
                case "&":
                    CipherEncrypt = "7b";
                    break;
                case "(":
                    CipherEncrypt = "60";
                    break;
                case ")":
                    CipherEncrypt = "5f";
                    break;
                case "*":
                    CipherEncrypt = "5e";
                    break;
                case ",":
                    CipherEncrypt = "5d";
                    break;
                case ".":
                    CipherEncrypt = "5c";
                    break;
                case ":":
                    CipherEncrypt = "5b";
                    break;
                case ";":
                    CipherEncrypt = "40";
                    break;
                case "?":
                    CipherEncrypt = "3f";
                    break;
                case "@":
                    CipherEncrypt = "3b";
                    break;
                case "[":
                    CipherEncrypt = "3a";
                    break;
                case "\\":
                    CipherEncrypt = "2e";
                    break;
                case "]":
                    CipherEncrypt = "2c";
                    break;
                case "^":
                    CipherEncrypt = "2a";
                    break;
                case "_":
                    CipherEncrypt = "29";
                    break;
                case "`":
                    CipherEncrypt = "28";
                    break;
                case "{":
                    CipherEncrypt = "26";
                    break;
                case "|":
                    CipherEncrypt = "25";
                    break;
                case "}":
                    CipherEncrypt = "24";
                    break;
                case "~":
                    CipherEncrypt = "23";
                    break;
                case "+":
                    CipherEncrypt = "22";
                    break;
                case "<":
                    CipherEncrypt = "21";
                    break;
                case "=":
                    CipherEncrypt = "2d";
                    break;
                case ">":
                    CipherEncrypt = "27";
                    break;
            }

            return CipherEncrypt;
        }
    }
}
