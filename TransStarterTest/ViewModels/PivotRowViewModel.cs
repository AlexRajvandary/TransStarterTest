using System.ComponentModel;


namespace TransStarterTest.ViewModels
{
    public class PivotRowViewModel : BaseViewModel
    {
        private double _december;
        private double _january;
        private double _febuary;
        private double _march;
        private double _april;
        private double _may;
        private double _june;
        private double _july;
        private double _august;
        private double _september;
        private double _october;
        private double _november;

        public string RowKey { get; set; }

        public double January
        {
            get => _january;
            set { _january = value; OnPropertyChanged(nameof(January)); }
        }

        public double Febuary
        {
            get => _febuary;
            set { _febuary = value; OnPropertyChanged(nameof(Febuary)); }
        }

        public double March
        {
            get => _march;
            set { _march = value; OnPropertyChanged(nameof(March)); }
        }

        public double April
        {
            get => _april;
            set { _april = value; OnPropertyChanged(nameof(April)); }
        }

        public double May
        {
            get => _may;
            set { _may = value; OnPropertyChanged(nameof(May)); }
        }

        public double June
        {
            get => _june;
            set { _june = value; OnPropertyChanged(nameof(June)); }
        }

        public double July
        {
            get => _july;
            set { _july = value; OnPropertyChanged(nameof(July)); }
        }

        public double August
        {
            get => _august;
            set { _august = value; OnPropertyChanged(nameof(August)); }
        }

        public double September
        {
            get => _september;
            set { _september = value; OnPropertyChanged(nameof(September)); }
        }

        public double October
        {
            get => _october;
            set { _october = value; OnPropertyChanged(nameof(October)); }
        }

        public double November
        {
            get => _november;
            set { _november = value; OnPropertyChanged(nameof(November)); }
        }

        public double December
        {
            get => _december;
            set { _december = value; OnPropertyChanged(nameof(December)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}