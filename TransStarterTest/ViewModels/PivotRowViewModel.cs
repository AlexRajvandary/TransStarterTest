namespace TransStarterTest.ViewModels
{
    public class PivotRowViewModel
    {
        public string RowKey { get; set; }
        public Dictionary<string, decimal> Cells { get; set; } = new();
    }
}
