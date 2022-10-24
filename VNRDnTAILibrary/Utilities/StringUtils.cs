using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace VNRDnTAILibrary.Utilities
{
    public static class StringUtils
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

        public static string GenerateRandom(int length)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string normaliseVietnamese(string str)
        {
            str = str.Trim().ToLower();
            str = Regex.Replace(str, "[à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ]", "a");
            str = Regex.Replace(str, "[è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ]", "e");
            str = Regex.Replace(str, "[ì|í|ị|ỉ|ĩ]", "i");
            str = Regex.Replace(str, "[ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ]", "o");
            str = Regex.Replace(str, "[ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ]", "u");
            str = Regex.Replace(str, "[ỳ|ý|ỵ|ỷ|ỹ]", "y");
            str = Regex.Replace(str, "[đ]", "d");
            return str;
        }
    }
}
