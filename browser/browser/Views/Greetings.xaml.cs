using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Media;
using System.Collections;
using System.Windows.Data;
using System.Globalization;
using Microsoft.Win32;
using System.Diagnostics;

namespace browser
{
    /// <summary>
    /// Interaction logic for Greetings.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string home, startText, homeText;
        public int currTabIndex = 0;
        public TabItem tLast;
        public ArrayList history;
        public bool firstNavigated = true;
        public MainWindow()
        {
            InitializeComponent();
            Closing += main_window_Closing;
            Closed += main_window_Closed;            
            KeyDown += main_window_KeyDown;
            DataContext = this;

            // To enable the use of IE 11
            RegistryKey Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            Key.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", 11001, RegistryValueKind.DWord);
            Key.Close();
            //

            // Read history
            history = Properties.Settings.Default.History;
            if (history != null && history.Count > 0)
            {
                for (int i=0; i<history.Count; i++)
                {
                    cb.Items.Add(history[i] as string);
                }
                button_clear_history.IsEnabled = true;
            }

            // Check favorites
            if (Properties.Settings.Default.Favorites != null && Properties.Settings.Default.Favorites.Count > 0)
            {
                button_clear_favorites.IsEnabled = true;
            }
                        
            
            home = "http://www.iconics.com/Home.aspx";
            homeText = "ICONICS - HMI/SCADA Software, Building Automation & Manufacturing Intelligence Software";
            startText = "Enter URL(Format: http://www.example.com) here and press enter or click the blue 'Go' button to navigate. Use Ctrl + Enter to autocomplete address";            
            cb.Text = home;            
            cb.GotFocus += cb_GotFocus;
            
            TabItem t1 = regular_TabItem(true);            
            WebBrowser wbTab = t1.Content as WebBrowser;            
            wbTab.Navigate(home);            
            tLast = last_TabItem();
            tab_control.Items.Add(t1);
            tab_control.Items.Add(tLast);
            tLast.IsSelected = false;
            tab_control.Items.MoveCurrentTo(t1);
            tab_control.SelectionChanged += tab_control_SelectionChanged;            
        }
        
        // Methods and events related to control type Window: (start)
        /// <summary>
        /// Logic for the Closed event in the window. Describes what 
        /// happens when the window is about to close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
                                 
        public void main_window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.History = new ArrayList(cb.Items);            
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Logic for the KeyDown event in the window. Describes what
        /// happens when different keys or combination of keys are
        /// pressed in the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void main_window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.T)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    int lastTabIndex = tab_control.Items.Count - 1;
                    Button plus = (tab_control.Items.GetItemAt(lastTabIndex) as TabItem).Header as Button;
                    if (plus != null)
                    {
                        button_add_tab_Click(plus, new RoutedEventArgs());
                    }
                    
                }
            }
            if (e.Key == Key.W)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    Button close = ((tab_control.SelectedItem as TabItem).Header as StackPanel).Children[1] as Button;
                    if (close != null)
                    {
                        button_close_tab_Click(close, new RoutedEventArgs());
                    }
                    
                }
            }
            if (e.Key == Key.R)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {                    
                    button_refresh_Click(sender, new RoutedEventArgs());
                }
            }
        }

        /// <summary>
        /// Logic for the Closing event in the window. Describes what
        /// happens when the user tries to close the window. Provides
        /// option to cancel the event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void main_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int totalTabs = tab_control.Items.Count;
            if (totalTabs > 2)
            {
                MessageBoxResult result = MessageBox.Show("Do you wish to close multiple tabs?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Do you wish to close window?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    e.Cancel = true;
                }
            }

        }
        // Methods and events related to control typ Window: (end)

        // Methods and events related to control type Button: (start)
        /// <summary>
        /// Logic for the 'back' button-click event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_back_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wbTab = tab_control.SelectedContent as WebBrowser;
            if (wbTab.CanGoBack)
            {
                wbTab.GoBack();

            }
        }

        /// <summary>
        /// Logic for the 'forward' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_forward_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wbTab = tab_control.SelectedContent as WebBrowser;
            if (wbTab.CanGoForward)
            {
                wbTab.GoForward();
            }

        }

        /// <summary>
        /// Logic for the 'go' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_go_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wbTab = tab_control.SelectedContent as WebBrowser;
            if (cb.Text != string.Empty && cb.Text != startText)
            {
                Uri currentUri;
                if (Uri.TryCreate(cb.Text, UriKind.Absolute, out currentUri))
                {
                    try
                    {
                        wbTab.Navigate(currentUri);
                    }
                    catch (UriFormatException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid URL of the format http://www.example.com or alternatively type example and use Ctrl + Enter to autocomplete URL(i.e., insert 'http://www.' at the beginning and '.com' at the end).");
                }
            }
        }

        /// <summary>
        /// Logic for the 'stop' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_stop_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wbTab = tab_control.SelectedContent as WebBrowser;
            wbTab.InvokeScript("eval", "document.execCommand('Stop');");
        }

        /// <summary>
        /// Logic for the 'home' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_home_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wbTab = tab_control.SelectedContent as WebBrowser;
            wbTab.Navigate(home);
        }

        /// <summary>
        /// Logic for the 'refresh' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_refresh_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wbTab = tab_control.SelectedContent as WebBrowser;
            if (wbTab.Source != null)
            {
                wbTab.Refresh();
            }
        }

        /// <summary>
        /// Logic for the 'favorites' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_favorites_Click(object sender, RoutedEventArgs e)
        {
            favoritesView f1 = new favoritesView(tab_control.SelectedContent as WebBrowser, cb, button_clear_favorites);
            f1.Show();
        }

        /// <summary>
        /// Logic for the 'close other tabs' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_close_other_tabs_Click(object sender, RoutedEventArgs e)
        {
            TabItem t1 = tab_control.SelectedItem as TabItem;
            tab_control.SelectionChanged -= tab_control_SelectionChanged;
            tab_control.Items.Clear();
            tab_control.Items.Add(t1);
            tab_control.Items.Add(tLast);
            t1.IsSelected = true;
            tLast.IsSelected = false;
            currTabIndex = 0;
            tab_control.SelectionChanged += tab_control_SelectionChanged;
            button_close_other_tabs.IsEnabled = false;
        }

        /// <summary>
        /// Logic for the 'clear history' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_clear_history_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.History.Clear();
            string currText = cb.Text;
            cb.Items.Clear();
            cb.Text = currText;
            button_clear_history.IsEnabled = false;
        }

        /// <summary>
        /// Logic for the 'clear favorites' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clear_favorties_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Favorites.Clear();
            Properties.Settings.Default.Save();
            button_clear_favorites.IsEnabled = false;
        }       

        /// <summary>
        /// Logic for the 'add new tab' or the '+' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_add_tab_Click(object sender, RoutedEventArgs e)
        {
            TabItem thisTab = (sender as Button).Parent as TabItem;
            int thisIndex = tab_control.Items.IndexOf(thisTab);
            TabItem t1 = regular_TabItem(false);
            tab_control.Items.Insert(thisIndex, t1);
            (tab_control.Items.GetItemAt(thisIndex) as TabItem).IsSelected = true;
            cb.Text = startText;
            cb.GotFocus += cb_GotFocus;
            if (tab_control.Items.Count > 2)
            {
                button_close_other_tabs.IsEnabled = true;
            }
        }

        /// <summary>
        /// Logic for the 'close tab' or the 'x' button-click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_close_tab_Click(object sender, RoutedEventArgs e)
        {
            int tabCount = tab_control.Items.Count;
            if (tabCount == 2)
            {
                Application.Current.MainWindow.Close();
            }
            else if (tabCount > 2)
            {
                TabItem thisTab = ((sender as Button).Parent as StackPanel).Parent as TabItem;
                int thisIndex = tab_control.Items.IndexOf(thisTab);
                int nextIndex = thisIndex + 1;
                tab_control.SelectionChanged -= tab_control_SelectionChanged;
                tab_control.Items.Remove(thisTab);
                tab_control.SelectionChanged += tab_control_SelectionChanged;
                if (nextIndex == tabCount - 1)
                {
                    nextIndex = thisIndex - 1;
                }
                if (thisIndex < currTabIndex)
                {
                    currTabIndex--;
                }
                else if (thisIndex == currTabIndex)
                {
                    (tab_control.Items.GetItemAt(nextIndex) as TabItem).IsSelected = true;
                }
                if (tab_control.Items.Count == 2)
                {
                    button_close_other_tabs.IsEnabled = false;
                }
            }
        }
        // Methods and events related to control type Button: (end)        

        // Methods and events related to control type ComboBox (Address bar): (start)
        /// <summary>
        /// Logic for the GotFocus event in the ComboBox. Describes what
        /// happens when address bar is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cb_GotFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cb1 = (ComboBox)sender;
            if (cb1.Text == startText)
            {
                cb1.Text = string.Empty;
                cb1.GotFocus -= cb_GotFocus;
            }

        }

        /// <summary>
        /// Logic for the KeyDown event in the ComboBox. Describes what
        /// happens when different keys or combination of keys are
        /// pressed when the ComboBox is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    cb.Text = "http://www." + cb.Text + ".com";
                }
                WebBrowser wbTab = tab_control.SelectedContent as WebBrowser;
                if (cb.Text != string.Empty && cb.Text != startText)
                {
                    Uri currentUri;
                    if (Uri.TryCreate(cb.Text, UriKind.Absolute, out currentUri))
                    {
                        try
                        {
                            wbTab.Navigate(currentUri);
                        }
                        catch (UriFormatException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid URL of the format http://www.example.com or alternatively type example and use Ctrl + Enter to autocomplete URL(i.e., insert 'http://www.' at the beginning and '.com' at the end).");
                    }
                }                
            }
        }

        /// <summary>
        /// Logic for the MouseEnter event in the ComboBox. Describes what
        /// happens when the mouse enters the ComboBox area.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cb_MouseEnter(object sender, MouseEventArgs e)
        {
            cb.ToolTip = startText;
        }
        // Methods and events related to control type ComboBox (Address bar): (end)

        // Methods and events related to control type TabControl: (start)
        /// <summary>
        /// Logic for the SelectionChanged event in the TabControl. Describes what
        /// happens when a different tab is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void tab_control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            TabItem selectedTab = tab_control.SelectedItem as TabItem;
            if (selectedTab == null || tab_control.SelectedIndex == -1)
            {
                throw new Exception("Some tab has to be always selected!");
            }
            if (selectedTab.Name as string == string.Empty)
            {
                currTabIndex = tab_control.SelectedIndex;
                WebBrowser wbTab = selectedTab.Content as WebBrowser;
                if (wbTab != null)
                {
                    if (wbTab.Source != null)
                    {
                        cb.Text = wbTab.Source.AbsoluteUri.ToString();
                    }
                    else
                    {
                        if (cb.IsKeyboardFocusWithin)
                            cb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        cb.Text = startText;
                        cb.GotFocus += cb_GotFocus;
                    }
                }
            }
            else
            {
                (tab_control.Items.GetItemAt(currTabIndex) as TabItem).IsSelected = true;
            }
        }
        // Methods and events related to control type TabControl: (end)

        // Methods and events related to control type WebBrowser: (start)
        /// <summary>
        /// Logic for the LoadCompleted event in the WebBrowser control. Describes what
        /// happens when the document (of the current website) has finished downloading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void web_browser_LoadCompleted(object sender, NavigationEventArgs e)
        {   
            TextBlock tb1 = (((sender as WebBrowser).Parent as TabItem).Header as StackPanel).Children[0] as TextBlock;
            string docTitle = getTitle(sender as WebBrowser, cb.Text);            
            tb1.ToolTip = docTitle;
            tb1.Text = docTitle.Length > 15 ? docTitle.Substring(0,15): docTitle;
        }        

        /// <summary>
        /// Logic for the Navigated event in the WebBrowser control. Describes what
        /// happens when the document (of the current website) has started
        /// downloading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void web_browser_Navigated(object sender, NavigationEventArgs e)
        {
            if (firstNavigated)
            {
                firstNavigated = false;
                return;
            }
            WebBrowser wbTab = (WebBrowser)sender;
            if (wbTab == tab_control.SelectedContent as WebBrowser)
            {
                if (wbTab.Source.AbsoluteUri != null)
                {
                    cb.Text = wbTab.Source.AbsoluteUri;
                    cb.Items.Add(cb.Text);
                    if (!button_clear_history.IsEnabled)
                    {
                        button_clear_history.IsEnabled = true;
                    }
                }
                else
                {
                    cb.Text = startText;
                    cb.GotFocus += cb_GotFocus;
                }
            }                        
        }
        // Methods and events related to control type WebBrowser: (end) 

        // Generic methods: (start)
        /// <summary>
        /// Logic to create all regular TabItems.
        /// </summary>
        /// <param name="isHome"></param>
        /// <returns></returns>
        public TabItem regular_TabItem(bool isHome)
        {            
            TextBlock b1 = new TextBlock();            
            b1.Text = isHome ? homeText:"New Tab";
            b1.TextAlignment = TextAlignment.Center;
            b1.VerticalAlignment = VerticalAlignment.Center;
            b1.HorizontalAlignment = HorizontalAlignment.Center;
            b1.Margin = new Thickness(10);
            Binding binding = new Binding();
            binding.Source = main_window;
            binding.Path = new PropertyPath("Width");            
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            binding.Converter = new widthConverter();
            binding.ConverterParameter = 0.075;
            BindingOperations.SetBinding(b1, WidthProperty, binding);
            b1.MaxWidth = 100;

            Button b2 = new Button();
            b2.Background = Brushes.Transparent;
            b2.Margin = new Thickness(5);
            b2.BorderBrush = Brushes.Transparent;
            b2.ToolTip = "Click button or press Ctrl + W to close current tab";
            b2.Opacity = 1;
            StackPanel sp = new StackPanel();
            Image i1 = new Image();
            ImageSourceConverter isc = new ImageSourceConverter();
            i1.Source = (ImageSource)isc.ConvertFromString("C:\\Users\\Ramachandran\\Documents\\Visual Studio 2015\\Projects\\browser\\icons\\close_tab.png");
            sp.Children.Add(i1);
            b2.Content = sp;
            RoutedEventHandler r2 = new RoutedEventHandler(button_close_tab_Click);
            b2.Click += r2;

            StackPanel bigSP = new StackPanel();            
            bigSP.Orientation = Orientation.Horizontal;
            bigSP.Children.Add(b1);
            bigSP.Children.Add(b2);
            
            WebBrowser wb = new WebBrowser();
            HideScriptErrors(wb, true);
            wb.Margin = new Thickness(0, 20, 0, 0);
            wb.HorizontalAlignment = HorizontalAlignment.Stretch;
            wb.VerticalAlignment = VerticalAlignment.Top;            
            wb.Navigated += web_browser_Navigated;
            wb.LoadCompleted += web_browser_LoadCompleted;
            TabItem t1 = new TabItem();            
            t1.Header = bigSP;
            t1.Content = wb;
            t1.IsSelected = true;
            return t1;
        }

        /// <summary>
        /// Logic to create the last TabItem.
        /// </summary>
        /// <returns></returns>
        public TabItem last_TabItem()
        {
            Button b2 = new Button();            
            b2.Background = Brushes.Transparent;            
            b2.BorderBrush = Brushes.Transparent;
            b2.Name = "plus";
            b2.ToolTip = "Click button or press Ctrl + T to open a new tab";
            b2.Opacity = 1;
            StackPanel sp = new StackPanel();
            Image i1 = new Image();
            ImageSourceConverter isc = new ImageSourceConverter();
            i1.Source = (ImageSource)isc.ConvertFromString("C:\\Users\\Ramachandran\\Documents\\Visual Studio 2015\\Projects\\browser\\icons\\new_tab.png");
            sp.Children.Add(i1);
            b2.Content = sp;
            RoutedEventHandler r2 = new RoutedEventHandler(button_add_tab_Click);
            b2.Click += r2;
            b2.MaxWidth = 16;
            b2.HorizontalAlignment = HorizontalAlignment.Stretch;            
            TabItem t1 = new TabItem();
            t1.Header = b2;
            t1.Name = "last";
            t1.Content = null;
            t1.IsSelected = false;
            return t1;
        }

        /// <summary>
        /// Logic to get the title of the document of the current website.
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string getTitle(WebBrowser webBrowser, string address)
        {
            if (webBrowser.Source == null)
            {
                return string.Empty;
            }
            string title = ((dynamic)(webBrowser.Document)).Title as string;
            if (title == null)
            {
                int idx = address.IndexOf('w');
                string tmp_string = address.Substring(idx + 4);
                idx = tmp_string.IndexOf('.');
                tmp_string = tmp_string.Substring(0, idx);
                title = FirstLetterToUpper(tmp_string);
            }
            return title;
        }        

        /// <summary>
        /// Logic to change the first letter of a string to upper case. Used
        /// when the HideScriptErrors method fails to download the full
        /// document of the website. In this case the title of the page
        /// is taken to be the name given in the url.
        /// Source: http://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case-for-maximum-performance#4135491
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        /// <summary>
        /// Logic to supress JavaScript related errors in the web browser.
        /// Source: http://stackoverflow.com/questions/1298255/how-do-i-suppress-script-errors-when-using-the-wpf-webbrowser-control
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="hide"></param>
        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are too early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }
        // Generic methods: (end)
    }
    
    /// <summary>
    /// Logic for the converter class used in the binding. Describes how
    /// to combine the value and the parameter to yield the correct
    /// binding.
    /// </summary>
    public class widthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}