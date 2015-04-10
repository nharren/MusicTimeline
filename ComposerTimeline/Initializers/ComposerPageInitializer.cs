using Database;
using NathanHarrenstein.ComposerTimeline.Data.Providers;
using NathanHarrenstein.ComposerTimeline.Providers;
using NathanHarrenstein.Controls;
using NathanHarrenstein.Converters;
using System;
using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline.Initializers
{
    public static class ComposerPageInitializer
    {
        public static void Initialize(ComposerPage composerPage)
        {
            var composer = (Composer)App.Current.Properties["SelectedComposer"];

            composerPage.ComposerImages = GetComposerImages(composer);
            composerPage.Flags = GetFlags(composer);
            composerPage.HeaderText = GetComposerName(composer);
            composerPage.Influenced = GetInfluenced(composer);
            composerPage.Influences = GetInfluences(composer);
            composerPage.HasInfluences = HasInfluences(composerPage);
            composerPage.HasInfluenced = HasInfluenced(composerPage);
            composerPage.Links = GetLinks(composer);
            composerPage.LinksVisibility = composerPage.Links.Count() > 0 ? Visibility.Visible : Visibility.Collapsed; ;
            composerPage.Born = GetBorn(composer);
            composerPage.Died = GetDied(composer);
            composerPage.Biography = composer.Biography;
            composerPage.TreeViewItems = GetTreeViewItems(composer).OrderBy(tvi => tvi.Header);
            //composerPage.SetInitialCompositionGrouping();
            //composerPage.SortByCatalog();
        }

        private static IEnumerable<BitmapImage> GetComposerImages(Composer composer)
        {
            return composer.ComposerImages.Select(ci => LoadImage(ci.Image));
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var image = new BitmapImage();

            using (var memoryStream = new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memoryStream;
                image.EndInit();
            }

            image.Freeze();

            return image;
        }

        private static string GetBorn(Composer composer)
        {
            var composerDates = (ExtendedDateTimeInterval)ExtendedDateTimeFormatParser.Parse(composer.Dates);
            
            if (!string.IsNullOrWhiteSpace(composer.DeathLocation.Name))
            {
                return string.Format("{0}; {1}", composerDates.Start, composer.DeathLocation.Name);
            }
            else
            {
                return composerDates.Start.ToString();
            }
        }

        private static string GetComposerName(Composer composer)
        {
            return NameConverter.ToFirstLast(composer.Name);
        }

        private static string GetDied(Composer composer)
        {
            var composerDates = (ExtendedDateTimeInterval)ExtendedDateTimeFormatParser.Parse(composer.Dates);

            if (!string.IsNullOrWhiteSpace(composer.BirthLocation.Name))
            {
                return string.Format("{0}; {1}", composerDates.End, composer.BirthLocation.Name);
            }
            else
            {
                return composerDates.End.ToString();
            }
        }

        private static IEnumerable<Flag> GetFlags(Composer composer)
        {
            return composer.Nationalities.Select(n => FlagProvider.GetFlag(n.Name, FlagSize.Large));
        }

        private static IEnumerable<InfluenceData> GetInfluenced(Composer composer)
        {
            return composer.Influenced.Select(i => InfluenceDataProvider.GetInfluenceData(i));
        }

        private static IEnumerable<InfluenceData> GetInfluences(Composer composer)
        {
            return composer.Influences.Select(i => InfluenceDataProvider.GetInfluenceData(i));
        }

        private static IEnumerable<LinkData> GetLinks(Composer composer)
        {
            return composer.ComposerLinks.Select(cl => LinkDataProvider.GetLinkData(cl.URL));
        }

        private static IEnumerable<TreeViewItem> GetTreeViewItems(Composer composer)
        {
            var compositionCollectionTreeViewItems = composer.CompositionCollections.Select<CompositionCollection, TreeViewItem>(cc => CompositionCollectionTreeViewItemProvider.GetCompositionCollectionTreeViewItem(cc, null));

            var compositionTreeViewItems = composer.Compositions.Select<Composition, TreeViewItem>(c => CompositionTreeViewItemProvider.GetCompositionTreeViewItem(c, null));

            return compositionCollectionTreeViewItems.Concat(compositionTreeViewItems);
        }

        private static Visibility HasInfluenced(ComposerPage composerPage)
        {
            var influencedCount = composerPage.Influenced.Count();
            var nonzeroInfluencedCount = influencedCount > 0;
            var hasValidInfluenced = false;

            foreach (var influenced in composerPage.Influenced)
            {
                if (!string.IsNullOrWhiteSpace(influenced.Name))
                {
                    hasValidInfluenced = true;

                    break;
                }
            }

            if (nonzeroInfluencedCount && hasValidInfluenced)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        private static Visibility HasInfluences(ComposerPage composerPage)
        {
            var influenceCount = composerPage.Influences.Count();
            var nonzeroInfluenceCount = influenceCount > 0;
            var hasValidInfluence = false;

            foreach (var influence in composerPage.Influences)
            {
                if (!string.IsNullOrWhiteSpace(influence.Name))
                {
                    hasValidInfluence = true;

                    break;
                }
            }

            if (nonzeroInfluenceCount && hasValidInfluence)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
    }
}