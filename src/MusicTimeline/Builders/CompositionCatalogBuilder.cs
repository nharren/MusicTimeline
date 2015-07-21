using NathanHarrenstein.MusicDB;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class CompositionCatalogBuilder
    {
        public static CompositionCatalog Build(string prefix, Composer composer, DataProvider dataProvider)
        {
            var compositionCatalog = new CompositionCatalog();
            compositionCatalog.ID = GenerateID(dataProvider);
            compositionCatalog.Prefix = prefix;
            compositionCatalog.Composer = composer;

            return compositionCatalog;
        }

        private static short GenerateID(DataProvider dataProvider)
        {
            short newID = 1;

            var usedIds = dataProvider.CompositionCatalogs
                .Select(entity => entity.ID)
                .Concat(dataProvider.CompositionCatalogs.Local
                    .Select(entity => entity.ID))
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