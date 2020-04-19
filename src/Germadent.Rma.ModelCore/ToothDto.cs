namespace Germadent.Rma.ModelCore
{
    public class ToothDto
    {
        public int WorkOrderId { get; set; }

        public int ToothNumber { get; set; }

        public int ConditionId { get; set; }

        public string ConditionName { get; set; }

        public int ProstheticsId { get; set; }

        public string ProstheticsName { get; set; }

        public int MaterialId { get; set; }

        public string MaterialName { get; set; }


        public bool HasBridge { get; set; }

        public override string ToString()
        {
            return string.Format(
                "WorkOrderId={0}, TootoNumber={1}, MaterialId={2}, MaterialName={3}, ProstheticsId={4}, ProstheticsName={5}, HasBridge={6}",
                WorkOrderId, ToothNumber, MaterialId, MaterialName, ProstheticsId, ProstheticsName, HasBridge);
        }
    }
}
