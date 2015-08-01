using HtmlAgilityPack;
using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Builders;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NathanHarrenstein.MusicTimeline.Scrapers
{
    public static class KlassikaScraper
    {
        public static void Scrape(string composerUrl, Composer composer, DataProvider dataProvider)
        {
            var key = Regex.Match(composerUrl, "(?<=klassika.info/Komponisten/)(\\w*)(?=/index.html)").Captures[0].Value;

            if (key == null)
            {
                return;
            }

            var compositionsUrl = $"http://www.klassika.info/Komponisten/{key}/wv_abc.html";

            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;

            var htmlSource = (string)null;

            try
            {
                htmlSource = webClient.DownloadString(compositionsUrl);
            }
            catch (Exception)
            {
                return;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HtmlEntity.DeEntitize(htmlSource));
            var tables = htmlDocument.DocumentNode.Descendants("table");
            var wvTable = tables.FirstOrDefault(n =>
            {
                var classAttribute = n.Attributes["class"];

                if (classAttribute == null || classAttribute.Value != "wv")
                {
                    return false;
                }

                return true;
            });

            var trs = wvTable.Elements("tr");

            foreach (var tr in trs)
            {
                var nameNode = tr.Elements("td").ElementAt(1);
                var catalogNode = tr.Elements("td").ElementAt(2);
                var dateNode = tr.Elements("td").ElementAt(4);
                //var categoryNode = tr.ChildNodes[6];

                var composition = CompositionBuilder.Build(nameNode.InnerText, new ObservableCollection<Composer> { composer }, dataProvider);
                composition.Dates = dateNode.InnerText == "?" ? null : dateNode.InnerText;

                if (!string.IsNullOrWhiteSpace(catalogNode.InnerText))
                {
                    var catalogParts = catalogNode.InnerText.Split(new string[] { "&nbsp;" }, 2, StringSplitOptions.RemoveEmptyEntries);

                    if (catalogParts.Length == 2)
                    {
                        var catalogCatalogString = catalogParts[0];
                        var catalogNumberString = catalogParts[1];

                        var compositionCatalogQuery = dataProvider.CompositionCatalogs.FirstOrDefault(cc => cc.Composer.Name == composer.Name && cc.Prefix == catalogCatalogString);

                        if (compositionCatalogQuery == null)
                        {
                            compositionCatalogQuery = CompositionCatalogBuilder.Build(catalogCatalogString, composer, dataProvider);

                            composer.CompositionCatalogs.Add(compositionCatalogQuery);
                        }

                        var catalogNumber = CatalogNumberBuilder.Build(compositionCatalogQuery, null, composition, dataProvider);
                        catalogNumber.Number = catalogNumberString;

                        compositionCatalogQuery.CatalogNumbers.Add(catalogNumber);
                    }
                }

                composer.Compositions.Add(composition);

                var compositionUrlAttribute = nameNode.Element("a").Attributes["href"];

                if (compositionUrlAttribute != null)
                {
                    var compositionUrl = $"http://www.klassika.info/{compositionUrlAttribute.Value}";
                    var compositionHtmlSource = (string)null;

                    try
                    {
                        compositionHtmlSource = webClient.DownloadString(compositionUrl);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    var compositionHtmlDocument = new HtmlDocument();
                    compositionHtmlDocument.LoadHtml(HtmlEntity.DeEntitize(compositionHtmlSource));

                    var movementsHeader = compositionHtmlDocument.DocumentNode.Descendants("h2").FirstOrDefault(n => n.InnerText == "Sätze:");

                    if (movementsHeader != null)
                    {
                        var compositionWvTable = movementsHeader.NextSibling.NextSibling;

                        var movementTrs = compositionWvTable.Elements("tr");

                        foreach (var movementTr in movementTrs)
                        {
                            var movementNumberNode = movementTr.Elements("td").ElementAt(0);
                            var movementNameNode = movementTr.Elements("td").ElementAt(1);

                            var movementNumber = short.Parse(Regex.Replace(movementNumberNode.InnerText, ". Satz:", ""));

                            var movement = MovementBuilder.Build(movementNameNode.InnerText, composition, dataProvider);
                            movement.Number = movementNumber;
                            composition.Movements.Add(movement);
                        }
                    }
                }

                Debug.WriteLine(composition.Name);
            }

            dataProvider.SaveChanges();
        }
    }
}