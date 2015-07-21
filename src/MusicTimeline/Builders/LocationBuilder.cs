using NathanHarrenstein.MusicDB;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class LocationBuilder
    {
        public static Location Build(string locationName, DataProvider dataProvider)
        {
            var location = new Location();
            location.ID = GenerateID(dataProvider);
            location.Name = locationName;

            return location;
        }

        private static int GenerateID(DataProvider dataProvider)
        {
            int newID = 1;

            var usedIds = dataProvider.Locations
                .Select(entity => entity.ID)
                .Concat(dataProvider.Locations.Local
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