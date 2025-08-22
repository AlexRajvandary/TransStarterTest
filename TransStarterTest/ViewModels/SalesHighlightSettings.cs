namespace TransStarterTest.ViewModels
{
    public class SalesHighlightSettings : BaseViewModel
    {
        private bool _isEnabled;
        private double _threshold;

        public SalesHighlightSettings(bool isEnabled, double threshold)
        {
            IsEnabled = isEnabled;
            Threshold = threshold;
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public double Threshold
        {
            get => _threshold;
            set => SetProperty(ref _threshold, value);
        }
    }
}
