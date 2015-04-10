using System.Windows;

namespace NathanHarrenstein.Controls
{
    public interface IPan
    {
        Vector ValidatePanVector(Vector delta);

        void Pan(Vector delta);
    }
}