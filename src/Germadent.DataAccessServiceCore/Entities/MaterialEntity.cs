namespace Germadent.DataAccessServiceCore.Entities
{

    /// <summary>
    /// Материал
    /// </summary>
    public class MaterialEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// Признак неиспользуемости
        /// </summary>
        public bool FlagUnused { get; set; }
    }
}
