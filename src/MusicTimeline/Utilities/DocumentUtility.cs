using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class DocumentUtility
    {
        /// <summary>
        /// Adds one flowdocument to another.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public static void AddDocument(FlowDocument from, FlowDocument to)
        {
            TextRange range = new TextRange(from.ContentStart, from.ContentEnd);

            MemoryStream stream = new MemoryStream();

            System.Windows.Markup.XamlWriter.Save(range, stream);

            range.Save(stream, DataFormats.XamlPackage);

            TextRange range2 = new TextRange(to.ContentEnd, to.ContentEnd);

            range2.Load(stream, DataFormats.XamlPackage);
        }

        /// <summary>
        /// Adds a block to a flowdocument.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public static void AddBlock(Block from, FlowDocument to)
        {
            if (from != null)
            {
                TextRange range = new TextRange(from.ContentStart, from.ContentEnd);

                MemoryStream stream = new MemoryStream();

                System.Windows.Markup.XamlWriter.Save(range, stream);

                range.Save(stream, DataFormats.XamlPackage);

                TextRange textRange2 = new TextRange(to.ContentEnd, to.ContentEnd);

                textRange2.Load(stream, DataFormats.XamlPackage);
            }
        }


    }
}
