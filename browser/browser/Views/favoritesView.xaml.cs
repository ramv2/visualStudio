using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace browser
{
    /// <summary>
    /// Interaction logic for favoritesView.xaml
    /// </summary>
    public partial class favoritesView : Window
    {
        public favoritesViewModel viewModel = new favoritesViewModel();
        public string url_title, url_value;
        public ObservableCollection<favoritesModel> favs;
        public bool addFlag1, addFlag2, editFlag1, editFlag2;
        public WebBrowser mw_wb;
        public ComboBox mw_cb;
        public Button mw_cf;

        public favoritesView(WebBrowser main_window_wb, ComboBox main_window_cb, Button main_window_cf)
        {
            favs = viewModel.favorites;
            DataContext = viewModel;
            InitializeComponent();
            mw_wb = main_window_wb;
            mw_cb = main_window_cb;
            mw_cf = main_window_cf;
            string tmp_str = MainWindow.getTitle(mw_wb, mw_cb.Text);
            url_title = tmp_str.Length > 15 ? tmp_str.Substring(0, 15) : tmp_str;
            url_value = main_window_cb.Text;
            Closed += fav_window_Closed;
        }

        // Methods and events related to control type Window: (start)
        /// <summary>
        /// Logic for the Closed event in the window. Describes what 
        /// happens when the window is about to close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fav_window_Closed(object sender, EventArgs e)
        {
            ArrayList updateFavs = new ArrayList();
            for (int i=0; i<favs.Count; i++)
            {
                string currFav = favs[i].title + ";" + favs[i].url;
                updateFavs.Add(currFav);
            }
            Properties.Settings.Default.Favorites = updateFavs;
            Properties.Settings.Default.Save();
            mw_cf.IsEnabled = favs.Count > 0;
        }
        // Methods and events related to control type Window: (end)

        // Methods and events related to control type ComboBox: (start)
        /// <summary>
        /// Logic for the 'fav_cb' combobox-SelectionChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fav_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fav_cb.SelectedItem != null)
            {
                button_delete_favs.IsEnabled = true;
                button_edit_favs.IsEnabled = true;
                button_load_in_current_tab.IsEnabled = true;
            }
        }
        // Methods and events related to control type ComboBox: (end)

        // Methods and events related to control type TextBox: (start)
        /// <summary>
        /// Logic for 'add_favs_title' textbox-TextChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_favs_title_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            addFlag1 = true;
            if (addFlag1 && addFlag2)
                button_add_favs_ok.IsEnabled = true;
        }

        /// <summary>
        /// Logic for 'add_favs_url' textbox-TextChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_favs_url_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            addFlag2 = true;
            if (addFlag1 && addFlag2)
                button_add_favs_ok.IsEnabled = true;
        }

        /// <summary>
        /// Logic for 'add_favs_url' textbox-KeyDown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_favs_url_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    add_favs_url_tb.Text = "http://" + add_favs_url_tb.Text + ".com";
                }
            }
        }

        /// <summary>
        /// Logic for 'edit_favs_title' textbox-TextChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_favs_title_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            editFlag1 = true;
            if (editFlag1 && editFlag2)
                button_edit_favs.IsEnabled = true;
        }

        /// <summary>
        /// Logic for 'edit_favs_url' textbox-TextChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_favs_url_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            editFlag2 = true;
            if (editFlag1 && editFlag2)
                button_edit_favs.IsEnabled = true;
        }

        /// <summary>
        /// Logic for 'edit_favs_url' textbox-KeyDown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_favs_url_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    edit_favs_url_tb.Text = "http://" + edit_favs_url_tb.Text + ".com";
                }
            }
        }
        // Methods and events related to control type TextBox: (end)

        // Methods and events related to control type Button: (start)
        /// <summary>
        /// Logic for the 'add current to favs' button-Click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_add_current_to_favs_Click(object sender, RoutedEventArgs e)
        {
            add_favs_title_tb.Text = url_title;
            add_favs_url_tb.Text = url_value;            
        }

        /// <summary>
        /// Logic for the 'add favs ok' button-Click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_add_favs_ok_Click(object sender, RoutedEventArgs e)
        {
            favoritesModel fav = new favoritesModel();
            fav.title = add_favs_title_tb.Text;
            fav.url = add_favs_url_tb.Text;
            Uri currentUri;
            if (!Uri.TryCreate(fav.url, UriKind.Absolute, out currentUri))
            {
                MessageBox.Show("Please enter a valid URL of the format http://www.example.com or alternatively type example and use Ctrl + Enter to autocomplete URL(i.e., insert 'http://www.' at the beginning and '.com' at the end).");
                return;
            }
            if (favsContains(fav))
            {
                MessageBox.Show("This item already exists in favorites. Please add a new favorite item.");
            }
            else
            {
                favs.Add(fav);
                MessageBox.Show("Favorite added");
                resetAddTextboxes();
            }
        }

        /// <summary>
        /// Logic for the 'add favs cancel' button-Click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_add_favs_cancel_Click(object sender, RoutedEventArgs e)
        {
            fav_window.Close();
        }

        /// <summary>
        /// Logic for the 'load in current tab' button-Click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_load_in_current_tab_Click(object sender, RoutedEventArgs e)
        {
            mw_cb.Text = edit_favs_url_tb.Text;
            mw_wb.Navigate(edit_favs_url_tb.Text);
            fav_window.Close();
        }

        /// <summary>
        /// Logic for the 'edit favs' button-Click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_edit_favs_Click(object sender, RoutedEventArgs e)
        {
            Button b1 = sender as Button;
            if (b1.Content as string == "Edit")
            {
                edit_favs_title_tb.IsEnabled = true;
                edit_favs_url_tb.IsEnabled = true;
                b1.IsEnabled = false;
                b1.Content = "Save";
                button_delete_favs.IsEnabled = false;
                button_load_in_current_tab.IsEnabled = false;
            }
            else
            {
                favoritesModel fav = new favoritesModel();
                fav.title = edit_favs_title_tb.Text;
                fav.url = edit_favs_url_tb.Text;
                Uri currentUri;
                if (!Uri.TryCreate(fav.url, UriKind.Absolute, out currentUri))
                {
                    MessageBox.Show("Please enter a valid URL of the format http://www.example.com or alternatively type example and use Ctrl + Enter to autocomplete URL(i.e., insert 'http://www.' at the beginning and '.com' at the end).");
                    return;
                }
                if (favsContains(fav))
                {
                    MessageBox.Show("This item already exists in favorites!. Please add a new item.");
                }
                else
                {                    
                    favs.Add(fav);
                    MessageBox.Show("Favorite saved.");
                    resetEditTextboxes();
                    b1.Content = "Edit";
                    button_delete_favs.IsEnabled = true;
                    button_load_in_current_tab.IsEnabled = true;
                }
            }            
        }

        /// <summary>
        /// Logic for the 'delete favs' button-Click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_delete_favs_Click(object sender, RoutedEventArgs e)
        {
            favoritesModel thisFav = fav_cb.SelectedItem as favoritesModel;
            MessageBoxResult result = MessageBox.Show("Are you sure you wish to delete favorite item: " + thisFav.ToString(), "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                favs.Remove(thisFav);
                MessageBox.Show("Removed favorite item.");
                resetEditTextboxes();
                if (favs.Count == 0)
                {
                    button_load_in_current_tab.IsEnabled = false;
                    button_edit_favs.IsEnabled = false;
                    button_delete_favs.IsEnabled = false;
                }
            }            
        }
        // Methods and events related to control type Button: (end)

        // Generic methods: (start)
        /// <summary>
        /// Logic for the favsContains method. Used to check the 
        /// ObservableCollection favs contains the item fav
        /// </summary>
        /// <param name="fav"></param>
        /// <returns></returns>
        private bool favsContains(favoritesModel fav)
        {
            bool flag1 = false;
            bool flag2 = false;
            foreach (favoritesModel f in favs)
            {
                if (fav.title == f.title && !flag1)
                    flag1 = true;
                if (fav.url == f.url && !flag2)
                    flag2 = true;
            }
            return flag1 && flag2;
        }

        /// <summary>
        /// Logic to reset all the text boxes in the add new tab.
        /// </summary>
        private void resetAddTextboxes()
        {
            add_favs_title_tb.Text = string.Empty;
            add_favs_url_tb.Text = string.Empty;
            addFlag1 = false;
            addFlag2 = false;
            button_add_favs_ok.IsEnabled = false;
        }

        /// <summary>
        /// Logic to reset all the text boxes in the view/edit tab.
        /// </summary>
        private void resetEditTextboxes()
        {
            edit_favs_title_tb.Text = string.Empty;
            edit_favs_url_tb.Text = string.Empty;
            edit_favs_title_tb.IsEnabled = false;
            edit_favs_url_tb.IsEnabled = false;
            editFlag1 = false;
            editFlag2 = false;
        }        
    }
}
