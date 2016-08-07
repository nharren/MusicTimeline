using System.Windows;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class BulletStyle
    {
        public BulletStyle(TextMarkerStyle textMarkerStyle, string displayName, char representativeSymbol, FontFamily fontFamily)
        {
            TextMarkerStyle = textMarkerStyle;
            DisplayName = displayName;
            RepresentativeSymbol = representativeSymbol;
            FontFamily = fontFamily;
        }

        public string DisplayName { get; private set; }
        public char RepresentativeSymbol { get; private set; }
        public TextMarkerStyle TextMarkerStyle { get; private set; }
        public FontFamily FontFamily { get; private set; }
    }
}