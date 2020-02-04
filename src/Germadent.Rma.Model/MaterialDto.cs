namespace Germadent.Rma.Model
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

        public string Name { get; set; }

        public bool IsObsolete { get; set; }
    }
}