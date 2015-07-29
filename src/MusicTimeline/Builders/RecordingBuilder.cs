using NathanHarrenstein.MusicDB;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class RecordingBuilder
    {
        public static Recording Build(DataProvider dataProvider)
        {
            var recording = new Recording();
            recording.ID = GenerateID(dataProvider);

            return recording;
        }

        private static int GenerateID(DataProvider dataProvider)
        {
            int newID = 1;

            var usedIds = dataProvider.Recordings
                .AsEnumerable()
                .Concat(dataProvider.Recordings.Local)
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