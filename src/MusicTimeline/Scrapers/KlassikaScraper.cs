﻿using HtmlAgilityPack;
using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Builders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NathanHarrenstein.MusicTimeline.Scrapers
{
    public static class KlassikaScraper
    {
        public static void ScrapeComposersPage(ClassicalMusicDbContext classicalMusicDbContext)
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
                    var composer = classicalMusicDbContext.Composers.FirstOrDefault(co => co.Name == composerName);

                    if (composer == null)
                    {
                        composer = new Composer();
                        composer.Name = composerName;

                        classicalMusicDbContext.Composers.Add(composer);
                    }

                    ScrapeComposerDetailPage($"http://www.klassika.info/{hypertextReference.Value}", composer, classicalMusicDbContext);
                }
            }

            classicalMusicDbContext.SaveChanges();
        }

        public static void ScrapeComposerDetailPage(string url, Composer composer, ClassicalMusicDbContext classicalMusicDbContext, IProgress<double> progress = null, CancellationToken? cancellationToken = null)
        {
            var composerLink = composer.Links.FirstOrDefault(l => l.Url.Contains("klassika.info"));

            if (composerLink == null)
            {
                composerLink = new Link();
                composerLink.Composers.Add(composer);
                composerLink.Url = url;

                composer.Links.Add(composerLink);
            }

            var urlParts = url.Split('/');

            if (urlParts.Length < 2)
            {
                return;
            }

            var composerKey = urlParts.ElementAt(urlParts.Length - 2);

            ScrapeCompositionsPage($"http://www.klassika.info/Komponisten/{composerKey}/wv_abc.html", composer, classicalMusicDbContext, progress, cancellationToken);
        }

        public static void ScrapeCompositionsPage(string url, Composer composer, ClassicalMusicDbContext classicalMusicDbContext, IProgress<double> progress = null, CancellationToken? cancellationToken = null)
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

            if (table == null)
            {
                return;
            }

            var tableRows = table.Elements("tr").ToArray();
            var increment = 1d;

            foreach (var tableRow in tableRows)
            {
                if (cancellationToken != null && cancellationToken.Value.IsCancellationRequested)
                {
                    return;
                }

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
                        composition = composer.Compositions.FirstOrDefault(c => c.Links.Any(l => l.Url == $"http://www.klassika.info/{hypertextReference.Value}"));
                    }
                }

                var catalogTableData = tableDatas[2];
                var catalogString = HtmlEntity.DeEntitize(catalogTableData.InnerText);

                var dateTableData = tableDatas[4];
                var compositionDate = HtmlEntity.DeEntitize(dateTableData.InnerText);

                var compositionTypeTableData = tableDatas[6];
                var compositionTypeName = HtmlEntity.DeEntitize(compositionTypeTableData.InnerText).Replace("(", "").Replace(")", "");

                if (composition == null)
                {
                    composition = new Composition();
                    composition.Name = compositionName;
                    composition.Composers = new List<Composer> { composer };
                    composition.Dates = compositionDate == "?" ? null : compositionDate;

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => composer.Compositions.Add(composition)));
                }

                var catalogNumberString = (string)null;
                var compositionCatalogString = (string)null;

                if (!string.IsNullOrWhiteSpace(compositionTypeName))
                {
                    var genre = classicalMusicDbContext.Genres.FirstOrDefault(ct => ct.Name == compositionTypeName);

                    if (genre == null)
                    {
                        genre = new Genre();
                        genre.Name = compositionTypeName;
                        genre.Compositions.Add(composition);

                        classicalMusicDbContext.Genres.Add(genre);
                    }

                    if (composition.Genre == null)
                    {
                        composition.Genre = genre;
                    }
                }

                if (!string.IsNullOrWhiteSpace(catalogString))
                {
                    var catalogParts = catalogString.Split(new char[] { ' ' }, 2);

                    if (catalogParts.Length == 2)
                    {
                        compositionCatalogString = catalogParts[0];
                        catalogNumberString = catalogParts[1];

                        var catalog = composer.Catalogs.FirstOrDefault(cc => cc.Prefix == compositionCatalogString);

                        if (catalog == null)
                        {
                            catalog = new Catalog();
                            catalog.Prefix = compositionCatalogString;
                            catalog.Composer = composer;

                            composer.Catalogs.Add(catalog);
                        }

                        var catalogNumber = catalog.CatalogNumbers.FirstOrDefault(cn => cn.Value == catalogNumberString);

                        if (catalogNumber == null)
                        {
                            catalogNumber = new CatalogNumber();
                            catalogNumber.Catalog = catalog;
                            catalogNumber.Compositions.Add(composition);
                            catalogNumber.Value = catalogNumberString;

                            catalog.CatalogNumbers.Add(catalogNumber);
                        }
                    }
                }

                if (hypertextReference != null)
                {
                    ScrapeCompositionDetailPage($"http://www.klassika.info/{hypertextReference.Value}", composition, classicalMusicDbContext);
                }

                if (progress != null)
                {
                    progress.Report(increment++ / (double)tableRows.Length);
                }

#if DEBUG
                Debug.WriteLine(composition.Name);
#endif
            }
        }

        public static void ScrapeCompositionDetailPage(string url, Composition composition, ClassicalMusicDbContext classicalMusicDbContext)
        {
            var compositionWebpage = composition.Links.FirstOrDefault(l => l.Url.Contains("klassika.info"));

            if (compositionWebpage == null)
            {
                compositionWebpage = new Link();
                compositionWebpage.Compositions.Add(composition);
                compositionWebpage.Url = url;

                composition.Links.Add(compositionWebpage);
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
                        var key = classicalMusicDbContext.Keys.FirstOrDefault(k => k.Name == value);

                        if (key == null)
                        {
                            key = new Key();
                            key.Name = value;
                            key.Compositions.Add(composition);

                            composition.Key = key;

                            classicalMusicDbContext.Keys.Add(key);
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
                        var instrumentation = classicalMusicDbContext.Instrumentations.FirstOrDefault(i => i.Name == value);

                        if (instrumentation == null)
                        {
                            instrumentation = new Instrumentation();
                            instrumentation.Name = value;
                            instrumentation.Compositions.Add(composition);

                            composition.Instrumentation = instrumentation;

                            classicalMusicDbContext.Instrumentations.Add(instrumentation);
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

                    short movementNumber;
                    
                    if (!short.TryParse(HtmlEntity.DeEntitize(movementNumberTableData.InnerText).Replace(". Satz:", ""), out movementNumber))
                    {
                        var tableRowsArray = tableRows.ToArray();
                        var tableRowIndex = Array.IndexOf(tableRowsArray, tableRow);

                        movementNumber = Convert.ToInt16(tableRowIndex + 1);
                    }

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