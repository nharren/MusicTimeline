using Database;
using NathanHarrenstein.Input;
using System;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline.Providers
{
    public static class InfluenceDataProvider
    {
        public static Influence GetInfluenceData(Composer composer)
        {
            var influence = new Influence();
            influence.Composer = composer;
            influence.Name = composer.Name;
            influence.Click = GetClickCommand(composer);

            return influence;
        }

        private static ICommand GetClickCommand(Composer composer)
        {
            Predicate<object> canClick = o =>
            {
                return true;
            };

            Action<object> click = o =>
            {
                App.Current.Properties.Add("SelectedComposer", composer);

                var mainWindow = (MainWindow)App.Current.MainWindow;
                mainWindow.Frame.Navigate(new Uri(@"\Pages\ComposerPage.xaml"));
            };

            return new DelegateCommand(click, canClick);
        }
    }
}