using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Controls
{
    public class NumberBox : TextBox
    {
        static NumberBox()
        {
            TextProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(string.Empty, null, new CoerceValueCallback(CoerceText)));
        }

        private static object CoerceText(DependencyObject d, object basevalue)
        {
            var numberBox = (NumberBox)d;
            var inputString = basevalue as string;
            int parsedInteger;

            if (string.IsNullOrEmpty(inputString))
            {
                return inputString;
            }

            if (int.TryParse(inputString, out parsedInteger))
            {
                return inputString;
            }
            else
            {
                return numberBox.Text;
            }
        }
    }
}