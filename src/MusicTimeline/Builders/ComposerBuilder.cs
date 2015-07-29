using NathanHarrenstein.MusicDB;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class ComposerBuilder
    {
        public static Composer Build(string composerName, DataProvider dataProvider)
        {
            var composer = new Composer();
            composer.ID = GenerateID(dataProvider);
            composer.Name = composerName;

            return composer;
        }

        private static short GenerateID(DataProvider dataProvider)
        {
            short newID = 1;

            var usedIds = dataProvider.Composers
                .AsEnumerable()
                .Concat(dataProvider.Composers.Local)
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