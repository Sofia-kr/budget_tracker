using System;
using System.Collections.Generic;
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

namespace budget_tracker
{
    /// <summary>
    /// Interaction logic for EditingUserData.xaml
    /// </summary>
    public partial class EditingUserData : Window
    {
        public EditingUserData()
        {
            InitializeComponent();
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            //UserSettings settingsWindow = new UserSettings();
            //settingsWindow.Show();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //UserSettings settingsWindow = new UserSettings();
            //settingsWindow.Show();
            this.Close();
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
