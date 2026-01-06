using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
            _ = viewModel.FetchRabisCaseData("USA");
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
                    await viewModel.FetchRabisCaseData(countryName);
                    viewModel.CalculateMostAffectedState();
                    viewModel.IsBusy = false;
                }
            }
        }
    }
}