using System.Windows;

namespace NathanHarrenstein.Timeline
{
    public interface IPan
    {
        Vector CoercePan(Vector delta);

        void Pan(Vector delta);
    }
}