﻿namespace Germadent.Model
{
    public class AttributeKeyNames
    {
        public const string CarcassColor = "CarcassColor";
    }

    /// <summary>
    /// Атрибут 
    /// </summary>
    public class AttributeDto
    {
        /// <summary>
        /// Идентификатор заказ-наряда
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Идентификатор атрибута
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// Ключевое имя атрибута
        /// </summary>
        public string AttributeKeyName { get; set; }

        /// <summary>
        /// Наименование атрибута атрибута по-русски
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Позволяет определить является ли данный атрибут устаревшим
        /// </summary>
        public bool IsObsolete { get; set; }

        /// <summary>
        /// Идентификатор значения атрибута
        /// </summary>
        public int AttributeValueId { get; set; }

        /// <summary>
        /// Значение атрибута
        /// </summary>
        public string AttributeValue { get; set; }
    }
}
