namespace Germadent.WebApi.Entities
{
    public class AttributesEntity
    {
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
    }
}
