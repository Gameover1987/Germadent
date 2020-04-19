namespace Germadent.Rma.ModelCore
{
    public class ProstheticConditionDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }

    public class ProstheticsTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class MaterialDto
    {
        public int Id { get; set; }

        public string MaterialName { get; set; }

        public bool IsObsolete { get; set; }
    }
}