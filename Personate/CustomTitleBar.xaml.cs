using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Personate
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CustomTitleBar : UserControl
    {
        private string iconpath =
        Path.GetFullPath(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\Resources\Icons"));
        public CustomTitleBar()
        {
            InitializeComponent();


            CloseIcon.Source = new BitmapImage(new Uri(iconpath + "\\close.png"));
            MaximizeIcon.Source = new BitmapImage(new Uri(iconpath + "\\maximize.png"));
            MinimizeIcon.Source = new BitmapImage(new Uri(iconpath + "\\minimize.png"));

        }

        private static Window MainWindow => Application.Current.MainWindow;
        
        private void CloseIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Close();
        }

        private void MaximizeIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.WindowState == WindowState.Normal)
                MainWindow.WindowState = WindowState.Maximized;
            else MainWindow.WindowState = WindowState.Normal;
        }

        private void MinimizeIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.WindowState = WindowState.Minimized;
        }
        
        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                MainWindow.DragMove();
        }
    }
}
