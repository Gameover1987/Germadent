namespace Germadent.WebApi.Entities
{
    /// <summary>
    /// Условие протезирования
    /// </summary>
    public class ProstheticConditionEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int ConditionId { get; set; }

        /// <summary>
        /// Название 
        /// </summary>
        public string ConditionName { get; set; }
    }
}