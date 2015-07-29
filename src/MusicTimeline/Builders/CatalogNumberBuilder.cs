using NathanHarrenstein.MusicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.MusicTimeline.Builders
{
    public static class CatalogNumberBuilder
    {
        public static CatalogNumber Build(CompositionCatalog compositionCatalog, CompositionCollection compositionCollection, Composition composition, DataProvider dataProvider)
        {
            var catalogNumber = new CatalogNumber();
            catalogNumber.ID = GenerateID(dataProvider);
            catalogNumber.CompositionCatalog = compositionCatalog;
            catalogNumber.Composition = composition;
            catalogNumber.CompositionCollection = compositionCollection;

            return catalogNumber;
        }

        private static short GenerateID(DataProvider dataProvider)
        {
            short newID = 1;

            var usedIds = dataProvider.CatalogNumbers
                .AsEnumerable()
                .Concat(dataProvider.CatalogNumbers.Local)
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
