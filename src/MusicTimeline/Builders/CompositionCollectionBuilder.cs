using NathanHarrenstein.MusicDB;
using System.Collections.ObjectModel;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class CompositionCollectionBuilder
    {
        public static CompositionCollection Build(string compositionCollectionName, ObservableCollection<Composer> composers, DataProvider dataProvider)
        {
            var compositionCollection = new CompositionCollection();
            compositionCollection.ID = GenerateID(dataProvider);
            compositionCollection.Name = compositionCollectionName;
            compositionCollection.Composers = composers;

            return compositionCollection;
        }

        private static short GenerateID(DataProvider dataProvider)
        {
            short newID = 1;

            var usedIds = dataProvider.CompositionCollections
                .Select(entity => entity.ID)
                .Concat(dataProvider.CompositionCollections.Local
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