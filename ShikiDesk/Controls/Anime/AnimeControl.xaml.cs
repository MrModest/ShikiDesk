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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShikiDesk.Controls
{
    /// <summary>
    /// Логика взаимодействия для AnimeControl.xaml
    /// </summary>
    public partial class AnimeControl : UserControl
    {
        public AnimeControl()
        {
            InitializeComponent();
        }

        private void localSearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "Поиск...")
            {
                (sender as TextBox).Clear();
            }
        }

        private void localSearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                (sender as TextBox).Text = "Поиск...";
            }
        }
    }
}
