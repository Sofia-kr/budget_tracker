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
    /// Interaction logic for FAddCategoryExpenses.xaml
    /// </summary>
    public partial class FAddCategoryExpenses : Window
    {
        public FAddCategoryExpenses()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteECategory deleteECategory = new DeleteECategory();
            deleteECategory.Show();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            AddECategory add = new AddECategory();
            add.Show();
        }
    }
}
