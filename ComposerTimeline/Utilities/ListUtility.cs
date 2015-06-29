﻿using System.Collections;
using System.Collections.Generic;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class ListUtility
    {
        public static void AddMany<T>(IList list, IEnumerable<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                list.Add(item);
            }
        }
    }
}