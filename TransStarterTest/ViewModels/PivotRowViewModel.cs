using System.ComponentModel;


namespace TransStarterTest.ViewModels
{
    public class PivotRowViewModel
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
            set { _january = value;  }
        }

        public double Febuary
        {
            get => _febuary;
            set { _febuary = value; }
        }

        public double March
        {
            get => _march;
            set { _march = value; }
        }

        public double April
        {
            get => _april;
            set { _april = value; }
        }

        public double May
        {
            get => _may;
            set { _may = value; }
        }

        public double June
        {
            get => _june;
            set { _june = value; }
        }

        public double July
        {
            get => _july;
            set { _july = value; }
        }

        public double August
        {
            get => _august;
            set { _august = value; }
        }

        public double September
        {
            get => _september;
            set { _september = value; }
        }

        public double October
        {
            get => _october;
            set { _october = value; }
        }

        public double November
        {
            get => _november;
            set { _november = value; }
        }

        public double December
        {
            get => _december;
            set { _december = value; }
        }
    }
}