using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.Models;
using NathanHarrenstein.MusicTimeline.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Providers
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
                System.Windows.Application.Current.Properties["SelectedComposer"] = composer;

                var frame = (Frame)System.Windows.Application.Current.MainWindow.FindName("Frame");

                frame.Navigate(new ComposerPage());
            };

            return new DelegateCommand(click, canClick);
        }
    }
}