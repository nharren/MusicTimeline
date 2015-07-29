using NathanHarrenstein.MusicDB;
using System.Collections.ObjectModel;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class CompositionBuilder
    {
        public static Composition Build(string compositionName, ObservableCollection<Composer> composers, DataProvider dataProvider)
        {
            var composition = new Composition();
            composition.ID = GenerateID(dataProvider);
            composition.Name = compositionName;

            return composition;
        }

        private static int GenerateID(DataProvider dataProvider)
        {
            int newID = 1;

            var usedIds = dataProvider.Compositions
                .AsEnumerable()
                .Concat(dataProvider.Compositions.Local)
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