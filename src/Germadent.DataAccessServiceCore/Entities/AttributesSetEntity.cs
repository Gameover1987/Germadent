namespace Germadent.DataAccessServiceCore.Entities
{
    public class AttributesSetEntity
    {
        public int WorkOrderId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeKeyName { get; set; }
        public int AttrValueId { get; set; }
        public string AttributeValue { get; set; }
    }
}
