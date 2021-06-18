using System.Collections.Generic;
using System.ComponentModel;

namespace Germadent.Rma.App.Infrastructure
{
    public class ColumnInfo
    {
        /// <summary>
        /// Ширина столбца
        /// Если Width == 0, то параметр не используется
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Идентификатор столбца
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название в интерфейсе
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Направление сортировки
        /// </summary>
        public ListSortDirection? SortDirection { get; set; }

        /// <summary>
        /// Видимость столбца
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Порядковый номер столбца (для изменения порядка следования сстолбцов на UI)
        /// </summary>
        public int DisplayIndex { get; set; }
    }

    public class RmaUserSettings
    {
        public RmaUserSettings()
        {
            UserNames = new string[0];
        }

        public string LastLogin { get; set; }

        public string[] UserNames { get; set; }

        public ColumnInfo[] Columns { get; set; }
    }
}