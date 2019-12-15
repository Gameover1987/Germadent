﻿namespace Germadent.Rma.Model
{
    public class MaterialDto
    {
        public string Name { get; set; }

        public bool IsObsolete { get; set; }
    }

    public class Teeth
    {
        public int Number { get; set; }

        public MaterialDto Material { get; set; }

        public bool HasBridge { get; set; }
    }
}