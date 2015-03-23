using System.Windows;

namespace NathanHarrenstein.Controls
{
    public interface IPan
    {
        Vector CanPan(Vector delta);

        void Pan(Vector delta);
    }
}