using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class LocationUtility
    {
        public static bool IsOrphaned(Location location)
        {
            return location.BirthLocationComposerCollection.Count + location.DeathLocationComposerCollection.Count + location.Recordings.Count == 0;
        }

        public static void RemoveBirthLocation(Location birthLocation, Composer composer, ClassicalMusicContext classicalMusicContext)
        {
            composer.BirthLocation = null;

            birthLocation.BirthLocationComposerCollection.Remove(composer);

            if (IsOrphaned(birthLocation))
            {
                classicalMusicContext.DeleteObject(composer.BirthLocation);
            }
        }

        public static void UpdateBirthLocation(string birthLocationName, Composer composer, ClassicalMusicContext classicalMusicContext)
        {
            if (composer.BirthLocation.Name == birthLocationName)
            {
                return;
            }

            RemoveBirthLocation(composer.BirthLocation, composer, classicalMusicContext);

            composer.BirthLocation = classicalMusicContext.Locations.FirstOrDefault(location => location.Name == birthLocationName);

            if (composer.BirthLocation == null)
            {
                composer.BirthLocation = CreateLocation(birthLocationName, classicalMusicContext);
            }
        }

        public static void RemoveDeathLocation(Location deathLocation, Composer composer, ClassicalMusicContext classicalMusicContext)
        {
            composer.DeathLocation = null;

            deathLocation.DeathLocationComposerCollection.Remove(composer);

            if (IsOrphaned(deathLocation))
            {
                classicalMusicContext.DeleteObject(composer.DeathLocation);
            }
        }

        public static void UpdateDeathLocation(string deathLocationName, Composer composer, ClassicalMusicContext classicalMusicContext)
        {
            if (composer.DeathLocation.Name == deathLocationName)
            {
                return;
            }

            RemoveDeathLocation(composer.DeathLocation, composer, classicalMusicContext);

            composer.DeathLocation = classicalMusicContext.Locations.FirstOrDefault(location => location.Name == deathLocationName);

            if (composer.DeathLocation == null)
            {
                composer.DeathLocation = CreateLocation(deathLocationName, classicalMusicContext);
            }
        }

        public static Location CreateLocation(string locationName, ClassicalMusicContext classicalMusicContext)
        {
            var location = new Location();
            location.Name = locationName;

            classicalMusicContext.AddToLocations(location);

            return location;
        }
    }
}