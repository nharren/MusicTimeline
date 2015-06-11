using Database;
using NathanHarrenstein.ComposerTimeline.Providers;
using NathanHarrenstein.Controls;
using NathanHarrenstein.Converters;
using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.IO;
using System.Linq;
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
            composerPage.LinksVisibility = composerPage.Links.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;
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
            if (composer.BirthLocation != null)
            {
                return string.Format("{0}; {1}", ExtendedDateTimeInterval.Parse(composer.Dates).Start, composer.BirthLocation.Name);
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
        }

        private static string GetComposerName(Composer composer)
        {
            return NameConverter.ToFirstLast(composer.Name);
        }

        private static string GetDied(Composer composer)
        {
            if (composer.DeathLocation != null)
            {
                return string.Format("{0}; {1}", ExtendedDateTimeInterval.Parse(composer.Dates).End, composer.DeathLocation.Name);
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).End.ToString();
        }

        private static IEnumerable<Flag> GetFlags(Composer composer)
        {
            return composer.Nationalities.Select(n => FlagProvider.GetFlag(n.Name, FlagSize.Large));
        }

        private static IEnumerable<Influence> GetInfluenced(Composer composer)
        {
            return composer.Influenced.Select(i => InfluenceDataProvider.GetInfluenceData(i));
        }

        private static IEnumerable<Influence> GetInfluences(Composer composer)
        {
            return composer.Influences.Select(i => InfluenceDataProvider.GetInfluenceData(i));
        }

        private static IEnumerable<Link> GetLinks(Composer composer)
        {
            return composer.ComposerLinks.Select(cl => LinkProvider.GetLink(cl.URL));
        }

        private static IEnumerable<TreeViewItem> GetTreeViewItems(Composer composer)
        {
            foreach (var compositionCollection in composer.CompositionCollections)
            {
                yield return CompositionCollectionTreeViewItemProvider.GetCompositionCollectionTreeViewItem(compositionCollection, null);
            }

            foreach (var composition in composer.Compositions)
            {
                yield return CompositionTreeViewItemProvider.GetCompositionTreeViewItem(composition, null);
            }
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