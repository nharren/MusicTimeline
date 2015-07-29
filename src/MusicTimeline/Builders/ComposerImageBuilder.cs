using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Utilities;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class ComposerImageBuilder
    {
        public static ComposerImage Build(Composer composer, string imagePath, DataProvider dataProvider)
        {
            var imageBytes = FileUtility.GetImage(imagePath);

            if (imageBytes == null)
            {
                return null;
            }

            var composerImage = new ComposerImage();
            composerImage.ID = GenerateID(dataProvider);
            composerImage.Composer = composer;
            composerImage.Image = imageBytes;

            return composerImage;
        }

        private static short GenerateID(DataProvider dataProvider)
        {
            short newID = 1;

            var usedIds = dataProvider.ComposerImages
                .AsEnumerable()
                .Concat(dataProvider.ComposerImages.Local)
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