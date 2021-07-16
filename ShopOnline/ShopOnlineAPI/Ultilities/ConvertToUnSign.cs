using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Ultilities
{
    public static class ConvertToUnSign
    {
        public static string Convert(string input)
        {
            input = input.Trim();

            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);

            string output = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');


            return output;
        }
    }
}
