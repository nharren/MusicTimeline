using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class TreeViewItem : Control
    {
        public static readonly DependencyProperty CanExpandProperty = DependencyProperty.Register("CanExpand", typeof(bool), typeof(TreeViewItem));
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children", typeof(IEnumerable<TreeViewItem>), typeof(TreeViewItem));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(TreeViewItem));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(TreeViewItem));
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool?), typeof(TreeViewItem), new PropertyMetadata(IsExpandedChanged));

        static TreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(typeof(TreeViewItem)));
        }

        public bool CanExpand
        {
            get { return (bool)GetValue(CanExpandProperty); }
            set { SetValue(CanExpandProperty, value); }
        }

        public IEnumerable<TreeViewItem> Children
        {
            get { return (IEnumerable<TreeViewItem>)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public bool? IsExpanded
        {
            get { return (bool?)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        new public TreeViewItem Parent { get; set; }

        protected virtual void LoadChildren()
        {
        }

        protected virtual void UnloadChildren()
        {
        }

        private static void IsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeViewItem = (TreeViewItem)d;
            var isExpanded = (bool?)e.NewValue;

            if (e.NewValue == e.OldValue)
            {
                return;
            }

            if (isExpanded == true)
            {
                treeViewItem.LoadChildren();
            }
            else
            {
                treeViewItem.UnloadChildren();
            }
        }
    }
}