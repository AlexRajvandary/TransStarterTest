using System.Windows;
using System.Windows.Controls;

namespace TransStarterTest.Behaviors
{
    public static class DataGridColumnsBehavior
    {
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.RegisterAttached(
                "BindableColumns",
                typeof(IEnumerable<DataGridColumn>),
                typeof(DataGridColumnsBehavior),
                new PropertyMetadata(null, OnBindableColumnsChanged));

        public static void SetBindableColumns(DependencyObject element, IEnumerable<DataGridColumn> value)
            => element.SetValue(BindableColumnsProperty, value);

        public static IEnumerable<DataGridColumn> GetBindableColumns(DependencyObject element)
            => (IEnumerable<DataGridColumn>)element.GetValue(BindableColumnsProperty);

        private static void OnBindableColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid grid)
            {
                grid.Columns.Clear();
                if (e.NewValue is IEnumerable<DataGridColumn> columns)
                {
                    foreach (var column in columns)
                        grid.Columns.Add(column);
                }
            }
        }
    }
}
