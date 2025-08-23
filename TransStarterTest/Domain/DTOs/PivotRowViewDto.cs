using System.ComponentModel;

namespace TransStarterTest.Models.DTOs
{
    public class PivotRowViewDto
    {
        [Description("Группа")]
        public string RowKey { get; set; }

        [Description("Январь")]
        public double January { get; set; }

        [Description("Февраль")]
        public double Febuary { get; set; }

        [Description("Март")]
        public double March { get; set; }

        [Description("Апрель")]
        public double April { get; set; }

        [Description("Май")]
        public double May { get; set; }

        [Description("Июнь")]
        public double June { get; set; }

        [Description("Июль")]
        public double July { get; set; }

        [Description("Август")]
        public double August { get; set; }

        [Description("Сентябрь")]
        public double September { get; set; }

        [Description("Октябрь")]
        public double October { get; set; }

        [Description("Ноябрь")]
        public double November { get; set; }

        [Description("Декабрь")]
        public double December { get; set; }
    }
}