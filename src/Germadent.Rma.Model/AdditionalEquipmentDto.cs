namespace Germadent.Rma.Model
{
    public class AdditionalEquipmentDto
    {
        public int WorkOrderId { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public int Quantity { get; set; }
    }
}
