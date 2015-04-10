using NathanHarrenstein.ComposerDatabase;
using NathanHarrenstein.ComposerTimeline.Data;
using NathanHarrenstein.ComposerTimeline.UI.Data;
using NathanHarrenstein.ComposerTimeline.UI.Data.Providers;
using NathanHarrenstein.Controls;
using NathanHarrenstein.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline.UI.Providers
{
    public class ComposerPageProvider
    {
        public ComposerPage GetComposerPage(Composer composer)
        {
            var defaultComposerImageUri = new Uri("pack://application:,,,/User Interface/Resources/NoImage.jpg");

            var composerPage = new ComposerPage();

            composerPage.ComposerImages = GetComposerImages(composer).DefaultIfEmpty(defaultComposerImageUri);
            composerPage.Flag = GetFlag(composer);
            composerPage.HeaderText = GetComposerName(composer);
            composerPage.Influenced = GetInfluenced(composer);
            composerPage.Influences = GetInfluences(composer);
            composerPage.HasInfluences = HasInfluences(composerPage);
            composerPage.HasInfluenced = HasInfluenced(composerPage);
            composerPage.Links = GetLinks(composer);
            composerPage.LinksVisibility = composerPage.Links.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;;
            composerPage.Born = GetBorn(composer);
            composerPage.Died = GetDied(composer);
            composerPage.Biography = GetBiography(composer);
            composerPage.TreeViewItems = GetAlphabetizedTreeViewItems(composer);
            //composerPage.SetInitialCompositionGrouping();
            //composerPage.SortByCatalog();

            return composerPage;
        }

        private IEnumerable<TreeViewItem> GetAlphabetizedTreeViewItems(Composer composer)
        {
            return GetTreeViewItems(composer).OrderBy(tvi => tvi.Header);
        }

        private IEnumerable<TreeViewItem> GetTreeViewItems(Composer composer)
        {
            foreach (var compositionGroup in composer.CompositionGroups)
            {
                var compositionGroupTreeViewItemProvider = new CompositionGroupTreeViewItemProvider();

                yield return compositionGroupTreeViewItemProvider.GetCompositionGroupTreeViewItem(compositionGroup, null);;
            }

            foreach (var composition in composer.Compositions)
            {
                var compositionTreeViewItemProvider = new CompositionTreeViewItemProvider();

                yield return compositionTreeViewItemProvider.GetCompositionTreeViewItem(composition, null);
            }
        }

        private string GetBiography(Composer composer)
        {
            foreach (var composerProperty in composer.ComposerProperties)
            {
                if (composerProperty.Key == "Biography")
                {
                    return composerProperty.Value;
                }
            }

            return string.Empty;
        }

        private Visibility HasInfluenced(ComposerPage composerPage)
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

        private Visibility HasInfluences(ComposerPage composerPage)
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

        private string GetBorn(Composer composer)
        {
            var born = string.Empty;
            var birthplace = string.Empty;

            foreach (var composerProperty in composer.ComposerProperties)
            {
                if (composerProperty.Key == "Birthday")
                {
                    born = composerProperty.Value;

                    if (birthplace != string.Empty)
                    {
                        break;
                    }
                }
                else if (composerProperty.Key == "Birthplace")
                {
                    birthplace = composerProperty.Value;

                    if (born != string.Empty)
                    {
                        break;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(birthplace))
            {
                born += "; " + birthplace;
            }

            return born;
        }

        private IEnumerable<Uri> GetComposerImages(Composer composer)
        {
            foreach (var composerProperty in composer.ComposerProperties)
            {
                var isImageProperty = composerProperty.Key == "Image";
                var fileExists = File.Exists(composerProperty.Value);

                if (isImageProperty && fileExists)
                {
                    yield return new Uri(composerProperty.Value);
                }
            }
        }

        private string GetDied(Composer composer)
        {
            var died = string.Empty;
            var deathplace = string.Empty;

            foreach (var composerProperty in composer.ComposerProperties)
            {
                if (composerProperty.Key == "Deathday")
                {
                    died = composerProperty.Value;

                    if (deathplace != string.Empty)
                    {
                        break;
                    }
                }
                else if (composerProperty.Key == "Birthplace")
                {
                    deathplace = composerProperty.Value;

                    if (died != string.Empty)
                    {
                        break;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(deathplace))
            {
                died += "; " + deathplace;
            }

            return died;
        }

        private FlagData GetFlag(Composer composer)
        {
            var flagProvider = new FlagDataProvider();

            foreach (var composerProperty in composer.ComposerProperties)
            {
                if (composerProperty.Key == "Nationality")
                {
                    return flagProvider.GetFlagData(composerProperty.Value);
                }
            }

            return flagProvider.GetFlagData(null);
        }

        private string GetComposerName(Composer composer)
        {
            var nameConverter = new NameConverter();

            foreach (var composerProperty in composer.ComposerProperties)
            {
                var isNameProperty = composerProperty.Key == "Name";
                var notNullOrWhitespace = !string.IsNullOrWhiteSpace(composerProperty.Value);

                if (isNameProperty && notNullOrWhitespace)
                {
                    return nameConverter.ToFirstNameLastName(composerProperty.Value);
                }
            }

            throw new InvalidOperationException("There is no name property for the composer with the ID: " + composer.ID);
        }

        private IEnumerable<InfluenceData> GetInfluenced(Composer composer)
        {
            foreach (var composerProperty in composer.ComposerProperties)
            {
                if (composerProperty.Key == "Influenced")
                {
                    var influenceDataProvider = new InfluenceDataProvider();

                    yield return influenceDataProvider.GetInfluenceData(composerProperty.Value);
                }
            }
        }

        private IEnumerable<InfluenceData> GetInfluences(Composer composer)
        {
            foreach (var composerProperty in composer.ComposerProperties)
            {
                if (composerProperty.Key == "Influence")
                {
                    var influenceDataProvider = new InfluenceDataProvider();

                    yield return influenceDataProvider.GetInfluenceData(composerProperty.Value);
                }
            }
        }



        private IEnumerable<LinkData> GetLinks(Composer composer)
        {
            foreach (var composerProperty in composer.ComposerProperties)
            {
                var isLinkProperty = composerProperty.Key == "Link";
                var notNullOrWhitespace = !string.IsNullOrWhiteSpace(composerProperty.Value);

                if (isLinkProperty && notNullOrWhitespace)
                {
                    var linkDataProvider = new LinkDataProvider();

                    yield return linkDataProvider.GetLinkData(composerProperty.Value);
                }
            }
        }
    }
}
