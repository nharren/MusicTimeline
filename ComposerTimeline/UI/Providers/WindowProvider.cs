using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace NathanHarrenstein.ComposerTimeline
{
    public class WindowProvider
    {
        public Window ProvideWindow()
        {
            var window = new NavigationWindow();
            var pageProvider = new PageProvider();

            window.ShowsNavigationUI = false;
            window.Content = pageProvider.GetTimelinePage();

            return window;
        }
    }
}
