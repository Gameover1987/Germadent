namespace Germadent.Rma.Model.Pricing
{
    public class ProductDto
    {
        /// <summary>
        /// Идентификатор изделия
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public int PriceGroupId { get; set; }

        /// <summary>
        /// Идентификатор ценовой позиции
        /// </summary>
        public int PricePositionId { get; set; }

        /// <summary>
        /// Пользовательский код ценовой позиции
        /// </summary>
        public string PricePositionCode { get; set; }

        /// <summary>
        /// Идентификатор материала
        /// </summary>
        public int? MaterialId { get; set; }

        /// <summary>
        /// Наименование материала
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// Наименование изделия
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Тип филиала
        /// </summary>
        public BranchType BranchType { get; set; }

        /// <summary>
        /// Актуальная цена при наличии stl-файла
        /// </summary>
        public decimal PriceStl { get; set; }

        /// <summary>
        /// Актуальная цена при наличии модели
        /// </summary>
        public decimal PriceModel { get; set; }
    }
}