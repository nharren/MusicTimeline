using System;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Models
{
    public class Link
    {
        private readonly string _faviconUrl;
        private readonly string _title;
        private readonly ICommand _clickCommand;
        private readonly string _url;

        public Link(string url, string title, string faviconUrl, ICommand clickCommand)
        {
            _url = url;
            _title = title;
            _faviconUrl = faviconUrl;
            _clickCommand = clickCommand;
        }

        public string FaviconUrl
        {
            get
            {
                return _faviconUrl;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
        }
    }
}