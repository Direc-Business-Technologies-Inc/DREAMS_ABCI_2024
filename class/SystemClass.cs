using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABROWN_DREAMS
{
    public class SystemClass
    {
        public static double TextIsZero(string txt)
        {
            double dRet;
            if (double.TryParse(txt, out dRet) == false)
            { txt = "0"; }
            return double.Parse(txt);
        }

        public static string ToDecimal(string oValue)
        { return TextIsZero(oValue).ToString("###0.00"); }
        public static string ToCurrency(string oValue)
        { return TextIsZero(oValue).ToString("#,##0.00"); }
        public static string NoDecimal(string oValue)
        { return TextIsZero(oValue).ToString("#,##0.##"); }
        public static string ToNumeric(string oValue)
        { return TextIsZero(oValue).ToString("###0.00"); }

        public static double GetDiscountAmount(string Amount,string Percentage)
        {
            try
            {
                return double.Parse(Amount) * (double.Parse(Percentage) / 100);
            }
            catch
            {
                return 0;
            }
        }
    }
}