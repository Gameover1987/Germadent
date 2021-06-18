using System;

namespace Germadent.Client.Common.Reporting.PropertyGrid
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrintableOrderPropertyAttribute : Attribute
    {
        /// <summary>
        /// Название свойства на UI
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string GroupName { get; set; }
    }
}