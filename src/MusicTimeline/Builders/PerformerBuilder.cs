using NathanHarrenstein.MusicDB;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class PerformerBuilder
    {
        public static Performer Build(string performerName, DataProvider dataProvider)
        {
            var performer = new Performer();
            performer.ID = GenerateID(dataProvider);
            performer.Name = performerName;

            return performer;
        }

        private static int GenerateID(DataProvider dataProvider)
        {
            int newID = 1;

            var usedIds = dataProvider.Performers
                .Select(entity => entity.ID)
                .Concat(dataProvider.Performers.Local
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