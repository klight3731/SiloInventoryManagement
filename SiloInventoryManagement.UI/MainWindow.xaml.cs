using SiloInventoryManagement.UI.ViewModel;
using System.Windows;

namespace SiloInventoryManagement.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (System.Exception ex)
            {
                ErrorHelper.LogAndDisplayError(ex);
            }
        }
    }
}
