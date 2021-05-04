namespace Germadent.Model
{
    public class AdditionalEquipmentDto
    {
        public int WorkOrderId { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public int QuantityIn { get; set; }
        public int QuantityOut { get; set; }
    }
}
