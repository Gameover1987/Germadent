namespace Germadent.WebApi.Entities
{
    public class CustomerEntity
    {
        /// <summary>
        /// Идентификатор заказчика
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Наименование заказчика
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Телефон заказчика
        /// </summary>
        public string CustomerPhone { get; set; }
        /// <summary>
        /// Электропочта заказчика
        /// </summary>
        public string CustomerEmail { get; set; }
        /// <summary>
        /// Сайт заказчика
        /// </summary>
        public string CustomerWebSite { get; set; }
        /// <summary>
        /// Описание заказчика
        /// </summary>
        public string CustomerDescription { get; set; }
    }
}
