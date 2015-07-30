using System.Windows.Controls;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class NonBlockingFlowDocumentScrollViewer : FlowDocumentScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (VerticalScrollBarVisibility != ScrollBarVisibility.Disabled)
            {
                base.OnMouseWheel(e);
            }
        }
    }
}