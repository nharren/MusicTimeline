using NathanHarrenstein.MusicDB;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class AlbumBuilder
    {
        public static Album Build(string albumName, Recording recording, DataProvider dataProvider)
        {
            var album = new Album();
            album.ID = GenerateID(dataProvider);
            album.Name = albumName;
            album.Recordings.Add(recording);

            return album;
        }

        private static short GenerateID(DataProvider dataProvider)
        {
            short newID = 1;

            var usedIds = dataProvider.Albums
                .AsEnumerable()
                .Concat(dataProvider.Albums.Local)
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