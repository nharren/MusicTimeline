using System.Windows;

namespace NathanHarrenstein.Timeline
{
    public interface IScroll
    {
        Vector ReviseScrollingDisplacement(Vector displacement);

        void Scroll(Vector displacement);
    }
}