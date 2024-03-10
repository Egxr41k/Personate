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

namespace Personate.General
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NotifyIconId = 1;
        private System.Drawing.Icon appIcon;
        public MainWindow()
        {
            InitializeComponent();
            appIcon = new System.Drawing.Icon("C:\\Users\\Egxr41k\\Desktop\\Personate\\Personate\\Resources\\Personate.ico");
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            NotifyIcon.HideNotifyIcon(this, NotifyIconId);
            appIcon.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NotifyIcon.ShowNotifyIcon(this, NotifyIconId, "My App", appIcon);
        }
    }
}
