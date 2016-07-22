using NathanHarrenstein.MusicTimeline.Data;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class LocationUtility
    {
        public static bool IsOrphaned(Location location)
        {
            return location.BirthLocationComposers.Count + location.DeathLocationComposers.Count == 0;
        }

        public static void RemoveBirthLocation(Location birthLocation, Composer composer, ClassicalMusicEntities classicalMusicContext)
        {
            composer.BirthLocation = null;

            birthLocation.BirthLocationComposers.Remove(composer);

            if (IsOrphaned(birthLocation))
            {
                classicalMusicContext.DeleteObject(composer.BirthLocation);
            }
        }

        public static void UpdateBirthLocation(string birthLocationName, Composer composer, ClassicalMusicEntities classicalMusicContext)
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

        public static void RemoveDeathLocation(Location deathLocation, Composer composer, ClassicalMusicEntities classicalMusicContext)
        {
            composer.DeathLocation = null;

            deathLocation.DeathLocationComposers.Remove(composer);

            if (IsOrphaned(deathLocation))
            {
                classicalMusicContext.DeleteObject(composer.DeathLocation);
            }
        }

        public static void UpdateDeathLocation(string deathLocationName, Composer composer, ClassicalMusicEntities classicalMusicContext)
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

        public static Location CreateLocation(string locationName, ClassicalMusicEntities classicalMusicContext)
        {
            var location = new Location();
            location.Name = locationName;

            classicalMusicContext.AddToLocations(location);

            return location;
        }
    }
}