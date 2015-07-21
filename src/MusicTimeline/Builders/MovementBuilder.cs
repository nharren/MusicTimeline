using NathanHarrenstein.MusicDB;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public class MovementBuilder
    {
        public static Movement Build(string movementName, Composition composition, DataProvider dataProvider)
        {
            var movement = new Movement();
            movement.ID = GenerateID(dataProvider);
            movement.Name = movementName;
            movement.Composition = composition;

            return movement;
        }

        private static int GenerateID(DataProvider dataProvider)
        {
            int newID = 1;

            var usedIds = dataProvider.Movements
                .Select(entity => entity.ID)
                .Concat(dataProvider.Movements.Local
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