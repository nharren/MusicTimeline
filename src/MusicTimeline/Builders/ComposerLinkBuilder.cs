using NathanHarrenstein.MusicDB;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class ComposerLinkBuilder
    {
        public static ComposerLink Build(string url, Composer composer, DataProvider dataProvider)
        {
            var composerLink = new ComposerLink();
            composerLink.ID = GenerateID(dataProvider);
            composerLink.Composer = composer;
            composerLink.URL = url;

            return composerLink;
        }

        private static short GenerateID(DataProvider dataProvider)
        {
            short newID = 1;

            var usedIds = dataProvider.ComposerLinks
                .AsEnumerable()
                .Concat(dataProvider.ComposerLinks.Local)
                .Select(entity => entity.ID)
                .Distinct()
                .OrderBy(id => id);

            foreach (var usedID in usedIds)
            {
                if (newID == usedID)
                {
                    newID++;
                }
                else
                {
                    break;
                }
            }

            return newID;
        }
    }
}