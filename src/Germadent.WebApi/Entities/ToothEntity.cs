namespace Germadent.WebApi.Entities
{
    public class ToothEntity
    {
        public int ToothNumber { get; set; }

        public string ConditionName { get; set; }

        public string MaterialName { get; set; }

        public string ProstheticsName { get; set; }

        public bool FlagBridge { get; set; }
    }
}