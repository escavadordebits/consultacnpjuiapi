using System.Globalization;
using System.Text.RegularExpressions;

namespace SapConsultaCep
{
    public static class StringExtensions
    {
        public static string Left(this string source, int characters)
        {
            return string.IsNullOrEmpty(source) || source.Length < characters ? source : source.Substring(0, characters);
        }

        public static string Right(this string source, int characters)
        {
            return string.IsNullOrEmpty(source) || source.Length < characters ? source : source.Substring(source.Length - characters, characters);
        }

        public static string RemoveLeftZeros(this string source)
        {
            return string.IsNullOrEmpty(source) ? source : source.TrimStart('0');
        }

        public static string RemoveWhiteSpaces(this string source)
        {
            return string.IsNullOrEmpty(source) ? source : Regex.Replace(source, @"\s+", string.Empty);
        }

        public static string OnlyNumbers(this string source)
        {
            return string.IsNullOrEmpty(source) ? source : Regex.Replace(source, "[^0-9]", string.Empty);
        }

        public static string OnlyAlphanumerics(this string source)
        {
            return string.IsNullOrEmpty(source) ? source : Regex.Replace(source, "[^0-9a-zA-Z]", string.Empty);
        }

        public static string ToTitleCase(this string source)
        {
            return string.IsNullOrEmpty(source) ? source : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(source);
        }

        public static string ToCamelCase(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            var titleCase = source.ToTitleCase();
            return $"{char.ToLowerInvariant(titleCase[0])}{titleCase.Substring(1)}".Replace("_", string.Empty);
        }

        public static string FormatCPF(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            if (source.Length != 11) return source;

            return source.Substring(0, 3) + "." + source.Substring(3, 3) + "." + source.Substring(6, 3) + "-" +
                   source.Substring(9, 2);
        }

        public static string FormatCNPJ(this string source)
        {
            if (string.IsNullOrEmpty(source)) return null;

            var result = source.Trim();

            if (result.Length == 8)
            {
                result = result.PadRight(14, 'x');
            }
            else if (result.Length < 14)
            {
                for (var i = 0; i < 14 - source.Length; i++)
                    result = "0" + result;
            }

            return result.Substring(0, 2) + "." + result.Substring(2, 3) + "." + result.Substring(5, 3) + "/" +
                   result.Substring(8, 4) + "-" + result.Substring(12, 2);
        }

        public static string FormatCPFCNPJ(this string source)
        {
            if (string.IsNullOrEmpty(source)) return null;

            return source.Trim().Length > 11 ? source.FormatCNPJ() : source.FormatCPF();
        }

        public static string FormatCEP(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            if (source.Length == 8)
            {
                return source.Substring(0, 5) + '-' + source.Substring(5, 3);
            }
            return source;
        }

        public static string Truncate(this string source, int maxLength)
        {
            if (!string.IsNullOrEmpty(source) && source.Length > maxLength)
                return source.Substring(0, maxLength);
            return source;
        }
    }
}
