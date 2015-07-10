using System.Collections;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class ListUtility
    {
        public static void AddMany(IList list, IEnumerable itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                list.Add(item);
            }
        }

        public static void RemoveMany(IList list, IEnumerable itemsToRemove)
        {
            foreach (var item in itemsToRemove)
            {
                list.Remove(item);
            }
        }
    }
}