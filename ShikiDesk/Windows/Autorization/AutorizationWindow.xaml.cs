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

namespace ShikiDesk
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        public ShikiApiLib.ShikiApi user;
        public AutorizationWindow()
        {
            InitializeComponent();
            passwordPBox.PasswordChanged += PasswordPBox_PasswordChanged;
        }

        private void PasswordPBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel.AutorizationVM).Password = passwordPBox.Password;
        }

        private void exitAppButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
