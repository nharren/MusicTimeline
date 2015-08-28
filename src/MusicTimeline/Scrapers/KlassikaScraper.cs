using HtmlAgilityPack;
using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Builders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
        private static readonly Dictionary<string, string> GenreTranslations = new Dictionary<string, string>
        {
            { "Oboenquartett", "Oboe Quartet" },
            { "Filmmusik", "Film Music" },
            { "Harfenkonzert", "Harp Concerto" },
            { "Sonate", "Sonata" },
            { "Hornsonate", "Horn Sonata" },
            { "Marsch", "March" },
            { "Sextett", "Sextet" },
            { "Symphonische Dichtung", "Symphonic Poem" },
            { "Posaunenkonzert", "Trombone Concerto" },
            { "Orgelstück", "Organ Piece" },
            { "Violakonzert", "Viola Concerto" },
            { "Menuett", "Minuet" },
            { "Orgelkonzert", "Organ Concerto" },
            { "Ouvertüre", "Overture" },
            { "Klarinettenquartett", "Clarinet Quartet" },
            { "Melodram", "Melodrama" },
            { "Oboensonate", "Oboe Sonata" },
            { "Violinsonate", "Violin Sonata" },
            { "Hornkonzert", "Horn Concerto" },
            { "Flötenkonzert", "Flute Concerto" },
            { "Messe", "Mass" },
            { "Hymne", "Hymn" },
            { "Operette", "Operetta" },
            { "Oper", "Opera" },
            { "Streichersonate", "String Sonata" },
            { "Geistliches Werk", "Sacred Work" },
            { "Quintett", "Quintet" },
            { "Klarinettenkonzert", "Clarinet Concerto" },
            { "Romanze", "Romance" },
            { "Oratorium", "Oratorio" },
            { "Fagottsonate", "Bassoon Sonata" },
            { "Streichsextett", "String Sextet" },
            { "Cembalokonzert", "Harpsichord Concerto" },
            { "Sonstiges", "Miscellaneous" },
            { "Trompetenkonzert", "Trumpet Concerto" },
            { "Klaviertrio", "Piano Trio" },
            { "Mandolinensonate", "Mandolin Sonata" },
            { "Septett", "Septet" },
            { "Bühnenmusik", "Incidental Music" },
            { "Oktett", "Octet" },
            { "Arie", "Aria" },
            { "Präludium", "Prelude" },
            { "Cellosonate", "Cello Sonata" },
            { "Konzertstück", "Concert Piece" },
            { "Rhapsodie", "Rhapsody" },
            { "Terzett", "Terzet" },
            { "Cellokonzert", "Cello Concerto" },
            { "Gitarrenquartett", "Guitar Quartet" },
            { "Elegie", "Elegy" },
            { "Chorwerk", "Choral Work" },
            { "Flötenquartett", "Flute Quartet" },
            { "Streichquartett", "String Quartet" },
            { "Kammermusik", "Chamber Music" },
            { "Oboenkonzert", "Oboe Concerto" },
            { "Kanon", "Canon" },
            { "Klavierkonzert", "Piano Concerto" },
            { "Klaviersonate", "Piano Sonata" },
            { "Tanz", "Dance" },
            { "Orgelsonate", "Organ Sonata" },
            { "Werk für Cembalo", "Harpsichord Work" },
            { "Bearbeitung", "Arrangement" },
            { "Mandolinenkonzert", "Mandolin Concerto" },
            { "Duett", "Duet" },
            { "Kantate", "Cantata" },
            { "Flötensonate", "Flute Sonata" },
            { "Flötenquintett", "Flute Quintet" },
            { "Streichquintett", "String Quintet" },
            { "Triosonate", "Trio Sonata" },
            { "Fantasie", "Fantasia" },
            { "Violinromanze", "Violin Romance" },
            { "Violasonate", "Viola Sonata" },
            { "Hornquintett", "Horn Quintet" },
            { "Fuge", "Fugue" },
            { "Orchesterwerk", "Orchestral Work" },
            { "Cellosuite", "Cello Suite" },
            { "Sonatine", "Sonatina" },
            { "Klaviermusik", "Piano Music" },
            { "Gitarrenquintett", "Guitar Quintet" },
            { "Cembalosonate", "Harpsichord Sonata" },
            { "Streichoktett", "String Octet" },
            { "Klarinettensonate", "Clarinet Sonata" },
            { "Horntrio", "Horn Trio" },
            { "Ballett", "Ballet" },
            { "Klavierquartett", "Piano Quartet" },
            { "Etüde", "Etude" },
            { "Transkription", "Transcription" },
            { "Streichtrio", "String Trio" },
            { "Violinkonzert", "Violin Concerto" },
            { "Werk für Gitarre", "Guitar Work" },
            { "Konzert", "Concerto" },
            { "Quartett", "Quartet" },
            { "Fagottkonzert", "Bassoon Concerto" },
            { "Klarinettenquintett", "Clarinet Quintet" },
            { "Symphonie", "Symphony" },
            { "Klavierquintett", "Piano Quintet" },
            { "Motette", "Motet" },
            { "Ballade", "Ballad" },
            { "Klarinettentrio", "Clarinet Trio" },
            { "Flötentrio", "Flute Trio" },
            { "Harfensonate", "Harp Sonata" },
            { "Klavierstück", "Piano Piece" },
            { "Nonett", "Nonet" },
            { "Klaviersextett", "Piano Sextet" },
            { "Lied", "Song" },
            { "Guitarrenkonzert", "Guitar Concerto" },
            { "Harfenquintett", "Harp Quintet" },
            { "Notturno", "Nocturne" }
        };

        internal static string TranslateGenre(string genre)
        {
            string genreName = null;

            if (GenreTranslations.TryGetValue(genre, out genreName))
            {
                return genreName;
            }

            return genre;
        }


        public async static Task ScrapeComposersPageAsync(ClassicalMusicContext classicalMusicContext)
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var webClient = new WebClient();
                var htmlSource = await webClient.DownloadStringTaskAsync(new Uri($"http://www.klassika.info/Komponisten/lindex_{c}.html"));

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlSource);

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
                    var composer = classicalMusicContext.Composers.FirstOrDefault(co => co.Name == composerName);

                    if (composer == null)
                    {
                        composer = new Composer();
                        composer.Name = composerName;

                        classicalMusicContext.Composers.Add(composer);
                    }

                    await ScrapeComposerDetailPageAsync($"http://www.klassika.info/{hypertextReference.Value}", composer, classicalMusicContext);
                }
            }

            classicalMusicContext.SaveChanges();
        }

        public async static Task ScrapeComposerDetailPageAsync(string url, Composer composer, ClassicalMusicContext classicalMusicContext, IProgress<double> progress = null, CancellationToken? cancellationToken = null)
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

            await ScrapeCompositionsPageAsync($"http://www.klassika.info/Komponisten/{composerKey}/wv_abc.html", composer, classicalMusicContext, progress, cancellationToken);
        }

        public async static Task ScrapeCompositionsPageAsync(string url, Composer composer, ClassicalMusicContext classicalMusicContext, IProgress<double> progress = null, CancellationToken? cancellationToken = null)
        {
            var webClient = new WebClient();
            var htmlSource = await webClient.DownloadStringTaskAsync(new Uri(url));

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlSource);

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

                var genreTableData = tableDatas[6];
                var genreName = TranslateGenre(HtmlEntity.DeEntitize(genreTableData.InnerText).Replace("(", "").Replace(")", ""));

                if (composition == null)
                {
                    composition = new Composition();
                    composition.Name = compositionName;
                    composition.Composers = new List<Composer> { composer };
                    composition.Dates = compositionDate == "?" ? null : compositionDate;

                    composer.Compositions.Add(composition);
                }

                var catalogNumberString = (string)null;
                var compositionCatalogString = (string)null;

                if (!string.IsNullOrWhiteSpace(genreName))
                {
                    if (classicalMusicContext.Genres.Local.Count == 0)
                    {
                        classicalMusicContext.Genres.Load();
                    }

                    var genre = classicalMusicContext.Genres.Local.FirstOrDefault(ct => ct.Name == genreName);

                    if (genre == null)
                    {
                        genre = new Genre();
                        genre.Name = genreName;
                        genre.Compositions.Add(composition);

                        classicalMusicContext.Genres.Add(genre);
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
                    await ScrapeCompositionDetailPageAsync($"http://www.klassika.info/{hypertextReference.Value}", composition, classicalMusicContext);
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

        public async static Task ScrapeCompositionDetailPageAsync(string url, Composition composition, ClassicalMusicContext classicalMusicContext)
        {
            var compositionWebpage = composition.Links.FirstOrDefault(l => l.Url.Contains("klassika.info"));

            if (compositionWebpage == null)
            {
                compositionWebpage = new Link();
                compositionWebpage.Compositions.Add(composition);
                compositionWebpage.Url = url;

                composition.Links.Add(compositionWebpage);
            }

            var webClient = new WebClient();
            var htmlSource = await webClient.DownloadStringTaskAsync(new Uri(url));

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlSource);

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
                        var key = classicalMusicContext.Keys.FirstOrDefault(k => k.Name == value);

                        if (key == null)
                        {
                            key = new Key();
                            key.Name = value;
                            key.Compositions.Add(composition);

                            composition.Key = key;

                            classicalMusicContext.Keys.Add(key);
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
                        var instrumentation = classicalMusicContext.Instrumentations.FirstOrDefault(i => i.Name == value);

                        if (instrumentation == null)
                        {
                            instrumentation = new Instrumentation();
                            instrumentation.Name = value;
                            instrumentation.Compositions.Add(composition);

                            composition.Instrumentation = instrumentation;

                            classicalMusicContext.Instrumentations.Add(instrumentation);
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