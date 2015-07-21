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
                .Select(entity => entity.ID)
                .Concat(dataProvider.Recordings.Local
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