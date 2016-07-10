using CefSharp;
using CefSharp.Wpf;
using NathanHarrenstein.MusicTimeline.Views;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class ChromiumWebBrowserDisplayHandler : IDisplayHandler
    {
        private ComposerPage composerPage;
        private Dictionary<ChromiumWebBrowser, LayoutInfo> layoutInfoDictionary = new Dictionary<ChromiumWebBrowser, LayoutInfo>();

        public ChromiumWebBrowserDisplayHandler(ComposerPage composerPage)
        {
            this.composerPage = composerPage;
        }

        public void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs)
        {
        }

        public bool OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs)
        {
            return true;
        }

        public void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls)
        {
        }

        public void OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen)
        {
            if (fullscreen)
            {
                var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

                try
                {
                    Action enterFullScreenAction = () =>
                    {
                        typeof(ChromiumWebBrowser)
                            .GetField("ignoreUriChange", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance)
                            .SetValue(chromiumWebBrowser, true);

                        var panel = (Panel)chromiumWebBrowser.Parent;
                        var layoutInfo = new LayoutInfo(panel, panel.Children.IndexOf(chromiumWebBrowser), chromiumWebBrowser.Height);

                        layoutInfo.Panel.Children.Remove(chromiumWebBrowser);

                        chromiumWebBrowser.Height = double.NaN;

                        var rootGrid = (Grid)composerPage.Content;

                        Grid.SetRowSpan(chromiumWebBrowser, 3);

                        rootGrid.Children.Add(chromiumWebBrowser);

                        layoutInfoDictionary[chromiumWebBrowser] = layoutInfo;
                    };

                    App.Current.Dispatcher.Invoke(enterFullScreenAction);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

                Action exitFullScreenAction = () =>
                {
                    var rootGrid = (Grid)composerPage.Content;

                    rootGrid.Children.Remove(chromiumWebBrowser);

                    Grid.SetRowSpan(chromiumWebBrowser, 1);

                    LayoutInfo layoutInfo;

                    if (!layoutInfoDictionary.TryGetValue(chromiumWebBrowser, out layoutInfo))
                    {
                        return;
                    }

                    chromiumWebBrowser.Height = layoutInfo.Height;

                    layoutInfo.Panel.Children.Insert(layoutInfo.Index, chromiumWebBrowser);

                    typeof(ChromiumWebBrowser)
                        .GetField("ignoreUriChange", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance)
                        .SetValue(chromiumWebBrowser, false);
                };

                App.Current.Dispatcher.Invoke(exitFullScreenAction);
            }
        }

        public void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs)
        {
        }

        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs)
        {
        }

        public bool OnTooltipChanged(IWebBrowser browserControl, string text)
        {
            return true;
        }

        private struct LayoutInfo
        {
            public LayoutInfo(Panel panel, int index, double height)
            {
                Panel = panel;
                Index = index;
                Height = height;
            }

            public double Height { get; private set; }
            public int Index { get; private set; }
            public Panel Panel { get; set; }
        }
    }
}