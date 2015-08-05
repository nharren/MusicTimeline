using HtmlAgilityPack;
using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Builders;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace NathanHarrenstein.MusicTimeline.Scrapers
{
    public static class KlassikaScraper
    {
        public static void ScrapeComposersPage(DataProvider dataProvider)
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var htmlWeb = new HtmlWeb();
                htmlWeb.PreRequest = delegate (HttpWebRequest webRequest)
                {
                    webRequest.Timeout = 300000;
                    return true;
                };

                var htmlDocument = htmlWeb.Load($"http://www.klassika.info/Komponisten/lindex_{c}.html");

                var column1 = htmlDocument.GetElementbyId("personen_s1v3");
                var column2 = htmlDocument.GetElementbyId("personen_s2v3");

                var column1Anchors = column1.Elements("a");
                var column2Anchors = column2.Elements("a");

                var anchors = column1Anchors.Concat(column2Anchors);

                foreach (var anchor in anchors)
                {
                    var hypertextReference = anchor.Attributes["href"];

                    if (hypertextReference == null)
                    {
                        continue;
                    }

                    var composerNameAndDate = HtmlEntity.DeEntitize(anchor.InnerText);
                    var composerNameAndDateParts = composerNameAndDate.Split(new string[] { " (" }, 2, StringSplitOptions.None);
                    var composerName = composerNameAndDateParts.Length == 2 ? composerNameAndDateParts[0] : composerNameAndDate;
                    var composer = dataProvider.Composers.FirstOrDefault(co => co.Name == composerName);

                    if (composer == null)
                    {
                        composer = new Composer();
                        composer.Name = composerName;

                        dataProvider.Composers.Add(composer);
                    }

                    ScrapeComposerDetailPage($"http://www.klassika.info/{hypertextReference.Value}", composer, dataProvider);
                }
            }

            dataProvider.SaveChanges();
        }

        public static void ScrapeComposerDetailPage(string url, Composer composer, DataProvider dataProvider)
        {
            var klassikaLink = composer.ComposerLinks.FirstOrDefault(l => l.URL.Contains("klassika.info"));

            if (klassikaLink == null)
            {
                klassikaLink = new ComposerLink();
                klassikaLink.Composer = composer;
                klassikaLink.URL = url;

                composer.ComposerLinks.Add(klassikaLink);
            }

            var urlParts = url.Split('/');

            if (urlParts.Length < 2)
            {
                return;
            }

            var composerKey = urlParts.ElementAt(urlParts.Length - 2);

            ScrapeCompositionsPage($"http://www.klassika.info/Komponisten/{composerKey}/wv_abc.html", composer, dataProvider);
        }

        public static void ScrapeCompositionsPage(string url, Composer composer, DataProvider dataProvider)
        {
            var htmlWeb = new HtmlWeb();
            htmlWeb.PreRequest = delegate (HttpWebRequest webRequest)
            {
                webRequest.Timeout = 300000;
                return true;
            };

            var htmlDocument = htmlWeb.Load(url);

            var tables = htmlDocument.DocumentNode.Descendants("table");
            var table = tables.FirstOrDefault(n =>
            {
                var classAttribute = n.Attributes["class"];

                if (classAttribute == null || classAttribute.Value != "wv")
                {
                    return false;
                }

                return true;
            });

            var tableRows = table.Elements("tr");

            foreach (var tableRow in tableRows)
            {
                var tableDatas = tableRow.Elements("td").ToArray();

                var nameTableData = tableDatas[1];
                var compositionName = HtmlEntity.DeEntitize(nameTableData.InnerText);

                var compositionAnchor = nameTableData.Element("a");
                var hypertextReference = (HtmlAttribute)null;

                var composition = (Composition)null;

                if (compositionAnchor != null)
                {
                    hypertextReference = compositionAnchor.Attributes["href"];

                    if (hypertextReference != null)
                    {
                        composition = composer.Compositions.FirstOrDefault(c => c.CompositionLinks.Any(l => l.URL == $"http://www.klassika.info/{hypertextReference.Value}"));
                    }
                }

                var catalogTableData = tableDatas[2];
                var catalog = HtmlEntity.DeEntitize(catalogTableData.InnerText);

                var dateTableData = tableDatas[4];
                var compositionDate = HtmlEntity.DeEntitize(dateTableData.InnerText);

                var compositionTypeTableData = tableDatas[6];
                var compositionTypeName = HtmlEntity.DeEntitize(compositionTypeTableData.InnerText).Replace("(", "").Replace(")", "");

                if (composition == null)
                {
                    composition = new Composition();
                    composition.Name = compositionName;
                    composition.Composers = new ObservableCollection<Composer> { composer };
                    composition.Dates = compositionDate == "?" ? null : compositionDate;

                    composer.Compositions.Add(composition);
                }

                var catalogNumberString = (string)null;
                var compositionCatalogString = (string)null;

                if (!string.IsNullOrWhiteSpace(compositionTypeName))
                {
                    var compositionType = dataProvider.CompositionTypes.FirstOrDefault(ct => ct.Name == compositionTypeName);

                    if (compositionType == null)
                    {
                        compositionType = new CompositionType();
                        compositionType.Name = compositionTypeName;
                        compositionType.Compositions.Add(composition);

                        dataProvider.CompositionTypes.Add(compositionType);              
                    }

                    if (composition.CompositionType == null)
                    {
                        composition.CompositionType = compositionType;
                    }
                }

                if (!string.IsNullOrWhiteSpace(catalog))
                {
                    var catalogParts = catalog.Split(new char[] { ' ' }, 2);

                    if (catalogParts.Length == 2)
                    {
                        compositionCatalogString = catalogParts[0];
                        catalogNumberString = catalogParts[1];

                        var compositionCatalog = dataProvider.CompositionCatalogs.FirstOrDefault(cc => cc.Composer.Name == composer.Name && cc.Prefix == compositionCatalogString);

                        if (compositionCatalog == null)
                        {
                            compositionCatalog = new CompositionCatalog();
                            compositionCatalog.Prefix = compositionCatalogString;
                            compositionCatalog.Composer = composer;

                            composer.CompositionCatalogs.Add(compositionCatalog);
                        }

                        var catalogNumber = compositionCatalog.CatalogNumbers.FirstOrDefault(cn => cn.Number == catalogNumberString);

                        if (catalogNumber == null)
                        {
                            catalogNumber = new CatalogNumber();
                            catalogNumber.CompositionCatalog = compositionCatalog;
                            catalogNumber.Composition = composition;
                            catalogNumber.Number = catalogNumberString;

                            compositionCatalog.CatalogNumbers.Add(catalogNumber);
                        }
                    }
                }

                if (hypertextReference != null)
                {
                    ScrapeCompositionDetailPage($"http://www.klassika.info/{hypertextReference.Value}", composition, dataProvider);
                }

                Debug.WriteLine(composition.Name);
            }
        }

        public static void ScrapeCompositionDetailPage(string url, Composition composition, DataProvider dataProvider)
        {
            var klassikaLink = composition.CompositionLinks.FirstOrDefault(l => l.URL.Contains("klassika.info"));

            if (klassikaLink == null)
            {
                klassikaLink = new CompositionLink();
                klassikaLink.Composition = composition;
                klassikaLink.URL = url;

                composition.CompositionLinks.Add(klassikaLink);
            }

            var htmlWeb = new HtmlWeb();
            htmlWeb.PreRequest = delegate (HttpWebRequest webRequest)
            {
                webRequest.Timeout = 300000;
                return true;
            };

            var htmlDocument = htmlWeb.Load(url);

            var generalInformationHeader = htmlDocument.DocumentNode
                .Descendants("h2")
                .FirstOrDefault(n => n.InnerText == "Allgemeine Angaben zum Werk:");

            if (generalInformationHeader != null)
            {
                var generalInformationTable = generalInformationHeader.NextSibling.NextSibling;

                var tableRows = generalInformationTable.Descendants("tr");

                foreach (var tableRow in tableRows)
                {
                    var tableDatas = tableRow
                        .Elements("td")
                        .ToArray();

                    var headerTableData = tableDatas[0];
                    var valueTableData = tableDatas[1];

                    var header = HtmlEntity.DeEntitize(headerTableData.InnerText);
                    var value = HtmlEntity.DeEntitize(valueTableData.InnerText);

                    if (header == "Tonart:")
                    {
                        var key = dataProvider.Keys.FirstOrDefault(k => k.Name == value);

                        if (key == null)
                        {
                            key = new Key();
                            key.Name = value;
                            key.Compositions.Add(composition);

                            composition.Key = key;

                            dataProvider.Keys.Add(key);
                        }

                        if (composition.Key == null)
                        {
                            composition.Key = key;
                        }

                    }
                    else if (header == "Widmung:")
                    {
                        if (composition.Dedication == null)
                        {
                            composition.Dedication = value;
                        }
                    }
                    else if (header == "Besetzung:")
                    {
                        var instrumentation = dataProvider.Instrumentations.FirstOrDefault(i => i.Name == value);

                        if (instrumentation == null)
                        {
                            instrumentation = new Instrumentation();
                            instrumentation.Name = value;
                            instrumentation.Compositions.Add(composition);

                            composition.Instrumentation = instrumentation;

                            dataProvider.Instrumentations.Add(instrumentation);
                        }

                        if (composition.Instrumentation == null)
                        {
                            composition.Instrumentation = instrumentation;
                        }
                    }
                    else if (header == "Uraufführung:")
                    {
                        if (composition.Premiere == null)
                        {
                            composition.Premiere = value;
                        }
                    }
                    else if (header == "Entstehungszeit:")
                    {
                        if (composition.Dates == null)
                        {
                            composition.Dates = value;
                        }
                    }
                    else if (header == "Anlass:")
                    {
                        if (composition.Occasion == null)
                        {
                            composition.Occasion = value;
                        }
                    }
                    else if (header == "Bemerkung:")
                    {
                        if (composition.Comment == null)
                        {
                            composition.Comment = value;
                        }
                    }
                }
            }

            var movementsHeader = htmlDocument.DocumentNode
                .Descendants("h2")
                .FirstOrDefault(n => n.InnerText == "Sätze:");

            if (movementsHeader != null)
            {
                var movementTable = movementsHeader.NextSibling.NextSibling;

                var tableRows = movementTable.Descendants("tr");

                foreach (var tableRow in tableRows)
                {
                    var movementNumberTableData = tableRow
                        .Elements("td")
                        .ElementAt(0);

                    var movementNameTableData = tableRow
                        .Elements("td")
                        .ElementAt(1);

                    var movementNumber = short.Parse(HtmlEntity.DeEntitize(movementNumberTableData.InnerText).Replace(". Satz:", ""));
                    var movementName = HtmlEntity.DeEntitize(movementNameTableData.InnerText);

                    var movement = composition.Movements.FirstOrDefault(m => m.Number == movementNumber);

                    if (movement == null)
                    {
                        movement = new Movement();
                        movement.Name = movementName;
                        movement.Composition = composition;
                        movement.Number = movementNumber;

                        composition.Movements.Add(movement);
                    } 
                }
            }
        }
    }
}