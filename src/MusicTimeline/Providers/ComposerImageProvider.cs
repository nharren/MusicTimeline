using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Utilities;
using System.IO;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Providers
{
    public static class ComposerImageProvider
    {
        public static ComposerImage GetComposerImage(Composer composer, string imagePath, DataProvider dataProvider)
        {
            var imageExtension = Path.GetExtension(imagePath);

            if (imageExtension != ".jpg"
                && imageExtension != ".png"
                && imageExtension != ".gif"
                && imageExtension != ".jpeg")
            {
                return null;
            }

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
            short id = 1;

            var composerImageIds = dataProvider.ComposerImages
                .Select(ci => ci.ID)
                .Concat(dataProvider.ComposerImages.Local
                    .Select(lci => lci.ID))
                .OrderBy(i => i)
                .ToArray();

            foreach (var cid in composerImageIds)
            {
                if (cid == id || id == composerImageIds.Length)
                {
                    id++;
                }
                else
                {
                    break;
                }
            }

            return id;
        }
    }
}