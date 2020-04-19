namespace Germadent.DataAccessServiceCore.Entities
{
    public partial class OrderEntity
    {
        /// <summary>
        /// Дополнительная ифнормация
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Цвет каркаса (Vita Classical, Opak, Translucen)
        /// </summary>
        public string CarcassColor { get; set; }

        /// <summary>
        /// Система имплантов
        /// </summary>
        public string ImplantSystem { get; set; }

        /// <summary>
        /// Индивидуальная обработка абатмента
        /// </summary>
        public string IndividualAbutmentProcessing { get; set; }

        /// <summary>
        /// Докомплектовать заказ (винт, титановая основа)
        /// </summary>
        public string Understaff { get; set; }
    }
}
