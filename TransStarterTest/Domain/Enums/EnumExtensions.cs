namespace TransStarterTest.Domain.Enums
{
    public static class EnumExtensions
    {
        public static string GetString (this GroupingOptions options)
        {
            return options switch
            {
                GroupingOptions.Model => "models",
                GroupingOptions.Customer => "customers",
                GroupingOptions.Brand => "brands",
                _ => string.Empty,
            };
        }
    }
}