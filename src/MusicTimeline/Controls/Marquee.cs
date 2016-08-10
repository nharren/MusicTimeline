using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class Marquee : ContentControl
    {
        public static readonly DependencyProperty AccelerationProperty = DependencyProperty.Register("Acceleration", typeof(double), typeof(Marquee), new PropertyMetadata(0d));
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(double), typeof(Marquee), new PropertyMetadata(ValueChanged));
        public static readonly DependencyProperty FinalPauseProperty = DependencyProperty.Register("FinalPause", typeof(double), typeof(Marquee), new PropertyMetadata(0d));
        public static readonly DependencyProperty InitialPauseProperty = DependencyProperty.Register("InitialPause", typeof(double), typeof(Marquee), new PropertyMetadata(0d));

        private readonly DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
        private readonly Storyboard storyboard = new Storyboard();
        private FrameworkElement contentPart;
        private bool isLoaded;

        static Marquee()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Marquee), new FrameworkPropertyMetadata(typeof(Marquee)));
        }

        public Marquee()
        {
            Loaded += Marquee_Loaded;
        }

        /// <summary>
        /// Value in the range 0 to 1 representing how much acceleration to apply to the animation. Zero gives a linear animation and 1 is just silly.
        /// </summary>
        public double Acceleration
        {
            get
            {
                return (double)GetValue(AccelerationProperty);
            }
            set
            {
                SetValue(AccelerationProperty, value);
            }
        }

        /// <summary>
        /// Length in seconds of the animation effect
        /// </summary>
        public double Duration
        {
            get
            {
                return (double)GetValue(DurationProperty);
            }
            set
            {
                SetValue(DurationProperty, value);
            }
        }

        /// <summary>
        /// Length in seconds of the pause at the end of the animation
        /// </summary>
        public double FinalPause
        {
            get
            {
                return (double)GetValue(FinalPauseProperty);
            }
            set
            {
                SetValue(FinalPauseProperty, value);
            }
        }

        /// <summary>
        /// Length in seconds of the pause at the start of the animation
        /// </summary>
        public double InitialPause
        {
            get
            {
                return (double)GetValue(InitialPauseProperty);
            }
            set
            {
                SetValue(InitialPauseProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            contentPart = GetTemplateChild("PART_Content") as FrameworkElement;

            SizeChanged += Marquee_SizeChanged;

            if (contentPart != null)
            {
                contentPart.SizeChanged += Marquee_SizeChanged;
            }
        }

        private static void ValueChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var marquee = (Marquee)sender;
            marquee.RestartAnimation();
        }

        private void Marquee_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;

            RestartAnimation();

            MouseEnter += Marquee_MouseEnter;
            MouseLeave += Marquee_MouseLeave;
        }

        private void Marquee_MouseEnter(object sender, MouseEventArgs e)
        {
            storyboard.Pause();
        }

        private void Marquee_MouseLeave(object sender, MouseEventArgs e)
        {
            storyboard.Resume();
        }

        private void Marquee_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RestartAnimation();
        }

        private void RestartAnimation()
        {
            if (ActualHeight == 0)
            {
                if (storyboard != null)
                {
                    storyboard.Stop();
                }

                return;
            }

            if (contentPart != null && isLoaded)
            {
                storyboard.Stop();

                double value = InitialPause + Duration * contentPart.ActualWidth / ActualWidth;
                animation.Duration = new Duration(TimeSpan.FromSeconds(value));

                animation.KeyFrames.Clear();
                animation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                animation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(InitialPause))));
                animation.KeyFrames.Add(new SplineDoubleKeyFrame(Math.Min(ActualWidth - contentPart.ActualWidth - 12, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(value)), new KeySpline(new Point(Acceleration, 0), new Point(1 - Acceleration, 1))));

                storyboard.Duration = new Duration(TimeSpan.FromSeconds(value + FinalPause));

                if (storyboard.Children.Count == 0)
                {
                    storyboard.Children.Add(animation);

                    Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Left)"));
                    Storyboard.SetTarget(animation, contentPart);

                    storyboard.RepeatBehavior = RepeatBehavior.Forever;
                }

                storyboard.Begin();
            }
        }
    }
}