namespace TransStarterTest.ViewModels
{
    public class DynamicPivotRow
    {
        public string RowKey { get; set; }
        public Dictionary<string, double> Values { get; set; } = new();
    }
}