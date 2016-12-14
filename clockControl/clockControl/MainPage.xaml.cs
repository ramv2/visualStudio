using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace clockControl
{
    public partial class MainPage : UserControl
    {
        string stop_watchFormat, countdownTimerFormat, dateFormat, timeFormat;
        DispatcherTimer digital_clock_timer, stop_watch_timer, countdown_timer;
        DateTime stop_wtach_timer_start, stop_watch_timer_now;
        TimeSpan stop_watch_timer_elapsed, countdown_time_span;
        TimeSpan offset = TimeSpan.Zero;

        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            digital_clock_timer = new DispatcherTimer();
            digital_clock_timer.Interval = new TimeSpan(0, 0, 1);
            digital_clock_timer.Tick += digital_clock_timer_Tick;
            digital_clock_timer.Start();

            // Different formats for time and dates. These are hardcoded for now but,
            // they can be set to be editable by the user.
            stop_watchFormat = @"hh\:mm\:ss";
            countdownTimerFormat = @"hh\:mm\:ss";
            dateFormat = "D";
            timeFormat = "T";

            date.Text = DateTime.Now.ToString(dateFormat);
            stop_watch.Text = TimeSpan.Zero.ToString(stop_watchFormat);
            count_down_timer.Text = TimeSpan.Zero.ToString(countdownTimerFormat);
        }

        /// <summary>
        /// Logic for the animation of the analog clock hands.
        /// Source: https://msdn.microsoft.com/en-us/library/bb404709%28v=vs.95%29.aspx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetAndStartClock(object sender, RoutedEventArgs e)
        {
            // The current date and time.
            System.DateTime currentDate = DateTime.Now;

            // Find the appropriate angle (in degrees) for the hour hand
            // based on the current time.
            double hourangle = (((float)currentDate.Hour) / 12) * 360 + currentDate.Minute / 2;


            // The same as for the minute angle.
            double minangle = (((float)currentDate.Minute) / 60) * 360;

            // The same for the second angle.
            double secangle = (((float)currentDate.Second) / 60) * 360;

            // Set the beginning of the animation (From property) to the angle 
            // corresponging to the current time.
            hourAnimation.From = hourangle;

            // Set the end of the animation (To property)to the angle 
            // corresponding to the current time PLUS 360 degrees. Thus, the
            // animation will end after the clock hand moves around the clock 
            // once. Note: The RepeatBehavior property of the animation is set
            // to "Forever" so the animation will begin again as soon as it completes.
            hourAnimation.To = hourangle + 360;

            // Same as with the hour animation.
            minuteAnimation.From = minangle;
            minuteAnimation.To = minangle + 360;

            // Same as with the hour animation.
            secondAnimation.From = secangle;
            secondAnimation.To = secangle + 360;

            // Start the storyboard.
            clockStoryboard.Begin();
        }

        // Click events for different buttons: (start)
        /// <summary>
        /// Logic for the 'stop_watch_start' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_watch_start_Click(object sender, RoutedEventArgs e)
        {
            stop_watch_timer = new DispatcherTimer();
            stop_watch_timer.Interval = new TimeSpan(0, 0, 1);
            stop_watch_timer.Tick += stop_watch_timer_Tick;
            stop_watch_timer.Start();
            stop_wtach_timer_start = DateTime.Now;
            stop_watch_stop.IsEnabled = true;
            stop_watch_start.IsEnabled = false;
        }

        /// <summary>
        /// Logic for the 'stop_watch_stop' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_watch_stop_Click(object sender, RoutedEventArgs e)
        {
            stop_watch_stop.IsEnabled = false;
            stop_watch_start.IsEnabled = true;
            offset = stop_watch_timer_elapsed;
            stop_watch_timer.Stop();
        }

        /// <summary>
        /// Logic for the 'stop_watch_reset' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_watch_reset_Click(object sender, RoutedEventArgs e)
        {
            stop_watch_stop.IsEnabled = false;
            offset = TimeSpan.Zero;
            stop_watch_timer_elapsed = TimeSpan.Zero;
            stop_watch.Text = stop_watch_timer_elapsed.ToString("c");
            stop_watch_timer.Stop();
        }

        /// <summary>
        /// Logic for the 'count_down_timer_set' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void count_down_timer_set_Click(object sender, RoutedEventArgs e)
        {
            int h = Convert.ToInt32(hours.Text as string);
            int m = Convert.ToInt32(minutes.Text as string);
            int s = Convert.ToInt32(seconds.Text as string);
            countdown_time_span = new TimeSpan(h, m, s);
            if (countdown_time_span == TimeSpan.Zero)
            {
                return;
            }
            count_down_timer.Text = countdown_time_span.ToString(countdownTimerFormat);
            count_down_timer_start.IsEnabled = true;
            count_down_timer_set.IsEnabled = false;
            countdown_timer = new DispatcherTimer();
            countdown_timer.Interval = new TimeSpan(0, 0, 1);
            countdown_timer.Tick += count_down_timer_Tick;
        }

        /// <summary>
        /// Logic for the 'count_down_timer_start' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void count_down_timer_start_Click(object sender, RoutedEventArgs e)
        {
            count_down_timer_pause.Content = "Pause";
            count_down_timer_pause.IsEnabled = true;
            count_down_timer_start.IsEnabled = false;
            countdown_timer.Start();
        }

        /// <summary>
        /// Logic for the 'count_down_timer_start' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void count_down_timer_pause_Click(object sender, RoutedEventArgs e)
        {
            Button b1 = sender as Button;
            if (b1.Content as string == "Pause")
            {
                b1.Content = "Continue";
                if (countdown_timer.IsEnabled)
                {
                    countdown_timer.Stop();
                }

            }
            else
            {
                b1.Content = "Pause";
                if (!countdown_timer.IsEnabled)
                {
                    countdown_timer.Start();
                }
            }
        }

        /// <summary>
        /// Logic for the 'count_down_timer_reset' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void count_down_timer_reset_Click(object sender, RoutedEventArgs e)
        {
            count_down_timer_set.IsEnabled = true;
            count_down_timer_pause.IsEnabled = false;
            count_down_timer_start.IsEnabled = false;
            hours.Text = "00";
            hours.GotFocus += tb_GotFocus;
            minutes.Text = "00";
            minutes.GotFocus += tb_GotFocus;
            seconds.Text = "00";
            seconds.GotFocus += tb_GotFocus;
            countdown_time_span = TimeSpan.Zero;
            count_down_timer.Text = countdown_time_span.ToString(countdownTimerFormat);
            countdown_timer.Stop();
        }
        // Click events for different buttons: (end)

        // Tick events for different timers: (start)
        /// <summary>
        /// Logic for the 'digital_clock' timer's tick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void digital_clock_timer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToString(timeFormat);
        }

        /// <summary>
        /// Logic for the 'stop_watch' timer's tick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_watch_timer_Tick(object sender, EventArgs e)
        {
            stop_watch_timer_now = DateTime.Now;
            stop_watch_timer_elapsed = stop_watch_timer_now.Subtract(stop_wtach_timer_start);
            stop_watch_timer_elapsed = stop_watch_timer_elapsed.Add(offset);
            string output = stop_watch_timer_elapsed.ToString(stop_watchFormat);
            stop_watch.Text = output;
        }

        /// <summary>
        /// Logic for the 'countdown' timer's tick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void count_down_timer_Tick(object sender, EventArgs e)
        {
            if (countdown_time_span == TimeSpan.Zero)
            {
                count_down_timer_set.IsEnabled = true;
                count_down_timer_start.IsEnabled = false;
                count_down_timer_pause.IsEnabled = false;
                count_down_timer_pause.Content = "Pause";
                count_down_timer_reset.IsEnabled = true;
                MessageBox.Show("Countdown Complete!");
                count_down_timer_reset_Click(sender, new RoutedEventArgs());
                countdown_timer.Stop();
                return;
            }
            countdown_time_span = countdown_time_span.Add(TimeSpan.FromSeconds(-1.00));
            count_down_timer.Text = countdown_time_span.ToString(countdownTimerFormat);
        }
        // Tick events for different timers: (end)        

        /// <summary>
        /// Logic for the 'GotFocus' event of the TextBox. Describes what
        /// happens when the TextBox is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb1 = sender as TextBox;
            tb1.Text = string.Empty;
            tb1.GotFocus -= tb_GotFocus;
        }
    }
}
