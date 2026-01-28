using Syncfusion.UI.Xaml.HeatMap;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace RabiesTracker
{
    public class RabiesTrackerViewModel : INotifyPropertyChanged
    {
        #region Fields

        private RabiesTrackerService? rabiesTrackerService;
        private string countryName = "USA";
        private ObservableCollection<RabiesTrackerInfo>? data;
        private ObservableCollection<RabiesTrackerInfo>? loadingData;
        private TableMapping? tableMappingData;
        private bool isEnabled;
        private bool isBusy;
        private ColorMappingCollection? colorMappingData;
        private string _mostAffectedState;
        private string _leastAffectedState;
        private string _mostAffectedTrendIndicator;
        private string _leastAffectedTrendIndicator;
        private Brush _mostAffectedTrendColor;
        private Brush _leastAffectedTrendColor;
        private string _mostAffectedPercentageChange;
        private string _leastAffectedPercentageChange;

        #endregion

        #region Constructor

        public RabiesTrackerViewModel()
        {
            DummyData = new ObservableCollection<RabiesTrackerInfo>()
            {
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
                new() { State = "-", Y2018 = 0, Y2019 = 0, Y2020 = 0, Y2021 = 0, Y2022 = 0, Y2023 = 0, Y2024 = 0, Y2025 = 0},
            };

            CaseData = DummyData;
            IsBusy = true;
            isEnabled = true;
            CalculateMostAffectedState();
        }

        #endregion

        #region Properties

        public ObservableCollection<RabiesTrackerInfo>? CaseData
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged(nameof(CaseData));
            }
        }

        public ObservableCollection<RabiesTrackerInfo>? DummyData
        {
            get => loadingData;
            set
            {
                loadingData = value;
                OnPropertyChanged(nameof(DummyData));
            }
        }

        public TableMapping? TableMappingData
        {
            get => tableMappingData;
            set
            {
                tableMappingData = value;
                OnPropertyChanged(nameof(TableMappingData));
            }
        }

        public ColorMappingCollection? ColorMappingData
        {
            get => colorMappingData;
            set
            {
                colorMappingData = value;
                OnPropertyChanged(nameof(ColorMappingData));
            }
        }

        public string CountryName
        {
            get => countryName;
            set
            {
                countryName = value;
                OnPropertyChanged(nameof(CountryName));
            }
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public string MostAffectedState
        {
            get { return _mostAffectedState; }
            set
            {
                _mostAffectedState = value;
                OnPropertyChanged(nameof(MostAffectedState));
            }
        }

        public string LeastAffectedState
        {
            get { return _leastAffectedState; }
            set
            {
                _leastAffectedState = value;
                OnPropertyChanged(nameof(LeastAffectedState));
            }
        }

        public string MostAffectedTrendIndicator
        {
            get { return _mostAffectedTrendIndicator; }
            set
            {
                _mostAffectedTrendIndicator = value;
                OnPropertyChanged(nameof(MostAffectedTrendIndicator));
            }
        }

        public string LeastAffectedTrendIndicator
        {
            get { return _leastAffectedTrendIndicator; }
            set
            {
                _leastAffectedTrendIndicator = value;
                OnPropertyChanged(nameof(LeastAffectedTrendIndicator));
            }
        }

        public Brush MostAffectedTrendColor
        {
            get { return _mostAffectedTrendColor; }
            set
            {
                _mostAffectedTrendColor = value;
                OnPropertyChanged(nameof(MostAffectedTrendColor));
            }
        }

        public Brush LeastAffectedTrendColor
        {
            get { return _leastAffectedTrendColor; }
            set
            {
                _leastAffectedTrendColor = value;
                OnPropertyChanged(nameof(LeastAffectedTrendColor));
            }
        }

        public string MostAffectedPercentageChange
        {
            get { return _mostAffectedPercentageChange; }
            set
            {
                _mostAffectedPercentageChange = value;
                OnPropertyChanged(nameof(MostAffectedPercentageChange));
            }
        }

        public string LeastAffectedPercentageChange
        {
            get { return _leastAffectedPercentageChange; }
            set
            {
                _leastAffectedPercentageChange = value;
                OnPropertyChanged(nameof(LeastAffectedPercentageChange));
            }
        }

        #endregion

        #region Property Changed Event

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Methods

        internal void CalculateMostAffectedState()
        {
            if (CaseData == null || !CaseData.Any())
            {
                MostAffectedState = "-";
                LeastAffectedState = "-";
                MostAffectedTrendIndicator = "-";
                LeastAffectedTrendIndicator = "-";
                MostAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9f7fc"));
                LeastAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9f7fc"));
                MostAffectedPercentageChange = "_%";
                LeastAffectedPercentageChange = "_%";
                return;
            }

            // Calculate total cases across all years for each state
            var stateAnalysis = CaseData.Select(record => new
            {
                State = record.State,
                TotalCases = record.Y2018 + record.Y2019 + record.Y2020 + record.Y2021 +
                            record.Y2022 + record.Y2023 + record.Y2024 + record.Y2025,
                Change = record.Y2025 - record.Y2024,

                // Calculate percentage change between 2024 and 2025
                PercentageChange = record.Y2024 > 0 ?
                    Math.Round(((double)(record.Y2025 - record.Y2024) / record.Y2024) * 100, 1) : 0
            }).ToList();

            // Find the state with the HIGHEST TOTAL cases
            var mostAffected = stateAnalysis.OrderByDescending(x => x.TotalCases).FirstOrDefault();

            // Find the state with the LOWEST TOTAL cases
            var leastAffected = stateAnalysis.OrderBy(x => x.TotalCases).FirstOrDefault();

            if (mostAffected != null)
            {
                MostAffectedState = mostAffected.State;
                MostAffectedTrendIndicator = mostAffected.Change > 0 ? "↑" :
                                             mostAffected.Change < 0 ? "↓" : "-";

                // Format percentage change with proper wording
                double absPercentage = Math.Abs(mostAffected.PercentageChange);

                if (mostAffected.PercentageChange > 0)
                {
                    MostAffectedPercentageChange = $"{absPercentage}% increase";
                }
                else if (mostAffected.PercentageChange < 0)
                {
                    MostAffectedPercentageChange = $"{absPercentage}% decrease";
                }
                else
                {
                    MostAffectedPercentageChange = "_%";
                }

                // Most Affected: Up Arrow = Red (bad), Down Arrow = Green (good)
                if (mostAffected.Change > 0)
                {
                    MostAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f5423e"));
                }
                else if (mostAffected.Change < 0)
                {
                    MostAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E301"));
                }
                else
                {
                    MostAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9f7fc"));
                }

                OnPropertyChanged(nameof(MostAffectedState));
            }

            if (leastAffected != null)
            {
                LeastAffectedState = leastAffected.State;
                LeastAffectedTrendIndicator = leastAffected.Change > 0 ? "↑" :
                                              leastAffected.Change < 0 ? "↓" : "-";

                // Format percentage change with proper wording
                double absPercentage = Math.Abs(leastAffected.PercentageChange);

                if (leastAffected.PercentageChange > 0)
                {
                    LeastAffectedPercentageChange = $"{absPercentage}% increase";
                }
                else if (leastAffected.PercentageChange < 0)
                {
                    LeastAffectedPercentageChange = $"{absPercentage}% decrease";
                }
                else
                {
                    LeastAffectedPercentageChange = "_%";
                }

                // Least Affected: Up Arrow = Red (bad), Down Arrow = Green (good)
                if (leastAffected.Change > 0)
                {
                    LeastAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f5423e"));
                }
                else if (leastAffected.Change < 0)
                {
                    LeastAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E301"));
                }
                else
                {
                    LeastAffectedTrendColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9f7fc"));
                }

                OnPropertyChanged(nameof(LeastAffectedState));
            }
        }

        internal async Task FetchRabiesCaseData(string countryName)
        {
            rabiesTrackerService = new RabiesTrackerService();
            IsBusy = true;
            var newRabiesData = await rabiesTrackerService.PredictRabiesData(countryName);
            CaseData = new ObservableCollection<RabiesTrackerInfo>(newRabiesData);
            GenerateDynamicColorMapping();
            CalculateMostAffectedState();
            IsBusy = false;
        }

        private void GenerateDynamicColorMapping()
        {
            if (CaseData == null || !CaseData.Any())
            {
                ColorMappingData = new ColorMappingCollection();
                return;
            }

            // Find min and max values across all year properties
            double minVal = double.MaxValue;
            double maxVal = double.MinValue;

            foreach (var item in CaseData)
            {
                minVal = Math.Min(minVal, item.Y2018);
                minVal = Math.Min(minVal, item.Y2019);
                minVal = Math.Min(minVal, item.Y2020);
                minVal = Math.Min(minVal, item.Y2021);
                minVal = Math.Min(minVal, item.Y2022);
                minVal = Math.Min(minVal, item.Y2023);
                minVal = Math.Min(minVal, item.Y2024);
                minVal = Math.Min(minVal, item.Y2025);

                maxVal = Math.Max(maxVal, item.Y2018);
                maxVal = Math.Max(maxVal, item.Y2019);
                maxVal = Math.Max(maxVal, item.Y2020);
                maxVal = Math.Max(maxVal, item.Y2021);
                maxVal = Math.Max(maxVal, item.Y2022);
                maxVal = Math.Max(maxVal, item.Y2023);
                maxVal = Math.Max(maxVal, item.Y2024);
                maxVal = Math.Max(maxVal, item.Y2025);
            }

            // Handle cases where minVal might still be MaxValue (e.g., all 0s or empty data)
            if (minVal == double.MaxValue)
            {
                // Default to 0 if no valid numbers found
                minVal = 0;
            }
            if (maxVal == double.MinValue)
            {
                // Default to 1 if no valid numbers found, to avoid division by zero
                maxVal = 1;
            }

            // Define a set of colors for your gradient
            Color startColor = Color.FromRgb(232, 245, 233); // Very Low (light green)
            Color middleColor1 = Color.FromRgb(165, 214, 167); // Low (soft green)
            Color middleColor2 = Color.FromRgb(255, 245, 157); // Moderate (yellow)
            Color middleColor3 = Color.FromRgb(251, 192, 45);   // Elevated (amber)
            Color middleColor4 = Color.FromRgb(251, 140, 0);    // High (orange)
            Color endColor = Color.FromRgb(198, 40, 40);      // Critical (deep red)

            // Calculate thresholds based on min/max and the desired number of color segments
            double range = maxVal - minVal;
            double segmentSize = range / 5.0; // 6 color mappings means 5 segments

            ColorMappingCollection newColorMapping = new ColorMappingCollection
            {
                new ColorMapping { Value = minVal, Color = startColor },
                new ColorMapping { Value = minVal + segmentSize, Color = middleColor1 },
                new ColorMapping { Value = minVal + 2 * segmentSize, Color = middleColor2 },
                new ColorMapping { Value = minVal + 3 * segmentSize, Color = middleColor3 },
                new ColorMapping { Value = minVal + 4 * segmentSize, Color = middleColor4 },
                new ColorMapping { Value = maxVal, Color = endColor }
            };

            ColorMappingData = newColorMapping;
        }

        internal async Task ValidateCredential()
        {
            if (rabiesTrackerService != null)
            {
                await rabiesTrackerService.ValidateCredential();

                if (!rabiesTrackerService.IsValid)
                {
                    IsEnabled = false;
                    CountryName = "USA";
                }
                else
                {
                    IsEnabled = true;
                }
            }
        }

        #endregion
    }
}
