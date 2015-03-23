using NathanHarrenstein.ComposerTimeline.Database;
using NathanHarrenstein.ComposerTimeline.UI.Data.Providers;
using NathanHarrenstein.ComposerTimeline.UI.Providers;
using NathanHarrenstein.Controls;
using NathanHarrenstein.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NathanHarrenstein.ComposerTimeline.UI.Initializers
{
    public static class ComposerPageInitializer
    {
        public static void Initialize(ComposerPage composerPage)
        {
            var composer = (Composer)App.Current.Properties["SelectedComposer"];

            composerPage.ComposerImages = GetComposerImages(composer);
            composerPage.Flag = GetFlag(composer);
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
            composerPage.TreeViewItems = GetAlphabetizedTreeViewItems(composer);
            //composerPage.SetInitialCompositionGrouping();
            //composerPage.SortByCatalog();
        }

        private static IEnumerable<Uri> GetComposerImages(Composer composer)
        {
            return composer.ComposerImages.Select(ci => new Uri(ci.Path, UriKind.Relative))
                                          .DefaultIfEmpty(new Uri("pack://application:,,,/User Interface/Resources/NoImage.jpg"));
        }

        private static IEnumerable<TreeViewItem> GetAlphabetizedTreeViewItems(Composer composer)
        {
            return GetTreeViewItems(composer).OrderBy(tvi => tvi.Header);
        }

        private static string GetBorn(Composer composer)
        {
            if (!string.IsNullOrWhiteSpace(composer.DeathPlace.Name))
            {
                return string.Format("{0}; {1}", composer.BirthYear, composer.DeathPlace.Name);
            }
            else
            {
                return composer.BirthYear.ToString();
            }
        }

        private static string GetComposerName(Composer composer)
        {
            var nameConverter = new NameConverter();

            return nameConverter.ToFirstNameLastName(composer.Name);
        }

        private static string GetDied(Composer composer)
        {
            if (!string.IsNullOrWhiteSpace(composer.BirthPlace.Name))
            {
                return string.Format("{0}; {1}", composer.DeathYear, composer.BirthPlace.Name);
            }
            else
            {
                return composer.DeathYear.ToString();
            }
        }

        private static FlagData GetFlag(Composer composer)
        {
            var flagProvider = new FlagDataProvider();

            return flagProvider.GetFlagData(composer.Nationality.Name);
        }

        private static IEnumerable<InfluenceData> GetInfluenced(Composer composer)
        {
            return composer.ComposersInfluenced.Select(ci => InfluenceDataProvider.GetInfluenceData(ci.Influenced));
        }

        private static IEnumerable<InfluenceData> GetInfluences(Composer composer)
        {
            return composer.ComposerInfluences.Select(ci => InfluenceDataProvider.GetInfluenceData(ci.Influence));
        }

        private static IEnumerable<LinkData> GetLinks(Composer composer)
        {
            return composer.ComposerLinks.Select(cl =>
            {
                var linkDataProvider = new LinkDataProvider();

                return linkDataProvider.GetLinkData(cl.URL);
            });
        }

        private static IEnumerable<TreeViewItem> GetTreeViewItems(Composer composer)
        {
            var compositionCollectionTreeViewItems = composer.CompositionCollections.Select<CompositionCollection, TreeViewItem>(cc =>
            {
                var compositionGroupTreeViewItemProvider = new CompositionGroupTreeViewItemProvider();

                return compositionGroupTreeViewItemProvider.GetCompositionGroupTreeViewItem(cc, null);
            });

            var compositionTreeViewItems = composer.Compositions.Select<Composition, TreeViewItem>(c =>
            {
                var compositionTreeViewItemProvider = new CompositionTreeViewItemProvider();

                return compositionTreeViewItemProvider.GetCompositionTreeViewItem(c, null);
            });

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