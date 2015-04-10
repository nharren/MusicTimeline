using System;

namespace NathanHarrenstein.Converters
{
    public static class NameConverter
    {
        public static string ToLastFirst(string name)
        {
            var delimiters = new string[] { " " };

            var nameParts = name.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length > 2)
            {
                throw new InvalidOperationException("The name \"" + name + "\" had more than one delimiter and cannot be properly converted to last name first name format.");
            }

            if (nameParts.Length > 1)
            {
                return nameParts[1] + ", " + nameParts[0];
            }
            else
            {
                return nameParts[0];
            }
        }

        public static string ToFirstLast(string name)
        {
            var delimiters = new string[] { ", " };

            var nameParts = name.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length > 2)
            {
                throw new InvalidOperationException("The name \"" + name + "\" had more than one delimiter and cannot be properly converted to first name last name format.");
            }

            if (nameParts.Length > 1)
            {
                return nameParts[1] + " " + nameParts[0];
            }
            else
            {
                return nameParts[0];
            }
        }
    }
}