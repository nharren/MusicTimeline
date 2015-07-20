using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class DecimalToRomanConverter : IValueConverter
    {
        private static Dictionary<int, string> DecimalRoman = new Dictionary<int, string>()
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" },
        };

        private static Dictionary<char, int> RomanDecimal = new Dictionary<char, int>()
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DecimalToRoman((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return RomanToDecimal((string)value);
        }

        public static  string DecimalToRoman(int number)
        {
            var roman = new StringBuilder();

            foreach (var map in DecimalRoman)
            {
                while (number - map.Key >= 0)
                {
                    roman.Append(map.Value);
                    number -= map.Key;
                }
            }

            return roman.ToString();
        }

        public static int RomanToDecimal(string roman)
        {
            int number = 0;

            for (int i = 0; i < roman.Length; i++)
            {
                if (i + 1 < roman.Length && RomanDecimal[roman[i]] < RomanDecimal[roman[i + 1]])
                {
                    number -= RomanDecimal[roman[i]];
                }
                else
                {
                    number += RomanDecimal[roman[i]];
                }
            }

            return number;
        }
    }
}