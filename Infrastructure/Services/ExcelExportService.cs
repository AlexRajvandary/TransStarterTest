using ClosedXML.Excel;
using Domain.Interfaces;
using System.ComponentModel;
using System.Reflection;

namespace Infrastructure.Services
{
    public class ExcelExportService : IExportService
    {
        public Task ExportReportAsync<T>(string reportTitle, IEnumerable<T> items, string fullPath)
        {
            return Task.Run(() =>
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(reportTitle);

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                for (int i = 0; i < properties.Length; i++)
                {
                    var prop = properties[i];
                    var descriptionAttr = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                              .Cast<DescriptionAttribute>()
                                              .FirstOrDefault();

                    var header = descriptionAttr?.Description ?? prop.Name;

                    worksheet.Cell(1, i + 1).Value = header;
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                }

                int row = 2;
                foreach (var item in items)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(item);

                        if (value is DateTime dt)
                        {
                            worksheet.Cell(row, col + 1).Value = dt;
                            worksheet.Cell(row, col + 1).Style.DateFormat.Format = "dd.MM.yyyy";
                        }
                        else if (value is double d)
                        {
                            worksheet.Cell(row, col + 1).Value = d;
                            worksheet.Cell(row, col + 1).Style.NumberFormat.Format = "#,##0.00";
                        }
                        else
                        {
                            worksheet.Cell(row, col + 1).Value = value?.ToString() ?? string.Empty;
                        }
                    }
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(fullPath);
            });
        }
    }
}