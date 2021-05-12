using DocumentFormat.OpenXml.Spreadsheet;

namespace Germadent.Client.Common.Reporting.PropertyGrid
{
    public class PropertyItem
    {
        /// <summary>
        /// Название свойства на UI
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
    }
}