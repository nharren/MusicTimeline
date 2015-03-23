using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Controls
{
    // See: "file:C:\Users\Nathan\SkyDrive\Programming\ComposerTimeline\Framework\Diagrams\TreeView Diagram.png"
    public class TreeView : Control
    {
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children", typeof(IEnumerable<TreeViewItem>), typeof(TreeView));

        static TreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(typeof(TreeView)));
        }

        public IEnumerable<TreeViewItem> Children
        {
            get
            {
                return (IEnumerable<TreeViewItem>)GetValue(ChildrenProperty);
            }
            set
            {
                SetValue(ChildrenProperty, value);
            }
        }
    }
}