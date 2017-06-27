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
    /// Логика взаимодействия для WatchingAlarmWindow.xaml
    /// </summary>
    public partial class WatchingAlarmWindow : Window
    {
        public WatchingAlarmWindow()
        {
            InitializeComponent();
            var primaryMonitorArea = SystemParameters.WorkArea;
            Left = primaryMonitorArea.Right - Width - 10;
            Top = primaryMonitorArea.Bottom - Height - 10;
        }

        public WatchingAlarmWindow(string titleName) : this()
        {
            titleNameTBlock.Text = titleName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
