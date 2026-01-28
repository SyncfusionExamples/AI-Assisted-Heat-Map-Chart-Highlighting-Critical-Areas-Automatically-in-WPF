using Syncfusion.Windows.Tools.Controls;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RabiesTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _ = viewModel.FetchRabiesCaseData("USA");
        }

        private async void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _ = viewModel.ValidateCredential();

                busyIndicator.AnimationType = Syncfusion.Windows.Controls.Notification.AnimationTypes.Flower;

                string countryName = countryTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(countryName))
                {
                    viewModel.CaseData?.Clear();
                    viewModel.CaseData = viewModel.DummyData;
                    viewModel.MostAffectedState = "-";
                    viewModel.LeastAffectedState = "-";
                    viewModel.MostAffectedTrendIndicator = "-";
                    viewModel.LeastAffectedTrendIndicator = "-";
                    viewModel.MostAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9f7fc"));
                    viewModel.LeastAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9f7fc"));
                    viewModel.MostAffectedPercentageChange = "_%";
                    viewModel.LeastAffectedPercentageChange = "_%";
                    viewModel.IsBusy = true;
                    await viewModel.FetchRabiesCaseData(countryName);
                    viewModel.CalculateMostAffectedState();
                    viewModel.IsBusy = false;
                }
            }
        }
    }
}