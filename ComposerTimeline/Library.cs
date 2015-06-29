using System.IO;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class Library
    {
        public const string LibraryPath = "library.txt";

        public static void AddFilePath(string filePath)
        {
            using (var fileStream = new FileStream(LibraryPath, FileMode.OpenOrCreate))
            {
                if (!FileUtility.HasLine(fileStream, filePath))
                {
                    FileUtility.WriteLine(fileStream, filePath);
                }
            }
        }

        public static string[] GetFilePaths()
        {
            return File.ReadAllLines(LibraryPath);
        }

        public static bool HasFilePath(string filePath)
        {
            using (var fileStream = new FileStream(LibraryPath, FileMode.OpenOrCreate))
            {
                return FileUtility.HasLine(fileStream, filePath);
            }
        }
    }
}