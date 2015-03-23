using System;

namespace NathanHarrenstein.ExtendedDateTimeFormat.Converters
{
    public class ConversionException : Exception
    {
        public ConversionException(string message)
            : base(message)
        {
        }
    }
}