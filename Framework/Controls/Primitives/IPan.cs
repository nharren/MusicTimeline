using System.Windows;

namespace NathanHarrenstein.Controls
{
    public interface IPan
    {
        Vector CoercePan(Vector delta);

        void Pan(Vector delta);
    }
}