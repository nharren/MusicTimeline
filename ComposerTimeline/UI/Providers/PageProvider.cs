using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NathanHarrenstein.ComposerTimeline
{
    public class PageProvider
    {
        public Page GetTimelinePage()
        {
            var page = new Page();
            var composerTimelineProvider = new ComposerTimelineProvider();

            page.Content = composerTimelineProvider.GetComposerTimeline();

            return page;
        }
    }
}
